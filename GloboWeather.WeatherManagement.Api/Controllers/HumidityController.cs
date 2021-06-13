using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GloboWeather.WeatherManagement.Application.Models.Weather;
using GloboWeather.WeatherManegement.Application.Contracts.Weather;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using GloboWeather.WeatherManagement.Application.Requests;
using MediatR;
using GloboWeather.WeatherManagement.Application.Features.Events.Queries.GetEventsListByCateIdAndStaId;

namespace GloboWeather.WeatherManagement.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class HumidityController : ControllerBase
    {
        private readonly IMediator _mediator;

        public HumidityController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost("get-min-max-humidity", Name = "GetMinMaxHumidity")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<HumidityPredictionResponse>> GetMinMaxHumidity([FromBody] GetMinMaxHumidityRequest request)
        {
            var response = await _mediator.Send(request);
            return Ok(response);
        }

        [HttpPost("get-humidity", Name = "GetHumidity")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<HumidityResponse>> GetHumidity([FromBody] GetHumidityRequest request)
        {
            var response = await _mediator.Send(request);
            return Ok(response);
        }
    }
}