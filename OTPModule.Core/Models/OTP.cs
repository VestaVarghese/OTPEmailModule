using System;

namespace OTPModule.Core.Models
{
    public class OTP
    {
        public int Id { get; set; }
        public string EmailAddress { get; set; } = string.Empty;
        public string OTPCode { get; set; } = string.Empty;
        public int RetryCount { get; set; }
        public DateTime CreatedDate { get; set; }
    }
}
