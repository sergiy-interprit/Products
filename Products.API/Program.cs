using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using NLog.Web;
using Products.Data;
using System;

namespace Products.API
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var logger = NLogBuilder.ConfigureNLog("nlog.config").GetCurrentClassLogger();
            try
            {
                logger.Info("Initializing application...");

                var host = CreateHostBuilder(args).Build();

                using(var scope = host.Services.CreateScope())
                {               
                    try
                    {
                        // Get context for database bootstrapping
                        var context = scope.ServiceProvider.GetService<ProductsDbContext>();

                        // NOTE: EnsureDeleted() is for demo purposes only!!!
                        // Delete the database & migrate on startup so we can start with a clean slate
                        context.Database.EnsureDeleted();

                        // Create database and apply all migrations
                        context.Database.Migrate();    
                    }
                    catch (Exception ex)
                    {
                        logger.Error(ex, "An error ocured while migrating the database.");
                    }
                }

                // run the web app
                host.Run();
            }
            catch (Exception ex)
            {
                logger.Error(ex, "Application stopped because of exception.");
            }
            finally
            {
                NLog.LogManager.Shutdown();
            }
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>()
                    .UseNLog(); // Added to use NLog as logging provider
                });
    }
}
