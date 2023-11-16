using Microsoft.Extensions.Configuration;
using OTPModule.Services.Services;
using OTPModule.Core.Enums;
using OTPModule.DataAccess.Repositories;

namespace OTPModule.Test
{
    public class OTPServicesTest
   {
       private OTPServices OTPServices { get; set; } = null!;
       [SetUp]
       public void Setup()
       {
           var config = new ConfigurationBuilder().AddJsonFile("appsettings.json").Build();
           OTPServices = new OTPServices(new EmailService(config), new OTPRepository(), config);
       }
       [Test]
       public void HasValidDomain_SuccessTest()
       {
           //Assign
           string email = "ABC@dso.sg.org";
           //Act
           bool v = OTPServices.HasValidDomain(email);
           //Assert
           Assert.That(v, Is.EqualTo(true));
       }
       [Test]
       public void HasValidDomain_FailTest()
       {
           //Assign
           string email = "ABC@yahoo.com";
           //Act
           bool v = OTPServices.HasValidDomain(email);
           //Assert
           Assert.That(v, Is.EqualTo(false));
       }
       [Test]
       public void ValidateEmailAddress_SuccessTest()
       {
           //Assign
           string email = "ABC@dso.sg.org";
           //Act
           bool v = OTPServices.ValidateEmailAddress(email);
           //Assert
           Assert.That(v, Is.EqualTo(true));
       }
       [Test]
       public void ValidateEmailAddressn_FailTest()
       {
           //Assign
           string email = "ABC";
           //Act
           bool v = OTPServices.ValidateEmailAddress(email);
           //Assert
           Assert.That(v, Is.EqualTo(false));
       }
       [Test]
       public void GenerateOTP_SuccessTest()
       {
           //Assign
           //Act
           string v = OTPServices.GenerateOTP();
           bool v2 = v.Length == 6;
           //Assert
           Assert.That(v2, Is.EqualTo(true));
       }
       [Test]
       public async Task SendOTP_SuccessTest()
       {
           //Assign
           string email = "ABC@dso.sg.org";
           //Act
           var status = await OTPServices.Generate_OTP_Email(email);
           //Assert
           Assert.That(status, Is.EqualTo(EnumEmailStatus.STATUS_EMAIL_OK.ToString()));
       }
       [Test]
       public async Task SendOTP_FailureTest()
       {
           //Assign
           string email = "ABCdso.sg.org";
           //Act
           var status = await OTPServices.Generate_OTP_Email(email);
           //Assert
           Assert.That(status, Is.EqualTo(EnumEmailStatus.STATUS_EMAIL_INVALID.ToString()));
       }
       [Test]
       public async Task ValidateOTP_SuccessTest()
       {
           //Assign
           string email = "ABC@dso.sg.org";
           //Act
           await OTPServices.Generate_OTP_Email(email);
           var record = await OTPServices.GetOTPRecord(email);
           var status = await OTPServices.ValidateOTPAsync(email, record?.OTPCode);
           //Assert
           Assert.That(status, Is.EqualTo(EnumValidOTP.STATUS_OTP_OK.ToString()));
       }
       [Test]
       public async Task ValidateOTP_FailTest()
       {
           //Assign
           string email = "ABC@dso.sg.org";
           //Act
           await OTPServices.Generate_OTP_Email(email);
           var status = await OTPServices.ValidateOTPAsync(email, "1");
           //Assert
           Assert.That(status, Is.EqualTo(EnumValidOTP.STATUS_OTP_FAIL.ToString()));
       }
       [Test]
       public async Task ValidateOTP_TimeOutTest()
       {
           //Assign
           string email = "ABC@dso.sg.org";
           //Act
           await OTPServices.Generate_OTP_Email(email);
           var record = await OTPServices.GetOTPRecord(email);
           await Task.Delay(TimeSpan.FromMinutes(2));
           var status = await OTPServices.ValidateOTPAsync(email, record?.OTPCode);
           //Assert
           Assert.That(status, Is.EqualTo(EnumValidOTP.STATUS_OTP_TIMEOUT.ToString()));
       }
       [Test]
       public async Task ValidateOTP_RetryCountTest()
       {
           //Assign
           string email = "ABC@dso.sg.org";
           //Act
           await OTPServices.Generate_OTP_Email(email);
           for (int i = 1; i < 10; i++)
            {
               await OTPServices.ValidateOTPAsync(email, "1");
           }
           var status = await OTPServices.ValidateOTPAsync(email, "1");
           //Assert
           Assert.That(status, Is.EqualTo(EnumValidOTP.Status_OTP_MaxRetryCountReached.ToString()));
       }
   }
}