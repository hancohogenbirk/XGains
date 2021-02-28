using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using XGains.Options;
using XGains.Providers;

namespace XGains.Clients
{
    public class KrakenClient : HttpClientBase<KrakenClientOptions>, IKrakenClient
    {
        private readonly IOptions<KrakenClientOptions> _options;
        private readonly ISignatureProvider _signatureProvider;

        public KrakenClient(
            IOptions<KrakenClientOptions> options,
            ISignatureProvider signatureProvider)
            : base(options)
        {
            _options = options;
            _signatureProvider = signatureProvider;
        }

        public async Task<TResponse> PostUrlEncoded<TResponse>(
            string endpoint,
            Dictionary<string, string> urlParameters)
        {
            if (urlParameters == null)
                urlParameters = new Dictionary<string, string>();

            var apiKey = _options.Value.ApiKey;
            var signature = _signatureProvider.SignRequest(
                endpoint,
                ref urlParameters,
                _options.Value.ApiSecret);

            var headers = new Dictionary<string, string>
            {
                { "API-Key", apiKey },
                { "API-Sign", signature }
            };

            return await base.PostUrlEncoded<TResponse>(endpoint, urlParameters, headers);
        }
    }
}
