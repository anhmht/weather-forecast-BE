using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;
using Newtonsoft.Json;
using System.IO;
using System.Threading;
using GloboWeather.WeatherManagement.Application.Models.Mail;
using GloboWeather.WeatherManegement.Application.Contracts.Infrastructure;
using Microsoft.EntityFrameworkCore.Query.SqlExpressions;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace GloboWeather.WeatherManagement.Infrastructure.Mail
{
    public class GmailService : IEmailService
    {
        private readonly GmailSettings _gmailSettings;
        private readonly ILogger<EmailService> _logger;

        public GmailService(IOptions<GmailSettings> mailSettings, ILogger<EmailService> logger)
        {
            _gmailSettings = mailSettings.Value;
            _logger = logger;
        }

        public async Task<bool> SendAsync(string toEmail, string subject, string content
            , List<ReceiverAddress> listCc = null
            , List<ReceiverAddress> listBcc = null
            , List<string> attachments = null)
        {
            var to = new List<ReceiverAddress>
            {
                new ReceiverAddress
                {
                    Email = toEmail
                }
            };

            return await SendMultipleAsync(to, subject, content, listCc, listBcc, attachments);
        }

        public async Task<bool> SendMultipleAsync(List<ReceiverAddress> toEmails, string subject, string content
            , List<ReceiverAddress> listCc = null
            , List<ReceiverAddress> listBcc = null
            , List<string> attachments = null
            , CancellationToken cancellationToken = default)
        {
            try
            {
                //Set up allowing less secure apps to access gmail account
                //https://www.google.com/settings/security/lesssecureapps.
                if (string.IsNullOrEmpty(_gmailSettings.SenderEmail) || string.IsNullOrEmpty(_gmailSettings.SenderPassword))
                {
                    throw new Exception("Config Invalid!");
                }

                if (!toEmails.Any())
                {
                    throw new Exception("Email must send to some body. Please input email send to");
                }

                using SmtpClient smtp = new SmtpClient();
                var message = new MailMessage {From = new MailAddress(_gmailSettings.SenderEmail, _gmailSettings.SenderName) };

                foreach (var sendTo in toEmails)
                {
                    message.To.Add(new MailAddress(sendTo.Email, sendTo.DisplayName));
                }

                if (listCc?.Any() == true)
                {
                    foreach (var ccItem in listCc)
                    {
                        message.CC.Add(new MailAddress(ccItem.Email, ccItem.DisplayName));
                    }
                }

                if (listBcc?.Any() == true)
                {
                    foreach (var bccItem in listBcc)
                    {
                        message.CC.Add(new MailAddress(bccItem.Email, bccItem.DisplayName));
                    }
                }

                if (attachments?.Any() == true)
                {
                    foreach (var attachment in attachments)
                    {
                        if (File.Exists(attachment))
                        {
                            message.Attachments.Add(new Attachment(attachment));
                        }
                    }
                }

                message.Subject = subject;
                message.IsBodyHtml = true; //to make message body as html  
                message.Body = content;
                smtp.Port = _gmailSettings.Port;
                smtp.Host = _gmailSettings.Host;
                smtp.EnableSsl = true;
                smtp.UseDefaultCredentials = false;
                smtp.Credentials = new NetworkCredential(_gmailSettings.SenderEmail, _gmailSettings.SenderPassword);
                smtp.DeliveryMethod = SmtpDeliveryMethod.Network;
                await smtp.SendMailAsync(message, cancellationToken);
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex,
                    $"SendMail.Fail|Message={ex.Message},ToEmails={JsonConvert.SerializeObject(toEmails)}");
                return false;
            }
        }

        #region Implement interface
        public async Task SendEmail(Email email)
        {
            await SendAsync(email.To, email.Subject, email.Body);
        }

        public async Task SendEmailConfirmationAsync(string email, string callBackUrl)
        {
            var subject = "Confirm your email";
            var htmlContent = $"Please confirm your email by clicking here: <a href='{callBackUrl}'>link</a>";
            await SendAsync(email, subject, htmlContent).ConfigureAwait(false);
        }

        public async Task SendPasswordResetAsync(string email, string callBackUrl)
        {
            var subject = "Reset your password";
            var htmlContent = $"Please reset your password by clicking here: <a href='{callBackUrl}'>link</a>";
            await SendAsync(email, subject, htmlContent).ConfigureAwait(false);
        }

        public Task SendSqlException(SqlExpression ex)
        {
            throw new NotImplementedException();
        }

        public Task SendException(Exception ex)
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}
