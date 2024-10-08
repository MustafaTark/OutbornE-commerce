﻿using OutbornE_commerce.BAL.Dto.Sizes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OutbornE_commerce.BAL.Dto.ProductSizes
{
    public class ProductSizeDto
    {
        public Guid Id { get; set; }
        public Guid ProductId { get; set; }
        public Guid SizeId { get; set; }
        public SizeDto? Size { get; set; }
    }
}
