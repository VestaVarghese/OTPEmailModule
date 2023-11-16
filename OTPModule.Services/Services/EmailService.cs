using Microsoft.Extensions.Configuration;
using OTPModule.Core.Interfaces.Services;
using System;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;

namespace OTPModule.Services.Services
{
    public class EmailService : IEmailService
    {
        private readonly IConfiguration _configuration;
        private readonly string password;
        private readonly string fromMailAddress;
        private readonly int port;
        private readonly SmtpClient smtpClient;

        public EmailService(IConfiguration configuration)
        {
            _configuration = configuration;
            password = _configuration["EmailSettings:Password"] ?? string.Empty;
            fromMailAddress = _configuration["EmailSettings:From"] ?? string.Empty;
            port = Convert.ToInt32(_configuration["EmailSettings:Port"]);
            smtpClient = new SmtpClient("smtp.gmail.com")
            {
                EnableSsl = true,
                DeliveryMethod = SmtpDeliveryMethod.Network,
                Credentials = new NetworkCredential(fromMailAddress, password),
                Port = port

            };
        }


        #region Public Methods

        public async Task<bool> SendEmailAsync(string body, string subject, string toEmail)
        {
            try
            {
                MailMessage mailMessage = new()
                {
                    From = new MailAddress(fromMailAddress),
                    Body = body,
                    Subject = subject
                };
                mailMessage.To.Add(toEmail);
                await smtpClient.SendMailAsync(mailMessage);
                return true;
            }
            catch (Exception)
            {

                return false;
            }
        }

        #endregion

    }
}
