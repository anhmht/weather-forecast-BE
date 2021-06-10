using System.Reflection;
using AutoMapper;
using GloboWeather.WeatherManagement.Application.Features.WeatherInformations.Commands.CreateWeatherInformation;
using MediatR;
using Microsoft.Extensions.DependencyInjection;

namespace GloboWeather.WeatherManagement.Application
{
    public static class ApplicationServiceRegistration
    {
        public static IServiceCollection AddApplicationServices(this IServiceCollection services)
        {
            services.AddAutoMapper(Assembly.GetExecutingAssembly());
            services.AddMediatR(Assembly.GetExecutingAssembly());

            return services;
        }
    }
}