using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using GloboWeather.WeatherManagement.Application.Features.HydrologicalForeCasts.Import;
using GloboWeather.WeatherManagement.Application.Responses;
using MediatR;

namespace GloboWeather.WeatherManagement.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class HydrologicalForeCastController : ControllerBase
    {
        private readonly IMediator _mediator;

        public HydrologicalForeCastController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost("import", Name = "ImportHydrologicalForeCastAsync")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<ImportResponse>> ImportAsync(
            [FromForm] ImportHydrologicalForeCastCommand request)
        {
            var response = await _mediator.Send(request);
            return Ok(response);
        }

    }
}
