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
    public class WindLevelController : ControllerBase
    {
        private readonly IWindLevelService _windLevelService;

        public WindLevelController(IWindLevelService windLevelService)
        {
            _windLevelService = windLevelService;
        }
     
        [HttpGet("get-min-max-wind-level", Name = "GetMinMaxWindLevel")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<WindLevelPredictionResponse>> GetMinMaxWindLevelByDiemId(string diemDuBaoId)
        {
            var dtos = await _windLevelService.GetWindLevelMinMaxByDiemId(diemDuBaoId: diemDuBaoId);
            return Ok(dtos);
        }
        
        [HttpGet("get-wind-level", Name = "GetWindLevel")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<WindLevelResponse>> GetWindLevelBy(string diemDuBaoId)
        {
            var dtos = await _windLevelService.GetWindLevelBy(diemDuBaoId);
            return Ok(dtos);
        }
    }
}