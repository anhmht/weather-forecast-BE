using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using GloboWeather.WeatherManagement.Application.Features.Commons.Commands.CreateStatus;
using GloboWeather.WeatherManagement.Application.Features.Commons.Queries;
using GloboWeather.WeatherManagement.Application.Features.Commons.Queries.GetPositionStackLocation;
using GloboWeather.WeatherManagement.Application.Models.Astronomy;
using GloboWeather.WeatherManegement.Application.Contracts.Astronomy;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace GloboWeather.WeatherManagement.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CommonController : ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly ILocationService _locationService;

        public CommonController(IMediator mediator, ILocationService locationService)
        {
            _mediator = mediator;
            _locationService = locationService;
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
            [FromQuery] GetLocationCommand getLocationCommand)
        {
            return Ok(await _locationService.GetAstronomyData(getLocationCommand, CancellationToken.None));
        }
        
        
        [HttpGet("Location/GetCurrentLocation")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<GetLocationResponse>> GetCurrentLocation(
            [FromQuery] GetPositionStackLocationCommand getLocationCommand)
        {
            return Ok(await _locationService.GetCurrentLocation(getLocationCommand, CancellationToken.None));
        }
    }
}