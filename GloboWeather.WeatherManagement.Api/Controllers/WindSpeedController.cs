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
    public class WindSpeedController : ControllerBase
    {
        private readonly IWindSpeedService _windSpeedService;

        public WindSpeedController(IWindSpeedService windSpeedService)
        {
            _windSpeedService = windSpeedService;
        }

        [HttpGet("get-min-max-wind-speed", Name = "GetMinMaxWindSpeed")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<WindSpeedPredictionResponse>> GetMinMaxWindSpeed(string diemDuBaoId)
        {
            var dtos = await _windSpeedService.GetWindSpeedMinMaxByDiemId(diemDuBaoId: diemDuBaoId);
            return Ok(dtos);
        }
        
        [HttpGet("get-wind-speed", Name = "GetWindSpeed")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<WindSpeedResponse>> GetWindSpeedBy(string diemDuBaoId)
        {
            var dtos = await _windSpeedService.GetWindSpeedBy(diemDuBaoId);
            return Ok(dtos);
        }
    }
}