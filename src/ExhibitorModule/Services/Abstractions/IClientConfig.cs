namespace ExhibitorModule
{
    public interface IClientConfig
    {
        /// <summary>
        /// Base address for the HttpClient
        /// </summary>
        string BaseAddress { get; }
        /// <summary>
        /// Auth key (code) for Azure
        /// </summary>
        string AuthKey { get; }
        /// <summary>
        /// User authentication token
        /// </summary>
        string UserToken { get; }
    }
}
