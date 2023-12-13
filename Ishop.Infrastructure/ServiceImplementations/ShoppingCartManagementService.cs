using Ishop.Application.Services;
using Ishop.Domain.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System.Data.SqlClient;
using MySql.Data.MySqlClient;
using Serilog;
using Dapper;
using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ishop.Application.Interfaces;
using System.Drawing.Text;

namespace Ishop.Infrastructure.ServiceImplementations
{
    public class ShoppingCartManagementService : IShoppingCartService
    {
        private readonly IConfiguration _configuration;
        private readonly SqlConnectionStringBuilder _builder;

        public ShoppingCartManagementService(IConfiguration configuration)
        {
            _configuration = configuration;
            string connectionString = _configuration.GetConnectionString("MySqlConnection");
             if (string.IsNullOrEmpty(connectionString))
                {
                    throw new InvalidOperationException("Connection string 'MySqlConnection' not found in the configuration.");
                }
            // Update the connection string to include Windows authentication and specify the database
             var  builder = new SqlConnectionStringBuilder(connectionString)
                {
                       IntegratedSecurity = true, // Use Windows authentication
                       TrustServerCertificate = true, // Validate the server's SSL certificate
                       InitialCatalog = "ShoppingCart"
                };
                 _builder = builder;
        }
        
              
     

        public async Task<List<CartItem>> GetCartItems()
        {
            try
            {
                using (var conn = new SqlConnection(_builder.ConnectionString))
                {
                    conn.Open();

                    string sql = "SELECT * FROM CartTable2";

                    // Use Dapper to execute the query and map the result to the CartItem class
                    var cartItems = await conn.QueryAsync<CartItem>(sql);

                    return cartItems.ToList();
                }
            }
            catch (Exception ex)
            {
                Log.Error($"Error getting cartItems: {ex.Message}");
                return null; // Return null to indicate failure
            }
        }

        public async Task<bool> DeleteCartItem(int cartid)
        {
            try
            {
                using (var conn = new SqlConnection(_builder.ConnectionString))
                {
                    await conn.OpenAsync(); // Use async Open
                    string sql = "DELETE FROM CartTable2 WHERE ProductId = @cartid";
                    int affectedRows = await conn.ExecuteAsync(sql, new { cartid = cartid });
                    return affectedRows > 0; // True if deleted, False otherwise
                }
            }
            catch (Exception ex)
            {
                Log.Error($"Error deleting cartItem with CartId {cartid}: {ex.Message}");
                return false; // False on error
            }
        }

        public async Task<CartItem> GetCartItem(int id)
        {
            try
            {
                using (var conn = new SqlConnection(_builder.ConnectionString))
                {
                    conn.Open();
                    string sql = "SELECT * FROM CartTable2 WHERE ProductId = @id";
                    var cartitem = await conn.QuerySingleAsync<CartItem>(sql, new { id = id });
                    return cartitem;
                }
            }
            catch (Exception ex)
            {
                Log.Error($"Error getting category with ID {id}: {ex.Message}");
                return null; // Return null to indicate failure
            }
        }

        public async Task<bool> PostCartItem(CartItem cartItem)
        {
            try
            {
                using (var conn = new SqlConnection(_builder.ConnectionString))
                {
                    await conn.OpenAsync();

                    string sql = @"
                INSERT INTO CartTable2 (ProductId, ProductName, ProductImage, ProductDescription, Quantity, Price, DateCreated)
                VALUES (@ProductId,@ProductName, @ProductImage, @ProductDescription, @Quantity, @Price, @DateCreated);
            ";

                    int affectedRows = await conn.ExecuteAsync(sql, cartItem);

                    return affectedRows > 0; // True if inserted, False otherwise
                }
            }
            catch (Exception ex)
            {
                Log.Error($"Error inserting cartItem: {ex.Message}");
                return false; // False on error
            }
        }

    }
}





