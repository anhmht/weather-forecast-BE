using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GloboWeather.WeatherManagement.Application.Contracts.Monitoring;
using GloboWeather.WeatherManagement.Application.Models.Monitoring;
using GloboWeather.WeatherManagement.Application.Models.Weather;
using GloboWeather.WeatherManegement.Application.Contracts.Weather;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace GloboWeather.WeatherManagement.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TramKttvController : ControllerBase
    {
        private readonly IMonitoringService _monitoringService;

        public TramKttvController(IMonitoringService monitoringService)
        {
            _monitoringService = monitoringService;
        }
        
        [HttpGet("get-kttv", Name = "GetTramKttv")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<List<TramKttvResponse>>> GetNhietDoBy(string diaDuBaoId)
        {
            var dtos = await _monitoringService.GetTramKttvList();
            return Ok(dtos);
        }
    }
}