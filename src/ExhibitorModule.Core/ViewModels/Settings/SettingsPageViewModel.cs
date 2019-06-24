using ExhibitorModule.Helpers;

namespace ExhibitorModule.ViewModels
{
    public class SettingsPageViewModel : ViewModelBase
    {
        public SettingsPageViewModel(IBase @base) : base(@base)
        {
            Title = "SETTINGS";
        }
    }
}
