using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OutbornE_commerce.BAL.Dto.Orders
{
    public class OrderItemForCreateDto
    {
        public Guid ProductId { get; set; }
        public Guid? ColorId { get; set; }
        public Guid? SizeId { get; set; }
        public int Quantity { get; set; }
        public bool IsWholesale { get; set; }
    }
}
