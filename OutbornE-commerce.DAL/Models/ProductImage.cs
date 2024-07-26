﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OutbornE_commerce.DAL.Models
{
    public class ProductImage : BaseEntity
    {
        public Guid ProductColorId { get; set; }
        public ProductColor? ProductColor { get; set; }
        public string ImageUrl { get; set; }
    }
}