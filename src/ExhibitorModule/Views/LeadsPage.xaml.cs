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
        }

        void Remove_Clicked(object sender, System.EventArgs e)
        {
            // HACK: Stupid workaround
            var vm = BindingContext as ViewModels.LeadsPageViewModel;
            vm?.RemoveLeadCommand?.Execute((Models.LeadContactInfo)(sender as MenuItem).CommandParameter);
        }
    }
}