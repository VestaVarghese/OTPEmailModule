using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using OTPModule.Core.Interfaces.Repositories;
using OTPModule.Core.Models;

namespace OTPModule.DataAccess.Repositories
{
    public class OTPRepository : IOTPRepository
    {
        private const int MaxRetryCount = 10;
     
        /// <summary>
        /// Get the OTP against email for validation
        /// </summary>
        /// <param name="email"></param>
        /// <returns></returns>
        public async Task<OTP> GetOTPRecordAsync(string email)
        {
            using DBContext dBContext = new ();
            return await dBContext.OTP.FirstAsync(x => x.EmailAddress == email);
        }

        /// <summary>
        /// Insert or Update the OTP against the email in DB
        /// </summary>
        /// <param name="otp"></param>
        /// <param name="email"></param>
        /// <returns></returns>

        public async Task SaveOTPAsync(string otp, string email)
        {
            using DBContext dbContext = new ();
            var record = await dbContext.OTP.FirstOrDefaultAsync(x => x.EmailAddress == email);
            if (record != null)
            {
                record.OTPCode = otp;
                record.CreatedDate = DateTime.UtcNow;
                record.RetryCount = 0;
                dbContext.Update(record);
            }
            else
            {
                var otpRecord = new OTP() { EmailAddress = email, OTPCode = otp, CreatedDate = DateTime.UtcNow, RetryCount = 0 };
                dbContext.Add(otpRecord);
            }
            await dbContext.SaveChangesAsync();
        }

        /// <summary>
        /// Update teh retry count if not a valid OTP. Delete if max retry count is reached
        /// </summary>
        /// <param name="email"></param>
        /// <returns>True if max retry count is reached, otherwise false</returns>
        public async Task<bool> UpdateRetryCountAsync(string email)
        {
            using DBContext dBContext = new();
            var record = dBContext.OTP.FirstOrDefault(x => x.EmailAddress == email);
            if(record != null)
            {
                if(record.RetryCount +1 == MaxRetryCount)
                {
                    dBContext.Remove(record);
                    await dBContext.SaveChangesAsync();
                    return true;
                }
                else
                {
                    record.RetryCount = record.RetryCount + 1;
                    dBContext.Update(record);
                    await dBContext.SaveChangesAsync();
                }
            }
            return false;
        }
    }
}
