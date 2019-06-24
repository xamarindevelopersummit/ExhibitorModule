using System;
using System.IO;
using System.Net.Http;
using System.Threading.Tasks;

namespace ExhibitorModule.Services.Abstractions
{
    public interface IApiService
    {
        /// <summary>
        /// Processes <c>Get</c> request and retries before throwing exception.
        /// </summary>
        /// <returns>TResult</returns>
        /// <param name="uri">URI</param>
        /// <typeparam name="T">TResult</typeparam>
        Task<T> Get<T>(Uri uri) where T : HttpResponseMessage;

        /// <summary>
        /// Processes <c>Get</c> request and retries before throwing exception.
        /// </summary>
        /// <returns>byte[]</returns>
        /// <param name="apiPath">API path.</param>
        Task<byte[]> Get(string apiPath);

        Task<string> GetString(string apiPath);
        Task<string> GetString(Uri uri);
        Task<Stream> GetStream(string apiPath);

        /// <summary>
        /// API POST. Uses Host in Settings. Serializes into JSON.
        /// </summary>
        /// <returns>TResult</returns>
        /// <param name="uri">Relative path to API. Uses host stored in settings.</param>
        /// <param name="content">Content to post to API. Will be sent as JSON.</param>
        /// <typeparam name="T">TResult</typeparam>
        Task<T> Post<T>(Uri uri, HttpContent content) where T : HttpResponseMessage;

        /// <summary>
        /// API POST. Uses Host in Settings. Serializes into JSON.
        /// </summary>
        /// <returns>TResult</returns>
        /// <param name="apiPath">API path.</param>
        /// <param name="content">Content to post to API. Will be sent as JSON.</param>
        /// <typeparam name="T">TResult</typeparam>
        Task<T> Post<T>(string apiPath, HttpContent content) where T : HttpResponseMessage;
    }
}
