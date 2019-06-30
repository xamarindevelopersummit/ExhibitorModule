using System;
using FFImageLoading.Forms;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace ExhibitorModule.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class LeadsPage : ContentPage
    {
        public LeadsPage()
        {
            InitializeComponent();
            AddFButton.Source = new EmbeddedResourceImageSource("ExhibitorModule.Resources.add_circle.svg", typeof(LeadsPage).Assembly);
        }

        void Remove_Clicked(object sender, System.EventArgs e)
        {
            // HACK: Stupid workaround
            var vm = BindingContext as ViewModels.LeadsPageViewModel;
            vm?.RemoveLeadCommand?.Execute((Models.LeadContactInfo)(sender as MenuItem).CommandParameter);
        }

        void Handle_ItemSelected(object sender, Xamarin.Forms.SelectedItemChangedEventArgs e)
        {
            // HACK: Stupid workaround
            var vm = BindingContext as ViewModels.LeadsPageViewModel;
            vm?.ShowNotes?.Execute((Models.LeadContactInfo)e.SelectedItem);

            (sender as ListView).SelectedItem = null;
        }
    }
}