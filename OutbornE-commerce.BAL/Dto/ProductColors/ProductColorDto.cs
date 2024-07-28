using Microsoft.AspNetCore.Http;
using OutbornE_commerce.BAL.Dto.Colors;
using OutbornE_commerce.DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OutbornE_commerce.BAL.Dto.ProductColors
{
    public class ProductColorDto
    {
        public Guid Id { get; set; }
        public Guid ProductId { get; set; }
        public Guid ColorId { get; set; }
        public ColorDto? Color { get; set; }
        public bool IsDefault { get; set; }
        public List<IFormFile>? Images { get; set; }
        public List<ProductImage>? ProductImages { get; set; }
    }
}
