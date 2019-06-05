using System;
using System.IO;
using System.Net.Http;
using System.Threading.Tasks;
using ExhibitorModule.Common;
using ExhibitorModule.Models;
using ExhibitorModule.Services.Abstractions;

namespace ExhibitorModule.Services
{
    public class ApiService : IApiService
    {
        readonly HttpClient _client;
        readonly INetworkService _networkService;
        readonly IEssentialsService _essentialsService;


        public ApiService(INetworkService networkService, IEssentialsService essentialsService)
        {
            _networkService = networkService;
            _essentialsService = essentialsService;

            _client = new HttpClient
            {
                BaseAddress = new Uri(AppConstants.Host)
            };
        }

        public Task<T> Get<T>(Uri uri) where T : HttpResponseMessage
        {
            return GetAndRetryInner<T>(uri);
        }

        public Task<string> GetString(Uri uri)
        {
            return _client.GetStringAsync(uri);
        }

        public async Task<string> GetString(string apiPath)
        {
            return await _client.GetStringAsync(apiPath);
        }

        public async Task<Stream> GetStream(string apiPath)
        {
            return await _client.GetStreamAsync(apiPath);
        }

        public async Task<byte[]> Get(string apiPath)
        {
            return await _client.GetByteArrayAsync(apiPath);
        }

        public Task<T> Post<T>(Uri uri, HttpContent content) where T : HttpResponseMessage
        {
            return PostAndRetryInner<T>(uri, content);
        }

        public Task<T> Post<T>(string apiPath, HttpContent content) where T : HttpResponseMessage
        {
            return PostAndRetryInner<T>(new Uri(apiPath, UriKind.Absolute), content);
        }

        #region Internal
        internal Task<T> GetAndRetryInner<T>(Uri uri, int retryCount = AppConstants.DefaultGetRetryCount, Func<Exception, int, Task> onRetry = null) where T : HttpResponseMessage
        {
            return _networkService.Retry<T>(new Func<Task<T>>(() => ProcessRequest<T>(uri, isGet: true)), retryCount, onRetry);
        }

        internal Task<byte[]> GetAndRetryInner(string apiPath, int retryCount = AppConstants.DefaultGetRetryCount, Func<Exception, int, Task> onRetry = null)
        {
            return _networkService.Retry<byte[]>(new Func<Task<byte[]>>(() => FetchGetBytes(apiPath)), retryCount, onRetry);
        }

        internal Task<T> PostAndRetryInner<T>(Uri uri, HttpContent content, string contentType = AppConstants.ContentTypeJson, int retryCount = 1, Func<Exception, int, Task> onRetry = null) where T : HttpResponseMessage
        {
            return _networkService.Retry<T>(new Func<Task<T>>(() => ProcessRequest<T>(uri, content)), retryCount, onRetry);
        }

        internal async Task<T> ProcessRequest<T>(Uri uri, HttpContent content = null, bool isGet = false) where T : HttpResponseMessage
        {
            if (!_essentialsService.IsNetworkAvailable())
                throw new OfflineException();

            var result = isGet ? await FetchGetResponse<T>(uri) : await FetchPostResponse<T>(uri, content);

            //await HandleErrors(result.Errors);

            return result;
        }

        async Task<T> FetchGetResponse<T>(Uri uri) where T : HttpResponseMessage
        {
            return (T)await _client.GetAsync(uri);
        }

        async Task<T> FetchPostResponse<T>(Uri uri, HttpContent content) where T : HttpResponseMessage
        {
            return (T)await _client.PostAsync(uri, content);
        }

        internal Task<byte[]> FetchGetBytes(string apiPath)
        {
            return _client.GetByteArrayAsync(apiPath);
        }
        #endregion
    }
}
