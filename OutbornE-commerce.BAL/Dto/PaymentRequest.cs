using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OutbornE_commerce.BAL.Dto
{
    public class PaymentRequest
    {
        public Guid? OrderId { get; set; }
        public string ProductName { get; set; }
        public decimal Price { get; set; }
        public string UserName { get; set; }
        public string UserEmail { get; set; }
        public string PaymentMethod { get; set; }
        public string? ShippingAddress { get; set; }
        public int Quantity { get; set; }
        public string OrderNotes { get; set; }
    }
}
