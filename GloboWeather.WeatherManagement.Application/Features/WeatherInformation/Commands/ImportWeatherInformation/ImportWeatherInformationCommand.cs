using MediatR;
using Microsoft.AspNetCore.Http;
using System;

namespace GloboWeather.WeatherManagement.Application.Features.WeatherInformations.Commands.ImportWeatherInformation
{
    public class ImportWeatherInformationCommand : IRequest<ImportWeatherInformationResponse>
    {
        public IFormFile File { get; set; }
    }
}