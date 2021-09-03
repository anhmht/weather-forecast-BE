using System;
using System.Collections.Generic;
using GloboWeather.WeatherManagement.Api.Context;
using GloboWeather.WeatherManagement.Api.Middleware;
using GloboWeather.WeatherManagement.Api.Services;
using GloboWeather.WeatherManagement.Application;
using GloboWeather.WeatherManagement.Application.SignalR;
using GloboWeather.WeatherManagement.Identity;
using GloboWeather.WeatherManagement.Infrastructure;
using GloboWeather.WeatherManagement.Monitoring;
using GloboWeather.WeatherManagement.Persistence;
using GloboWeather.WeatherManagement.Weather;
using GloboWeather.WeatherManegement.Application.Contracts;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.AspNetCore.Server.Kestrel.Core;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using Serilog;
using WeatherBackgroundService;

namespace GloboWeather.WeatherManagement.Api
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
            AddSwagger(services);
            services.AddApplicationServices(Configuration);
            services.AddInfrastructureServices(Configuration);
            services.AddPersistenceServices(Configuration);
            services.AddIdentityServices(Configuration);
            services.AddWeatherService(Configuration);
            services.AddMonitoringService(Configuration);
            services.AddWeatherBackgroundService(Configuration);
            services.AddScoped<ILoggedInUserService, LoggedInUserService>();
            services.AddControllers();
            services.AddScoped<WeatherContext>();
            services.AddCors(options =>
            {
                options.AddDefaultPolicy(builder =>
                    builder.SetIsOriginAllowed(_ => true)
                    .AllowAnyMethod()
                    .AllowAnyHeader()
                    .AllowCredentials().WithOrigins("http://localhost:53353", "https://localhost:8000", "http://localhost:8000"));
            });

            services.Configure<IISServerOptions>(options =>
            {
                options.MaxRequestBodySize = int.MaxValue;
            });
            services.Configure<FormOptions>(options =>
            {
                // Set the limit to 256 MB
                options.ValueLengthLimit = int.MaxValue;
                options.MultipartBodyLengthLimit = int.MaxValue;
                options.MultipartHeadersCountLimit = int.MaxValue;
            });
            services.Configure<KestrelServerOptions>(options =>
            {
                options.Limits.MaxRequestBodySize = int.MaxValue; // if don't set default value is: 30 MB
            });
            services.AddSignalR().AddAzureSignalR();

        }

        private void AddSwagger(IServiceCollection services)
        {
            services.AddSwaggerGen(c =>
            {
                c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme()
                {
                    Description = @"JWT Authorization header using the Bearer scheme. \r\n\r\n 
                      Enter 'Bearer' [space] and then your token in the text input below.
                      \r\n\r\nExample: 'Bearer 12345abcdef'",
                    Name = "Authorization",
                    In = ParameterLocation.Header,
                    Type = SecuritySchemeType.ApiKey,
                    Scheme = "Bearer"
                });
                c.AddSecurityRequirement(new OpenApiSecurityRequirement()
                {
                    {
                        new OpenApiSecurityScheme()
                        {
                            Reference = new OpenApiReference()
                            {
                                Type = ReferenceType.SecurityScheme,
                                Id = "Bearer"
                            },
                            Scheme = "oauth2",
                            Name = "Bearer",
                            In = ParameterLocation.Header
                        },
                        new List<string>()
                    }
                });
                c.SwaggerDoc("v1", new OpenApiInfo()
                {
                    Version = "v1",
                    Title = "GloboWeather Weather Management API"
                });
                c.OperationFilter<FileResultContentTypeOperationFilter>();
            });
        }
        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
               
            }
           
            app.UseHttpsRedirection();
            app.UseRouting();

            app.UseCors();
            var hostName = Environment.GetEnvironmentVariable("HOST_HOSTNAME");
            app.Use((context, next) =>
            {
                context.Response.Headers["Server"] = string.IsNullOrEmpty(hostName) ? Environment.MachineName : hostName;
                context.Response.Headers["Access-Control-Allow-Origin"] = "*";
                context.Response.Headers["Access-Control-Allow-Methods"] = "*";
                context.Response.Headers["Access-Control-Allow-Headers"] = "*";
                return next();
            });
            app.UseMiddleware<SignalRConnectedMiddleware>();
            app.UseAuthentication();
            
            app.UseSwagger();
            app.UseSwaggerUI(c =>
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "GloboWeather.WeatherManagement.Api v1"));
            
            app.UseCustomExceptionHander();           
            
            app.UseAuthorization();
            app.UseEndpoints(endpoints => { endpoints.MapControllers(); });

            app.UseAzureSignalR(routes =>
            {              
                routes.MapHub<NotificationHub>("/notificationsdev");
            });

            app.UseSerilogRequestLogging();
        }
    }
}