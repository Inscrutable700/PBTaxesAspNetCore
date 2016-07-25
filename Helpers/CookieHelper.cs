using Microsoft.AspNetCore.Http;

namespace PBTaxesAspNetCore.Helpers
{
    public static class CookieHelper
    {
        private static IHttpContextAccessor HttpContextAccessor;
        public static void Configure(IHttpContextAccessor httpContextAccessor)
        {
            HttpContextAccessor = httpContextAccessor;
        }

        /// <summary>
        /// The pb session identifier cookie name.
        /// </summary>
        private const string PBSessionIDCookieName = "pbSessionID";

        /// <summary>
        /// Gets or sets the pb session identifier.
        /// </summary>
        public static string PBSessionID
        {
            get
            {
                
                return HttpContextAccessor.HttpContext.Request.Cookies[PBSessionIDCookieName];
            }
            set
            {
                HttpContext context = HttpContextAccessor.HttpContext;
                context.Response.Cookies.Delete(PBSessionIDCookieName);
                context.Response.Cookies.Append(PBSessionIDCookieName, value);
            }
        }
    }
}