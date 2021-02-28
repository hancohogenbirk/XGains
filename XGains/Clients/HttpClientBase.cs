using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using XGains.Options;

namespace XGains.Clients
{
    public abstract class HttpClientBase<T> : HttpClient, IHttpClient
        where T : HttpClientOptionsBase, new()
    {
        protected HttpClientBase(IOptions<T> options)
        {
            BaseAddress = new Uri(options.Value.BaseUrl);
            
            foreach (var (key, value) in options.Value.DefaultHeaders)
                DefaultRequestHeaders.Add(key, value);
        }

        public virtual async Task<TResponse> Get<TResponse>(
            string endpoint,
            Func<HttpResponseMessage, Task<TResponse>> handleResultCallback)
        {
            var fullUri = new Uri(BaseAddress, endpoint);

            var response = await GetAsync(fullUri);
            return await handleResultCallback(response);
        }

        public virtual async Task<TResponse> PostUrlEncoded<TResponse>(
            string endpoint,
            Dictionary<string, string> urlParameters,
            Dictionary<string, string> requestHeaders = null)
        {
            var fullUri = new Uri(BaseAddress, endpoint);

            var stringParameters = string.Join(
                "&",
                urlParameters.Select(x => $"{x.Key}={x.Value}"));

            var content = new StringContent(
                stringParameters,
                Encoding.UTF8,
                "application/x-www-form-urlencoded");

            AddHeaders(requestHeaders, content);

            var response = await PostAsync(fullUri, content);

            return await HandleCallResult<TResponse>(response);
        }

        private static void AddHeaders(Dictionary<string, string> requestHeaders, HttpContent content)
        {
            if (requestHeaders != null && requestHeaders.Any())
                foreach (var (key, value) in requestHeaders)
                    content.Headers.Add(key, value);
        }

        public virtual async Task<TResponse> HandleCallResult<TResponse>(HttpResponseMessage response)
        {
            if (!response.IsSuccessStatusCode)
            {
                var reason = await response.Content.ReadAsStringAsync();
                throw new Exception($"Could not deserialize error response from called API. Reason: {reason}");
            }

            var content = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<TResponse>(content);
        }
    }

    

}
