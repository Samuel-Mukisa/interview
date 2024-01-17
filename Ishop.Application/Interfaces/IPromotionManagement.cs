using Ishop.Domain.Entities;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Ishop.Application.Interfaces
{
    public interface IPromotionManagement
    {
        Task<List<Promotion>> GetAllPromotions();
        Task<Promotion> GetPromotion(int id);
        Task<int> CreatePromotion(Promotion promotion);
        Task<bool> DeletePromotion(int id);
        Task<bool> UpdatePromotion(int id, [FromBody] Promotion promotionDto);
        Task<List<Promotion>> SearchPromotions(string searchTerm);
    }
}