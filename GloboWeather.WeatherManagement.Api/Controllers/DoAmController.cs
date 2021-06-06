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
    public class DoAmController : ControllerBase
    {
        private readonly IHumidityService _humidityService;

        public DoAmController(IHumidityService humidityService)
        {
            _humidityService = humidityService;
        }
     
        [HttpGet("get-du-bao-do-am", Name = "GetDuBaoDoAm")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<HumidityPredictionResponse>> GetHumidityByDay(string diaDuBaoId)
        {
            var dtos = await _humidityService.GetHumidityByDiemId(diemDuBaoId: diaDuBaoId);
            return Ok(dtos);
        }
    }
}