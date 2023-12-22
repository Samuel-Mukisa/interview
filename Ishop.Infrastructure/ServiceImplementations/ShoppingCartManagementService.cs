using Ishop.Application.Services;
using Ishop.Domain.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using MySql.Data.MySqlClient;
using Serilog;
using Dapper;
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
        private readonly MySqlConnection _connection;

        public ShoppingCartManagementService(IConfiguration configuration)
        {
            _configuration = configuration;
            string connectionString = _configuration.GetConnectionString("MySqlConnection");
            if (string.IsNullOrEmpty(connectionString))
            {
                throw new InvalidOperationException("Connection string 'MySqlConnection' not found in the configuration.");
            }

            _connection = new MySqlConnection(connectionString);
        }

        public async Task<List<CartItem>> GetCartItems()
        {
            try
            {
                await _connection.OpenAsync();

                string sql = "SELECT * FROM CartTable2";

                var cartItems = await _connection.QueryAsync<CartItem>(sql);

                return cartItems.ToList();
            }
            catch (Exception ex)
            {
                Log.Error($"Error getting cart items: {ex.Message}");
                return null;
            }
            finally
            {
                _connection.Close();
            }
        }

        public async Task<bool> DeleteCartItem(int cartid)
        {
            try
            {
                await _connection.OpenAsync();

                string sql = "DELETE FROM CartTable2 WHERE ProductId = @cartid";
                int affectedRows = await _connection.ExecuteAsync(sql, new { cartid = cartid });

                return affectedRows > 0;
            }
            catch (Exception ex)
            {
                Log.Error($"Error deleting cart item with ProductId {cartid}: {ex.Message}");
                return false;
            }
            finally
            {
                _connection.Close();
            }
        }

        public async Task<CartItem> GetCartItem(int id)
        {
            try
            {
                await _connection.OpenAsync();

                string sql = "SELECT * FROM CartTable2 WHERE ProductId = @id";
                var cartitem = await _connection.QuerySingleAsync<CartItem>(sql, new { id = id });

                return cartitem;
            }
            catch (Exception ex)
            {
                Log.Error($"Error getting cart item with ProductId {id}: {ex.Message}");
                return null;
            }
            finally
            {
                _connection.Close();
            }
        }

        public async Task<bool> PostCartItem(CartItem cartItem)
        {
            try
            {
                await _connection.OpenAsync();

                string sql = @"
                    INSERT INTO CartTable2 (ProductId, ProductName, ProductImage, ProductDescription, Quantity, Price, DateCreated)
                    VALUES (@ProductId, @ProductName, @ProductImage, @ProductDescription, @Quantity, @Price, @DateCreated);
                ";

                int affectedRows = await _connection.ExecuteAsync(sql, cartItem);

                return affectedRows > 0;
            }
            catch (Exception ex)
            {
                Log.Error($"Error inserting cart item: {ex.Message}");
                return false;
            }
            finally
            {
                _connection.Close();
            }
        }
    }
}
