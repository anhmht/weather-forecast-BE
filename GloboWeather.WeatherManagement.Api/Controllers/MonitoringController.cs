using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GloboWeather.WeatherManagement.Api.Helpers;
using GloboWeather.WeatherManagement.Application.Contracts.Monitoring;
using GloboWeather.WeatherManagement.Application.Models.Monitoring;
using GloboWeather.WeatherManagement.Application.Models.Weather;
using GloboWeather.WeatherManagement.Monitoring.Services;
using GloboWeather.WeatherManegement.Application.Contracts.Weather;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace GloboWeather.WeatherManagement.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MonitoringController : ControllerBase
    {
        private readonly IMonitoringService _monitoringService;

        public MonitoringController(IMonitoringService monitoringService)
        {
            _monitoringService = monitoringService;
        }
        
        [HttpGet("get-kttv", Name = "GetTramKttv")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<List<TramKttvResponse>>> GetKTTVList()
        {
            var dtos = await _monitoringService.GetTramKttvList();
            return Ok(dtos);
        }
        
        [HttpGet("get-rain-quantity", Name = "GetRainQuantity")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<List<GetRainResponse>>> GetRainQuantityList([FromServices]IRainingService rainingService,
            [ModelBinder(binderType: typeof(ArrayModelBinder))] IEnumerable<int> ids)
        {
            var dtos = await rainingService.GetRainingQuantityAsync();
            return Ok(dtos);
        }

        [HttpGet("get-meteorological", Name = "GetMeteorological")]
        [ProducesResponseType(statusCode: StatusCodes.Status200OK)]
        public async Task<ActionResult<List<GetMeteorologicalResponse>>> GetMeteorologicalList(
            [FromServices] IMeteorologicalService meteorologicalService)
        {
            var dtos = await meteorologicalService.GetMeteorologicalAsync();
            return Ok(dtos);
        }

        [HttpGet("get-hydrological", Name = "GetHydrological")]
        [ProducesResponseType(statusCode: StatusCodes.Status200OK)]
        public async Task<ActionResult<List<GetHydrologicalResponse>>> GetHydrologicalList(
            [FromServices] IHydrologicalService hydrologicalService)
        {
            var dtos = await hydrologicalService.GetHydrologicalAsync();
            return Ok(dtos);
        }
        
        
    }
}