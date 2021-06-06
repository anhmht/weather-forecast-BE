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
    public class WeatherController : ControllerBase
    {
        private readonly IWeatherService _weatherService;

        public WeatherController(IWeatherService weatherService)
        {
            _weatherService = weatherService;
        }
     
        [HttpGet("get-min-max-weather", Name = "GetMinMaxWeather")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<WeatherPredictionResponse>> GetMinMaxWeatherByDiemId(string diemDuBaoId)
        {
            var dtos = await _weatherService.GetWeatherMinMaxByDiemId(diemDuBaoId: diemDuBaoId);
            return Ok(dtos);
        }
        
        
        [HttpGet("get-weather", Name = "GetWeather")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<WeatherResponse>> GetWeatherBy(string diemDuBaoId)
        {
            var dtos = await _weatherService.GetWeatherBy(diemDuBaoId: diemDuBaoId);
            return Ok(dtos);
        }
    }
}