using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using GloboWeather.WeatherManagement.Application.Contracts.Persistence;
using GloboWeather.WeatherManagement.Domain.Entities;
using GloboWeather.WeatherManagement.Weather.IRepository;
using GloboWeather.WeatherManegement.Application.Contracts.Weather;

namespace GloboWeather.WeatherManagement.Weather.Services
{
    public class WindRankService : IWindRankService
    {
        private readonly IMapper _mapper;
        private readonly IWindRankRepository _windRankRepository;
        private readonly ICapGioRepository _capGioRepository;

        public WindRankService(IMapper mapper,
            IWindRankRepository windRankRepository,
            ICapGioRepository capGioRepository
            )
        {
            _mapper = mapper;
            _windRankRepository = windRankRepository;
            _capGioRepository = capGioRepository;
        }

        /// <summary>
        /// Download from thoitiet mysql database
        /// </summary>
        /// <returns></returns>
        public async Task<int> DownloadAsync()
        {
            var listCapGio = await _capGioRepository.ListAllAsync();
            var windRanks = _mapper.Map<List<WindRank>>(listCapGio);

            return await _windRankRepository.DownloadDataAsync(windRanks);
        }

    }
}