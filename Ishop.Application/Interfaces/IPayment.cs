using Ishop.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Ishop.Domain.Entities.Payment;

namespace Ishop.Application.Interfaces
{
    public interface IPayment
    {
        Task<string> MakeChargeRequest(Payment request);
    }

}
