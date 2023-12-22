using Ishop.Domain.Entities;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ishop.Application.Interfaces
{
    public interface IShoppingCart
    {
         public Task<List<CartItem>> GetCartItems();
      
        public Task<CartItem> GetCartItem(int id);
        public Task<bool> PostCartItem(CartItem cartItem);
        public  Task<bool> DeleteCartItem(int id);
    }
}
