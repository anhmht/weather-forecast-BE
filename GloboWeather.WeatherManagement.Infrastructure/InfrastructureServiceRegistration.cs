using GloboWeather.WeatherManagement.Application.Models.Mail;
using GloboWeather.WeatherManagement.Infrastructure.Mail;
using GloboWeather.WeatherManegement.Application.Contracts.Infrastructure;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace GloboWeather.WeatherManagement.Infrastructure
{
    public static class InfrastructureServiceRegistration
    {
        public static IServiceCollection AddInfrastructureServices(this IServiceCollection services,
            IConfiguration configuration)
        {
            services.Configure<EmailSettings>(configuration.GetSection("EmailSetting"));

            services.AddTransient<IEmailService, EmailService>();
            
            return services;
        }
    }
}