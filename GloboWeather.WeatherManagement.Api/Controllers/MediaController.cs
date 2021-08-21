using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GloboWeather.WeatherManagement.Application.Models.Authentication;
using GloboWeather.WeatherManagement.Application.Models.Media;
using GloboWeather.WeatherManegement.Application.Contracts.Media;
using Microsoft.AspNetCore.Authorization;
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
        public async Task<ActionResult<ImageResponse>> UploadImage(IFormFile file)
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
        
        [HttpDelete(Name = "DeleteAllImageInTempsContainer")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesDefaultResponseType]
        public async Task<ActionResult<List<string>>> DeleteImagesList()
        {
            return Ok(await _imageService.DeleteAllImagesTempContainerAsync());
        }
        
        [HttpPost("uploadAvatar")]
        public async Task<ActionResult<ImageResponse>> UploadAvatar([FromForm]UploadAvatarRequest request)
        {
            return Ok(await _imageService.UploadAvatarForUserAsync(request.UserId, request.Image));
        }

        [HttpGet("generate-qr-code")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesDefaultResponseType]
        [Authorize(Roles = "SuperAdmin,DTH")]
        public async Task<ActionResult<ImageResponse>> GenerateQRCodeAsync([FromQuery] string text)
        {
            return Ok(await _imageService.GenerateQRCodeAsync(text));
        }

        [HttpPost("uploadFile")]
        public async Task<ActionResult<DocumentResponse>> UploadFileAsync(IFormFile file)
        {
            return Ok(await _imageService.UploadFileAsync(file));
        }

        [HttpPost("UploadVideo")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesDefaultResponseType]
        public async Task<ActionResult> UploadVideoAsync([FromServices]IVideoService videoService, IFormFile file)
        {
            var result =   await videoService.UploadVideoAsync(file);
            return Ok(result);
        }
        
        [HttpPost("UploadVideoSocial")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesDefaultResponseType]
        public async Task<ActionResult> UploadVideoSocialAsync([FromServices]IVideoService videoService, IFormFile file)
        {
            var result =   await videoService.UploadVideoSocialAsync(file);
            return Ok(result);
        }

    }
}