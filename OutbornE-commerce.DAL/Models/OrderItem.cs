using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OutbornE_commerce.DAL.Models
{
    public class OrderItem : BaseEntity
    {
        [Required]
        public Guid ProductId { get; set; }
        public Product? Product { get; set; }
        [Required]
        public decimal Price { get; set; }
        public Guid? ColorId { get; set; }
        public Color? Color { get; set; }   
        public Guid? SizeId { get; set; }
        public Size? Size { get; set; }   
        public bool IsWholesale { get; set; }
        public int Qunatity { get; set; }
        public decimal Total { get; set; }
    }
}
