using System.Threading.Tasks;
using GloboWeather.WeatherManagement.Application.Features.WeatherInformations.Commands.ImportWeatherInformation;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace GloboWeather.WeatherManagement.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class WeatherInformationController : ControllerBase
    {
        private readonly IMediator _mediator;

        public WeatherInformationController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost("import", Name = nameof(ImportAsync))]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesDefaultResponseType]
        public async Task<ActionResult<bool>> ImportAsync(IFormFile file)
        {
            ImportWeatherInformationCommand cmd = new ImportWeatherInformationCommand() { File = file };
            var response = await _mediator.Send(cmd);
            return Ok(response);
        }
    }
}