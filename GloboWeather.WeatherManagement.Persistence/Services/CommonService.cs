﻿using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GloboWeather.WeatherManagement.Application.Caches;
using GloboWeather.WeatherManagement.Application.Contracts.Persistence;
using GloboWeather.WeatherManagement.Application.Contracts.Persistence.Service;
using GloboWeather.WeatherManagement.Domain.Entities;

namespace GloboWeather.WeatherManagement.Persistence.Services
{
    public class CommonService : ICommonService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ICacheStore _cacheStore;

        public CommonService(IUnitOfWork unitOfWork, ICacheStore cacheStore)
        {
            _unitOfWork = unitOfWork;
            _cacheStore = cacheStore;
        }

        public async Task<List<Province>> GetAllProvincesAsync()
        {
            var provinceCacheKey = new ProvinceCacheKey();
            var cachedProvinces = _cacheStore.Get(provinceCacheKey);
            if (cachedProvinces != null)
                return cachedProvinces;

            //Get from database
            var response = (await _unitOfWork.ProvinceRepository.GetAllAsync()).ToList();

            //Save cache
            _cacheStore.Add(response, provinceCacheKey);

            return response;
        }

        public async Task<List<District>> GetAllDistrictsAsync()
        {
            var districtCacheKey = new DistrictCacheKey();
            var cachedDistricts = _cacheStore.Get(districtCacheKey);
            if (cachedDistricts != null)
                return cachedDistricts;

            //Get from database
            var response = (await _unitOfWork.DistrictRepository.GetAllAsync()).ToList();

            //Save cache
            _cacheStore.Add(response, districtCacheKey);

            return response;
        }
    }
}