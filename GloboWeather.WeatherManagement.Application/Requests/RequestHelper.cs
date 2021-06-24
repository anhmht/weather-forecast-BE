using System;
using System.Linq;
using GloboWeather.WeatherManagement.Application.Features.WeatherInformations.Queries.GetWeatherInformation;
using GloboWeather.WeatherManagement.Application.Helpers.Common;

namespace GloboWeather.WeatherManagement.Application.Requests
{
    public static class RequestHelper
    {
        public static void StandadizeGetWeatherInformationBaseRequest(GetWeatherInformationBaseRequest request, bool isWholeDay = true)
        {
            if (!request.FromDate.HasValue)
            {
                if (!request.ToDate.HasValue)
                {
                    request.FromDate = DateTime.Now.GetStartOfDate();
                    request.ToDate = DateTime.Now.GetEndOfDate();
                }
                else
                {
                    if (isWholeDay)
                    {
                        request.FromDate = request.ToDate.GetStartOfDate();
                        request.ToDate = request.ToDate.GetEndOfDate();
                    }
                }
            }
            else
            {
                if (isWholeDay)
                {
                    request.FromDate = request.FromDate.GetStartOfDate();
                }

                if (!request.ToDate.HasValue)
                {
                    request.ToDate = request.FromDate.GetEndOfDate();
                }
                else
                {
                    if (isWholeDay)
                    {
                        request.ToDate = request.ToDate.GetEndOfDate();
                    }
                }
            }

            if (request.FromDate > request.ToDate)
            {
                var dateTemp = request.FromDate;
                request.FromDate = request.ToDate;
                request.ToDate = dateTemp;
            }

            if (request.WeatherTypes == null || !request.WeatherTypes.Any())
            {
                request.WeatherTypes = Enum.GetValues<WeatherType>();
            }
        }

    }
}
