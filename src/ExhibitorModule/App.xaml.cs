using System;
using System.Threading.Tasks;
using Acr.UserDialogs;
using ExhibitorModule.Data;
using ExhibitorModule.Data.Abstractions;
using ExhibitorModule.Helpers;
using ExhibitorModule.Services;
using ExhibitorModule.Services.Abstractions;
using ExhibitorModule.ViewModels;
using ExhibitorModule.Views;
using FFImageLoading.Helpers;
using Microsoft.AppCenter;
using Microsoft.AppCenter.Analytics;
using Microsoft.AppCenter.Crashes;
using Microsoft.AppCenter.Distribute;
using Plugin.Calendars;
using Plugin.Calendars.Abstractions;
using Plugin.DeviceInfo;
using Plugin.DeviceInfo.Abstractions;
using Plugin.FirebasePushNotification;
using Plugin.FirebasePushNotification.Abstractions;
using Plugin.Permissions;
using Plugin.Permissions.Abstractions;
using Prism;
using Prism.Ioc;
using Prism.Logging;
using Prism.Modularity;
using Prism.Plugin.Popups;
using Prism.Unity;
using Unity;
using Xamarin.Essentials;
using DebugLogger = ExhibitorModule.Services.DebugLogger;

namespace ExhibitorModule
{
    public partial class App : PrismApplication
    {
        /* 
         * NOTE: 
         * The Xamarin Forms XAML Previewer in Visual Studio uses System.Activator.CreateInstance.
         * This imposes a limitation in which the App class must have a default constructor. 
         * App(IPlatformInitializer initializer = null) cannot be handled by the Activator.
         */
        public App() : this(null) {}

        public App(IPlatformInitializer initializer) : base(initializer)
        {
            // https://docs.microsoft.com/en-us/mobile-center/sdk/distribute/xamarin
            Distribute.ReleaseAvailable = OnReleaseAvailable;

            // https://docs.microsoft.com/en-us/mobile-center/sdk/push/xamarin-forms
            var fb = Container.Resolve<IFirebasePushNotification>();
            fb.OnNotificationReceived += OnPushNotificationReceived;
            fb.OnTokenRefresh += OnTokenRefreshed;
            fb.OnNotificationOpened += Handle_OnNotificationOpened;

            // NOTE: Make sure to build CIC project to generate Secrets.cs in obj folder.
            // Handle when your app starts
            var appId = Container.Resolve<IDeviceInfo>().Platform == Plugin.DeviceInfo.Abstractions.Platform.Android ?
                $"android={Secrets.AppCenter_Android_Secret};" :
                $"ios={Secrets.AppCenter_iOS_Secret};";

            AppCenter.Start(appId, typeof(Analytics), typeof(Crashes), typeof(Distribute));
            SetDefault();
        }

        private void SetDefault()
        {
            // Cache any configs for local use
        }

        protected override async void OnInitialized()
        {
            InitializeComponent();
            LogUnobservedTaskExceptions();
            //SubsribeEvents();
            await NavigationService.NavigateAsync($"{nameof(RootNavigationPage)}/{nameof(MainPage)}");//?selectedTab=EventsPage");//WebPage?title_key=Testing&source_url_key=http://cypressislamiccenter.org/blog/2017/09/06/hiring-teachers-now-for-sunday-school/");
        }

        protected override void RegisterTypes(IContainerRegistry containerRegistry)
        {
            containerRegistry.Register<IClientConfig, Configs>();
        }

        protected override void ConfigureModuleCatalog(IModuleCatalog moduleCatalog)
        {
            moduleCatalog.AddModule<ExhibitorModule>();
        }

        protected override async void OnStart()
        {
            // Handle when your app starts
            if (await Analytics.IsEnabledAsync())
            {
                System.Diagnostics.Debug.WriteLine("Analytics is enabled");
                FFImageLoading.ImageService.Instance.Config.Logger = (IMiniLogger)Container.Resolve<ILoggerFacade>();
            }
            else
            {
                System.Diagnostics.Debug.WriteLine("Analytics is disabled");
            }
        }

        
        void Handle_OnNotificationOpened(object source, FirebasePushNotificationResponseEventArgs e)
        {
            Console.WriteLine("Push tapped:");
            foreach (var data in e.Data)
            {
                Console.WriteLine($"{data.Key}:{data.Value}");
            }
        }


        private void OnTokenRefreshed(object sender, FirebasePushNotificationTokenEventArgs e)
        {
            Console.WriteLine($"FCM TOKEN refreshed: {e.Token}");
        }

        private void OnPushNotificationReceived(object sender, FirebasePushNotificationDataEventArgs e)
        {
            var summary = $"Push notification received:";
            if (e.Data != null)
            {
                summary += "\n\tCustom data:\n";
                foreach (var key in e.Data.Keys)
                {
                    summary += $"\t\t{key} : {e.Data[key]}\n";
                }
            }

            // Send the notification summary to debug output
            System.Diagnostics.Debug.WriteLine(summary);
            Container.Resolve<ILoggerFacade>().Log(summary, Category.Info, Priority.None);
        }

        private bool OnReleaseAvailable(ReleaseDetails releaseDetails)
        {
            // Look at releaseDetails public properties to get version information, release notes text or release notes URL
            string versionName = releaseDetails.ShortVersion;
            string versionCodeOrBuildNumber = releaseDetails.Version;
            string releaseNotes = releaseDetails.ReleaseNotes;
            Uri releaseNotesUrl = releaseDetails.ReleaseNotesUrl;

            // custom dialog
            var title = "Version " + versionName + " available!";
            Task answer;

            // On mandatory update, user cannot postpone
            if (releaseDetails.MandatoryUpdate)
            {
                answer = Current.MainPage.DisplayAlert(title, releaseNotes, "Download and Install");
            }
            else
            {
                answer = Current.MainPage.DisplayAlert(title, releaseNotes, "Download and Install", "Not Now");
            }
            answer.ContinueWith((task) =>
            {
                // If mandatory or if answer was positive
                if (releaseDetails.MandatoryUpdate || (task as Task<bool>).Result)
                {
                    // Notify SDK that user selected update
                    Distribute.NotifyUpdateAction(UpdateAction.Update);
                }
                else
                {
                    // Notify SDK that user selected postpone (for 1 day)
                    // Note that this method call is ignored by the SDK if the update is mandatory
                    Distribute.NotifyUpdateAction(UpdateAction.Postpone);
                }
            });

            // Return true if you are using your own dialog, false otherwise
            return true;
        }

        private void LogUnobservedTaskExceptions()
        {
            TaskScheduler.UnobservedTaskException += (sender, e) =>
            {
                Container.Resolve<ILoggerFacade>().Log(e.Exception.Message, Category.Exception, Priority.High);
            };
        }
    }

    class Configs : IClientConfig
    {
        public Configs()
        {
            BaseAddress = "https://xamdevsummitbackend.azurewebsites.net";
            AuthKey = "pCrzwQ7qdnUTRa12e3Q1bA7iHxWF8VInBwKz9qxdjSyofuxJ4XGYfg==";
            UserToken = "eyJhbGciOiJodHRwOi8vd3d3LnczLm9yZy8yMDAxLzA0L3htbGRzaWctbW9yZSNobWFjLXNoYTI1NiIsInR5cCI6IkpXVCJ9.eyJ1aWQiOiI5MGY3MGQwNS0xNmZlLTRjYTYtOThiNy05MTU2NDZkMDFlOTgiLCJmaXJzdE5hbWUiOiJIdXNzYWluIiwibGFzdE5hbWUiOiJBYmJhc2kiLCJ0aWNrZXRUeXBlIjoiU3BlYWtlciIsInRpY2tldE51bWJlciI6IjMyMDctMSIsImVtYWlsIjoiaG5hYmJhc2lAb3V0bG9vay5jb20iLCJpc3MiOiJYYW1EZXZTdW1taXQiLCJzdWIiOiJYYW1EZXZTdW1taXQiLCJuYmYiOiI2LzIzLzIwMTkgMjozMDo0NiBBTSIsImV4cCI6IjcvMjMvMjAxOSAyOjMwOjQ2IEFNIiwiZWlkIjoiZjZmYzc1ZDEtYWVjOS00YjE2LWE0MmMtMGJiZjMyMDg3YzE3In0.8gca_kU7bgR_cz3T7zDEuf7NqABFckxlFN9BGjd05r8";
        }
        public string BaseAddress { get; set; }
        public string AuthKey { get; set; }
        public string UserToken { get; set; }
    }

    public class ExhibitorModule : IModule, IDisposable
    {
        public async void OnInitialized(IContainerProvider containerProvider)
        {
            SubsribeEvents();
        }

        public void RegisterTypes(IContainerRegistry containerRegistry)
        {
            containerRegistry.RegisterPopupNavigationService();

            containerRegistry.RegisterInstance(CreateLogger());
            containerRegistry.RegisterInstance<IUserDialogs>(UserDialogs.Instance);
            containerRegistry.RegisterInstance<IDeviceInfo>(CrossDeviceInfo.Current);
            containerRegistry.RegisterInstance<ICalendars>(CrossCalendars.Current);
            containerRegistry.RegisterInstance<IPermissions>(CrossPermissions.Current);
            containerRegistry.RegisterInstance<IFirebasePushNotification>(CrossFirebasePushNotification.Current);

            containerRegistry.RegisterForNavigation<RootNavigationPage, RootNavigationPageViewModel>();
            containerRegistry.RegisterForNavigation<AboutPage, AboutPageViewModel>();
            containerRegistry.RegisterForNavigation<SettingsPage, SettingsPageViewModel>();
            containerRegistry.RegisterForNavigation<MainPage, MainPageViewModel>();
            containerRegistry.RegisterForNavigation<LookupPage, LookupPageViewModel>();
            containerRegistry.RegisterForNavigation<AddPersonPage, AddPersonPageViewModel>();

            containerRegistry.Register<IBase, Base>();
            containerRegistry.Register<IApiService, ApiService>();
            containerRegistry.RegisterSingleton<ILeadsService, LeadsService>();
            containerRegistry.Register<IEssentialsService, EssentialsService>();
            containerRegistry.Register<INetworkService, NetworkService>();
            containerRegistry.Register<IDatabase, Database>();
            containerRegistry.Register<ICacheRepository, CacheRepository>();
            containerRegistry.Register<ILoggerFacade, DebugLogger>();

            containerRegistry.RegisterSingleton<IMemoryCache, MemoryCache>();
            containerRegistry.RegisterSingleton<IDeviceCache, DeviceCache>();
            containerRegistry.RegisterSingleton<ICacheService, CacheService>();

            containerRegistry.RegisterDialog<NotesDialog, NotesDialogViewModel>();

            if (!containerRegistry.IsRegistered<IClientConfig>())
                throw new MissingMemberException("Missing implementation for IClientConfig. Please register an instance of IClientConfig that has configurations for the Http client.");
        }

        private ILoggerFacade CreateLogger()
        {
            switch (Xamarin.Forms.Device.RuntimePlatform)
            {
                case "Android":
                    if (!string.IsNullOrWhiteSpace(Helpers.Secrets.AppCenter_Android_Secret))
                        return CreateAppCenterLogger();
                    break;
                case "iOS":
                    if (!string.IsNullOrWhiteSpace(Helpers.Secrets.AppCenter_iOS_Secret))
                        return CreateAppCenterLogger();
                    break;
            }
            return new DebugLogger();
        }

        private MCAnalyticsLogger CreateAppCenterLogger()
        {
            var logger = new MCAnalyticsLogger();
            FFImageLoading.ImageService.Instance.Config.Logger = (IMiniLogger)logger;
            return logger;
        }


        void Connectivity_ConnectivityChanged(object sender, Xamarin.Essentials.ConnectivityChangedEventArgs e)
        {
            if (Connectivity.NetworkAccess == NetworkAccess.None ||
                Connectivity.NetworkAccess == NetworkAccess.Unknown ||
                Connectivity.NetworkAccess == NetworkAccess.Local)
                UserDialogs.Instance.Toast(Strings.Resources.OfflineMessage);
        }

        private void SubsribeEvents()
        {
            Xamarin.Essentials.Connectivity.ConnectivityChanged += Connectivity_ConnectivityChanged;
        }

        private void UnsubsribeEvents()
        {
            Xamarin.Essentials.Connectivity.ConnectivityChanged -= Connectivity_ConnectivityChanged;
        }

        public void Dispose()
        {
            UnsubsribeEvents();
        }
    }
}
