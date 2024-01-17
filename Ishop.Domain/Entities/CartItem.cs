using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ishop.Domain.Entities
{
    public class CartItem
    {
        [Key]
        public int ProductId { get; set; }
        public string? ProductName { get; set; }
        public string? ProductImage { get; set; }
        public int RetailerID { get; set; } 
        public decimal PriceUnit { get; set; }
        public decimal TotalAmount { get; set; }
        public string? ProductDescription { get; set; }
        public int Quantity { get; set; }
        
        public System.DateTime DateCreated { get; set; } = DateTime.Now;

        // public static implicit operator bool(CartItem v)
        // {
        //     throw new NotImplementedException();
        // }
    }
}
