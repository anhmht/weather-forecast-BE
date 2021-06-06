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
    public class LuongMuaController : ControllerBase
    {
        private readonly IWeatherService _weatherService;

        public LuongMuaController(IWeatherService weatherService)
        {
            _weatherService = weatherService;
        }
     
        [HttpGet("get-du-bao-luong-mua", Name = "GetDuBaoLuongMua")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<RainAmountPredictionResponse>> GetLuongMuaByDay(string diaDuBaoId)
        {
            var dtos = await _weatherService.GetRainAmountByDiemId(diemDuBaoId: diaDuBaoId);
            return Ok(dtos);
        }
    }
}