using ExhibitorModule.Core.Helpers;

namespace ExhibitorModule
{
    public class Configs : IClientConfig
    {
        public string BaseAddress => Secrets.BaseAddress;
        public string AuthKey => Secrets.AuthKey;
        public string UserToken => Secrets.UserToken;
    }
}
