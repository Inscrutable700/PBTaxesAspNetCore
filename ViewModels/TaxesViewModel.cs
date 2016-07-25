using System;

namespace PBTaxesAspNetCore.ViewModels
{
    /// <summary>
    /// The taxes view model.
    /// </summary>
    public class TaxesViewModel
    {
        /// <summary>
        /// Gets or sets the credits.
        /// </summary>
        public Statement[] Statements { get; set; }

        /// <summary>
        /// Gets or sets the full amount.
        /// </summary>
        public double FullAmount { get; set; }

        /// <summary>
        /// Gets or sets the date start.
        /// </summary>
        public DateTime StartDate { get; set; }

        /// <summary>
        /// Gets or sets the date end.
        /// </summary>
        public DateTime EndDate { get; set; }

        /// <summary>
        /// Gets or sets the taxes amount.
        /// </summary>
        public double TaxesAmount { get; set; }

        /// <summary>
        /// Gets or sets the interest rate.
        /// </summary>
        public double InterestRate { get; set; }

        /// <summary>
        /// The credit view model.
        /// </summary>
        public class Statement
        {
            /// <summary>
            /// Gets or sets the amount.
            /// </summary>
            public double Amount { get; set; }

            /// <summary>
            /// Gets or sets the amount in uah.
            /// </summary>
            public double AmountInUAH { get; set; }

            /// <summary>
            /// Gets or sets the description.
            /// </summary>
            public string Description { get; set; }

            /// <summary>
            /// Gets or sets a value indicating whether this instance is taxed.
            /// </summary>
            public bool IsTaxed { get; set; }

            /// <summary>
            /// Gets or sets the CCY.
            /// </summary>
            public string CCY { get; set; }

            /// <summary>
            /// Gets or sets the date.
            /// </summary>
            public DateTime Date { get; set; }

            /// <summary>
            /// Gets or sets the currency exchange.
            /// </summary>
            public double CurrencyExchange { get; set; }
        }
    }
}