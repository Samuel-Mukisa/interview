using Dapper;
using Ishop.Application.Interfaces;
using Ishop.Application.Services;
using Ishop.Domain.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using MySql.Data.MySqlClient;
using Serilog;

namespace Ishop.Infrastructure.ServiceImplementations;

public class ProductManagementService : IProductManagementService
{
    private readonly IConfiguration _configuration;

    public ProductManagementService(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    public async Task<int> CreateProduct(Product product)
    {
        try
        {
            using (var conn = new MySqlConnection(_configuration.GetConnectionString("MySqlConnection")))
            {
                conn.Open();
            
                string sql = @"INSERT INTO products 
                            (Name, Description, Category, Price, Currency, ImageURL, VideoURL, Color, Size, Material, Weight, ReturnAndWarranty, StockQuantity, StockThreshold, ManufacturerID, Rating) 
                            VALUES 
                            (@Name, @Description, @Category, @Price, @Currency, @ImageURL, @VideoURL, @Color, @Size, @Material, @Weight, @ReturnAndWarranty, @StockQuantity, @StockThreshold, @ManufacturerID, @Rating)";

                // Use Dapper to execute the query with parameters
                await conn.ExecuteAsync(sql, new
                {
                    product.Name,
                    product.Description,
                    product.Category,
                    product.Price,
                    product.Currency,
                    product.ImageURL,
                    product.VideoURL,
                    product.Color,
                    product.Size,
                    product.Material,
                    product.Weight,
                    product.ReturnAndWarranty,
                    product.StockQuantity,
                    product.StockThreshold,
                    product.ManufacturerID,
                    product.Rating
                });
            }
        }
        catch (Exception ex)
        {
            Log.Error($"Error adding product: {ex.Message}");
            return 0; // Return 0 to indicate failure
        }

        return 1; // Return 1 to indicate success
    }

    public async Task<List<Product>> GetAllProducts()
    {
        try
        {
            using (var conn = new MySqlConnection(_configuration.GetConnectionString("MySqlConnection")))
            {
                conn.Open();
                string sql = "SELECT * FROM products";
                var products = await conn.QueryAsync<Product>(sql);
                return products.AsList();
            }
        }
        catch (Exception ex)
        {
            Log.Error($"Error getting products: {ex.Message}");
            return null; // Return null to indicate failure
        }
    }

    public async Task<Product> GetProduct(int id)
    {
        try
        {
            using (var conn = new MySqlConnection(_configuration.GetConnectionString("MySqlConnection")))
            {
                conn.Open();
                string sql = "SELECT * FROM products WHERE ProductID = @id";
                var product = await conn.QuerySingleAsync<Product>(sql, new { id });
                return product;
            }
        }
        catch (Exception ex)
        {
            Log.Error($"Error getting product with ID {id}: {ex.Message}");
            return null; // Return null to indicate failure
        }
    }

    public async Task<bool> DeleteProduct(int id)
    {
        try
        {
            using (var conn = new MySqlConnection(_configuration.GetConnectionString("MySqlConnection")))
            {
                conn.Open();
                string sql = "DELETE FROM products WHERE ProductID = @id";
                int affectedRows = await conn.ExecuteAsync(sql, new { id });
                return affectedRows > 0; // True if deleted, False otherwise
            }
        }
        catch (Exception ex)
        {
            Log.Error($"Error deleting product with ID {id}: {ex.Message}");
            return false; // False on error
        }
    }

    public async Task<bool> UpdateProduct(int id, [FromBody] Product updatedProduct)
    {
        try
        {
            using (var conn = new MySqlConnection(_configuration.GetConnectionString("MySqlConnection")))
            {
                conn.Open();

                string sql = "UPDATE products SET ";
                var parameters = new Dictionary<string, object>();

                // Update properties if not null
                if (updatedProduct.Name != null)
                {
                    sql += "Name = @Name, ";
                    parameters.Add("Name", updatedProduct.Name);
                }

                if (updatedProduct.Description != null)
                {
                    sql += "Description = @Description, ";
                    parameters.Add("Description", updatedProduct.Description);
                }

                // Add other properties to update as needed

                // Remove trailing comma and space if any updates were made
                if (parameters.Count > 0)
                {
                    sql = sql.TrimEnd(',', ' ');
                }

                sql += " WHERE ProductID = @ProductID";
                parameters.Add("ProductID", id);

                int affectedRows = await conn.ExecuteAsync(sql, parameters);
                return affectedRows > 0; // True if updated, False otherwise
            }
        }
        catch (Exception ex)
        {
            Log.Error($"Error updating product with ID {id}: {ex.Message}");
            return false; // False on error
        }
    }
}
