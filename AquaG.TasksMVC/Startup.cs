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
            GeneralFileLogger.Log($"StartUp.ConfigureServices: start");

            try
            {

                services.AddDbContextFactory<TasksDbContext>(
                    options => {
                        options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection"));
                        }
                    );
                //services.AddDbContext<TasksDbContext>(
                //    options => options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection"))
                //    );

                GeneralFileLogger.Log($"StartUp.ConfigureServices: 1");

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

                GeneralFileLogger.Log($"StartUp.ConfigureServices: 2");

                services.AddDistributedMemoryCache();
                services.AddSession();

                services.AddHttpContextAccessor();

                GeneralFileLogger.Log($"StartUp.ConfigureServices: 3");


                services.AddControllersWithViews();
                services.AddControllers();

                GeneralFileLogger.Log($"StartUp.ConfigureServices: end");
            }
            catch (Exception ex)
            {
                GeneralFileLogger.Log($"StartUp.ConfigureServices: {ex.Message}");
            }
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, ILoggerFactory loggerFactory)
        {
            GeneralFileLogger.Log($"StartUp.Configure: start");

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
                GeneralFileLogger.Log($"StartUp.Configure: 1");


                app.UseRewriter(new RewriteOptions().AddRedirect("(.*)/$", "$1"));

                #region Localization
                var supportedCultures = new[]
                {
                new CultureInfo("ru-RU"),
                new CultureInfo("ru"),
                new CultureInfo("en-US"),
                new CultureInfo("en-GB"),
                new CultureInfo("en")};

                var localizationOptions = new RequestLocalizationOptions
                {
                    DefaultRequestCulture = new RequestCulture("ru-RU"),
                    SupportedCultures = supportedCultures,
                    SupportedUICultures = supportedCultures
                };
                localizationOptions.RequestCultureProviders.Insert(0, new CustomRequestCultureProvider(context =>
                {
                    try
                    {
                        string lang = context.Request.Headers["Accept-Language"].ToString().Split(',')[0];
                        foreach (var culture in supportedCultures)
                        {
                            if (culture.Name.Equals(lang, StringComparison.OrdinalIgnoreCase))
                                return Task.FromResult(new ProviderCultureResult(lang));
                        }
                    }
                    catch (Exception ex)
                    {
                        GeneralFileLogger.Log($"StartUp.ConfigureServices.CustomRequestCultureProvider: {ex.Message}");
                    }
                    return Task.FromResult(new ProviderCultureResult("ru"));
                }));
                app.UseRequestLocalization(localizationOptions);
                #endregion //Localization

                GeneralFileLogger.Log($"StartUp.Configure: 2");

                app.UseStaticFiles();

                app.UseRouting();

                app.UseAuthentication();
                app.UseAuthorization();

                GeneralFileLogger.Log($"StartUp.Configure: 3");

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
                GeneralFileLogger.Log($"StartUp.Configure: end");
            }
            catch (Exception ex)
            {
                GeneralFileLogger.Log($"StartUp.Configure: {ex.Message}");
            }
        }
    }
}
