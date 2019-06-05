using System;
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
    }

    public class Base : IBase
    {
        public Base(INavigationService navigationService, IPageDialogService pageDialogService,
                             IDeviceService deviceService, ICacheService cacheService)
        {
            NavigationService = navigationService;
            PageDialogService = pageDialogService;
            DeviceService = deviceService;
            CacheService = cacheService;
        }

        public INavigationService NavigationService { get; }

        public ICacheService CacheService { get; }

        public IPageDialogService PageDialogService { get; }

        public IDeviceService DeviceService { get; }
    }
}
