using Microsoft.AspNetCore.Http;
using OutbornE_commerce.BAL.Dto.ProductSizes;
using OutbornE_commerce.DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OutbornE_commerce.BAL.Dto.Products
{
    public class ProductForCreateDto
    {
        public string NameEn { get; set; }
        public string NameAr { get; set; }
        public decimal Price { get; set; }
        public string? ImageUrl { get; set; }
        public IFormFile? Image { get; set; }
        public string AboutEn { get; set; }
        public string AboutAr { get; set; }
        public string MaterialEn { get; set; }
        public string MaterialAr { get; set; }
        public string SizeAndFitEn { get; set; }
        public string SizeAndFitAr { get; set; }
        public string DeliveryAndReturnEn { get; set; }
        public string DeliveryAndReturnAr { get; set; }
        public List<Guid> ProductSizesIds { get; set; }
        public List<Guid>? ProductCategoriesIds { get; set; }
        public Guid BrandId { get; set; }
        public Guid? SubBrandId { get; set; }
        public int Label { get; set; }
        public int ProductType { get; set; }
        public int QuantityInStock { get; set; }
        public decimal ShippingCost { get; set; }
        public int NumberOfReturnDays { get; set; }
    }
}
