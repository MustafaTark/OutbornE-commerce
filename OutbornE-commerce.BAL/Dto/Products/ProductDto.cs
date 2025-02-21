using OutbornE_commerce.BAL.Dto.ProductCategories;
using OutbornE_commerce.BAL.Dto.ProductSizes;
using OutbornE_commerce.DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OutbornE_commerce.BAL.Dto.Products
{
    public class ProductDto
    {
        public Guid Id { get; set; }
        public string NameEn { get; set; }
        public string NameAr { get; set; }
        public decimal PricePerUnit { get; set; }
        public decimal WholesalePrice { get; set; }
        public int WholesaleMinmumQuntity { get; set; }
        public string ImageUrl { get; set; }
        public string DemoUrl { get; set; }
        public string AboutEn { get; set; }
        public string AboutAr { get; set; }
        public string SizeAndFitEn { get; set; }
        public string SizeAndFitAr { get; set; }
        public Guid MainCategoryId { get; set; }
        public Guid SubCategoryId { get; set; }
        //public List<ProductSizeDto>? ProductSizes { get; set; }
        //public List<ProductCategoryDto>? ProductCategories { get; set; }
        //public List<Guid>? ProductSizesIds { get; set; }
        //public List<Guid>? ProductCategoriesIds { get; set; }
        public Guid? BrandId { get; set; }
        public int Label { get; set; }
       // public int ProductType { get; set; }
        //public int QuantityInStock { get; set; }
	}
}
