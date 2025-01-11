using OutbornE_commerce.DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OutbornE_commerce.BAL.Dto.Orders
{
    public class OrderDto
    {
        public Guid Id { get; set; }
        public decimal Total { get; set; }
        public decimal Discount { get; set; }
        public string? CouponCode { get; set; }
        public decimal TotalAfterDiscount { get; set; }
        public string? PaymentImageUrl { get; set; }
        public string? PaymentImageUrl2 { get; set; }
        public string? PaymentImageUrl3 { get; set; }
        public double? Longitude { get; set; }
        public double? Latitude { get; set; }
        public string? PhoneNumber { get; set; }
        public string? FullAddress { get; set; }
        public string? Notes { get; set; }
        public UserShowDto? User { get; set; }
        public int Status { get; set; }
        public DateTime CreatedOn { get; set; }
        public List<OrderItemDto> OrderItems { get; set; } = new List<OrderItemDto>();
    }
}
