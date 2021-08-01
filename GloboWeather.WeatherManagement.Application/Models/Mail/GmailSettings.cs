namespace GloboWeather.WeatherManagement.Application.Models.Mail
{
    public class GmailSettings
    {
        public string SenderEmail { get; set; }
        public string SenderPassword { get; set; }
        public string SenderName { get; set; }
        public string Host { get; set; }
        public int Port { get; set; }
    }
}