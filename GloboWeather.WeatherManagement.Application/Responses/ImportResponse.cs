using System.Collections.Generic;

namespace GloboWeather.WeatherManagement.Application.Responses
{
    public class ImportResponse: BaseResponse
    {
        public IEnumerable<RowError> RowErrors { get; set; }
    }

    public class RowError
    {
        public int RowIndex { get; set; }
        public IEnumerable<string> ErrorMessage { get; set; }
    }
}