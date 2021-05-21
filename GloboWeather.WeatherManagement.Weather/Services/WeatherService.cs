using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using GloboWeather.WeatherManagement.Application.Models.Weather;
using GloboWeather.WeatherManagement.Weather.IRepository;
using GloboWeather.WeatherManegement.Application.Contracts.Weather;

namespace GloboWeather.WeatherManagement.Weather.Services
{
    public class WeatherService : IWeatherService
    {
        private readonly IMapper _mapper;
        private readonly IDiemDuBaoRepository _diemDuBaoRepository;
        private readonly INhietDoRepository _nhietDoRepository;

        public WeatherService(IMapper mapper, 
            IDiemDuBaoRepository diemDuBaoRepository
            , INhietDoRepository nhietDoRepository)
        {
            _mapper = mapper;
            _diemDuBaoRepository = diemDuBaoRepository;
            _nhietDoRepository = nhietDoRepository;
        }
        public async Task<List<DiemDuBaoResponse>> GetDiemDuBaosList()
        {
            var diemdubaoEntityList = await _diemDuBaoRepository.ListAllAsync();

            return _mapper.Map<List<DiemDuBaoResponse>>(diemdubaoEntityList);

        }

        public async Task<NhietDoResponse> GetNhietDoBy(string diemDuBaoId)
        {
            var nhietDoEntity = await _nhietDoRepository.GetByIdAsync(diemDuBaoId);
            return _mapper.Map<NhietDoResponse>(nhietDoEntity);
        }
    }
}