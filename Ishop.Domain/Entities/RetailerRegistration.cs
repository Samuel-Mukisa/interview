using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ishop.Domain.Entities
{
    public class RetailerRegistration
    {
        public int RetailerId { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public string RetailerEmail { get; set; }
        public string RetailerBusinessName { get; set; }
        public string ReasonForSourcing { get; set; }
        public string RetailerLocation { get; set; }
        public string RetailerPhoneNumber { get; set; }

        public string ImageUrl { get; set; }

        public string FavoriteCategories { get; set; }



    }
}
