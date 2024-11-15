using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OutbornE_commerce.BAL.Dto.Orders
{
    public class OrderForCreateDto
    {
      //  public decimal Total { get; set; }
      //  public decimal Discount { get; set; }
        public string? CouponCode { get; set; }
      //  public Guid? CouponCodeId { get; set; }
    //    public decimal? TotalAfterDiscount { get; set; }
      
        public string PaymentImageUrl { get; set; }
        public IFormFile PaymentImage { get; set; }
        public string? PaymentImageUrl2 { get; }
        public IFormFile PaymentImage2 { get; set; }
        public string? PaymentImageUrl3 { get; }
        public IFormFile PaymentImage3 { get; set; }
        public double? Longitude { get; set; }
        public double? Latitude { get; set; }
        public string? PhoneNumber { get; set; }
        public string? FullAddress { get; set; }
        public string? Notes { get; set; }
        public List<OrderItemForCreateDto> OrderItems { get; set; } = new List<OrderItemForCreateDto>();
    }
}
