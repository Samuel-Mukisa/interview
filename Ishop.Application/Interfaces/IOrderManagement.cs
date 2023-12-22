using Ishop.Domain.Entities;

namespace Ishop.Application.Interfaces;

public interface IOrderManagement
{
    Task<int> CreateOrder(int userID, List<Product> products, decimal totalAmount);
    Task<Order> GetOrder(int orderID);
    Task<bool> UpdateOrderStatus(int orderID, string status);
    Task<List<Order>> GetAllOrders();
}