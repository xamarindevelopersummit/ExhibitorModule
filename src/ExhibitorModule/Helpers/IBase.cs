using System;
using Acr.UserDialogs;
using ExhibitorModule.Services.Abstractions;
using Prism.Navigation;
using Prism.Services;

namespace ExhibitorModule.Helpers
{
    public interface IBase
    {
        INavigationService NavigationService { get; }
        ICacheService CacheService { get; }
        IPageDialogService PageDialogService { get; }
        IDeviceService DeviceService { get; }
        IUserDialogs UserDialogs { get; }
    }

    public class Base : IBase
    {
        public Base(INavigationService navigationService, IPageDialogService pageDialogService,
                             IDeviceService deviceService, ICacheService cacheService, IUserDialogs userDialogs)
        {
            NavigationService = navigationService;
            PageDialogService = pageDialogService;
            DeviceService = deviceService;
            CacheService = cacheService;
            UserDialogs = userDialogs;
        }

        public INavigationService NavigationService { get; }

        public ICacheService CacheService { get; }

        public IPageDialogService PageDialogService { get; }

        public IDeviceService DeviceService { get; }

        public IUserDialogs UserDialogs { get; }
    }
}
