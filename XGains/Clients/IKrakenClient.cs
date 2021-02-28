using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace XGains.Clients
{
    public interface IKrakenClient : IHttpClient
    {
        Task<TResponse> PostUrlEncoded<TResponse>(
            string endpoint,
            Dictionary<string, string> urlParameters);
    }
}
