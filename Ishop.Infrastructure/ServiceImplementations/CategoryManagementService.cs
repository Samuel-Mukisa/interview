using Dapper;
using Ishop.Application.Services;
using Ishop.Domain.Entities;
using Microsoft.Extensions.Configuration;
using MySql.Data.MySqlClient;
using Serilog;


namespace Ishop.Infrastructure.ServiceImplementations
{
    public class CategoryManagementService : ICategoryManagementService
    {
        private readonly IConfiguration _configuration;

        public CategoryManagementService(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public async Task<int> CreateCategory(Category category)
        {
            try
            {
                using (var conn = new MySqlConnection(_configuration.GetConnectionString("MySqlConnection")))
                {
                    conn.Open();

                    string sql = "INSERT INTO categories (CategoryName, Description, NumberOfProducts, ImageVideoURL) " +
                                 "VALUES (@CategoryName, @Description, @NumberOfProducts, @ImageVideoURL)";

                    await conn.ExecuteAsync(sql, category);
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

                    string sql = "SELECT * FROM categories";

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

                    string sql = "SELECT * FROM categories WHERE CategoryId = @id";

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

                    string sql = "DELETE FROM categories WHERE CategoryId = @id";

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

                    string sql = "UPDATE categories SET ";

                    var parameters = new Dictionary<string, object>();

                    // Update CategoryName if not null
                    if (updatedCategory.CategoryName != null)
                    {
                        sql += "CategoryName = @CategoryName, ";
                        parameters.Add("CategoryName", updatedCategory.CategoryName);
                    }

                    // Update Description if not null
                    if (updatedCategory.Description != null)
                    {
                        sql += "Description = @Description, ";
                        parameters.Add("Description", updatedCategory.Description);
                    }

                    // Update NumberOfProducts if not null
                    if (updatedCategory.NumberOfProducts.HasValue)
                    {
                        sql += "NumberOfProducts = @NumberOfProducts, ";
                        parameters.Add("NumberOfProducts", updatedCategory.NumberOfProducts);
                    }

                    // Update ImageVideoURL if not null
                    if (updatedCategory.ImageVideoURL != null)
                    {
                        sql += "ImageVideoURL = @ImageVideoURL, ";
                        parameters.Add("ImageVideoURL", updatedCategory.ImageVideoURL);
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
        public async Task<List<Category>> SearchCategories(string searchTerm)
        {
            try
            {
                using (var conn = new MySqlConnection(_configuration.GetConnectionString("MySqlConnection")))
                {
                    conn.Open();

                    string sql = "SELECT * FROM categories WHERE CategoryName LIKE @searchTerm";
                    var categories = await conn.QueryAsync<Category>(sql, new { searchTerm = $"%{searchTerm}%" });

                    return categories.AsList();
                }
            }
            catch (Exception ex)
            {
                Log.Error($"Error searching categories: {ex.Message}");
                return null;
            }
        }
    }
    
}
