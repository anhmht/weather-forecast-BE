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
    public class AmountOfRainController : ControllerBase
    {
        private readonly IRainAmountService _rainAmountService;

        public AmountOfRainController(IRainAmountService rainAmountService)
        {
            _rainAmountService = rainAmountService;
        }

        [HttpGet("get-min-max-amount-of-rain", Name = "GetMinMaxAmountOfRain")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<RainAmountPredictionResponse>> GetMinMaxAmountOfRainBy(string diemDuBaoId)
        {
            var dtos = await _rainAmountService.GetRainAmountMinMaxByDiemId(diemDuBaoId: diemDuBaoId);
            return Ok(dtos);
        }
        
        [HttpGet("get-amount-of-rain", Name = "GetAmountOfRain")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<AmountOfRainResponse>> GetAmountOfRain(string diemDuBaoId)
        {
            var dtos = await _rainAmountService.GetAmountOfRainBy(diemDuBaoId: diemDuBaoId);
            return Ok(dtos);
        }
    }
}