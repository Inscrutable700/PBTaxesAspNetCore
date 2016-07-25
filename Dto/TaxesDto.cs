using System;
using System.Collections.Generic;

namespace PBTaxesAspNetCore.Dto
{
    /// <summary>
    /// The taxes DTO.
    /// </summary>
    public class TaxesDto
    {
        /// <summary>
        /// Gets or sets the statements.
        /// </summary>
        public List<PBStatementItemDto> Statements { get; set; }

        /// <summary>
        /// Gets or sets the full amount.
        /// </summary>
        public double FullAmount { get; set; }

        /// <summary>
        /// Gets or sets the start date.
        /// </summary>
        public DateTime StartDate { get; set; }

        /// <summary>
        /// Gets or sets the end date.
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
    }
}
