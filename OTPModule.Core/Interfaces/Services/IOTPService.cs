using OTPModule.Core.Enums;
using System.Threading.Tasks;

namespace OTPModule.Core.Interfaces.Services
{
    public interface IOTPService
    {
        /// <summary>
        /// Generate and send the OTP to the specified email after validating it
        /// </summary>
        /// <param name="user_email"></param>
        /// <returns></returns>
        public Task<string> Generate_OTP_Email(string user_email);

        /// <summary>
        /// Check if the specified OTP is valid or not
        /// </summary>
        /// <param name="email"></param>
        /// <param name="otp"></param>
        /// <returns></returns>
        public Task<string> ValidateOTPAsync(string email, string otp);
    }
}
