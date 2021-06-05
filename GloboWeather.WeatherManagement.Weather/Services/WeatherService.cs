using System;
using System.Collections.Generic;
using System.Linq;
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

        /// <summary>
        /// get list of NhietDo by DiemId
        /// </summary>
        /// <param name="diemDuBaoId"></param>
        /// <returns></returns>
        public async Task<TemperaturePredictionResponse> GetNhietDoByDiemId(string diemDuBaoId)
        {
            var nhietDoEntity = await _nhietDoRepository.GetByIdAsync(diemDuBaoId);

            var duBaohietDoResponse = new TemperaturePredictionResponse();
            if (nhietDoEntity == null)
                return new TemperaturePredictionResponse();

            duBaohietDoResponse.DiemId = diemDuBaoId;

            var currentDate = nhietDoEntity.RefDate;
            var listNhietDoTheoNgay = new List<TemperatureDayResponse>();
            var nhietDoTheoNgay = new TemperatureDayResponse()
            {
                Date = currentDate,
                TemperatureHours = new List<TemperatureHour>()
            };

            var listNhietDoTheoGioTmp = new List<TemperatureHour>();
            int currentDay = 0;
            var nhietDoTheoThoiGianMin = new List<TemperatureTime>();
            var nhietDoTheoThoiGianMax = new List<TemperatureTime>();
            for (int i = 1; i < 121; i++)
            {
                var nextHour = currentDate.AddHours(i);
                var nhietDo = nhietDoEntity.GetType().GetProperty($"_{i}").GetValue(nhietDoEntity, null);
                var nhietDoTheoGio = new TemperatureHour()
                {
                    Hour = nextHour.Hour,
                    Temperature = (int)nhietDo
                };
                listNhietDoTheoGioTmp.Add(nhietDoTheoGio);

                if ((nextHour.Hour == 23 && i > 1) || i == 120)
                {
                    nhietDoTheoNgay.TemperatureHours.AddRange(listNhietDoTheoGioTmp);
                    // calculate temperature min or max
                    var nhietDoMinTmp = listNhietDoTheoGioTmp.Min(x => x.Temperature);
                    var nhietDoMaxTmp = listNhietDoTheoGioTmp.Max(x => x.Temperature);
                    nhietDoTheoNgay.TemperatureMins.AddRange(listNhietDoTheoGioTmp.Where(x => x.Temperature == nhietDoMinTmp));
                    nhietDoTheoNgay.TemperatureMaxs.AddRange(listNhietDoTheoGioTmp.Where(x => x.Temperature == nhietDoMaxTmp));
                    nhietDoTheoNgay.TemperatureMin = nhietDoMinTmp;
                    nhietDoTheoNgay.TemperatureMax = nhietDoMaxTmp;

                    listNhietDoTheoNgay.Add(nhietDoTheoNgay);
                                       
                    // reinnit data
                    currentDay++;
                    nhietDoTheoNgay = new TemperatureDayResponse()
                    {
                        Date = currentDate.AddDays(currentDay),
                        TemperatureHours = new List<TemperatureHour>(),
                        TemperatureMaxs = new List<TemperatureHour>(),
                        TemperatureMins = new List<TemperatureHour>()
                    };
                    listNhietDoTheoGioTmp = new List<TemperatureHour>();
                }

            }
            duBaohietDoResponse.TemperatureDays = listNhietDoTheoNgay;
            duBaohietDoResponse.TemperatureMin = listNhietDoTheoNgay.Min(x=>x.TemperatureMins.Min(x=>x.Temperature));
            duBaohietDoResponse.TemperatureMax = listNhietDoTheoNgay.Max(x => x.TemperatureMaxs.Max(x => x.Temperature));
            return duBaohietDoResponse;
        }
    }
}