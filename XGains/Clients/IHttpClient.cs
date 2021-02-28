using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace XGains.Clients
{
    public interface IHttpClient
    {
        Task<TResponse> Get<TResponse>(
            string endpoint,
            Func<HttpResponseMessage, Task<TResponse>> handleResultCallback);

        Task<TResponse> HandleCallResult<TResponse>(HttpResponseMessage result);
    }
}
