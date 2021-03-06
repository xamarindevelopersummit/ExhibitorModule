﻿using ExhibitorModule.Core.Views;
using Microsoft.Extensions.DependencyInjection;
using Prism;
using Prism.DryIoc;
using Prism.Ioc;
using Prism.Modularity;
using Shiny.Prism;
using Xamarin.Forms;

namespace ExhibitorModule
{
    public partial class App
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
        }

        protected override async void OnInitialized()
        {
            InitializeComponent();
            var result = await NavigationService.NavigateAsync("MainPage/NavigationPage/LeadsPage");
            if(!result.Success)
            {
                System.Diagnostics.Debugger.Break();
            }
        }

        protected override void RegisterTypes(IContainerRegistry containerRegistry)
        {
            containerRegistry.RegisterForNavigation<NavigationPage>();
            containerRegistry.RegisterForNavigation<MainPage>();
            containerRegistry.RegisterMany<Configs>();
        }

        protected override void ConfigureModuleCatalog(IModuleCatalog moduleCatalog)
        {
            moduleCatalog.AddModule<ExhibitorModule>();
        }
    }

    public class ExhibitorStartup : PrismStartup
    {
        public ExhibitorStartup()
            : base(PrismContainerExtension.Current)
        {
        }

        public override void ConfigureServices(IServiceCollection services)
        {

        }
    }
}
