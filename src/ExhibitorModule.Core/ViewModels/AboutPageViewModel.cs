using System;
using ExhibitorModule.Common;
using ExhibitorModule.Helpers;
using ExhibitorModule.Views;
using Prism.Commands;
using Xamarin.Essentials.Interfaces;

namespace ExhibitorModule.ViewModels
{
    public class AboutPageViewModel : ViewModelBase
    {
        private readonly IAppInfo _appInfo;

        public AboutPageViewModel(IBase @base, IAppInfo appInfo)
            : base(@base)
        {
            Title = "ABOUT";

            InitializeCommand();
            _appInfo = appInfo;
        }

        private void InitializeCommand()
        {
            GoToWebsiteCommand = new DelegateCommand(GoToWebsiteExecute);
            ViewMapCommand = new DelegateCommand(ViewMapExecute);
            CreditsCommand = new DelegateCommand(CreditsExecute);
        }

        private void CreditsExecute()
        {
            NavigationService.NavigateAsync(nameof(CreditsPage));
        }

        private void ViewMapExecute()
        {
           // DeviceService.OpenUri(new Uri(AppConstants.XDSMapLink));
        }

        private void GoToWebsiteExecute()
        {
            //DeviceService.OpenUri(new Uri(AppConstants.XDSWebsiteLink));
        }

        public DelegateCommand GoToWebsiteCommand { get; private set; }
        public DelegateCommand CreditsCommand { get; private set; }
        public DelegateCommand ViewMapCommand { get; private set; }
        // public string AppVersion => $"Version {_appInfo.AppVersion}"; 
    }
}
