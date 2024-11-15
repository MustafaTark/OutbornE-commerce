using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OutbornE_commerce.DAL.Models
{
    public class OrderItem : BaseEntity
    {
        public Guid ProductId { get; set; }
        public decimal Price { get; set; }
        public Guid? ColorId { get; set; }
    }
}
