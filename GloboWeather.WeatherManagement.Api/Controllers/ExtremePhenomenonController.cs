using System;
using System.Threading.Tasks;
using GloboWeather.WeatherManagement.Application.Features.ExtremePhenomenons.Commands.CreateExtremePhenomenon;
using GloboWeather.WeatherManagement.Application.Features.ExtremePhenomenons.Commands.DeleteExtremePhenomenon;
using GloboWeather.WeatherManagement.Application.Features.ExtremePhenomenons.Commands.UpdateExtremePhenomenon;
using GloboWeather.WeatherManagement.Application.Features.ExtremePhenomenons.Queries.ExtremePhenomenonDetail;
using GloboWeather.WeatherManagement.Application.Features.ExtremePhenomenons.Queries.ExtremePhenomenonList;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace GloboWeather.WeatherManagement.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ExtremePhenomenonController : ControllerBase
    {
        private readonly IMediator _mediator;

        public ExtremePhenomenonController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost("GetAllExtremePhenomenons", Name = nameof(GetAllExtremePhenomenons))]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesDefaultResponseType]
        public async Task<ActionResult<GetExtremePhenomenonListResponse>> GetAllExtremePhenomenons([FromBody] GetExtremePhenomenonListQuery query)
        {
            var dtos = await _mediator.Send(query);
            return Ok(dtos);
        }

        [HttpGet("{id}", Name = "GetExtremePhenomenonById")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesDefaultResponseType]
        public async Task<ActionResult<ExtremePhenomenonDetailVm>> GetExtremePhenomenonById(Guid id)
        {
            var getExtremePhenomenonDetailQuery = new GetExtremePhenomenonDetailQuery() { Id = id };
            return Ok(await _mediator.Send(getExtremePhenomenonDetailQuery));
        }

        [HttpPost(Name = "AddExtremePhenomenon")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesDefaultResponseType]
        public async Task<ActionResult<Guid>> AddExtremePhenomenon([FromBody] CreateExtremePhenomenonCommand createExtremePhenomenonCommand)
        {
            var id = await _mediator.Send(createExtremePhenomenonCommand);
            return id;
        }

        [HttpPut(Name = "UpdateExtremePhenomenon")]
        [ProducesResponseType(statusCode: StatusCodes.Status204NoContent)]
        [ProducesResponseType(statusCode: StatusCodes.Status404NotFound)]
        [ProducesDefaultResponseType]
        public async Task<ActionResult<Guid>> UpdateExtremePhenomenon([FromBody] UpdateExtremePhenomenonCommand updateExtremePhenomenonCommand)
        {
            return await _mediator.Send(updateExtremePhenomenonCommand);
        }

        [HttpDelete("{id}", Name = "DeleteExtremePhenomenon")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesDefaultResponseType]
        public async Task<ActionResult> DeleteExtremePhenomenon(Guid id)
        {
            await _mediator.Send(new DeleteExtremePhenomenonCommand() { Id = id });
            return NoContent();
        }

    }
}