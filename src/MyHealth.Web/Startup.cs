using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using MyHealth.Web.AppBuilderExtensions;
using MyHealth.Model;
using Microsoft.AspNetCore.Identity;
using MyHealth.Data;
using MyHealth.API.Infrastructure;
using Microsoft.Extensions.Configuration;
using MyHealth.Data.Infraestructure;
using Microsoft.EntityFrameworkCore;

namespace MyHealth.Web
{
    public class Startup
    {
        public Startup(IWebHostEnvironment env)
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(env.ContentRootPath)
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true);

            if (env.IsDevelopment())
            {
                builder.AddUserSecrets<Startup>();
            }

            builder.AddEnvironmentVariables();
            Configuration = builder.Build();
        }

        public IConfiguration Configuration { get; set; }

        public void ConfigureServices(IServiceCollection services)
        {
            var connection = Configuration.GetConnectionString("DefaultConnection");
            services.AddDbContext<MyHealthContext>(options => options.UseSqlServer(connection));

            services.ConfigureDependencies();

            services.AddIdentity<ApplicationUser, IdentityRole>()
                .AddEntityFrameworkStores<MyHealthContext>()
                .AddDefaultTokenProviders();

            services.AddApplicationInsightsTelemetry(Configuration);

            services.AddControllersWithViews();

            services.AddMemoryCache();

            services.AddSession();

            services.AddAuthorization(Policies.Configuration);
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env,
            MyHealthDataInitializer dataInitializer)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
            }

            app.UseStaticFiles();

            app.UseRouting();

            app.UseSession();

            app.ConfigureSecurity();

            app.UseAuthorization();

            app.ConfigureRoutes();

            dataInitializer.InitializeDatabaseAsync(app.ApplicationServices).GetAwaiter().GetResult();
        }
    }
}
