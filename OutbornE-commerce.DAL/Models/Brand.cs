using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OutbornE_commerce.DAL.Models
{
    public class Brand : BaseEntity
    {
        public string NameEn {  get; set; }
        public string NameAr { get; set; }
        public string? ImageUrl {  get; set; }
    }
}
