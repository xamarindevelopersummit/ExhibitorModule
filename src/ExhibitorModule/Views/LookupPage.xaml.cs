using Xamarin.Forms;

namespace ExhibitorModule.Views
{
    public partial class LookupPage : ContentPage
    {
        ViewModels.LookupPageViewModel vm;

        public LookupPage()
        {
            InitializeComponent();
        }

        protected override void OnBindingContextChanged()
        {
            base.OnBindingContextChanged();
            vm = BindingContext as ViewModels.LookupPageViewModel;
        }

        /// <summary>
        /// HACK: Workaround while DataTrigger is not working properly
        /// </summary>
        /// <param name="sender">Sender.</param>
        /// <param name="e">E.</param>
        private void Handle_TextChanged(object sender, Xamarin.Forms.TextChangedEventArgs e)
        {
            if (vm == null) return;
            ManualAddButton.IsVisible = vm.SearchResults.Count < 1;
        }
    }
}
