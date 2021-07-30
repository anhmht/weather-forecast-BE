using GloboWeather.WeatherManagement.Application.Models.Astronomy;
using GloboWeather.WeatherManagement.Application.Models.Mail;
using GloboWeather.WeatherManagement.Application.Models.Media;
using GloboWeather.WeatherManagement.Application.Models.PositionStack;
using GloboWeather.WeatherManagement.Application.Models.Storage;
using GloboWeather.WeatherManagement.Infrastructure.Astronomy;
using GloboWeather.WeatherManagement.Infrastructure.Mail;
using GloboWeather.WeatherManagement.Infrastructure.Media;
using GloboWeather.WeatherManegement.Application.Contracts.Astronomy;
using GloboWeather.WeatherManegement.Application.Contracts.Infrastructure;
using GloboWeather.WeatherManegement.Application.Contracts.Media;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace GloboWeather.WeatherManagement.Infrastructure
{
    public static class InfrastructureServiceRegistration
    {
        public static IServiceCollection AddInfrastructureServices(this IServiceCollection services,
            IConfiguration configuration)
        {
            services.Configure<EmailSettings>(configuration.GetSection("EmailSettings"));
            services.Configure<AzureStorageConfig>(configuration.GetSection(key: "AzureStorageConfig"));
            services.Configure<AstronomySettings>(configuration.GetSection("AstronomySettings"));
            services.Configure<PositionStackSettings>(configuration.GetSection("PositionStackSettings"));
            services.Configure<GmailSettings>(configuration.GetSection("GmailSettings"));
            services.Configure<MediaVideoSettings>(configuration.GetSection("MediaVideoSettings"));

            services.AddHttpClient<LocationService>();

            services.AddTransient<IImageService, ImageService>();
            services.AddTransient<IVideoService, VideoService>();
            services.AddScoped<ILocationService, LocationService>();
            RegistSendMailProvider(services, configuration);

            return services;
        }

        public static void RegistSendMailProvider(IServiceCollection services,
            IConfiguration configuration)
        {
            var sendMailProvider = configuration.GetValue<string>("SendMailProvider");
            switch (sendMailProvider)
            {
                case "gmail":
                    services.AddTransient<IEmailService, GmailService>();
                    break;
                default:
                    services.AddTransient<IEmailService, EmailService>();
                    break;
            }
        }
    }
}