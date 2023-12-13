using Dapper;
using Ishop.Application.Services;
using Ishop.Domain.Entities;
using Microsoft.Extensions.Configuration;
using MySql.Data.MySqlClient;
using Serilog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ishop.Infrastructure.Services
{
    public class CustomerRegistrationService : ICustomerRegistrationService

    {
        private readonly IConfiguration _configuration;
        public CustomerRegistrationService(IConfiguration configuration)
        {
            _configuration= configuration;
        }
        public async  Task<int> RegisterCustomer(CustomerSignUp customerSignUp)
        {
            try
            {
                using (var conn = new MySqlConnection(_configuration.GetConnectionString("MySqlConnection")))
                {
                    conn.Open();
                    string sql = "INSERT INTO customers (EmailAddress,UserName, Password) VALUES ( @EmailAddress,@UserName, @Password)";
                    await conn.ExecuteAsync(sql, new { EmailAddress = customerSignUp.EmaiAddress, UserName = customerSignUp.UserName, Password = customerSignUp.Password });
                }

            }
            catch (Exception ex)
            {
                Log.Information(ex.Message);
            }

            return 1;
        }
    }
}
