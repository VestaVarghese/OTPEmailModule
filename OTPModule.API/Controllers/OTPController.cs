using Azure;
using Azure.Core;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using NPOI.Util;
using OTPModule.Core.Enums;
using OTPModule.Core.Interfaces.Services;
using System.IO;
using System.Reflection.PortableExecutable;
using System.Text;

namespace OTPModule.API.Controllers
{
    [ApiController]
    [Route("api/[controller]/[action]")]
    public class OTPController : ControllerBase
    {
        private readonly IOTPService _oTPService;
        public OTPController(IOTPService oTPService)
        {
            _oTPService = oTPService;
        }
         
       /// <summary>
       /// Generate and send OTP to specified email address after validating it
       /// </summary>
       /// <param name="email"></param>
       /// <returns>Relevant status</returns>
       [HttpGet(Name = "SendOTP")]
       public async Task<ActionResult<string>> Generate_OTP_Email(string user_email)
        {
            try
            {
                return Ok(await _oTPService.Generate_OTP_Email(user_email));
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// Check if the entered OTP is valid or not
        /// </summary>
        /// <param name="email"></param>
        /// <param name="otp"></param>
        /// <returns>The relevant status</returns>
        [HttpGet(Name = "ValidateOTP")]
        public async Task<ActionResult<string>> Check_OTP(string email, string otp)
        {
            try
            {
                return Ok(await _oTPService.ValidateOTPAsync(email, otp));
            }
            catch (Exception)
            {
                throw;
            }
        }
 
    }
}
