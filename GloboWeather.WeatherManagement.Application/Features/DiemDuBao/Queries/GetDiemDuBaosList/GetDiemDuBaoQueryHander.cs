using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using GloboWeather.WeatherManagement.Domain.Entities;
using GloboWeather.WeatherManegement.Application.Contracts.Persistence;
using MediatR;

namespace GloboWeather.WeatherManagement.Application.Features.DiemDuBao.Queries.GetDiemDuBaosList
{
    // public class GetDiemDuBaoQueryHander : IRequestHandler<GetDiemDuBaosLitstQuery, List<DiemDuBaoListVM>>
    // {
    //     private readonly IMapper _mapper;
    //     private readonly IAsyncRepositoryVN<Diemdubao> _diemdubaoRepository;
    //
    //     public GetDiemDuBaoQueryHander(IMapper mapper, IAsyncRepositoryVN<Diemdubao> diemdubaoRepository)
    //     {
    //         _mapper = mapper;
    //         _diemdubaoRepository = diemdubaoRepository;
    //     }
    //     public async Task<List<DiemDuBaoListVM>> Handle(GetDiemDuBaosLitstQuery request, CancellationToken cancellationToken)
    //     {
    //         var allDiemDuBaos = (await _diemdubaoRepository.ListAllAsync());
    //         return _mapper.Map<List<DiemDuBaoListVM>>(allDiemDuBaos);
    //     }
    // }
}