﻿using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace ExhibitorModule.Services.Helpers
{
    public class XdsHttpClientHandler : DelegatingHandler
    {
        readonly IClientConfig _config;
        public XdsHttpClientHandler(IClientConfig config) : base (new HttpClientHandler())
        {
            _config = config;
        }

        protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            request.Headers.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));
            request.Headers.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", _config.UserToken);
            request.Headers.Add("x-functions-key", _config.AuthKey);
            return base.SendAsync(request, cancellationToken);
        }
    }
}
