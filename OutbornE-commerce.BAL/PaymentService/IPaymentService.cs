using OutbornE_commerce.BAL.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OutbornE_commerce.BAL.PaymentService
{
    public interface IPaymentService
    {
        string InitiatePayment(PaymentRequest request);
    }
}
