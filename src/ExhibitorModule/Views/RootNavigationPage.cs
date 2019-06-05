using Xamarin.Forms.PlatformConfiguration.iOSSpecific;

namespace ExhibitorModule.Views
{
    public class RootNavigationPage : Xamarin.Forms.NavigationPage
    {
        public RootNavigationPage()
        {
            On<Xamarin.Forms.PlatformConfiguration.iOS>().SetPrefersLargeTitles(true);
        }
    }
}

