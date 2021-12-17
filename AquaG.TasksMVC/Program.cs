using AquaG.TasksDbModel;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AquaG.TasksMVC.Middleware;

namespace AquaG.TasksMVC
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            GeneralFileLogger.Log($"Program.Main: start");
            try
            {
                var host = CreateHostBuilder(args).Build();
                using (var scope = host.Services.CreateScope())
                {
                    var services = scope.ServiceProvider;
                    try
                    {
                        var userManager = services.GetRequiredService<UserManager<User>>();
                        var rolesManager = services.GetRequiredService<RoleManager<IdentityRole>>();
                        await DbInitializer.InitializeAsync(userManager, rolesManager);
                        GeneralFileLogger.Log($"Program.Main: end");

                    }
                    catch (Exception ex)
                    {
                        GeneralFileLogger.Log($"Program.Main: {ex.Message}");

                        var logger = services.GetRequiredService<ILogger<Program>>();
                        logger.LogError(ex, "Ошибка инициализации базы данных");
                    }
                }

                host.Run();
            }
            catch (Exception ex)
            {
                GeneralFileLogger.Log($"Program.Main1: {ex.Message}");
            }
        }
        public static IHostBuilder CreateHostBuilder(string[] args)
        {
            GeneralFileLogger.Log($"Program.CreateHostBuilder: start");
            try
            {
                return Host.CreateDefaultBuilder(args)
                    .ConfigureWebHostDefaults(webBuilder =>
                    {
                        webBuilder.UseStartup<Startup>();
                    });

            }
            catch (Exception ex)
            {
                GeneralFileLogger.Log($"Program.CreateHostBuilder: {ex.Message}");
                throw new Exception("Program.CreateHostBuilder" ,ex);
                return null;
            }
        }


    }
}
