using Microsoft.AspNetCore.Builder;

namespace MyHealth.Web.AppBuilderExtensions
{
    public static class SecurityExtensions
    {
        public static IApplicationBuilder ConfigureSecurity(this IApplicationBuilder app)
        {
            return app.UseAuthentication();
        }
    }
}
