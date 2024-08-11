using Microsoft.EntityFrameworkCore;
using OutbornE_commerce.DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OutbornE_commerce.BAL.Extentions
{
    public static class SearchProductExtention
    {
        public static IQueryable<Product> SearchByBrand(this IQueryable<Product> products, List<Guid>? brandsIds)
        {
            if(brandsIds != null)
                products = products.Where(p=>brandsIds.Contains(p.BrandId));
            return products;
        } 
        public static IQueryable<Product> SearchByType(this IQueryable<Product> products,int? Type)
        {
            if(Type != null)
                products = products.Where(p=>p.ProductType == Type);
            return products;
        }
        public static IQueryable<Product> SearchByTerm(this IQueryable<Product> products,string? searchTerm)
        {
            if (!string.IsNullOrEmpty(searchTerm))
            {
               products = products.Where(p => p.NameEn.Contains(searchTerm) || p.NameAr.Contains(searchTerm));
            }
            return products;
        }
        public static IQueryable<Product> SearchByCategories(this IQueryable<Product> products,List<Guid>? categoriesIds)
        {
            if(categoriesIds!=null && !categoriesIds.Any())
            {
                products = products.Include(p => p.ProductCategories)
                        .Where(p => p.ProductCategories!.Exists(c => categoriesIds.Contains(c.CategoryId)));
            }
            return products;
        }
        public static IQueryable<Product> SearchBySizes(this IQueryable<Product> products,List<Guid>? sizesIds)
        {
            if(sizesIds != null && !sizesIds.Any())
            {
                products = products.Include(p => p.ProductSizes)
                        .Where(p => p.ProductSizes!.Exists(c => sizesIds.Contains(c.SizeId)));
            }
            return products;
        } 
        public static IQueryable<Product> SearchByColors(this IQueryable<Product> products,List<Guid>? colorsIds)
        {
            if(colorsIds != null && !colorsIds.Any())
            {
                products = products.Include(p => p.ProductColors)
                        .Where(p => p.ProductColors!.Exists(c => colorsIds.Contains(c.ColorId)));
            }
            return products;
        }
        public static IQueryable<Product> SearchByPrice(this IQueryable<Product> products,decimal? minPrice,decimal? maxPrice)
        {
            if (minPrice != null)
                products = products.Where(p => p.Price >= minPrice);
            if(maxPrice != null)
                products = products.Where(p=>p.Price <= maxPrice);
            return products;
        }
    }
}
