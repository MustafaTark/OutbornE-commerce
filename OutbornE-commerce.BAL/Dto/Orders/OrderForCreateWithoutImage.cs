using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OutbornE_commerce.BAL.Dto.Orders
{
    public class OrderForCreateWithoutImage
    {
        public string? CouponCode { get; set; }
        public double? Longitude { get; set; }
        public double? Latitude { get; set; }
        public string? PhoneNumber { get; set; }
        public string? FullAddress { get; set; }
        public Guid? CityId { get; set; }
        public string? Notes { get; set; }
        public List<OrderItemForCreateDto> OrderItems { get; set; } = new List<OrderItemForCreateDto>();
    }
}
