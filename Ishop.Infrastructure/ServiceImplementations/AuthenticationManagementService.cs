using System;
using System.Data;
using System.Threading.Tasks;
using Ishop.Application.Services;
using Ishop.Domain.Entities;
using Microsoft.Extensions.Configuration;
using MySql.Data.MySqlClient;

namespace Ishop.Infrastructure.ServiceImplementations
{
    public class AuthenticationManagementService : IAuthenticationService
    {
        private readonly IConfiguration _configuration;

        public AuthenticationManagementService(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public async Task<int> CreateUser(Registration registration)
        {
            try
            {
                MySqlConnection conn =
                    new MySqlConnection(_configuration.GetConnectionString("MySqlConnection").ToString());
                MySqlCommand cmd =
                    new MySqlCommand(
                        "INSERT INTO manufacturers (UserName, Email, Password, IsActive, BusinessName, AddressCity, AddressCountry, TypeOfProducts, BusinessRegistrationNumber, PhoneNumber, imageUrl) VALUES " +
                        "(@UserName, @Email, @Password, @IsActive, @BusinessName, @AddressCity, @AddressCountry, @TypeOfProducts, @BusinessRegistrationNumber, @PhoneNumber, @imageUrl)",
                        conn);

                // Add parameters to prevent SQL injection
                cmd.Parameters.AddWithValue("@UserName", registration.UserName);
                cmd.Parameters.AddWithValue("@Email", registration.Email);
                cmd.Parameters.AddWithValue("@Password", registration.Password);
                cmd.Parameters.AddWithValue("@IsActive", registration.IsActive);
                cmd.Parameters.AddWithValue("@BusinessName", registration.BusinessName);
                cmd.Parameters.AddWithValue("@AddressCity", registration.AddressCity);
                cmd.Parameters.AddWithValue("@AddressCountry", registration.AddressCountry);
                cmd.Parameters.AddWithValue("@TypeOfProducts", registration.TypeOfProducts);
                cmd.Parameters.AddWithValue("@BusinessRegistrationNumber", registration.BusinessRegistrationNumber);
                cmd.Parameters.AddWithValue("@PhoneNumber", registration.PhoneNumber);
                cmd.Parameters.AddWithValue("@imageUrl", registration.imageUrl);

                conn.Open();
                int i = cmd.ExecuteNonQuery();
                conn.Close();

                return i > 0 ? 1 : 0;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        public async Task<int> LoginUser(Registration registration)
        {
            MySqlConnection conn =
                new MySqlConnection(_configuration.GetConnectionString("MySqlConnection").ToString());
            conn.Open();
            MySqlDataAdapter da =
                new MySqlDataAdapter(
                    "SELECT * FROM manufacturers WHERE Email = @Email AND Password = @Password AND IsActive = 1",
                    conn);

            // Add parameters to prevent SQL injection
            da.SelectCommand.Parameters.AddWithValue("@Email", registration.Email);
            da.SelectCommand.Parameters.AddWithValue("@Password", registration.Password);

            DataTable dt = new DataTable();
            da.Fill(dt);

            return dt.Rows.Count > 0 ? 1 : 0;
        }

        public async Task<Registration> GetManufacturerById(int id)
        {
            try
            {
                MySqlConnection conn =
                    new MySqlConnection(_configuration.GetConnectionString("MySqlConnection").ToString());
                conn.Open();

                MySqlCommand cmd = new MySqlCommand("SELECT * FROM manufacturers WHERE Id = @Id", conn);
                cmd.Parameters.AddWithValue("@Id", id);

                using (var reader = await cmd.ExecuteReaderAsync())
                {
                    if (reader.Read())
                    {
                        // Map the result to a Manufacturer object
                        var manufacturer = new Registration
                        {
                            Id = Convert.ToInt32(reader["Id"]),
                            UserName = reader["UserName"].ToString(),
                            Password = reader["Password"].ToString(),
                            Email = reader["Email"].ToString(),
                            IsActive = Convert.ToInt32(reader["IsActive"]),
                            BusinessName = reader["BusinessName"].ToString(),
                            AddressCity = reader["AddressCity"].ToString(),
                            AddressCountry = reader["AddressCountry"].ToString(),
                            TypeOfProducts = reader["TypeOfProducts"].ToString(),
                            BusinessRegistrationNumber = reader["BusinessRegistrationNumber"].ToString(),
                            PhoneNumber = reader["PhoneNumber"].ToString(),
                            imageUrl = reader["imageUrl"].ToString()
                        };

                        return manufacturer;
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }

            return null; // Return null if the manufacturer is not found
        }

    }
}
