using Ishop.Application.Services;
using Ishop.Domain.Entities;
using Microsoft.Extensions.Configuration;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ishop.Infrastructure.ServiceImplementations
{
    public class RetailerAuthenticationManagementService:IRetailerAuthenticationService
    {
        private readonly IConfiguration _configuration;
        
        

        public RetailerAuthenticationManagementService(IConfiguration configuration)
        {
            _configuration = configuration;
            
        }
        public async Task<int> CreateRetailer(RetailerRegistration registration)
        {
            try
            {
                MySqlConnection conn = new MySqlConnection(_configuration.GetConnectionString("MySqlConnection").ToString());

                string query = "INSERT INTO retailers (RetailerID,UserName, Email, Password, RetailerLocation, RetailerBusinessName, ReasonForSourcing,RetailerPhoneNumber,ImageURL) " +
                               "VALUES (@RetailerId, @RetaileruserName, @RetailerEmail, @RetailerPassword,  @RetailerLocation, @RetailerBusinessName, @ReasonforSourcing,@RetailerPhoneNumber,@ImageURL)";

                MySqlCommand cmd = new MySqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@RetailerId", registration.RetailerId);
                cmd.Parameters.AddWithValue("@RetaileruserName", registration.UserName);
                cmd.Parameters.AddWithValue("RetailerEmail", registration.RetailerEmail);
                cmd.Parameters.AddWithValue("@RetailerPassword", registration.Password);
                cmd.Parameters.AddWithValue("@RetailerLocation", registration.RetailerLocation);
                cmd.Parameters.AddWithValue("@RetailerBusinessName", registration.RetailerBusinessName);
                cmd.Parameters.AddWithValue("@ReasonforSourcing", registration.ReasonForSourcing);
                cmd.Parameters.AddWithValue("@RetailerPhoneNumber", registration.RetailerPhoneNumber);
                cmd.Parameters.AddWithValue("@ImageURL", registration.ImageUrl);
                conn.Open();
                int i = await cmd.ExecuteNonQueryAsync();
                conn.Close();

                return i > 0 ? 1 : 0;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        public async Task<int> LoginRetailer(string username, string password)
        {
            try
            {
                MySqlConnection conn = new MySqlConnection(_configuration.GetConnectionString("MySqlConnection").ToString());

                string query = "SELECT COUNT(*) FROM retailers WHERE UserName = @Username AND Password = @Password";

                MySqlCommand cmd = new MySqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@Username", username);
                cmd.Parameters.AddWithValue("@Password", password);
                conn.Open();

                int count = Convert.ToInt32(await cmd.ExecuteScalarAsync());

                conn.Close();

                // If count is greater than 0, the login is successful
                return count > 0 ? 1 : 0;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

    }
}   
    

    

