using ExhibitorModule.Core.Helpers;

namespace ExhibitorModule
{
    public class Configs : IClientConfig, IModuleConfig
    {
        public string BaseAddress => Secrets.BaseAddress;
        public string AuthKey => Secrets.AuthKey;
        public string UserToken => Secrets.UserToken;
        public int SearchResultsLimit => Secrets.SearchResultsLimit;
    }
}
