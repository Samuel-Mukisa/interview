using Dapper;
using Ishop.Application.Services;
using Ishop.Domain.Entities;
using Microsoft.Extensions.Configuration;
using MySql.Data.MySqlClient;
using Serilog;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Ishop.Application.Services;

namespace Ishop.Infrastructure.ServiceImplementations;

public class CategoryManagementService : ICategoryManagementService
{
    private readonly IConfiguration _configuration;

    public CategoryManagementService(IConfiguration configuration)
    {
        _configuration = configuration;
    } 
    public async Task<int> CreateCategory(string categoryName)
    {
        try
        {
            using (var conn = new MySqlConnection(_configuration.GetConnectionString("MySqlConnection")))
            {
                conn.Open();
                string sql = "INSERT INTO CategoryTable (CategoryName) VALUES (@categoryName)";
                await conn.ExecuteAsync(sql, new { CategoryName = categoryName});
            }
        }
        catch (Exception ex)
        {
            Log.Error($"Error adding category: {ex.Message}");
            return 0; // Return 0 to indicate failure
        }

        return 1; // Return 1 to indicate success
    }

    public async Task<List<Category>> GetAllCategories()
    {
        try
        {
            using (var conn = new MySqlConnection(_configuration.GetConnectionString("MySqlConnection")))
            {
                conn.Open();
                string sql = "SELECT * FROM CategoryTable";
                var categories = await conn.QueryAsync<Category>(sql);
                return categories.AsList();
            }
        }
        catch (Exception ex)
        {
            Log.Error($"Error getting categories: {ex.Message}");
            return null; // Return null to indicate failure
        }
    }
    public async Task<Category> GetCategory(int id)
    {
        try
        {
            using (var conn = new MySqlConnection(_configuration.GetConnectionString("MySqlConnection")))
            {
                conn.Open();
                string sql = "SELECT * FROM CategoryTable WHERE CategoryId = @id";
                var category = await conn.QuerySingleAsync<Category>(sql, new { id = id });
                return category;
            }
        }
        catch (Exception ex)
        {
            Log.Error($"Error getting category with ID {id}: {ex.Message}");
            return null; // Return null to indicate failure
        }
    }
    public async Task<bool> DeleteCategory(int id)
    {
        try
        {
            using (var conn = new MySqlConnection(_configuration.GetConnectionString("MySqlConnection")))
            {
                conn.Open();
                string sql = "DELETE FROM CategoryTable WHERE CategoryId = @id";
                int affectedRows = await conn.ExecuteAsync(sql, new { id = id });
                return affectedRows > 0; // True if deleted, False otherwise
            }
        }
        catch (Exception ex)
        {
            Log.Error($"Error deleting category with ID {id}: {ex.Message}");
            return false; // False on error
        }
    }
    
    public async Task<bool> UpdateCategory(int id, Category updatedCategory)
    {
        try
        {
            using (var conn = new MySqlConnection(_configuration.GetConnectionString("MySqlConnection")))
            {
                conn.Open();

                string sql = "UPDATE CategoryTable SET ";
                var parameters = new Dictionary<string, object>();

                // Update CategoryName if not null
                if (updatedCategory.CategoryName != null)
                {
                    sql += "CategoryName = @CategoryName, ";
                    parameters.Add("CategoryName", updatedCategory.CategoryName);
                }

                // Remove trailing comma and space if any updates were made
                if (parameters.Count > 0)
                {
                    sql = sql.TrimEnd(',', ' ');
                }

                sql += " WHERE CategoryId = @CategoryId";
                parameters.Add("CategoryId", id);

                int affectedRows = await conn.ExecuteAsync(sql, parameters);
                return affectedRows > 0; // True if updated, False otherwise
            }
        }
        catch (Exception ex)
        {
            Log.Error($"Error updating category with ID {id}: {ex.Message}");
            return false; // False on error
        }
    }
}