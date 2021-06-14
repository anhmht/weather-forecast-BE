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
        private readonly IHumidityService _humidityService;

        public HumidityController(IHumidityService humidityService)
        {
            _humidityService = humidityService;
        }

        [HttpGet("get-min-max-humidity", Name = "GetMinMaxHumidity")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<HumidityPredictionResponse>> GetMinMaxHumidityByDiemId(string diaDuBaoId)
        {
            var dtos = await _humidityService.GetHumidityMinMaxByDiemId(diemDuBaoId: diaDuBaoId);
            return Ok(dtos);
        }

        [HttpGet("get-humidity", Name = "GetHumidity")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<HumidityResponse>> GetHumidityBy(string diemDuBaoId)
        {
            var dtos = await _humidityService.GetHumidityBy(diemDuBaoId);
            return Ok(dtos);
        }
    }
}