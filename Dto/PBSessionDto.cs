namespace PBTaxesAspNetCore.Dto
{
    /// <summary>
    /// The session ID DTO.
    /// </summary>
    public class PBSessionDto
    {
        /// <summary>
        /// Gets or sets the identifier.
        /// </summary>
        public string ID { get; set; }

        /// <summary>
        /// Gets or sets the client identifier.
        /// </summary>
        public string ClientId { get; set; }

        /// <summary>
        /// Gets or sets the expires in.
        /// </summary>
        public double @ExpiresIn { get; set; }
    }
}
