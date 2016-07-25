using System.Collections.Generic;

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
        //[DeserializeAs(Name = "exchangeRate")]
        public List<ExchangeRateDto> ExchangeRates { get; set; }
    }
}
