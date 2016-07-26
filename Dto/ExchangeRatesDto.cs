using System.Collections.Generic;
using Newtonsoft.Json;

namespace PBTaxesAspNetCore.Dto
{
    /// <summary>
    /// The exchange rate DTO.
    /// </summary>
    public class ExchangeRatesDto
    {
        /// <summary>
        /// Gets or sets the exchange rates.
        /// </summary>
        [JsonProperty("exchangeRate")]
        public List<ExchangeRateDto> ExchangeRates { get; set; }
    }
}
