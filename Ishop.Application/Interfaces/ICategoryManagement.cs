using Ishop.Domain.Entities;
using Microsoft.AspNetCore.Mvc;


namespace Ishop.Application.Interfaces;

public interface ICategoryManagement
{
    Task<List<Category>> GetAllCategories();
    Task<Category> GetCategory(int id);
    Task<int> CreateCategory(Category category);
    Task<bool> DeleteCategory(int id);
    Task<bool> UpdateCategory(int id, [FromBody] Category categoryDto);
    Task<List<Category>> SearchCategories(string searchTerm);

}