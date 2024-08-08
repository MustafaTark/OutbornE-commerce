using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OutbornE_commerce.BAL.Dto.Products
{
    public class SearchModelDto
    {
        public string? SearchTerm { get; set; }
        public List<Guid>? BrandsIds { get; set; }
        public List<Guid>? CategoriesIds { get; set; }
        public List<Guid>? SizesIds { get; set; }
        public List<Guid>? ColorsIds {  get; set; }
        public decimal? MinPrice { get; set; }
        public decimal? MaxPrice { get; set;}
        public int? ProductType { get; set; }
        public int PageNumber { get; set; } = 1;
        public int PageSize { get; set; } = 20;
    }
}
