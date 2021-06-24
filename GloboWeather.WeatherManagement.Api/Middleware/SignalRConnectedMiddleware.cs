using Microsoft.AspNetCore.Http;
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
                var authorization = context.Request.Headers["sesionId"].FirstOrDefault();              
            }
            await Next.Invoke(context);
        }
    }
}
