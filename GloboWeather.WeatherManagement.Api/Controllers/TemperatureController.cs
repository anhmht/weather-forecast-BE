using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GloboWeather.WeatherManagement.Application.Models.Weather;
using GloboWeather.WeatherManegement.Application.Contracts.Weather;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace GloboWeather.WeatherManagement.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TemperatureController : ControllerBase
    {
        private readonly IWeatherService _weatherService;

        private readonly ITemperatureService _temperatureService;

        public TemperatureController(IWeatherService weatherService, ITemperatureService temperatureService)
        {
            _weatherService = weatherService;
            _temperatureService = temperatureService;
        }

        [HttpGet("get-temperature", Name = "GetTemperature")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<TemperatureResponse>> GetTemperatureBy(string diemDuBaoId)
        {
            var dtos = await _temperatureService.GetTemperatureBy(diemDuBaoId: diemDuBaoId);
            return Ok(dtos);
        }

        [HttpGet("get-min-max-temperature", Name = "GetMinMaxTemperature")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<TemperaturePredictionResponse>> GetMinMaxTemperature(string diemDuBaoId)
        {
            var dtos = await _temperatureService.GetTemperatureMinMaxByDiemId(diemDuBaoId: diemDuBaoId);
            return Ok(dtos);
        }
    }
}