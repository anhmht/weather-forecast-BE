using System.Collections.Generic;
using System.Threading.Tasks;
using GloboWeather.WeatherManagement.Api.Helpers;
using GloboWeather.WeatherManagement.Application.Contracts.Monitoring;
using GloboWeather.WeatherManagement.Application.Models.Monitoring;
using GloboWeather.WeatherManagement.Application.Models.Monitoring.Hydrological;
using GloboWeather.WeatherManagement.Application.Models.Monitoring.HydrologicalForeCast;
using GloboWeather.WeatherManagement.Application.Models.Monitoring.Meteoroligical;
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

        [HttpPost("get-rain-quantity", Name = "GetRainQuantity")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<GetRainListResponse>> GetRainQuantityList(
            [FromServices] IRainingService rainingService,
            [FromBody] GetRainsListQuery query)
        {
            var dtos = await rainingService.GetByPagedAsync(query);
            return Ok(dtos);
        }

        [HttpPost("get-meteorological", Name = "GetMeteorological")]
        [ProducesResponseType(statusCode: StatusCodes.Status200OK)]
        public async Task<ActionResult<GetMeteorologicalListResponse>> GetMeteorologicalList(
            [FromServices] IMeteorologicalService meteorologicalService,
            [FromBody] GetMeteorologicalListQuery query)
        {
            var dtos = await meteorologicalService.GetByPagedAsync(query);
            return Ok(dtos);
        }

        [HttpPost("get-hydrological", Name = "GetHydrological")]
        [ProducesResponseType(statusCode: StatusCodes.Status200OK)]
        public async Task<ActionResult<GetHydrologicalListResponse>> GetHydrologicalList(
            [FromServices] IHydrologicalService hydrologicalService,
            [FromBody] GetHydrologicalListQuery query)
        {
            var dtos = await hydrologicalService.GetByPagedAsync(query);
            return Ok(dtos);
        }

        [HttpPost("get-hydrologicalforecast", Name = "GetHydrologicalForecastListAsync")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<GetHydrologicalForecastListResponse>> GetHydrologicalForecastListAsync(
            [FromServices] IHydrologicalForecastService hydrologicalForecastingService,
            [FromBody] GetHydrologicalForecastListQuery query)
        {
            var dtos = await hydrologicalForecastingService.GetByPagedAsync(query);
            return Ok(dtos);
        }

        [HttpPost("get-hydrologicalforecast-by-station", Name = "GetHydrologicalForecastByStationAsync")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<GetHydrologicalForecastListResponse>> GetHydrologicalForecastByStationAsync(
            [FromServices] IHydrologicalForecastService hydrologicalForecastingService,
            [FromBody] GetHydrologicalForecastByStationQuery querySingle)
        {
            var query = new GetHydrologicalForecastListQuery()
            {
                DateFrom = querySingle.DateFrom, 
                Page = querySingle.Page, 
                Limit = querySingle.Limit, 
                DateTo = querySingle.DateTo, 
                StationIds = new[] { querySingle.StationId}
            };
            var dtos = await hydrologicalForecastingService.GetByPagedAsync(query);
            return Ok(dtos);
        }

    }
}