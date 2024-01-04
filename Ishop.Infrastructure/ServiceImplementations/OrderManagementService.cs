using System.Data;
using Dapper;
using Ishop.Application.Services;
using Ishop.Domain.Entities;
using Microsoft.Extensions.Configuration;
using MySql.Data.MySqlClient;

namespace Ishop.Infrastructure.ServiceImplementations;

public class OrderManagementService : IOrderManagementService
{
            private readonly IConfiguration _configuration;

        public OrderManagementService(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        private IDbConnection CreateConnection()
        {
            return new MySqlConnection(_configuration.GetConnectionString("MySqlConnection"));
        }

        public async Task<int> CreateOrder(int userID, List<Product> products, decimal totalAmount)
        {
            try
            {
                using (var conn = CreateConnection())
                {
                    conn.Open();
                    var orderDate = DateTime.Now;
                    var status = "Pending";

                    // Insert order
                    string insertOrderSql = "INSERT INTO Orders (UserID, TotalAmount, OrderDate, Status) VALUES (@UserID, @TotalAmount, @OrderDate, @Status); SELECT LAST_INSERT_ID()";
                    var orderID = await conn.ExecuteScalarAsync<int>(insertOrderSql, new { UserID = userID, TotalAmount = totalAmount, OrderDate = orderDate, Status = status });

                    // Insert order details
                    foreach (var product in products)
                    {
                        string insertOrderDetailsSql = "INSERT INTO OrderDetails (OrderID, ProductID, Quantity, UnitPrice) VALUES (@OrderID, @ProductID, @Quantity, @UnitPrice)";
                        await conn.ExecuteAsync(insertOrderDetailsSql, new { OrderID = orderID, ProductID = product.ProductID, Quantity = 1, UnitPrice = product.Price });
                    }

                    return orderID;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error creating order: {ex.Message}");
                return 0; // Return 0 to indicate failure
            }
        }

        public async Task<Order> GetOrder(int orderID)
        {
            try
            {
                using (var conn = CreateConnection())
                {
                    conn.Open();
                    string sql = "SELECT * FROM Orders WHERE OrderID = @OrderID";
                    var order = await conn.QueryFirstOrDefaultAsync<Order>(sql, new { OrderID = orderID });
                    return order;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error getting order with ID {orderID}: {ex.Message}");
                return null; // Return null to indicate failure
            }
        }

        public async Task<bool> UpdateOrderStatus(int orderID, string status)
        {
            try
            {
                using (var conn = CreateConnection())
                {
                    conn.Open();
                    string sql = "UPDATE Orders SET Status = @Status WHERE OrderID = @OrderID";
                    int affectedRows = await conn.ExecuteAsync(sql, new { OrderID = orderID, Status = status });
                    return affectedRows > 0; // True if updated, False otherwise
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error updating order status for ID {orderID}: {ex.Message}");
                return false; // False on error
            }
        }

        public async Task<List<Order>> GetAllOrders()
        {
            try
            {
                using (var conn = CreateConnection())
                {
                    conn.Open();
                    string sql = "SELECT * FROM Orders";
                    var orders = await conn.QueryAsync<Order>(sql);
                    return orders.AsList();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error getting orders: {ex.Message}");
                return null; // Return null to indicate failure
            }
        }
    }

    
    
    
