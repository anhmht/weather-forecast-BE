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
        public async Task<DuBaohietDoResponse> GetNhietDoByDiemId(string diemDuBaoId)
        {
            var nhietDoEntity = await _nhietDoRepository.GetByIdAsync(diemDuBaoId);

            var duBaohietDoResponse = new DuBaohietDoResponse();
            if (nhietDoEntity == null)
                return new DuBaohietDoResponse();

            duBaohietDoResponse.DiemId = diemDuBaoId;

            var currentDate = nhietDoEntity.RefDate;
            var listNhietDoTheoNgay = new List<NhietDoTheoNgayResponse>();
            var nhietDoTheoNgay = new NhietDoTheoNgayResponse()
            {
                Date = currentDate,
                NhietDoTheoGios = new List<NhietDoTheoGio>()
            };

            var listNhietDoTheoGioTmp = new List<NhietDoTheoGio>();
            int currentDay = 0;          
            var nhietDoTheoThoiGian = new List<NhietDoTheoThoiGian>();
            for (int i = 1; i < 121; i++)
            {
                var nextHour = currentDate.AddHours(i);
                var nhietDo = nhietDoEntity.GetType().GetProperty($"_{i}").GetValue(nhietDoEntity, null);
                var nhietDoTheoGio = new NhietDoTheoGio()
                {
                    Gio = nextHour.Hour,
                    NhietDo = (int)nhietDo
                };
                listNhietDoTheoGioTmp.Add(nhietDoTheoGio);

                if ((nextHour.Hour == 23 && i > 1) || i == 120)
                {
                    nhietDoTheoNgay.NhietDoTheoGios.AddRange(listNhietDoTheoGioTmp);
                    listNhietDoTheoNgay.Add(nhietDoTheoNgay);

                    nhietDoTheoNgay = new NhietDoTheoNgayResponse()
                    {
                        Date = currentDate.AddDays(currentDay),
                        NhietDoTheoGios = new List<NhietDoTheoGio>()
                    };
                    listNhietDoTheoGioTmp = new List<NhietDoTheoGio>();
                    currentDay++;
                }
                nhietDoTheoThoiGian.Add(new NhietDoTheoThoiGian()
                {

                    ThoiGian = nextHour,
                    NhietDo = (int)nhietDo
                });
            }
            var nhietDoMin = nhietDoTheoThoiGian.Min(x => x.NhietDo);
            var nhietDoMax = nhietDoTheoThoiGian.Max(x => x.NhietDo);
            duBaohietDoResponse.NhietDoMaxs = nhietDoTheoThoiGian.Where(x => x.NhietDo == nhietDoMax).ToList();
            duBaohietDoResponse.NhietDoMins = nhietDoTheoThoiGian.Where(x => x.NhietDo == nhietDoMin).ToList();
            duBaohietDoResponse.NhietDoTheoNgays = listNhietDoTheoNgay;
            duBaohietDoResponse.NhietDoMin = nhietDoMin;
            duBaohietDoResponse.NhietDoMax = nhietDoMax;
            return duBaohietDoResponse;
        }
    }
}