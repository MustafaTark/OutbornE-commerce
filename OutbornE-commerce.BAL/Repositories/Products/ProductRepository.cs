using Microsoft.EntityFrameworkCore;
using OutbornE_commerce.BAL.Dto;
using OutbornE_commerce.BAL.Dto.Products;
using OutbornE_commerce.BAL.Extentions;
using OutbornE_commerce.BAL.Repositories.BaseRepositories;
using OutbornE_commerce.DAL.Data;
using OutbornE_commerce.DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OutbornE_commerce.BAL.Repositories.Products
{
    public class ProductRepository : BaseRepository<Product> , IProductRepository
    {
       
        public ProductRepository(ApplicationDbContext context) : base(context)
        {
        }
        public async Task<PagainationModel<List<Product>>> SearchProducts(SearchModelDto model)
        {
            int totalCount = 0;
            var products = _context.Products
                                   .AsNoTracking()
                                   .Include(b=>b.Brand)
                                   .SearchByBrand(model.BrandId)
                                   .SearchByTerm(model.SearchTerm)
                                   .SearchByCategories(model.CategoriesIds)
                                   .SearchBySizes(model.SizesIds)
                                   .SearchByColors(model.ColorsIds)
                                   .SearchByPrice(model.MinPrice,model.MaxPrice);

            totalCount = products.Count();
            var data = await products.Skip(model.PageSize * (model.PageNumber - 1)).Take(model.PageSize).ToListAsync();

            return new PagainationModel<List<Product>>()
            {
                TotalCount = totalCount,
                Data = data,
                PageNumber = model.PageNumber,
                PageSize = model.PageSize,
            };
        }
    }
}
