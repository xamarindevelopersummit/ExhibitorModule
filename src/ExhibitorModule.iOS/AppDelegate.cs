using System;
using Foundation;
using Microsoft.AppCenter.Distribute;
using Plugin.FirebasePushNotification;
using UIKit;
using Xamarin.Forms.Platform.iOS;

namespace ExhibitorModule.iOS
{
    [Register("AppDelegate")]
    public partial class AppDelegate : FormsApplicationDelegate
    {
        public override bool FinishedLaunching(UIApplication uiApplication, NSDictionary launchOptions)
        {
            global::Xamarin.Forms.Forms.Init();
            global::Xamarin.Forms.FormsMaterial.Init();
            global::Rg.Plugins.Popup.Popup.Init();
            global::FFImageLoading.Forms.Platform.CachedImageRenderer.Init();
            global::FFImageLoading.ImageService.Instance.Initialize(new FFImageLoading.Config.Configuration()
            {
                Logger = new Services.DebugLogger()
            });
            Distribute.DontCheckForUpdatesInDebug();

            // Code for starting up the Xamarin Test Cloud Agent
#if DEBUG
            Xamarin.Calabash.Start();
#endif
            //CrossPayPalManager.Init(GetPayPalConfiguration());
            LoadApplication(new App(new iOSInitializer()));
            FirebasePushNotificationManager.Initialize(launchOptions, true);
            var finishedLaunching = base.FinishedLaunching(uiApplication, launchOptions);
            uiApplication.KeyWindow.TintColor = UIColor.FromRGB(0, 154, 67);
            //Console.WriteLine($">>> Push is enabled: {Push.IsEnabledAsync()}");
            return finishedLaunching;
        }

        public override void DidReceiveRemoteNotification(UIApplication application, NSDictionary userInfo, System.Action<UIBackgroundFetchResult> completionHandler)
        {
            FirebasePushNotificationManager.DidReceiveMessage(userInfo);
            Console.WriteLine($"User info: {userInfo}");
            completionHandler?.Invoke(UIBackgroundFetchResult.NewData);
        }

        public override void FailedToRegisterForRemoteNotifications(UIApplication application, NSError error)
        {
            //Push.FailedToRegisterForRemoteNotifications(error);
            FirebasePushNotificationManager.RemoteNotificationRegistrationFailed(error);
            Console.WriteLine($"Failed to register notifications. {error.Description}");
        }

        public override void RegisteredForRemoteNotifications(UIApplication application, NSData deviceToken)
        {
            //Push.RegisteredForRemoteNotifications(deviceToken);
            FirebasePushNotificationManager.DidRegisterRemoteNotifications(deviceToken);
            Console.WriteLine($"APNs TOKEN registered: {deviceToken.Description}");
        }
    }
}
