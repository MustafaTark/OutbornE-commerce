using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OutbornE_commerce.DAL.Models
{
    public class ProductColor : BaseEntity
    {
        public Guid ProductId { get; set; }
        public Product? Product { get; set; }
        public Guid ColorId { get; set; }
        public Color? Color { get; set; }
        public bool IsDefault { get; set; }
        public int AvailableQuntity { get; set; }
        public List<ProductImage>? ProductImages { get; set; }
    }
}
