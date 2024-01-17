// DeliveryAgentManagementService.cs
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Dapper;
using Ishop.Application.Interfaces;
using Ishop.Domain.Entities;
using Microsoft.Extensions.Configuration;
using MySql.Data.MySqlClient;
using Serilog;

namespace Ishop.Infrastructure.ServiceImplementations
{
    public class DeliveryAgentManagementService : IDeliveryAgentManagement
    {
        private readonly IConfiguration _configuration;

        public DeliveryAgentManagementService(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        private MySqlConnection CreateConnection()
        {
            return new MySqlConnection(_configuration.GetConnectionString("MySqlConnection"));
        }

        public async Task<List<DeliveryAgent>> GetAllDeliveryAgents()
        {
            try
            {
                using (var conn = CreateConnection())
                {
                    conn.Open();
                    string sql = "SELECT * FROM DeliveryAgents";
                    var deliveryAgents = await conn.QueryAsync<DeliveryAgent>(sql);
                    return deliveryAgents.AsList();
                }
            }
            catch (Exception ex)
            {
                Log.Error($"Error getting delivery agents: {ex.Message}");
                return null;
            }
        }

        public async Task<DeliveryAgent> GetDeliveryAgent(int id)
        {
            try
            {
                using (var conn = CreateConnection())
                {
                    conn.Open();
                    string sql = "SELECT * FROM DeliveryAgents WHERE AgentID = @AgentID";
                    var deliveryAgent = await conn.QueryFirstOrDefaultAsync<DeliveryAgent>(sql, new { AgentID = id });
                    return deliveryAgent;
                }
            }
            catch (Exception ex)
            {
                Log.Error($"Error getting delivery agent with ID {id}: {ex.Message}");
                return null;
            }
        }

        public async Task<int> CreateDeliveryAgent(DeliveryAgent deliveryAgent)
        {
            try
            {
                using (var conn = CreateConnection())
                {
                    conn.Open();
                    string insertSql =
                        "INSERT INTO DeliveryAgents (CompanyName, ImageUrl, Motto) " +
                        "VALUES (@CompanyName, @ImageUrl, @Motto); SELECT LAST_INSERT_ID()";

                    var agentID = await conn.ExecuteScalarAsync<int>(insertSql, deliveryAgent);
                    return agentID;
                }
            }
            catch (Exception ex)
            {
                Log.Error($"Error creating delivery agent: {ex.Message}");
                return 0;
            }
        }

        public async Task<bool> DeleteDeliveryAgent(int id)
        {
            try
            {
                using (var conn = CreateConnection())
                {
                    conn.Open();
                    string deleteSql = "DELETE FROM DeliveryAgents WHERE AgentID = @AgentID";
                    int affectedRows = await conn.ExecuteAsync(deleteSql, new { AgentID = id });
                    return affectedRows > 0;
                }
            }
            catch (Exception ex)
            {
                Log.Error($"Error deleting delivery agent with ID {id}: {ex.Message}");
                return false;
            }
        }

        public async Task<bool> UpdateDeliveryAgent(int id, DeliveryAgent updatedDeliveryAgent)
        {
            try
            {
                using (var conn = CreateConnection())
                {
                    conn.Open();
                    string updateSql =
                        "UPDATE DeliveryAgents SET CompanyName = @CompanyName, ImageUrl = @ImageUrl, Motto = @Motto WHERE AgentID = @AgentID";
                    int affectedRows = await conn.ExecuteAsync(updateSql, new
                    {
                        AgentID = id,
                        updatedDeliveryAgent.CompanyName,
                        updatedDeliveryAgent.ImageUrl,
                        updatedDeliveryAgent.Motto
                    });
                    return affectedRows > 0;
                }
            }
            catch (Exception ex)
            {
                Log.Error($"Error updating delivery agent with ID {id}: {ex.Message}");
                return false;
            }
        }
    }
}
