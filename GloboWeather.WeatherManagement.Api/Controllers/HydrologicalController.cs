using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using GloboWeather.WeatherManagement.Application.Features.Hydrologicals.Import;
using GloboWeather.WeatherManagement.Application.Responses;
using MediatR;

namespace GloboWeather.WeatherManagement.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class HydrologicalController : ControllerBase
    {
        private readonly IMediator _mediator;

        public HydrologicalController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost("import", Name = "ImportHydrologicalAsync")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<ImportResponse>> ImportAsync(
            [FromForm] ImportHydrologicalCommand request)
        {
            var response = await _mediator.Send(request);
            return Ok(response);
        }

    }
}
