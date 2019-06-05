using System;
using System.Threading.Tasks;
using ExhibitorModule.Services.Abstractions;
using Xamarin.Essentials;

namespace ExhibitorModule.Services
{
    public class EssentialsService : IEssentialsService
    {
        public string DevicePlatform => DeviceInfo.Platform.ToString();

        public object GetPreference(string key, object value)
        {
            if (value is String)
            {
                return Preferences.Get(key, value.ToString());
            }

            if (value is Int32 || value is Int64)
            {
                return Preferences.Get(key, int.Parse(value.ToString()));
            }

            if (value is Double)
            {
                return Preferences.Get(key, double.Parse(value.ToString()));
            }

            if (value is Double)
            {
                return Preferences.Get(key, double.Parse(value.ToString()));
            }

            if (value is Single)
            {
                return Preferences.Get(key, Single.Parse(value.ToString()));
            }

            if (value is Boolean)
            {
                return Preferences.Get(key, bool.Parse(value.ToString()));
            }
            return value;
        }

        public Task<bool> OpenBrowser(string link)
        {
            return Browser.OpenAsync(new Uri(link), new BrowserLaunchOptions { 
                LaunchMode = BrowserLaunchMode.SystemPreferred, 
                TitleMode = BrowserTitleMode.Default
            });
        }

        public bool PreferenceExists(string key)
        {
            return Preferences.ContainsKey(key);
        }

        public void SavePreference(string key, object value)
        {
            if (value is String)
            {
                Preferences.Set(key, value.ToString());
            }

            if (value is Int32 || value is Int64)
            {
                Preferences.Set(key, int.Parse(value.ToString()));
            }

            if (value is Double)
            {
                Preferences.Set(key, double.Parse(value.ToString()));
            }

            if (value is Double)
            {
                Preferences.Set(key, double.Parse(value.ToString()));
            }

            if (value is Single)
            {
                Preferences.Set(key, Single.Parse(value.ToString()));
            }

            if (value is Boolean)
            {
                Preferences.Set(key, bool.Parse(value.ToString()));
            }
        }

        public Task ShareUri(string uri, string title)
        {
            return Share.RequestAsync(new ShareTextRequest(uri, title));
        }

        public bool IsNetworkAvailable()
        {
            return Connectivity.NetworkAccess != NetworkAccess.None &&
                Connectivity.NetworkAccess != NetworkAccess.Unknown &&
                Connectivity.NetworkAccess != NetworkAccess.Local;
        }
    }
}
