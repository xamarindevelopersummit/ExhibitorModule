﻿using System;
using Acr.UserDialogs;
using ExhibitorModule.Common;
using ExhibitorModule.Data;
using ExhibitorModule.Data.Abstractions;
using ExhibitorModule.Helpers;
using ExhibitorModule.Services;
using ExhibitorModule.Services.Abstractions;
using ExhibitorModule.ViewModels;
using ExhibitorModule.Views;
using Prism.Ioc;
using Prism.Logging;
using Prism.Modularity;
using Shiny.Jobs;
using Xamarin.Essentials.Implementation;
using Xamarin.Essentials.Interfaces;

namespace ExhibitorModule
{
    // TODO: Extract module from the app
    public class ExhibitorModule : IModule
    {
        private IJobManager JobManager { get; }

        public ExhibitorModule(IJobManager jobManager)
        {
            JobManager = jobManager;
        }

        public async void OnInitialized(IContainerProvider _)
        {
            await JobManager.Schedule(new JobInfo
            {
                BatteryNotLow = true,
                Identifier = nameof(ExhibitorSyncJob),
                Repeat = true,
                RequiredInternetAccess = InternetAccess.Any,
                Type = typeof(ExhibitorSyncJob)
            });
        }

        public void RegisterTypes(IContainerRegistry containerRegistry)
        {
            if (!containerRegistry.IsRegistered<IClientConfig>())
                throw new MissingMemberException("Missing implementation for IClientConfig. Please register an instance of IClientConfig that has configurations for the Http client.");

            if(!containerRegistry.IsRegistered<IUserDialogs>())
            {
                containerRegistry.RegisterInstance<IUserDialogs>(UserDialogs.Instance);
            }

            containerRegistry.RegisterForNavigation<LookupPage, LookupPageViewModel>();
            containerRegistry.RegisterForNavigation<AddPersonPage, AddPersonPageViewModel>();
            containerRegistry.RegisterForNavigation<LeadsPage, LeadsPageViewModel>();

            containerRegistry.Register<IBase, Base>();
            containerRegistry.Register<IApiService, ApiService>();
            containerRegistry.RegisterSingleton<ILeadsService, LeadsService>();
            containerRegistry.Register<INetworkService, NetworkService>();
            containerRegistry.Register<IDatabase, Database>();
            containerRegistry.Register<ICacheRepository, CacheRepository>();

            containerRegistry.RegisterSingleton<IMemoryCache, MemoryCache>();
            containerRegistry.RegisterSingleton<IDeviceCache, DeviceCache>();
            containerRegistry.RegisterSingleton<ICacheService, CacheService>();

            containerRegistry.RegisterDialog<NotesDialog, NotesDialogViewModel>();
        }
    }
}