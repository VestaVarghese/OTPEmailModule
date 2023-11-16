using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using OTPModule.Core.Models;


namespace OTPModule.DataAccess
{
    public class DBContext : DbContext
    {
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            IConfigurationRoot configuration = new ConfigurationBuilder()
            .AddJsonFile("appsettings.json")
            .Build();
            
            optionsBuilder.UseSqlServer(configuration["DBConnectionString"]);
        }

        public DbSet<OTP> OTP {get;set;}
    }
}
