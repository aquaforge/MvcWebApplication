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
            string connection = Configuration.GetConnectionString("DefaultConnection");
            services.AddDbContext<TasksDbContext>(options => options.UseSqlServer(connection), ServiceLifetime.Transient);


            //services.ConfigureApplicationCookie(options =>
            //{
            //    //options.Cookie.Name = "CookieName";
            //    options.ExpireTimeSpan = TimeSpan.FromDays(30);
            //    options.SlidingExpiration = true;
            //});

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

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {

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

            if (env.IsDevelopment())
                app.UseDeveloperExceptionPage();
            else
                app.UseExceptionHandler("/Home/Error");

            app.UseRewriter(new RewriteOptions().AddRedirect("(.*)/$", "$1"));

            app.UseHttpsRedirection();

            //var supportedCultures = new[]
            //{
            //    new CultureInfo("ru-RU"),
            //    new CultureInfo("ru"),
            //    new CultureInfo("en-US"),
            //    new CultureInfo("en-GB"),
            //    new CultureInfo("en"),
            //};
            //app.UseRequestLocalization(new RequestLocalizationOptions
            //{
            //    DefaultRequestCulture = new RequestCulture("ru-RU"),
            //    SupportedCultures = supportedCultures,
            //    SupportedUICultures = supportedCultures
            //});

            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            //app.UseSession();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}
