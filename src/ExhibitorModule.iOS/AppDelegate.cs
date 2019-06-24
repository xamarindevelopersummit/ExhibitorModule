using Foundation;
using UIKit;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;

namespace ExhibitorModule.iOS
{
    [Register("AppDelegate")]
    public partial class AppDelegate : FormsApplicationDelegate
    {
        public override bool FinishedLaunching(UIApplication uiApplication, NSDictionary launchOptions)
        {
            Shiny.iOSShinyHost.Init(new ExhibitorStartup());

            Forms.SetFlags("CollectionView_Experimental");
            global::Xamarin.Forms.Forms.Init();
            global::Xamarin.Forms.FormsMaterial.Init();
            global::FFImageLoading.Forms.Platform.CachedImageRenderer.Init();
            global::FFImageLoading.ImageService.Instance.Initialize();

            //CrossPayPalManager.Init(GetPayPalConfiguration());
            LoadApplication(new App());
            var finishedLaunching = base.FinishedLaunching(uiApplication, launchOptions);
            uiApplication.KeyWindow.TintColor = UIColor.FromRGB(0, 154, 67);
            //Console.WriteLine($">>> Push is enabled: {Push.IsEnabledAsync()}");
            return finishedLaunching;
        }
    }
}
