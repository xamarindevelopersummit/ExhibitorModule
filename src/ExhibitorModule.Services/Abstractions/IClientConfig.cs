namespace ExhibitorModule
{
    public interface IClientConfig
    {
        /// <summary>
        /// Base address for the HttpClient
        /// </summary>
        string BaseAddress { get; set; }
        /// <summary>
        /// Auth key (code) for Azure
        /// </summary>
        string AuthKey { get; set; }
        /// <summary>
        /// User authentication token
        /// </summary>
        string UserToken { get; set; }
    }
}
