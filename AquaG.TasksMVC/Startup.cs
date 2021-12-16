using AquaG.TasksMVC.Models;
using AquaG.TasksDbModel;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Rewrite;
using System.Globalization;
using Microsoft.AspNetCore.Localization;
using AquaG.TasksMVC.Middleware;


namespace AquaG.TasksMVC
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.

        public void ConfigureServices(IServiceCollection services)
        {
            try
            {
                services.AddDbContextFactory<TasksDbContext>(
                    options => options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection"))
                    );
                //services.AddDbContext<TasksDbContext>(
                //    options => options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection"))
                //    );

                services.AddIdentity<User, IdentityRole>(opts =>
               {
                   opts.User.RequireUniqueEmail = true;
                   opts.Password.RequiredLength = 3;
                   opts.Password.RequireNonAlphanumeric = false;
                   opts.Password.RequireLowercase = false;
                   opts.Password.RequireUppercase = false;
                   opts.Password.RequireDigit = false;
                   opts.SignIn.RequireConfirmedAccount = false;
               })
                    .AddEntityFrameworkStores<TasksDbContext>();

                services.AddDistributedMemoryCache();
                services.AddSession();

                services.AddHttpContextAccessor();

                services.AddControllersWithViews();
                services.AddControllers();

            }
            catch (Exception ex)
            {
                GeneralFileLogger.Log($"StartUp.ConfigureServices: {ex.Message}");
            }
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, ILoggerFactory loggerFactory)
        {
            try
            {
                loggerFactory.AddFile("");

                if (env.IsDevelopment())
                    app.UseDeveloperExceptionPage();
                else
                {

                    app.UseHsts();
                    app.UseHttpsRedirection();
                    app.UseExceptionHandler("/Home/Error");
                }

                app.UseRewriter(new RewriteOptions().AddRedirect("(.*)/$", "$1"));

                #region Localization
                var supportedCultures = new[]
                {
                new CultureInfo("ru-RU"),
                new CultureInfo("ru"),
                new CultureInfo("en-US"),
                new CultureInfo("en-GB"),
                new CultureInfo("en")};

                app.UseRequestLocalization(new RequestLocalizationOptions
                {
                    DefaultRequestCulture = new RequestCulture("ru-RU"),
                    SupportedCultures = supportedCultures,
                    SupportedUICultures = supportedCultures
                });
                #endregion //Localization

                app.UseStaticFiles();

                app.UseRouting();

                app.UseAuthentication();
                app.UseAuthorization();

                app.Use(async (context, next) =>
                {
                    await next();

                    int statusCode = context.Response.StatusCode;
                    if (!context.Response.HasStarted && (statusCode == 400 || statusCode == 401 || statusCode == 404))
                    {
                        context.Items["originalPath"] = context.Request.Path.Value;
                        context.Request.Path = $"/Home/Error/{statusCode}";
                        await next();
                    }

                });

                app.UseEndpoints(endpoints =>
                {
                    endpoints.MapControllerRoute(
                        name: "default",
                        pattern: "{controller=Home}/{action=Index}/{id?}"); //.RequireHost("localhost:5001", "sub.domain.com");

                });
            }
            catch (Exception ex)
            {
                GeneralFileLogger.Log($"StartUp.Configure: {ex.Message}");
            }
        }
    }
}
