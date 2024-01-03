using Ishop.Application.Interfaces;
using Ishop.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Ishop.Domain.Entities.Payment;

namespace Ishop.Application.Services
{
    public interface IPaymentService:IPayment
    {
        Task<string> MakeChargeRequest(Payment request);
    }
}
