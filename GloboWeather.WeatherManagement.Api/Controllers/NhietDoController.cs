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

        public NhietDoController(IWeatherService weatherService)
        {
            _weatherService = weatherService;
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
        public async Task<ActionResult<DuBaohietDoResponse>> GetNhietDoByDay(string diaDuBaoId)
        {
            var dtos = await _weatherService.GetNhietDoByDiemId(diemDuBaoId: diaDuBaoId);
            return Ok(dtos);
        }
    }
}