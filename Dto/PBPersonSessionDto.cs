using System.Collections.Generic;

namespace PBTaxesAspNetCore.Dto
{
    /// <summary>
    /// The person session DTO.
    /// </summary>
    public class PBPersonSessionDto
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
        public double ExpiresIn { get; set; }

        /// <summary>
        /// Gets or sets the message.
        /// </summary>
        public string Message { get; set; }

        /// <summary>
        /// Gets or sets the roles.
        /// </summary>
        public List<string> Roles { get; set; }
    }
}
