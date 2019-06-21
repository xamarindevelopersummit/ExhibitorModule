using System.Net.Http;
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
            request.Headers.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("bearer", _config.UserToken);
            request.Headers.Add("x-functions-key", _config.AuthKey);
            return base.SendAsync(request, cancellationToken);
        }
    }
}
