using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using GloboWeather.WeatherManagement.Application.Models.Mail;
using GloboWeather.WeatherManegement.Application.Contracts.Infrastructure;
using Microsoft.EntityFrameworkCore.Query.SqlExpressions;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using SendGrid;
using SendGrid.Helpers.Mail;

namespace GloboWeather.WeatherManagement.Infrastructure.Mail
{
    public class EmailService : IEmailService
    {
        public EmailSettings _emailSettings;
        public ILogger<EmailService> _logger;

        public EmailService(IOptions<EmailSettings> mailSettings, ILogger<EmailService> logger)
        {
            _emailSettings = mailSettings.Value;
            _logger = logger;
        }

        public async Task SendEmail(Email email)
        {
            await SendSendGirdMessage(_emailSettings.FromAddress, _emailSettings.FromName,
                    new List<EmailAddress> {new EmailAddress(email.To)}, email.Subject, email.Body)
                .ConfigureAwait(false);
        }

        public async Task SendEmailConfirmationAsync(string email, string callBackUrl)
        {
            var subject = "Confirm your email";
            var htmlContent = $"Please confirm your email by clicking here: <a href='{callBackUrl}'>link</a>";

            await SendSendGirdMessage(_emailSettings.FromAddress, _emailSettings.FromName,
                new List<EmailAddress> {new EmailAddress(email)}, subject, htmlContent).ConfigureAwait(false);
        }

        public async Task SendPasswordResetAsync(string email, string callBackUrl)
        {
            var subject = "Reset your password";
            var htmlContent = $"Please reset your password by clicking here: <a href='{callBackUrl}'>link</a>";
            await SendSendGirdMessage(_emailSettings.FromAddress, _emailSettings.FromName,
                new List<EmailAddress> {new EmailAddress(email)}, subject, htmlContent).ConfigureAwait(false);
        }

        public Task SendException(Exception ex)
        {
            throw new NotImplementedException();
        }

        public Task SendSqlException(SqlExpression ex)
        {
            throw new NotImplementedException();
        }
        
        private async Task SendSendGirdMessage(string fromAddress, string fromName, List<EmailAddress> tos,
            string subject,
            string htmlContent)
        {
            var client = new SendGridClient(_emailSettings.ApiKey);
            var from = new EmailAddress(htmlContent, fromName);
            var msg = MailHelper.CreateSingleEmailToMultipleRecipients(from, tos, subject, "", htmlContent, false);
            var response = await client.SendEmailAsync(msg).ConfigureAwait(false);
        }
    }
}