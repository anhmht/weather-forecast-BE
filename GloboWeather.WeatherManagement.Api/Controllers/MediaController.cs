using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GloboWeather.WeatherManegement.Application.Contracts.Media;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace GloboWeather.WeatherManagement.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MediaController : ControllerBase
    {
        private readonly IImageService _imageService;

        public MediaController(IImageService imageService)
        {
            _imageService = imageService;
        }

        [HttpPost("uploadImage")]
        public async Task<ActionResult<string>> UploadImage(IFormFile file)
        {
            return Ok(await _imageService.UploadImageAsync(file));
        }
        
        [HttpGet(Name = "GetAllImages")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesDefaultResponseType]
        public async Task<ActionResult<List<string>>> GetImagesList()
        {
            return Ok(await _imageService.GetAllImagesAsync());
        }
        
    }
}