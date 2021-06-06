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
    public class GioGiatController : ControllerBase
    {
        private readonly IWindLevelService _windLevelService;

        public GioGiatController(IWindLevelService windLevelService)
        {
            _windLevelService = windLevelService;
        }
     
        [HttpGet("get-du-bao-gio-giat", Name = "GetDuBaoGioGiat")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<WindLevelPredictionResponse>> GetNhietDoByDay(string diaDuBaoId)
        {
            var dtos = await _windLevelService.GetWindLevelByDiemId(diemDuBaoId: diaDuBaoId);
            return Ok(dtos);
        }
    }
}