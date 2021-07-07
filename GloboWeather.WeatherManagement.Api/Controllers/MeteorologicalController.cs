using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using GloboWeather.WeatherManagement.Application.Features.Meteorologicals.Import;
using GloboWeather.WeatherManagement.Application.Responses;
using MediatR;

namespace GloboWeather.WeatherManagement.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MeteorologicalController : ControllerBase
    {
        private readonly IMediator _mediator;

        public MeteorologicalController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost("import", Name = "ImportMeteorologicalAsync")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<ImportResponse>> ImportAsync(
            [FromForm] ImportMeteorologicalCommand request)
        {
            var response = await _mediator.Send(request);
            return Ok(response);
        }

    }
}
