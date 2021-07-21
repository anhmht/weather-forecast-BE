using System;
using System.Globalization;
using System.Threading.Tasks;
using GloboWeather.WeatherManagement.Application.Models.Mail;
using Microsoft.EntityFrameworkCore.Query.SqlExpressions;

namespace GloboWeather.WeatherManegement.Application.Contracts.Infrastructure
{
    public interface IEmailService
    {
        Task SendEmail(Email email);
        Task SendEmailConfirmationAsync(string email, string callBackUrl);

        Task SendPasswordResetAsync(string email, string callBackUrl);

        Task SendException(Exception ex);
        Task SendSqlException(SqlExpression ex);
    }
}