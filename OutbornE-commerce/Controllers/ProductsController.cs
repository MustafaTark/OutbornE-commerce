using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using OutbornE_commerce.BAL.Dto.Products;
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
        private readonly IFilesManager _filesManager;

        public ProductsController(IProductRepository productRepository, IFilesManager filesManager, IProductSizeRepository productSizeRepository)
        {
            _productRepository = productRepository;
            _filesManager = filesManager;
            _productSizeRepository = productSizeRepository;
        }
        [HttpGet]
        public async Task<IActionResult> GetAllProducts()
        {
            string[] includes = new string[] { "ProductSizes" };
            var products = await _productRepository.FindAllAsync(includes);
            var data = products.Adapt<List<ProductDto>>();
            return Ok(new Response<List<ProductDto>>()
            {
                Data = data,
                IsError = false,
                Status = (int)StatusCodeEnum.Ok

            });
        }
        [HttpGet("{id}")]
        public async Task<IActionResult> GetProductById(Guid id)
        {
            string[] includes = new string[] { "ProductSizes" };
            var product = await _productRepository.Find(i => i.Id == id,false, includes);
            var data = product.Adapt<ProductDto>(); 
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

            foreach (var size in product.ProductSizes!)
            {
                size.CreatedBy = "admin";
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
        public async Task<IActionResult> UpdateProduct([FromBody]ProductDto model,CancellationToken cancellationToken)
        {
            var product = await _productRepository.Find(c => c.Id == model.Id, true);

            product = model.Adapt<Product>();
            product.UpdatedBy = "admin";
            _productRepository.Update(product);

            var newProductSizes = model.ProductSizes.Adapt<List<ProductSize>>();
            foreach (var size in newProductSizes)
            {
                size.CreatedBy = "admin";
            }
            await _productSizeRepository.DeleteRange(s => s.ProductId == product.Id);
            await _productSizeRepository.CreateRange(newProductSizes);

            await _productRepository.SaveAsync(cancellationToken);

            return Ok(new Response<Guid>()
            {
                Data = product.Id,
                IsError = false,
                Status = (int)StatusCodeEnum.Ok

            });
        }
    }
}
