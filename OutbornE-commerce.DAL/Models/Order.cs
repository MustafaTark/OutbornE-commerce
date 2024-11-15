using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OutbornE_commerce.DAL.Models
{
    public class Order : BaseEntity
    {
        public decimal Total { get; set; }
        public decimal Discount { get; set; }
        public string? CouponCode { get; set; }
        public Guid? CouponCodeId { get; set; }
        public decimal? TotalAfterDiscount { get; set; } 
        public string PaymentImageUrl { get; set; }
        public string? PaymentImageUrl2 { get; set; }
        public string? PaymentImageUrl3 { get; set; }
        public double? Longitude { get; set; }
        public double? Latitude { get; set; }
        public string? PhoneNumber { get; set; }
        public string? FullAddress { get; set; }
        public string? Notes { get; set; }
        public string UserId { get; set; }
        public User? User { get; set; } 
        [Required]
        public List<OrderItem> OrderItems { get; set; } = new List<OrderItem>();
    }
}
