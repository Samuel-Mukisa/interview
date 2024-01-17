using Ishop.Domain.Entities;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Ishop.Application.Interfaces
{
    public interface IProductManagement
    {
        Task<List<Product>> GetAllProducts();
        Task<Product> GetProduct(int id);
        Task<int> CreateProduct(Product product);
        Task<bool> DeleteProduct(int id);
        Task<bool> UpdateProduct(int id, [FromBody] Product productDto);
    }
}