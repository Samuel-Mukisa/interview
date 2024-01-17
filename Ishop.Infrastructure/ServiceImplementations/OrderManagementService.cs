using System;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;
using Dapper;
using Ishop.Application.Services;
using Ishop.Domain.Entities;
using Microsoft.Extensions.Configuration;
using MySql.Data.MySqlClient;

namespace Ishop.Infrastructure.ServiceImplementations
{
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

       public async Task<int> CreateOrder(Order order)
{
    try
    {
        using (var conn = CreateConnection())
        {
            conn.Open();
            var orderDate = DateTime.Now;
            var status = "Pending";

            // Insert order
            string insertOrderSql =
                "INSERT INTO orders (RetailerID, TotalAmount, OrderDate, Status, CustomerName, OrderNumber, Quantity, ShippingAddress, Contact, UnitPrice, Currency, PaymentMethod, DeliveryDeadline, ReturnPolicy, OrderNotes, DeliveryCharge, Discount, Subtotal, Promocode) " +
                "VALUES (@RetailerID, @TotalAmount, @OrderDate, @Status, @CustomerName, @OrderNumber, @Quantity, @ShippingAddress, @Contact, @UnitPrice, @Currency, @PaymentMethod, @DeliveryDeadline, @ReturnPolicy, @OrderNotes, @DeliveryCharge, @Discount, @Subtotal, @Promocode); SELECT LAST_INSERT_ID()";

            var orderID = await conn.ExecuteScalarAsync<int>(insertOrderSql, new
            {
                order.RetailerID,
                order.TotalAmount,
                OrderDate = orderDate,
                Status = status,
                order.CustomerName,
                order.OrderNumber,
                order.Quantity,
                order.ShippingAddress,
                order.Contact,
                order.UnitPrice,
                order.Currency,
                order.PaymentMethod,
                DeliveryDeadline = orderDate.AddDays(5),
                order.ReturnPolicy,
                order.OrderNotes,
                order.DeliveryCharge,
                order.Discount,
                order.Subtotal,
                order.Promocode
            });

            // Insert order details
            foreach (var product in order.Products)
            {
                string insertOrderDetailsSql =
                    "INSERT INTO orderDetails (OrderID, ProductID, Quantity, UnitPrice) VALUES (@OrderID, @ProductID, @Quantity, @UnitPrice)";
                await conn.ExecuteAsync(insertOrderDetailsSql, new
                {
                    OrderID = orderID,
                    product.ProductID,
                    Quantity = 1, // Replace with actual data
                    UnitPrice = product.Price
                });
            }

            return orderID;
        }
    }
    catch (Exception ex)
    {
        Console.WriteLine($"Error creating order: {ex.Message}");
        Console.WriteLine($"Stack Trace: {ex.StackTrace}");
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
                    string sql = "SELECT * FROM orders WHERE OrderID = @OrderID";
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
                    string sql = "UPDATE orders SET Status = @Status WHERE OrderID = @OrderID";
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
        public async Task<bool> UpdateOrder(int orderID, Order updatedOrder)
        {
            try
            {
                using (var conn = CreateConnection())
                {
                    conn.Open();

                    string sql = "UPDATE orders SET " +
                                 "CustomerName = @CustomerName, " +
                                 "OrderNumber = @OrderNumber, " +
                                 "Quantity = @Quantity, " +
                                 "ShippingAddress = @ShippingAddress, " +
                                 "Contact = @Contact, " +
                                 "UnitPrice = @UnitPrice, " +
                                 "Currency = @Currency, " +
                                 "PaymentMethod = @PaymentMethod, " +
                                 "DeliveryDeadline = @DeliveryDeadline, " +
                                 "ReturnPolicy = @ReturnPolicy, " +
                                 "OrderNotes = @OrderNotes, " +
                                 "DeliveryCharge = @DeliveryCharge, " +
                                 "Discount = @Discount, " +
                                 "Subtotal = @Subtotal, " +
                                 "Promocode = @Promocode " +
                                 "WHERE OrderID = @OrderID";

                    var parameters = new
                    {
                        OrderID = orderID,
                        updatedOrder.CustomerName,
                        updatedOrder.OrderNumber,
                        updatedOrder.Quantity,
                        updatedOrder.ShippingAddress,
                        updatedOrder.Contact,
                        updatedOrder.UnitPrice,
                        updatedOrder.Currency,
                        updatedOrder.PaymentMethod,
                        updatedOrder.DeliveryDeadline,
                        updatedOrder.ReturnPolicy,
                        updatedOrder.OrderNotes,
                        updatedOrder.DeliveryCharge,
                        updatedOrder.Discount,
                        updatedOrder.Subtotal,
                        updatedOrder.Promocode
                    };

                    int affectedRows = await conn.ExecuteAsync(sql, parameters);
                    return affectedRows > 0; // True if updated, False otherwise
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error updating order with ID {orderID}: {ex.Message}");
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
                    string sql = "SELECT * FROM orders";
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
}
