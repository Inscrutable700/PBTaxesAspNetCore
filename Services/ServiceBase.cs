using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Flurl.Http;
using Newtonsoft.Json;

namespace TaxesPrivatBank.Business.Services
{
    /// <summary>
    /// The service base.
    /// </summary>
    public class ServiceBase
    {
        /// <summary>
        /// The service URL.
        /// </summary>
        private string serviceUrl;

        /// <summary>
        /// Initializes a new instance of the <see cref="ServiceBase"/> class.
        /// </summary>
        /// <param name="serviceUrl">The service URL.</param>
        public ServiceBase(string serviceUrl)
        {
            this.serviceUrl = serviceUrl;
        }
        
        /// <summary>
        /// Gets the post response.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="uri">The URI.</param>
        /// <param name="apiEndpoint">The API endpoint.</param>
        /// <param name="parameters">The parameters.</param>
        /// <returns></returns>
        protected async Task<T> GetPOSTResponseAsync<T>(string apiEndpoint, Dictionary<string, string> parameters)
            where T : new()
        {
            var client = new FlurlClient($"{this.serviceUrl}{apiEndpoint}");
            client.WithHeader("Accept", "application/json");
            string result = await client.PostJsonAsync(parameters).ReceiveString();
            return JsonConvert.DeserializeObject<T>(result);
        }

        /// <summary>
        /// Gets the post response.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="uri">The URI.</param>
        /// <param name="apiEndpoint">The API endpoint.</param>
        /// <param name="parameters">The parameters.</param>
        /// <returns></returns>
        protected Task<string> GetPOSTResponseStringAsync(string apiEndpoint, Dictionary<string, string> parameters)
        {
            var client = new FlurlClient($"{this.serviceUrl}{apiEndpoint}");
            client.WithHeader("Accept", "application/json");
            return client.PostJsonAsync(parameters).ReceiveString();
        }

        /// <summary>
        /// Gets the get response.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="apiEndpoint">The API endpoint.</param>
        /// <param name="parameters">The parameters.</param>
        /// <param name="authorizationHeader">The authorization header.</param>
        /// <returns></returns>
        protected async Task<T> GetGETResponseAsync<T>(string apiEndpoint, Dictionary<string, string> parameters, string authorizationHeader = null)
            where T : new()
        {
            var url = new Flurl.Url($"{this.serviceUrl}{apiEndpoint}").SetQueryParams(parameters);
            var client = new FlurlClient(url);
            // client.WithHeader("Content-Type", "application/json");
            client.WithHeader("Accept", "application/json");

            if(!string.IsNullOrEmpty(authorizationHeader))
            {
                client.WithHeader("Authorization", authorizationHeader);
            }

            string result = await client.GetAsync().ReceiveString();
            return JsonConvert.DeserializeObject<T>(result);
        }

        protected Task<string> GetGETResponseStringAsync(string apiEndpoint, Dictionary<string, string> parameters, string authorizationHeader = null)
        {
            var url = new Flurl.Url($"{this.serviceUrl}{apiEndpoint}").SetQueryParams(parameters);
            var client = new FlurlClient(url);
            // client.WithHeader("Content-Type", "application/json");
            client.WithHeader("Accept", "application/json");

            if(!string.IsNullOrEmpty(authorizationHeader))
            {
                client.WithHeader("Authorization", authorizationHeader);
            }

            return client.GetAsync().ReceiveString();
        }
    }
}