using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PBTaxesAspNetCore.Dto;

namespace TaxesPrivatBank.Business.Services
{
    /// <summary>
    /// The privat 24 service.
    /// </summary>
    public class Privat24Service : ServiceBase
    {
        /// <summary>
        /// The privat bank API URL.
        /// </summary>
        private const string ApiUrl = "https://api.privatbank.ua/";

        /// <summary>
        /// Initializes a new instance of the <see cref="Privat24Service"/> class.
        /// </summary>
        public Privat24Service()
            : base(ApiUrl)
        {
        }
        
        /// <summary>
        /// Gets the exchange rate usd.
        /// </summary>
        /// <param name="date">The date.</param>
        /// <returns></returns>
        public async Task<ExchangeRateDto> GetExchangeRate(string currency, DateTime date)
        {
            string apiEndPoint = "p24api/exchange_rates";
            Dictionary<string, string> parameters = new Dictionary<string, string>
            {
                { "date", date.ToString("dd.MM.yyyy") },
                { "json", null }
            };

            var exchangeRate = await this.GetGETResponse<ExchangeRatesDto>(apiEndPoint, parameters);

            return exchangeRate.ExchangeRates
                .SingleOrDefault(er => er.Currency.Equals(currency, StringComparison.OrdinalIgnoreCase));
        }
    }
}
