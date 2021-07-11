using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GloboWeather.WeatherManagement.Application.Features.WeatherStates.Commands.CreateWeatherState;
using GloboWeather.WeatherManagement.Application.Helpers.Common;
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

        //[HttpPost("GetAllWeatherStates", Name = nameof(GetAllWeatherStates))]
        //[ProducesResponseType(StatusCodes.Status200OK)]
        //[ProducesDefaultResponseType]
        //public async Task<ActionResult<GetWeatherStatesListResponse>> GetAllWeatherStates([FromBody] GetWeatherStatesListQuery query)
        //{
        //    var dtos = await _mediator.Send(query);
        //    return Ok(GeneratePageList(query, dtos));
        //}

        //[HttpGet("{id}", Name = "GetWeatherStateById")]
        //[ProducesResponseType(StatusCodes.Status200OK)]
        //[ProducesDefaultResponseType]
        //public async Task<ActionResult<WeatherStateDetailVm>> GetWeatherStateById(Guid id)
        //{
        //    var getWeatherStateDetailQuery = new GetWeatherStateDetailQuery() {Id = id};
        //    return Ok(await _mediator.Send(getWeatherStateDetailQuery));
        //}

        [HttpPost(Name = "AddWeatherState")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesDefaultResponseType]
        public async Task<ActionResult<Guid>> AddWeatherState([FromForm] CreateWeatherStateCommand createWeatherStateCommand)
        {
            var id = await _mediator.Send(createWeatherStateCommand);
            return id;
        }

        //[HttpPut(Name = "UpdateWeatherState")]
        //[ProducesResponseType(statusCode:StatusCodes.Status204NoContent)]
        //[ProducesResponseType(statusCode:StatusCodes.Status404NotFound)]
        //[ProducesDefaultResponseType]
        //public async Task<ActionResult> UpdateWeatherState([FromBody] UpdateWeatherStateCommand updateWeatherStateCommand)
        //{
        //    await _mediator.Send(updateWeatherStateCommand);
        //    return NoContent();
        //}

        //[HttpDelete("{id}", Name = "DeleteWeatherState")]
        //[ProducesResponseType(StatusCodes.Status204NoContent)]
        //[ProducesResponseType(StatusCodes.Status404NotFound)]
        //[ProducesDefaultResponseType]
        //public async Task<ActionResult> DeleteWeatherState(Guid id)
        //{
        //    await _mediator.Send(new DeleteWeatherStateCommand() {WeatherStateId = id});
        //    return NoContent();
        //}

    }
}