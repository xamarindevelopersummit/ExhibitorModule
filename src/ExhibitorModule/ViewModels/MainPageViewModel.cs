using System;
using ExhibitorModule.Common;
using Prism.Commands;
using Prism.Navigation;
using Prism.Services;
using Plugin.DeviceInfo;
using Plugin.DeviceInfo.Abstractions;
using ExhibitorModule.Helpers;
using ExhibitorModule.Views;

namespace ExhibitorModule.ViewModels
{
    public class MainPageViewModel : ViewModelBase
    {
        public MainPageViewModel(IBase @base)
            : base(@base)
        {
            Title = Strings.Resources.MainPageTitle;
        }
    }
}
