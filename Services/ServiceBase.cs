using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Flurl.Http;

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
        protected T GetPOSTResponse<T>(string apiEndpoint, Dictionary<string, string> parameters)
            where T : new()
        {
            var client = new FlurlClient($"{this.serviceUrl}{apiEndpoint}");
            client.WithHeader("Accept", "application/json");
            var response = client
                .PostJsonAsync(parameters).ReceiveJson<T>();
            response.Wait();
            return response.Result;
        }

        /// <summary>
        /// Gets the get response.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="apiEndpoint">The API endpoint.</param>
        /// <param name="parameters">The parameters.</param>
        /// <param name="authorizationHeader">The authorization header.</param>
        /// <returns></returns>
        protected T GetGETResponse<T>(string apiEndpoint, Dictionary<string, string> parameters, string authorizationHeader = null)
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

            var task = client.GetAsync().ReceiveString();
            task.Wait();
            T res = Newtonsoft.Json.JsonConvert.DeserializeObject<T>(task.Result);
            return res;
        }
    }
}
