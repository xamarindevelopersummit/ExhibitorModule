using ExhibitorModule.Core.Helpers;

namespace ExhibitorModule
{
    // TODO: temporary
    public class Configs : IClientConfig
    {
        public string BaseAddress => Secrets.BaseAddress;
        public string AuthKey => Secrets.AuthKey;
        public string UserToken => Secrets.UserToken;
    }
}
