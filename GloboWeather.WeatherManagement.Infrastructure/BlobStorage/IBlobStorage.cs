using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace GloboWeather.WeatherManagement.Infrastructure.BobStorage
{
    public interface IBobStorage
    {
        Task<string> UploadBob(Stream file,string containerName, string blobRef);
        Task<string> MoveBob(string blob, string newBlobRef);
        Task<bool> DeleteBlob(string containerName, string blobRef);
        Task<string> GetFilesByRef(string blobRef);
    }
}