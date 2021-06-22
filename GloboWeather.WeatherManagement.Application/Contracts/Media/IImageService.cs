using System.Collections.Generic;
using System.Threading.Tasks;
using GloboWeather.WeatherManagement.Application.Models.Media;
using Microsoft.AspNetCore.Http;

namespace GloboWeather.WeatherManegement.Application.Contracts.Media
{
    public interface IImageService
    {
        Task<ImageResponse> UploadImageAsync(IFormFile file);
        Task<List<string>> GetAllImagesAsync();
        Task<bool> DeleteAllImagesTempContainerAsync();
        Task<bool> DeleteImagesInPostsContainerAsync(string eventId);
        Task<bool> DeleteImagesInPostsContainerByNameAsync(string eventId, List<string> imageUrls);
        Task<List<string>> CopyImageToEventPost(List<string> imageUrls, string eventId, string folderName);

        Task<ImageResponse> UploadAvatarForUserAsync(string userId, IFormFile file);

    }
}