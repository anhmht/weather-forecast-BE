using GloboWeather.WeatherManagement.Api.Context;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GloboWeather.WeatherManagement.Api.Middleware
{
    public class SignalRConnectedMiddleware
    {
        protected readonly RequestDelegate Next;
        public SignalRConnectedMiddleware(RequestDelegate next)
        {
            Next = next;
        }
        public async Task InvokeAsync(HttpContext context)
        {
            if (context.Request.Path.Value.Contains("notifications"))
            {
                //var sesionId = context.Request.Headers["sesionId"].FirstOrDefault();
                //var weatherContext = context.RequestServices.GetRequiredService<WeatherContext>();
                //weatherContext.SignalRSessionId = sesionId;
            }
            await Next.Invoke(context);
        }
    }
}
