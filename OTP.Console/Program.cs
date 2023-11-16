using Microsoft.Extensions.Configuration;
using OTPModule.Services.Services;
using OTPModule.DataAccess.Repositories;
using OTPModule.Core.Enums;
using OTP.Console;

#region Initializations
var config = new ConfigurationBuilder().AddJsonFile("appsettings.json").Build();
var oTPService = new OTPServices(new EmailService(config), new OTPRepository(), config);
Email_OTP_Module email_OTP_Module = new Email_OTP_Module(oTPService);
#endregion

#region Variables
string? user_email = "";
#endregion

try
{
    Console.WriteLine("Please enter the email for generating an OTP");
    user_email = Console.ReadLine();
    var result =  await email_OTP_Module.generate_OTP_email(user_email);
    Console.WriteLine(result);

    if(result == EnumEmailStatus.STATUS_EMAIL_OK.ToString())
    {
        Console.WriteLine("Please enter the OTP within the next 1 minute.");
        var task = Task.Run(() => readOTP());
        if (task.Wait(60000)) { }
        else
        {
            throw new TimeoutException();
        }

    }

    Console.ReadLine();
}
catch (TimeoutException)
{
    Console.WriteLine("Sorry, Timed Out!!!");
    Console.ReadLine();
}


// A method to read streaming input and call check_OTP method 
async Task readOTP()
{
    while (true)
    {
        string otp = Console.ReadLine();
        var resultOTP = await email_OTP_Module.check_OTP(user_email, otp);
        Console.WriteLine(resultOTP);
        if (resultOTP != EnumValidOTP.STATUS_OTP_FAIL.ToString())
        {
            break;
        }
        
    }
}

 