using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GloboWeather.WeatherManagement.Application.Features.DiemDuBao.Queries.GetDiemDuBaosList;
using GloboWeather.WeatherManagement.Application.Models.Weather;
using GloboWeather.WeatherManegement.Application.Contracts.Weather;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace GloboWeather.WeatherManagement.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DiemDuBaoController : ControllerBase
    {
        private readonly IWeatherService _weatherService;

        public DiemDuBaoController(IWeatherService weatherService)
        {
            _weatherService = weatherService;
        }
        
        [HttpGet("all", Name = "GetAllDiemDuBaos")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<List<DiemDuBaoResponse>>> GetAllDiemDuBaos()
        {
            var dtos = await _weatherService.GetDiemDuBaosList();
            return Ok(dtos);
        }
    }
}