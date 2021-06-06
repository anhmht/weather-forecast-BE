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
    public class NhietDoController : ControllerBase
    {
        private readonly IWeatherService _weatherService;

        private readonly ITemperatureService _temperatureService;

        public NhietDoController(IWeatherService weatherService, ITemperatureService temperatureService)
        {
            _weatherService = weatherService;
            _temperatureService = temperatureService;
        }

        [HttpGet("get-nhiet-do", Name = "GetNhietDo")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<NhietDoResponse>> GetNhietDoBy(string diaDuBaoId)
        {
            var dtos = await _weatherService.GetNhietDoBy(diemDuBaoId: diaDuBaoId);
            return Ok(dtos);
        }

        [HttpGet("get-du-bao-nhiet-do", Name = "GetDuBaoNhietDo")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<TemperaturePredictionResponse>> GetNhietDoByDay(string diaDuBaoId)
        {
            var dtos = await _temperatureService.GetTemperatureByDiemId(diemDuBaoId: diaDuBaoId);
            return Ok(dtos);
        }
    }
}