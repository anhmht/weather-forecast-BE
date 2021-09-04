using System;
using System.Threading.Tasks;
using GloboWeather.WeatherManagement.Identity.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Serilog;
using Serilog.Events;

namespace GloboWeather.WeatherManagement.Api
{
    public class Program
    {
        public async static Task Main(string[] args)
        {
            var config = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json")
                .Build();

            var cloudConnectionString = config.GetSection("AzureStorageConfig:ConnectionString").Get<string>();

            var logFileName = "{yyyy}/{MM}/{dd}_log.txt";
#if DEBUG
            logFileName = "{yyyy}/{MM}/{dd}_dev_log.txt";
#endif
            Log.Logger = new LoggerConfiguration()
                .WriteTo.AzureBlobStorage(cloudConnectionString, LogEventLevel.Error,
                    "logs", logFileName)
                .CreateLogger();

            var host = CreateHostBuilder(args).Build();

            using (var scope = host.Services.CreateScope())
            {
                var services = scope.ServiceProvider;
                var loggerFactory = services.GetRequiredService<ILoggerFactory>();
                try
                {
                    var useManager = services.GetRequiredService<UserManager<ApplicationUser>>();
                    var roleManager = services.GetRequiredService<RoleManager<IdentityRole>>();

                    await Identity.Seed.RolesCreator.SeedAsync(roleManager);
                    await Identity.Seed.UserCreator.SeedAsync(useManager);

                    Log.Information("Application Starting");
                }
                catch (Exception e)
                {
                    Log.Error(e, "An error occured while starting the application");
                }
            }

            host.Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .UseSerilog()
                .ConfigureWebHostDefaults(webBuilder => { webBuilder.UseStartup<Startup>(); });
    }
}