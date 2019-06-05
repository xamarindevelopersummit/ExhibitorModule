using System;
using ExhibitorModule.Common;
using ExhibitorModule.Helpers;
using ExhibitorModule.Views;
using Plugin.DeviceInfo.Abstractions;
using Prism.Commands;

namespace ExhibitorModule.ViewModels
{
    public class AboutPageViewModel : ViewModelBase
    {
        private readonly IDeviceInfo _deviceInfo;

        public AboutPageViewModel(IBase @base, IDeviceInfo deviceInfo)
            : base(@base)
        {
            Title = Strings.Resources.AboutPageTitle;

            InitializeCommand();
            _deviceInfo = deviceInfo;
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
            DeviceService.OpenUri(new Uri(AppConstants.XDSMapLink));
        }

        private void GoToWebsiteExecute()
        {
            DeviceService.OpenUri(new Uri(AppConstants.XDSWebsiteLink));
        }

        public DelegateCommand GoToWebsiteCommand { get; private set; }
        public DelegateCommand CreditsCommand { get; private set; }
        public DelegateCommand ViewMapCommand { get; private set; }
        public string AppVersion => $"Version {_deviceInfo.AppVersion}"; 
    }
}
