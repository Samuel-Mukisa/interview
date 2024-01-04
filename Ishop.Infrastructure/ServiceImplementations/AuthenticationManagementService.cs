using System.Data;
using Ishop.Application.Services;
using Ishop.Domain.Entities;
using Microsoft.Extensions.Configuration;
using MySql.Data.MySqlClient;

namespace Ishop.Infrastructure.ServiceImplementations;

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

                    "INSERT INTO users(UserName,Email,Password,IsActive) VALUES ('" + registration.UserName +

                    "','" + registration.Email + "','" + registration.Password + "','" + registration.IsActive + "')",
                    conn);
            conn.Open();
            int i = cmd.ExecuteNonQuery();
            conn.Close();
            if (i > 0)
            {
                return 1;
            }
            else
            {
                return 0;
            }

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

                "SELECT * FROM users WHERE Email = '" + registration.Email + "' AND Password = '" +

                registration.Password + "' AND IsActive = 1", conn);
        DataTable dt = new DataTable();
        da.Fill(dt);
        if (dt.Rows.Count > 0)
        {
            return 1;
        }
        else
        {
            return 0;
        }
        
    }


}