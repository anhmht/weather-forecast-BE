using MediatR;
using Microsoft.AspNetCore.Http;
using System;

namespace GloboWeather.WeatherManagement.Application.Features.WeatherInformations.Commands.ImportWeatherInformation
{
    public class ImportWeatherInformationDto
    {
        public string DiaDiemId { get; set; }
        public string NgayGio { get; set; }
        /// <summary>
        /// Do am
        /// </summary>
        public string DoAm { get; set; }
        /// <summary>
        /// Gio giat
        /// </summary>
        public string GioGiat { get; set; }
        /// <summary>
        /// Huong gio
        /// </summary>
        public string HuongGio { get; set; }
        /// <summary>
        /// Toc do gio
        /// </summary>
        public string TocDoGio { get; set; }
        /// <summary>
        /// Luong mua
        /// </summary>
        public string LuongMua { get; set; }
        /// <summary>
        /// Nhiet do
        /// </summary>
        public string NhietDo { get; set; }
        /// <summary>
        /// Thoi tiet
        /// </summary>
        public string ThoiTiet { get; set; }
    }
}