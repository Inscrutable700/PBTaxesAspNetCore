namespace PBTaxesAspNetCore.Dto
{
    /// <summary>
    /// The exchange rate DTO.
    /// </summary>
    public class ExchangeRateDto
    {
        /// <summary>
        /// Gets or sets the base currency.
        /// </summary>
        public string BaseCurrency { get; set; }

        /// <summary>
        /// Gets or sets the currency.
        /// </summary>
        public string Currency { get; set; }

        /// <summary>
        /// Gets or sets the sale rate nb.
        /// </summary>
        public double SaleRateNB { get; set; }

        /// <summary>
        /// Gets or sets the purchase rate nb.
        /// </summary>
        public double PurchaseRateNB { get; set; }
    }
}
