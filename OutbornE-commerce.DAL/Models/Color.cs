using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OutbornE_commerce.DAL.Models
{
    public class Color : BaseEntity
    {
        public string Hexadecimal { get; set; }
        public string NameAr {  get; set; }
        public string NameEn { get; set; }  
    }
}
