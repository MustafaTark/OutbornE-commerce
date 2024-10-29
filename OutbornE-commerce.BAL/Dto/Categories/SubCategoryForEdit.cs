using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OutbornE_commerce.BAL.Dto.Categories
{
    public class SubCategoryForEdit
    {
        public Guid? Id { get; set; }
        public string NameEn { get; set; }
        public string NameAr { get; set; }
        public string? ImageUrl { get; set; }
        public IFormFile? Image { get; set; }
        public Guid? ParentCategoryId { get; set; }
    }
}
