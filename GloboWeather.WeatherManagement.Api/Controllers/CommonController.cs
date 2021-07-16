using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using GloboWeather.WeatherManagement.Application.Contracts.Persistence.Service;
using GloboWeather.WeatherManagement.Application.Features.Commons.Commands.CreateStatus;
using GloboWeather.WeatherManagement.Application.Features.Commons.Queries;
using GloboWeather.WeatherManagement.Application.Features.Commons.Queries.GetPositionStackLocation;
using GloboWeather.WeatherManagement.Application.Models.Astronomy;
using GloboWeather.WeatherManagement.Domain.Entities;
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
        private readonly ILocationService _locationService;
        private readonly ICommonService _commonService;

        public CommonController(IMediator mediator, ILocationService locationService, ICommonService commonService)
        {
            _mediator = mediator;
            _locationService = locationService;
            _commonService = commonService;
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

        [HttpGet("Province/GetAll")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<List<Province>>> GetAllProvinceAsync()
        {
            return Ok(await _commonService.GetAllProvincesAsync());
        }

        [HttpGet("District/GetAll")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<List<District>>> GetAllDistrictAsync()
        {
            return Ok(await _commonService.GetAllDistrictsAsync());
        }

        [HttpPost("GetGeneralLookup")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<Dictionary<int, object>>> GetGeneralLookupDataAsync([FromBody] List<int> lookupTypes)
        {
            return Ok(await _commonService.GetGeneralLookupDataAsync(lookupTypes));
        }
    }
}