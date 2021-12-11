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
using AquaG.TasksMVC.Data;

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
            services.AddDbContext<TasksDbContext>(options => options.UseSqlServer(connection));


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


            services.AddHttpContextAccessor();

            services.AddControllersWithViews();
            services.AddControllers();

            services.AddDbContext<AquaGTasksMVCContext>(options =>
                    options.UseSqlServer(Configuration.GetConnectionString("AquaGTasksMVCContext")));
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
                    context.Request.Path = $"~/Home/Error/{statusCode}";
                    await next();
                }

            });

            if (env.IsDevelopment())
                app.UseDeveloperExceptionPage();
            else
                app.UseExceptionHandler("~/Home/Error");

            app.UseRewriter(new RewriteOptions().AddRedirect("(.*)/$", "$1"));

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();


            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}
