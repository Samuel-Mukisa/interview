// IDeliveryAgentManagement.cs
using System.Collections.Generic;
using System.Threading.Tasks;
using Ishop.Domain.Entities;

namespace Ishop.Application.Interfaces
{
    public interface IDeliveryAgentManagement
    {
        Task<List<DeliveryAgent>> GetAllDeliveryAgents();
        Task<DeliveryAgent> GetDeliveryAgent(int id);
        Task<int> CreateDeliveryAgent(DeliveryAgent deliveryAgent);
        Task<bool> DeleteDeliveryAgent(int id);
        Task<bool> UpdateDeliveryAgent(int id, DeliveryAgent updatedDeliveryAgent);
    }
}