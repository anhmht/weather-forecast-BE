namespace GloboWeather.WeatherManagement.Application.Features.Commons.Queries.GetPositionStackLocation
{
    public class GetPositionStackLocationCommand
    {
        public  decimal Lat { get; set; }
        public  decimal Lon { get; set; }
        
        public string IpAddress { get; set; }
    }  
}