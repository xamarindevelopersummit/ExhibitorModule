using System;
using Xamarin.Forms;

namespace ExhibitorModule.Views
{
    public partial class LookupPage : ContentPage, IDisposable
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
            vm.SearchResults.CollectionChanged += SearchResults_CollectionChanged;
        }

        private void SearchResults_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            NoResultsLabel.IsVisible = vm.SearchResults.Count < 1;
        }

        void IDisposable.Dispose()
        {
            try
            {
                vm.SearchResults.CollectionChanged -= SearchResults_CollectionChanged;
            }
            catch (Exception)
            {
                // VM already disposed
            }
        }
    }
}
