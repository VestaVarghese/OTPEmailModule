using OTPModule.Core.Interfaces.Repositories;
using OTPModule.Core.Interfaces.Services;
using OTPModule.Services.Services;
using OTPModule.DataAccess.Repositories;
namespace OTPModule.API
{
    public class Startup
    {
        public void ConfigurationServices(IServiceCollection services)
        {
            services.AddSingleton<IEmailService, EmailService>();
            services.AddScoped<IOTPService, OTPServices>();
            services.AddScoped<IOTPRepository, OTPRepository>();
        }
    }
}
