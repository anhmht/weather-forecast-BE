using System.Threading.Tasks;
using GloboWeather.WeatherManagement.Application.Models.Mail;

namespace GloboWeather.WeatherManegement.Application.Contracts.Infrastructure
{
    public interface IEmailService
    {
        Task<bool> SendEmail(Email email);
    }
}