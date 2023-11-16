using OTPModule.Core.Models;
using System.Threading.Tasks;

namespace OTPModule.Core.Interfaces.Repositories
{
    public interface IOTPRepository
    {
        /// <summary>
        /// Insert or Update the OTP against the email in DB
        /// </summary>
        /// <param name="otp"></param>
        /// <param name="email"></param>
        /// <returns></returns>
        public Task SaveOTPAsync(string otp, string email);

        /// <summary>
        /// Get the OTP against the email for validation
        /// </summary>
        /// <param name="email"></param>
        /// <returns></returns>
        public Task<OTP> GetOTPRecordAsync(string email);

        /// <summary>
        /// Update the retry count if not a valid OTP. Delete if max retry count is reached
        /// </summary>
        /// <param name="email"></param>
        /// <returns>True if max retry count is reached, otherwise false</returns>
        public Task<bool> UpdateRetryCountAsync(string email);
    }
}
