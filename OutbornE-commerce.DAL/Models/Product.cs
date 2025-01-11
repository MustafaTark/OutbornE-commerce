using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OutbornE_commerce.DAL.Models
{
    public class Product : BaseEntity
    {
        public string NameEn {  get; set; }
        public string NameAr { get; set; }
        public decimal PricePerUnit { get; set; }
        public decimal WholesalePrice { get; set; }
        public int WholesaleMinmumQuntity{ get; set; }
        public string ImageUrl { get; set; }
        public string? DemoUrl { get; set; }
        public string AboutEn { get; set; }
        public string AboutAr { get; set; }
        public string? SizeAndFitEn {  get; set; }
        public string? SizeAndFitAr {  get; set; }
        public List<ProductColor>? ProductColors { get; set; }
        public Guid MainCategoryId { get; set; }
        public Category? MainCategory { get; set; }
        public Guid SubCategoryId { get; set; }
        public Category? SubCategory { get; set; }
        public int ProductType { get; set; }
        public Guid? BrandId { get; set; }
        public Brand? Brand { get; set; }
        public int Label { get; set; }
        public int QuantityInStock { get; set; }
    }
}
