
using System.Collections.Generic;
using System.Threading.Tasks;
using GloboWeather.WeatherManagement.Api.Helpers;
using GloboWeather.WeatherManagement.Application.Contracts.Monitoring;
using GloboWeather.WeatherManagement.Application.Models.Monitoring;
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
            [ModelBinder(BinderType = typeof(ArrayModelBinder))] IEnumerable<int> zipcodes)
        {
            var dtos = await rainingService.GetRainingQuantityAsync(zipcodes);
            return Ok(dtos);
        }

        [HttpGet("get-meteorological", Name = "GetMeteorological")]
        [ProducesResponseType(statusCode: StatusCodes.Status200OK)]
        public async Task<ActionResult<List<GetMeteorologicalResponse>>> GetMeteorologicalList(
            [FromServices] IMeteorologicalService meteorologicalService,
            [ModelBinder(BinderType = typeof(ArrayModelBinder))] IEnumerable<int> zipcodes)
        {
            var dtos = await meteorologicalService.GetMeteorologicalAsync(zipcodes);
            return Ok(dtos);
        }

        [HttpGet("get-hydrological", Name = "GetHydrological")]
        [ProducesResponseType(statusCode: StatusCodes.Status200OK)]
        public async Task<ActionResult<List<GetHydrologicalResponse>>> GetHydrologicalList(
            [FromServices] IHydrologicalService hydrologicalService,
            [ModelBinder(BinderType = typeof(ArrayModelBinder))] IEnumerable<int> zipcodes)
        {
            var dtos = await hydrologicalService.GetHydrologicalAsync(zipcodes);
            return Ok(dtos);
        }
        
        
    }
}