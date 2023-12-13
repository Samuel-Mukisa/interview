using Ishop.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ishop.Application.Interfaces
{
    public interface ICustomerRegistration
    {
        Task<int> RegisterCustomer(CustomerSignUp customerSignUp);
    }
}
