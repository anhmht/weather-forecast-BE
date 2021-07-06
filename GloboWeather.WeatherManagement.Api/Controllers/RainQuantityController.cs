using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using GloboWeather.WeatherManagement.Application.Features.RainQuantities.Import;
using GloboWeather.WeatherManagement.Application.Responses;
using MediatR;

namespace GloboWeather.WeatherManagement.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RainQuantityController : ControllerBase
    {
        private readonly IMediator _mediator;

        public RainQuantityController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost("import", Name = "ImportRainQuantityAsync")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<ImportResponse>> ImportAsync(
            [FromForm] ImportRainQuantityCommand request)
        {
            var response = await _mediator.Send(request);
            return Ok(response);
        }

    }
}
