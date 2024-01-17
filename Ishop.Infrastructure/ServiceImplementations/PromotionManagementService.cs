using Dapper;
using Ishop.Application.Interfaces;
using Ishop.Domain.Entities;
using Microsoft.Extensions.Configuration;
using MySql.Data.MySqlClient;
using Serilog;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Ishop.Infrastructure.ServiceImplementations
{
    public class PromotionManagementService : IPromotionManagement
    {
        private readonly IConfiguration _configuration;

        public PromotionManagementService(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public async Task<int> CreatePromotion(Promotion promotion)
        {
            try
            {
                using (var conn = new MySqlConnection(_configuration.GetConnectionString("MySqlConnection")))
                {
                    conn.Open();

                    string sql = "INSERT INTO promotions (ManufacturerId, ManufacturerName, Package, ProductName, ImageUrl, " +
                                 "BillingMethod, PromotionDuration, PromotionDeadline, PhoneNumber, PromotionAmount, Views, " +
                                 "PostedTime, SimilarProducts, PromotionDescription) " +
                                 "VALUES (@ManufacturerId, @ManufacturerName, @Package, @ProductName, @ImageUrl, " +
                                 "@BillingMethod, @PromotionDuration, @PromotionDeadline, @PhoneNumber, @PromotionAmount, " +
                                 "@Views, @PostedTime, @SimilarProducts, @PromotionDescription)";

                    await conn.ExecuteAsync(sql, promotion);
                }
            }
            catch (Exception ex)
            {
                Log.Error($"Error adding promotion: {ex.Message}");
                return 0; // Return 0 to indicate failure
            }

            return 1; // Return 1 to indicate success
        }

        public async Task<List<Promotion>> GetAllPromotions()
        {
            try
            {
                using (var conn = new MySqlConnection(_configuration.GetConnectionString("MySqlConnection")))
                {
                    conn.Open();

                    string sql = "SELECT * FROM promotions";

                    var promotions = await conn.QueryAsync<Promotion>(sql);
                    return promotions.AsList();
                }
            }
            catch (Exception ex)
            {
                Log.Error($"Error getting promotions: {ex.Message}");
                return null; // Return null to indicate failure
            }
        }

        public async Task<Promotion> GetPromotion(int id)
        {
            try
            {
                using (var conn = new MySqlConnection(_configuration.GetConnectionString("MySqlConnection")))
                {
                    conn.Open();

                    string sql = "SELECT * FROM promotions WHERE PromotionId = @id";

                    var promotion = await conn.QuerySingleAsync<Promotion>(sql, new { id = id });
                    return promotion;
                }
            }
            catch (Exception ex)
            {
                Log.Error($"Error getting promotion with ID {id}: {ex.Message}");
                return null; // Return null to indicate failure
            }
        }

        public async Task<bool> DeletePromotion(int id)
        {
            try
            {
                using (var conn = new MySqlConnection(_configuration.GetConnectionString("MySqlConnection")))
                {
                    conn.Open();

                    string sql = "DELETE FROM promotions WHERE PromotionId = @id";

                    int affectedRows = await conn.ExecuteAsync(sql, new { id = id });
                    return affectedRows > 0; // True if deleted, False otherwise
                }
            }
            catch (Exception ex)
            {
                Log.Error($"Error deleting promotion with ID {id}: {ex.Message}");
                return false; // False on error
            }
        }

        public async Task<bool> UpdatePromotion(int id, Promotion updatedPromotion)
        {
            try
            {
                using (var conn = new MySqlConnection(_configuration.GetConnectionString("MySqlConnection")))
                {
                    conn.Open();

                    string sql = "UPDATE promotions SET ";

                    var parameters = new Dictionary<string, object>();

                    // Update ManufacturerId if not null
                    if (updatedPromotion.ManufacturerId.HasValue)
                    {
                        sql += "ManufacturerId = @ManufacturerId, ";
                        parameters.Add("ManufacturerId", updatedPromotion.ManufacturerId);
                    }

                    // Update ManufacturerName if not null
                    if (updatedPromotion.ManufacturerName != null)
                    {
                        sql += "ManufacturerName = @ManufacturerName, ";
                        parameters.Add("ManufacturerName", updatedPromotion.ManufacturerName);
                    }

                    // Update Package if not null
                    if (updatedPromotion.Package != null)
                    {
                        sql += "Package = @Package, ";
                        parameters.Add("Package", updatedPromotion.Package);
                    }

                    // Continue updating other properties similarly...

                    // Remove trailing comma and space if any updates were made
                    if (parameters.Count > 0)
                    {
                        sql = sql.TrimEnd(',', ' ');
                    }

                    sql += " WHERE PromotionId = @PromotionId";
                    parameters.Add("PromotionId", id);

                    int affectedRows = await conn.ExecuteAsync(sql, parameters);
                    return affectedRows > 0; // True if updated, False otherwise
                }
            }
            catch (Exception ex)
            {
                Log.Error($"Error updating promotion with ID {id}: {ex.Message}");
                return false; // False on error
            }
        }

        public async Task<List<Promotion>> SearchPromotions(string searchTerm)
        {
            try
            {
                using (var conn = new MySqlConnection(_configuration.GetConnectionString("MySqlConnection")))
                {
                    conn.Open();

                    string sql = "SELECT * FROM promotions WHERE ProductName LIKE @searchTerm";
                    var promotions = await conn.QueryAsync<Promotion>(sql, new { searchTerm = $"%{searchTerm}%" });

                    return promotions.AsList();
                }
            }
            catch (Exception ex)
            {
                Log.Error($"Error searching promotions: {ex.Message}");
                return null;
            }
        }
    }
}
