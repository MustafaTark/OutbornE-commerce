using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using OutbornE_commerce.BAL.Dto.ProductColors;
using OutbornE_commerce.BAL.Dto.Products;
using OutbornE_commerce.BAL.Repositories.ProductCateories;
using OutbornE_commerce.BAL.Repositories.Products;
using OutbornE_commerce.BAL.Repositories.ProductSizes;
using OutbornE_commerce.DAL.Models;
using OutbornE_commerce.FilesManager;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace OutbornE_commerce.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductsController : ControllerBase
    {
        private readonly IProductRepository _productRepository;
        private readonly IProductSizeRepository _productSizeRepository;
        private readonly IProductCategoryRepository _productCategoryRepository;
        private readonly IFilesManager _filesManager;

        public ProductsController(IProductRepository productRepository, IFilesManager filesManager, IProductSizeRepository productSizeRepository, IProductCategoryRepository productCategoryRepository)
        {
            _productRepository = productRepository;
            _filesManager = filesManager;
            _productSizeRepository = productSizeRepository;
            _productCategoryRepository = productCategoryRepository;
        }
        [HttpGet]
        public async Task<IActionResult> GetAllProducts(int pageNumber = 1, int pageSize = 10,string? searchTerm = null)
        {
            var items = new PagainationModel<IEnumerable<Product>>();
            if(string.IsNullOrEmpty(searchTerm))
                 items = await _productRepository.FindAllAsyncByPagination(null, pageNumber, pageSize, new string[] { "ProductSizes.Size" });
            else 
                items = await _productRepository.FindAllAsyncByPagination(b=>b.NameEn.Contains(searchTerm)
                                                                           ||b.NameAr.Contains(searchTerm)
                                                                           ||b.MaterialEn.Contains(searchTerm)
                                                                           ||b.MaterialAr.Contains(searchTerm)
                                                                           ||b.AboutEn.Contains(searchTerm)
                                                                            || b.AboutAr.Contains(searchTerm)
                                                                            || b.DeliveryAndReturnEn.Contains(searchTerm)
                                                                            || b.DeliveryAndReturnAr.Contains(searchTerm)
                                                                            || b.ProductSizes.Any(p=>p.Size.Name.Contains(searchTerm)), pageNumber, pageSize, new string[] { "ProductSizes.Size" });

            var data = items.Data.Adapt<List<ProductDto>>();

            return Ok(new PaginationResponse<List<ProductDto>>
            {
                Data = data,
                IsError = false,
                Status = (int)StatusCodeEnum.Ok,
                PageNumber = pageNumber,
                PageSize = pageSize,
                TotalCount = items.TotalCount
            });
        }
        [HttpGet("{id}")]
        public async Task<IActionResult> GetProductById(Guid id)
        {
            string[] includes = new string[] { "ProductSizes.Size" };
            var product = await _productRepository.Find(i => i.Id == id,false, includes);
            var data = product.Adapt<ProductDto>(); 

            data.ProductSizesIds = product.ProductSizes?.Select(s=>s.SizeId).ToList();
            data.ProductCategoriesIds = product.ProductCategories?.Select(s=>s.CategoryId).ToList();

            return Ok(new Response<ProductDto>()
            {
                Data = data,
                IsError = false,
                Status = (int)StatusCodeEnum.Ok

            });
        }
        [HttpPost]
        public async Task<IActionResult> CreateProduct([FromForm] ProductForCreateDto model,CancellationToken cancellationToken)
        {
            var product = model.Adapt<Product>();
            product.CreatedBy = "admin";
            if (model.Image != null)
            {
                var fileModel = await _filesManager.UploadFile(model.Image, "Products");
                product.ImageUrl = fileModel!.Url;
            }

            foreach (var sizeId in model.ProductSizesIds!)
            {
                product.ProductSizes = new List<ProductSize>();
                var size = new ProductSize
                {
                    SizeId = sizeId,
                    CreatedBy = "admin",
                    CreatedOn = DateTime.Now,
                    
                };
                product.ProductSizes!.Add(size);
            }
            foreach (var categoryId in model.ProductCategoriesIds!)
            {
                product.ProductCategories = new List<ProductCategory>();
                var category = new ProductCategory
                {
                    CategoryId = categoryId,
                    CreatedBy = "admin",
                    CreatedOn = DateTime.Now,
                    
                };
                product.ProductCategories!.Add(category);
            }

            await _productRepository.Create(product);
            await _productRepository.SaveAsync(cancellationToken);

            return Ok(new Response<Guid>()
            {
                Data = product.Id,
                IsError = false,
                Status = (int)StatusCodeEnum.Ok

            });
        }
        [HttpPut]
        public async Task<IActionResult> UpdateProduct([FromForm]ProductDto model,CancellationToken cancellationToken)
        {
            var product = await _productRepository.Find(c => c.Id == model.Id, false);

            product = model.Adapt<Product>();
            product.UpdatedBy = "admin";
            product.CreatedBy = "admin";
            _productRepository.Update(product);

            var newProductSizes = new List<ProductSize>();
            var newProductCategories = new List<ProductCategory>();
            //foreach (var size in newProductSizes)
            //{
            //    size.CreatedBy = "admin";
            //} 
            foreach (var sizeId in model.ProductSizesIds!)
            {
                var Newsize = new ProductSize
                {
                    SizeId = sizeId,
                    CreatedBy = "admin",
                    UpdatedBy = "admin",
                    CreatedOn= DateTime.UtcNow,
                    UpdatedOn = DateTime.UtcNow,
                    ProductId = model.Id

                };
                newProductSizes.Add(Newsize);
            }
            foreach (var categoryId in model.ProductCategoriesIds!)
            {
                var Newcategory = new ProductCategory
                {
                    CategoryId = categoryId,
                    CreatedBy = "admin",
                    UpdatedBy = "admin",
                    CreatedOn= DateTime.UtcNow,
                    UpdatedOn = DateTime.UtcNow,
                    ProductId = model.Id

                };
                newProductCategories.Add(Newcategory);
            }
            await _productSizeRepository.DeleteRange(s => s.ProductId == product.Id);
            await _productSizeRepository.CreateRange(newProductSizes);
            
            await _productCategoryRepository.DeleteRange(s => s.ProductId == product.Id);
            await _productCategoryRepository.CreateRange(newProductCategories);

            await _productRepository.SaveAsync(cancellationToken);

            return Ok(new Response<Guid>()
            {
                Data = product.Id,
                IsError = false,
                Status = (int)StatusCodeEnum.Ok

            });
        }
        [HttpGet("bestSeller")]
        public async Task<IActionResult> GetBestSellerProducts()
        {
            var products = await _productRepository.FindByCondition(p=>p.Label == (int) ProductLabelEnum.BestSeller);
            var data = products.Adapt<List<ProductCardDto>>();

            return Ok(new Response<List<ProductCardDto>>()
            {
                Data = data,
                IsError = false,
                Status = (int)StatusCodeEnum.Ok

            });
        }
        [HttpGet("peopleAlsoBought")]
        public async Task<IActionResult> GetPeopleAlsoBoughtProducts()
        {
            var products = await _productRepository.FindByCondition(p=>p.IsPeopleAlseBought);
            var data = products.Adapt<List<ProductCardDto>>();

            return Ok(new Response<List<ProductCardDto>>()
            {
                Data = data,
                IsError = false,
                Status = (int)StatusCodeEnum.Ok

            });
        }
        [HttpPost("search")]
        public async Task<IActionResult> SearchProducts([FromBody] SearchModelDto model)
        {
           var products = await _productRepository.SearchProducts(model);
            var data = products.Data.Adapt<List<SearchedProductDto>>();

            return Ok(new PaginationResponse<List<SearchedProductDto>>
            {
                Data = data,
                IsError = false,
                Status = (int)StatusCodeEnum.Ok,
                PageNumber = model.PageNumber,
                PageSize = model.PageSize,
                TotalCount = products.TotalCount
            });
        }
        [HttpGet("details")]
        public async Task<IActionResult> GetProductDetails(Guid productId)
        {
            var product = await _productRepository.Find(p => p.Id == productId,
                                                        trackChanges: true,
                                                        new string[] { "ProductSizes.Size",
                                                                       "ProductColors.Color",
                                                                       "ProductColors.ProductImages",
                                                                        "ProductCategories.Category",
                                                                         "Brand"});
            var data = product.Adapt<ProductDetailsDto>();

            return Ok(new Response<ProductDetailsDto>()
            {
                Data = data,
                IsError = false,
                Status = (int)StatusCodeEnum.Ok

            });
        }
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteProduct(Guid id,CancellationToken cancellationToken)
        {
            var product = await _productRepository.Find(p => p.Id == id);
            product.IsDeleted = true;
            _productRepository.Update(product);
            await _productRepository.SaveAsync(cancellationToken);
			return Ok(new Response<Guid>()
			{
				Data = id,
				IsError = false,
				Status = (int)StatusCodeEnum.Ok

			});
		}

    }
}
