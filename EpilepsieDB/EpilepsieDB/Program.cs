using EpilepsieDB.Data;
using EpilepsieDB.Source.Wrapper;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EpilepsieDB
{
    public class Program
    {
        public static void Main(string[] args)
        {
            IHost host = CreateHostBuilder(args).Build();

            CreateDbIfNotExists(host);

            host.Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                });

        private static void CreateDbIfNotExists(IHost host)
        {
            using (IServiceScope scope = host.Services.CreateScope())
            {
                IServiceProvider services = scope.ServiceProvider;
                try
                {
                    EpilepsieDBContext context = services.GetRequiredService<EpilepsieDBContext>();

                    // apply outstanding migrations to DB
                    if (context.Database.GetPendingMigrations().Any())
                        context.Database.Migrate();

                    // use code below to inject a user zero (systemadmin) into the database
                    // Password is set with the following:
                    // dotnet user-secrets init
                    // dotnet user-secrets set UserZeroPW <pw>

                    IConfiguration config = host.Services.GetRequiredService<IConfiguration>();
                    string userZeroPW = config["UserZeroPW"];
                    string userZero = config.GetValue<string>("UserZero");

                    Console.WriteLine(userZero);

                    SeedUser.SetSystemadmin(new ServiceProviderWrapper(services), userZero, userZeroPW).Wait();

                    // use code below to inject data into an empty database
                    //SeedData.Initialize(context).Wait();
                }
                catch (Exception ex)
                {
                    ILogger logger = services.GetRequiredService<ILogger<Program>>();
                    logger.LogError(ex, "An error occurred creating the DB.");
                }
            }
        }
    }
}
