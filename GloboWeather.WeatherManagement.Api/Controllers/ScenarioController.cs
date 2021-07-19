using System;
using System.Threading.Tasks;
using GloboWeather.WeatherManagement.Application.Features.Scenarios.Commands.CreateScenario;
using GloboWeather.WeatherManagement.Application.Features.Scenarios.Commands.CreateScenarioAction;
using GloboWeather.WeatherManagement.Application.Features.Scenarios.Commands.DeleteScenario;
using GloboWeather.WeatherManagement.Application.Features.Scenarios.Commands.DeleteScenarioAction;
using GloboWeather.WeatherManagement.Application.Features.Scenarios.Commands.UpdateScenario;
using GloboWeather.WeatherManagement.Application.Features.Scenarios.Commands.UpdateScenarioAction;
using GloboWeather.WeatherManagement.Application.Features.Scenarios.Queries.GetScenarioActionDetail;
using GloboWeather.WeatherManagement.Application.Features.Scenarios.Queries.GetScenarioDetail;
using GloboWeather.WeatherManagement.Application.Features.Scenarios.Queries.GetScenariosList;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace GloboWeather.WeatherManagement.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ScenarioController : ControllerBase
    {
        private readonly IMediator _mediator;

        public ScenarioController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost("GetAllScenario", Name = nameof(GetAllScenario))]
        [ProducesResponseType(statusCode: StatusCodes.Status200OK)]
        [ProducesDefaultResponseType]
        public async Task<ActionResult<GetScenariosListResponse>> GetAllScenario([FromBody] GetScenariosListQuery query)
        {
            var dtos = await _mediator.Send(query);
            return Ok(dtos);
        }

        [HttpGet("{id}", Name = "GetScenarioById")]
        [ProducesResponseType(statusCode: StatusCodes.Status200OK)]
        [ProducesDefaultResponseType]
        public async Task<ActionResult<ScenarioDetailVm>> GetScenarioById(Guid id)
        {
            var query = new GetScenarioDetailQuery() {ScenarioId = id};
            var dto = await _mediator.Send(query);
            return Ok(dto);
        }

        [HttpPost(Name = "AddScenario")]
        [ProducesResponseType(statusCode: StatusCodes.Status201Created)]
        [ProducesDefaultResponseType]
        public async Task<ActionResult<Guid>> CreateScenario([FromBody] CreateScenarioCommand query)
        {
            var scenarioId = await _mediator.Send(query);
            return Ok(scenarioId);
        }

        [HttpPut(Name = "UpdateScenario")]
        [ProducesResponseType(statusCode: StatusCodes.Status204NoContent)]
        [ProducesResponseType(statusCode: StatusCodes.Status404NotFound)]
        public async Task<ActionResult> UpdateScenario([FromBody] UpdateScenarioCommand query)
        {
            await _mediator.Send(query);
            return NoContent();
        }

        [HttpDelete("{id}", Name = "DeleteScenario")]
        [ProducesResponseType(statusCode: StatusCodes.Status204NoContent)]
        [ProducesResponseType(statusCode: StatusCodes.Status404NotFound)]
        public async Task<ActionResult> DeleteScenario(Guid id)
        {
            var query = new DeleteScenarioCommand() {ScenarioId = id};
            await _mediator.Send(query);
            return NoContent();
        }

        [HttpGet("action/{id}", Name = "GetScenarioActionDetailById")]
        [ProducesResponseType(statusCode: StatusCodes.Status200OK)]
        [ProducesDefaultResponseType]
        public async Task<ActionResult<ScenarioActionDetailVm>> GetScenarioActionDetailAsync(Guid id)
        {
            var query = new GetScenarioActionDetailQuery() { ScenarioId = id };
            var dto = await _mediator.Send(query);
            return Ok(dto);
        }

        [HttpPost("action", Name = "CreateScenarioActionAsync")]
        [ProducesResponseType(statusCode: StatusCodes.Status201Created)]
        [ProducesDefaultResponseType]
        public async Task<ActionResult<Guid>> CreateScenarioActionAsync([FromBody] CreateScenarioActionCommand query)
        {
            var scenarioId = await _mediator.Send(query);
            return Ok(scenarioId);
        }

        [HttpPut("action", Name = "UpdateScenarioActionAsync")]
        [ProducesResponseType(statusCode: StatusCodes.Status204NoContent)]
        [ProducesResponseType(statusCode: StatusCodes.Status404NotFound)]
        public async Task<ActionResult> UpdateScenarioActionAsync([FromBody] UpdateScenarioActionCommand query)
        {
            await _mediator.Send(query);
            return NoContent();
        }
        [HttpDelete("action/{id}", Name = "DeleteScenarioActionAsync")]
        [ProducesResponseType(statusCode: StatusCodes.Status204NoContent)]
        [ProducesResponseType(statusCode: StatusCodes.Status404NotFound)]
        public async Task<ActionResult> DeleteScenarioActionAsync(Guid id)
        {
            var query = new DeleteScenarioActionCommand() { ScenarioId = id };
            await _mediator.Send(query);
            return NoContent();
        }

    }
}