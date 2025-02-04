using OutbornE_commerce.BAL.Dto.Colors;
using OutbornE_commerce.BAL.Dto.Products;
using OutbornE_commerce.BAL.Dto.Sizes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OutbornE_commerce.BAL.Dto.Bags
{
    public class BagItemDtoAPI
    {
        public Guid Id { get; set; }
        public Guid ProductId { get; set; }
        public ProductCardDto? Product { get; set; }
        public Guid? ColorId { get; set; }
        public ColorForCreationDto? Color {  get; set; }
        public Guid? SizeId { get; set; }
        public SizeForCreationDto? Size { get; set; }
        public int Quantity { get; set; }
    }
}
