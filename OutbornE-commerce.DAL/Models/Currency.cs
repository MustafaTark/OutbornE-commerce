﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OutbornE_commerce.DAL.Models
{
    public class Currency : BaseEntity
    {
        public string NameAr {  get; set; }
        public string NameEn { get; set; }
        public string Sign {  get; set; }
        public decimal Price { get; set; }
        public bool IsDeafult { get; set; }
    }
}