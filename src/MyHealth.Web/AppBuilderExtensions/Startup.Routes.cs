using Microsoft.AspNetCore.Builder;

namespace MyHealth.Web.AppBuilderExtensions
{
    public static class RouteExtensions
    {
        public static IApplicationBuilder ConfigureRoutes(this IApplicationBuilder app)
        {
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
            });

            return app;
        }
    }
}
