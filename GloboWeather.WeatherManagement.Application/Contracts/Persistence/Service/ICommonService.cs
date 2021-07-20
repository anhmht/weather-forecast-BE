using System.Collections.Generic;
using System.Threading.Tasks;
using GloboWeather.WeatherManagement.Domain.Entities;

namespace GloboWeather.WeatherManagement.Application.Contracts.Persistence.Service
{
    public interface ICommonService
    {
        Task<List<Province>> GetAllProvincesAsync();
        Task<List<District>> GetAllDistrictsAsync();
        Task<Dictionary<int, object>> GetGeneralLookupDataAsync(List<int> lookupTypes);
        Task<List<CommonLookup>> GetAllCommonLookupAsync();
        Task<List<CommonLookup>> GetCommonLookupByNameSpaceAsync(string nameSpace);
    }
}