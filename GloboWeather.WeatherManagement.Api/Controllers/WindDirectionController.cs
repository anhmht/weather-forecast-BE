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
    public class WindDirectionController : ControllerBase
    {
        private readonly IWindDirectionService _windDirectionService;

        public WindDirectionController(IWindDirectionService windDirectionService)
        {
            _windDirectionService = windDirectionService;
        }

        [HttpGet("get-min-max-wind-direction", Name = "GetMinMaxWindDirection")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<WindDirectionPredictionResponse>> GetHuongGioByDay(string diemDuBaoId)
        {
            var dtos = await _windDirectionService.GetWindDirectionByDiemId(diemDuBaoId: diemDuBaoId);
            return Ok(dtos);
        }
    }
}