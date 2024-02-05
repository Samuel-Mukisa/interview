using Ishop.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ishop.Application.Interfaces
{
    public interface IRetailerAuthentication
    {

       public Task<int> CreateRetailer(RetailerRegistration registration);
       public Task<int> LoginRetailer(string retailerUsername, string password);
    }
}
