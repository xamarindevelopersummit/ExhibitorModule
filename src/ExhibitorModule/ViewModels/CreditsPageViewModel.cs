using System;
using System.Collections.Generic;
using ExhibitorModule.Helpers;
using Prism.Navigation;

namespace ExhibitorModule.ViewModels
{
    public class CreditsPageViewModel : ViewModelBase
    {
        public CreditsPageViewModel(IBase @base) : base(@base)
        {
            Title = "Software";
        }

        public List<Tuple<string,string>> Software { get; private set; }

        Tuple<string, string> _selectedSoftware;
        public Tuple<string, string> SelectedSoftware 
        {
            get => _selectedSoftware;
            set 
            {
                SetProperty(ref _selectedSoftware, value);

                //if(value != null)
                    //DeviceService.OpenUri(new Uri(value.Item2));
            }
        }

        public override void OnNavigatingTo(INavigationParameters parameters)
        {
            base.OnNavigatingTo(parameters);
            LoadPage();
        }

        private void LoadPage()
        {
            Software = new List<Tuple<string, string>> {
                new Tuple<string, string>("Xamarin.Forms", "http://www.xamarin.com/"),
                new Tuple<string, string>("Prism.Forms", "https://github.com/Intelliabb/XamarinControls/"),
                new Tuple<string, string>("Prism.Plugins.Popups", "https://github.com/Intelliabb/XamarinControls/"),
                new Tuple<string, string>("IntelliAbb Xamarin Controls", "https://github.com/Intelliabb/XamarinControls/"),
                new Tuple<string, string>("Acr.UserDialogs", "https://github.com/Intelliabb/XamarinControls/"),
                new Tuple<string, string>("HTML Agility Pack", "https://github.com/Intelliabb/XamarinControls/"),
                new Tuple<string, string>("CClarke.Plugin.Calendars", "https://github.com/Intelliabb/XamarinControls/"),
                new Tuple<string, string>("Fody", "https://github.com/Intelliabb/XamarinControls/"),
                new Tuple<string, string>("Microsoft AppCenter Suite", "https://github.com/Intelliabb/XamarinControls/"),
                new Tuple<string, string>("Mobile BuildTools", "https://github.com/Intelliabb/XamarinControls/"),
                new Tuple<string, string>("Newtonsoft", "https://github.com/Intelliabb/XamarinControls/"),
                new Tuple<string, string>("Google Firebase", "https://github.com/Intelliabb/XamarinControls/"),
                new Tuple<string, string>("Xamarin.FFImageLoading.Forms", "https://github.com/Intelliabb/XamarinControls/"),
                new Tuple<string, string>("Xamarin.Essentials", "https://github.com/Intelliabb/XamarinControls/"),
            };
        }
    }
}
