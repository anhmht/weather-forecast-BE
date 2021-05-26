using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using GloboWeather.WeatherManagement.Application.Features.Commons.Commands.CreateStatus;
using GloboWeather.WeatherManagement.Application.Features.Commons.Queries;
using GloboWeather.WeatherManagement.Application.Models.Astronomy;
using GloboWeather.WeatherManegement.Application.Contracts.Astronomy;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace GloboWeather.WeatherManagement.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CommonController : ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly IAstronomyService _astronomyService;

        public CommonController(IMediator mediator, IAstronomyService astronomyService)
        {
            _mediator = mediator;
            _astronomyService = astronomyService;
        }
        
        [HttpGet("Status/GetAllStatuses")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesDefaultResponseType]
        public async Task<ActionResult<List<StatusesListVm>>> GetAllCategories()
        {
            var dtos = await _mediator.Send(new GetStatusesListQuery());
            return dtos;
        }
        
        [HttpPost("Status/CreateStatus")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesDefaultResponseType]
        public async Task<ActionResult<CreateStatusCommandResponse>> CreateCategory([FromBody] CreateStatusCommand createStatusCommand)
        {
            var response = await _mediator.Send(createStatusCommand);
            return response;
        }

        [HttpGet("Location/GetAstronomy")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<GetAstronomyResponse>> GetAstronomyData(
            [FromQuery] GetAstronomyCommand getAstronomyCommand)
        {
            return Ok(await _astronomyService.GetAstronomyData(getAstronomyCommand, CancellationToken.None));
        }
    }
}