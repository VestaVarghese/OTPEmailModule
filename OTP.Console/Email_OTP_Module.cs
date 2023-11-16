using OTPModule.Core.Interfaces.Services;

namespace OTP.Console
{

    public class Email_OTP_Module
    {
        private readonly IOTPService _otpService;  
        public Email_OTP_Module(IOTPService oTPService)
        {
            _otpService = oTPService;
        }

        /// <summary>
        /// Generate and send email to the email specified
        /// </summary>
        /// <param name="user_email"></param>
        /// <returns></returns>
        public async Task<string> generate_OTP_email(string user_email = "")
        {
            var result = await _otpService.Generate_OTP_Email(user_email);
            return result;
        }

        /// <summary>
        /// Validate if the otp is correct or not
        /// </summary>
        /// <param name="user_email"></param>
        /// <param name="otp"></param>
        /// <returns></returns>
        public async Task<string> check_OTP(string user_email, string otp)
        {
            var result = await _otpService.ValidateOTPAsync(user_email, otp);
            return result;
        }
    }
}
