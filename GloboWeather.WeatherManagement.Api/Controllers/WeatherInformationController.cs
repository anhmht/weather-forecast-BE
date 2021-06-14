using System.Threading.Tasks;
using GloboWeather.WeatherManagement.Application.Features.Events.Queries.GetEventsListByCateIdAndStaId;
using GloboWeather.WeatherManagement.Application.Features.WeatherInformations.Commands.ImportWeatherInformation;
using GloboWeather.WeatherManagement.Application.Features.WeatherInformations.Queries.GetWeatherInformation;
using GloboWeather.WeatherManagement.Application.Models.Weather;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace GloboWeather.WeatherManagement.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class WeatherInformationController : ControllerBase
    {
        private readonly IMediator _mediator;

        public WeatherInformationController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost("import", Name = nameof(ImportAsync))]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesDefaultResponseType]
        public async Task<ActionResult<ImportWeatherInformationResponse>> ImportAsync(IFormFile file)
        {
            ImportWeatherInformationCommand cmd = new ImportWeatherInformationCommand() { File = file };
            var response = await _mediator.Send(cmd);
            return Ok(response);
        }

        [HttpPost("get-weather-information", Name = nameof(GetWeatherInformationAsync))]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesDefaultResponseType]
        public async Task<ActionResult<GetWeatherInformationResponse>> GetWeatherInformationAsync(GetWeatherInformationRequest request)
        {
            var response = await _mediator.Send(request);
            return Ok(response);
        }

        //[HttpPost("get-min-max-humidity", Name = "GetMinMaxHumidity")]
        //[ProducesResponseType(StatusCodes.Status200OK)]
        //public async Task<ActionResult<HumidityPredictionResponse>> GetMinMaxHumidity([FromBody] GetMinMaxHumidityRequest request)
        //{
        //    var response = await _mediator.Send(request);
        //    return Ok(response);
        //}

        //[HttpPost("get-humidity", Name = "GetHumidity")]
        //[ProducesResponseType(StatusCodes.Status200OK)]
        //public async Task<ActionResult<HumidityResponse>> GetHumidity([FromBody] GetHumidityRequest request)
        //{
        //    var response = await _mediator.Send(request);
        //    return Ok(response);
        //}
    }
}