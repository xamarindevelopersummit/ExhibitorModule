using System.Threading.Tasks;
using CIC.Helpers;
using Prism.AppModel;
using Prism.Navigation;
using Prism.Services;

namespace CIC.ViewModels
{

    public class SplashScreenPageViewModel : ViewModelBase
    {
        public SplashScreenPageViewModel(IBase @base) : base(@base) {}

        public override async void OnNavigatedTo(INavigationParameters parameters)
        {
            // TODO: Implement any initialization logic you need here. Example would be to handle automatic user login

            // Simulated long running task. You should remove this in your app.
            //await Task.Delay(4000);

            // After performing the long running task we perform an absolute Navigation to remove the SplashScreen from
            // the Navigation Stack.
            await NavigationService.NavigateAsync("/NavigationPage/MainPage");
        }
    }
}