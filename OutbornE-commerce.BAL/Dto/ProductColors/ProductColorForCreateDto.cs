using Microsoft.AspNetCore.Http;
using OutbornE_commerce.BAL.Dto.ProductImages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OutbornE_commerce.BAL.Dto.ProductColors
{
    public class ProductColorForCreateDto
    {
        public Guid ProductId { get; set; }
        public Guid ColorId { get; set; }
        public int AvailableQuntity { get; set; }
        public List<Guid>? SizeIds { get; set; }
        public List<Guid> SizesIds { get; set; }
        //public List<ProductImageDto>? ProductImages { get; set; }
        public List<IFormFile> Images { get; set; }
    }
}
