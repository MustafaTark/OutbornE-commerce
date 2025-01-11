using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OutbornE_commerce.BAL.Dto.Orders
{
    public class OrderItemDto
    {
        public string ProductNameEn { get; set; }
        public string ProductNameAr { get; set; }
        public string ProductImageUrl { get; set; }
        public decimal Price { get; set; }
        public int Quantity { get; set; }
        public decimal Total { get; set; }
        public string ColorNameEn { get; set; }
        public string ColorNameAr { get; set; }
        public string? SizeName{ get; set; }


    }
}
