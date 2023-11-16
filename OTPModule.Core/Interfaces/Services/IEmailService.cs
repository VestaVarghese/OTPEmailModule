using System.Threading.Tasks;

namespace OTPModule.Core.Interfaces.Services
{
    public interface IEmailService
    {
        /// <summary>
        /// Send email via SMTP server
        /// </summary>
        /// <param name="body"></param>
        /// <param name="subject"></param>
        /// <param name="toEmail"></param>
        /// <returns></returns>
        public Task<bool> SendEmailAsync(string body, string subject, string toEmail);
    }
}
