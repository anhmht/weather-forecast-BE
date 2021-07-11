using System;
using System.Threading.Tasks;
using GloboWeather.WeatherManagement.Application.Features.WeatherStates.Commands.CreateWeatherState;
using GloboWeather.WeatherManagement.Application.Features.WeatherStates.Commands.DeleteWeatherState;
using GloboWeather.WeatherManagement.Application.Features.WeatherStates.Commands.UpdateWeatherState;
using GloboWeather.WeatherManagement.Application.Features.WeatherStates.Queries.GetWeatherStateDetail;
using GloboWeather.WeatherManagement.Application.Features.WeatherStates.Queries.GetWeatherStateList;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace GloboWeather.WeatherManagement.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class WeatherStateController : ControllerBase
    {
        private readonly IMediator _mediator;

        public WeatherStateController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost("GetAllWeatherStates", Name = nameof(GetAllWeatherStates))]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesDefaultResponseType]
        public async Task<ActionResult<GetWeatherStateListResponse>> GetAllWeatherStates([FromBody] GetWeatherStateListQuery query)
        {
            var dtos = await _mediator.Send(query);
            return Ok(dtos);
        }

        [HttpGet("{id}", Name = "GetWeatherStateById")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesDefaultResponseType]
        public async Task<ActionResult<WeatherStateDetailVm>> GetWeatherStateById(Guid id)
        {
            var getWeatherStateDetailQuery = new GetWeatherStateDetailQuery() { Id = id };
            return Ok(await _mediator.Send(getWeatherStateDetailQuery));
        }

        [HttpPost(Name = "AddWeatherState")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesDefaultResponseType]
        public async Task<ActionResult<Guid>> AddWeatherState([FromForm] CreateWeatherStateCommand createWeatherStateCommand)
        {
            var id = await _mediator.Send(createWeatherStateCommand);
            return id;
        }

        [HttpPut(Name = "UpdateWeatherState")]
        [ProducesResponseType(statusCode: StatusCodes.Status204NoContent)]
        [ProducesResponseType(statusCode: StatusCodes.Status404NotFound)]
        [ProducesDefaultResponseType]
        public async Task<ActionResult<Guid>> UpdateWeatherState([FromForm] UpdateWeatherStateCommand updateWeatherStateCommand)
        {
            return await _mediator.Send(updateWeatherStateCommand);
        }

        [HttpDelete("{id}", Name = "DeleteWeatherState")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesDefaultResponseType]
        public async Task<ActionResult> DeleteWeatherState(Guid id)
        {
            await _mediator.Send(new DeleteWeatherStateCommand() { Id = id });
            return NoContent();
        }

    }
}