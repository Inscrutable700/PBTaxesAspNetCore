namespace PBTaxesAspNetCore.Helpers
{
    /// <summary>
    /// The configuration helper.
    /// </summary>
    public static class ConfigurationHelper
    {
        /// <summary>
        /// Gets the Privat Bank client identifier.
        /// </summary>
        public static string PBClientID
        {
            get
            {
                return "0042819a-0e52-4b12-b616-972a9f8b8257"; //ConfigurationManager.AppSettings["PBClientID"];
            }
        }

        /// <summary>
        /// Gets the pb client secret.
        /// </summary>
        public static string PBClientSecret
        {
            get
            {
                return "6bba30e42b1d9a5032e3956b9ad41f65"; //ConfigurationManager.AppSettings["PBClientSecret"];
            }
        }
    }
}
