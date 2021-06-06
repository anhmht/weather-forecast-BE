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
    public class HumidityController : ControllerBase
    {
        private readonly IWeatherService _weatherService;

        public HumidityController(IWeatherService weatherService)
        {
            _weatherService = weatherService;
        }
     
        [HttpGet("get-du-bao-toc-do-gio", Name = "GetDuBaoHumidity")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<WindSpeedPredictionResponse>> GetNhietDoByDay(string diaDuBaoId)
        {
            var dtos = await _weatherService.GetHumidityByDiemId(diemDuBaoId: diaDuBaoId);
            return Ok(dtos);
        }
    }
}