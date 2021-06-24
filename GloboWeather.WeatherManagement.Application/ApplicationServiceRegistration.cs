using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using AutoMapper;
using GloboWeather.WeatherManagement.Application.Caches;
using MediatR;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace GloboWeather.WeatherManagement.Application
{
    public static class ApplicationServiceRegistration
    {
        public static IServiceCollection AddApplicationServices(this IServiceCollection services,
            IConfiguration configuration)
        {
            services.AddAutoMapper(Assembly.GetExecutingAssembly());
            services.AddMediatR(Assembly.GetExecutingAssembly());
            services.AddMemoryCache();

            var children = configuration.GetSection("Caching").GetChildren();
            Dictionary<string, TimeSpan> cacheConfiguration =
                children.ToDictionary(child => child.Key, child => TimeSpan.Parse(child.Value));

            services.AddSingleton<ICacheStore>(x => new MemoryCacheStore(x.GetService<IMemoryCache>(), cacheConfiguration));

            return services;
        }
    }
}