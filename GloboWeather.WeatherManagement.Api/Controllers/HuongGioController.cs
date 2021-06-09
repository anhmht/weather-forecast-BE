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
    public class HuongGioController : ControllerBase
    {
        private readonly IWindDirectionService _windDirectionService;

        public HuongGioController(IWindDirectionService windDirectionService)
        {
            _windDirectionService = windDirectionService;
        }
     
        [HttpGet("get-du-bao-huong-gio", Name = "GetDuHuongGio")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<WeatherPredictionResponse>> GetHuongGioByDay(string diaDuBaoId)
        {
            var dtos = await _windDirectionService.GetWindDirectionByDiemId(diemDuBaoId: diaDuBaoId);
            return Ok(dtos);
        }
    }
}