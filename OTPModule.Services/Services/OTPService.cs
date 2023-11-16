using Microsoft.Extensions.Configuration;
using OTPModule.Core.Enums;
using OTPModule.Core.Interfaces.Repositories;
using OTPModule.Core.Interfaces.Services;
using OTPModule.Core.Models;
using System;
using System.Linq;
using System.Net.Mail;
using System.Threading.Tasks;
namespace OTPModule.Services.Services
{
    public class OTPServices : IOTPService
    {
        private readonly IEmailService _emailServices;
        private readonly IOTPRepository _otpRepository;
        private readonly IConfiguration _configuration;

        #region Public_Methods
        public OTPServices(IEmailService emailServices, IOTPRepository otpRepository, IConfiguration configuration)
        {
            _emailServices = emailServices;
            _otpRepository = otpRepository;
            _configuration = configuration;
        }

        /// <summary>
        /// Generate and send OTP to specified email address after validating it
        /// </summary>
        /// <param name="email"></param>
        public async Task<string> Generate_OTP_Email(string email)
        {
            try
            {
                bool isValid = ValidateEmailAddress(email);
                if (isValid)
                {
                    var otp = GenerateOTP();
                    var isSent = await SendEmailAsync(email, otp);
                    if (isSent)
                    {
                        await _otpRepository.SaveOTPAsync(otp, email);
                        return (EnumEmailStatus.STATUS_EMAIL_OK).ToString();
                    }
                    else
                    {
                        return (EnumEmailStatus.STATUS_EMAIL_FAIL).ToString();
                    }
                }
                return (EnumEmailStatus.STATUS_EMAIL_INVALID).ToString();
            }
            catch (Exception)
            {
                return (EnumEmailStatus.STATUS_EMAIL_FAIL).ToString();
            }
        }

        /// <summary>
        /// Check if the OTP entered is valid or not
        /// </summary>
        /// <param name="otp"></param>
        /// <returns></returns>
        public async Task<string> ValidateOTPAsync(string email, string otp)
        {
            if (ValidateEmailAddress(email))
            {
                var savedOTP = await GetOTPRecord(email);
                if (savedOTP?.OTPCode == otp)
                {
                    if (DateTime.UtcNow > savedOTP?.CreatedDate.AddMinutes(1))
                    {
                        return EnumValidOTP.STATUS_OTP_TIMEOUT.ToString();
                    }
                    return EnumValidOTP.STATUS_OTP_OK.ToString(); ;
                }
                bool hasMaxRetryCountReached = await _otpRepository.UpdateRetryCountAsync(email);
                if (hasMaxRetryCountReached) { return EnumValidOTP.Status_OTP_MaxRetryCountReached.ToString(); };
            }
            return EnumValidOTP.STATUS_OTP_FAIL.ToString(); ;
        }

        #endregion

        #region Internal_Methods

        /// <summary>
        /// Validate the email address to which the OTP has to be sent
        /// </summary>
        /// <param name="email"></param>
        /// <returns>True if Valid, False Otherwise</returns>
        internal bool ValidateEmailAddress(string email)
        {
            bool valid;
            try
            {
                var emailAddress = new MailAddress(email);
                valid = HasValidDomain(email);
            }
            catch
            {
                valid = false;
            }
            return valid;
        }

        /// <summary>
        /// Validate if the email has a valid domain
        /// </summary>
        /// <param name="email"></param>
        /// <returns></returns>
        internal bool HasValidDomain(string email)
        {
            var domains = _configuration.GetSection("EmailSettings:ValidDomains").GetChildren().ToList();
            var validDomains = domains.Select(xx => xx.Value).ToList();
            var atCharPosition = email.LastIndexOf('@');
            if (atCharPosition > 0)
            {
                var domain = email.Substring(atCharPosition + 1, email.Length - atCharPosition - 1);
                if (validDomains.IndexOf(domain) > -1) return true;
                return false;
            }
            return false;
        }
        /// <summary>
        /// Generate a 6 digita OTP
        /// </summary>
        /// <returns>Generated OTP</returns>
        /// 
        internal static string GenerateOTP()
        {
            try
            {
                Random random = new();
                var r = random.Next(0, 1000000).ToString("D6");
                return r;
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// Send OTP to the specified email address
        /// </summary>
        /// <param name="email"></param>
        /// <param name="otp"></param>
        /// <returns>True if email is sent, False otherwise</returns>
        internal async Task<bool> SendEmailAsync(string email, string otp)
        {
            try
            {
                var body = $"Your OTP code is {otp}. The code is valid for 1 minute";
                var subject = "OTP for login";
                return await _emailServices.SendEmailAsync(body, subject, email);
            }
            catch (Exception)
            {
                return false;
            }
        }

        /// <summary>
        /// Get the OTP record against the specified email
        /// </summary>
        /// <param name="email"></param>
        /// <returns></returns>
        internal async Task<OTP> GetOTPRecord(string email)
        {
            var savedOTP = await _otpRepository.GetOTPRecordAsync(email);
            return savedOTP;
        }

        #endregion
    }
}


