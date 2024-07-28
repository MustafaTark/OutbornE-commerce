using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using OutbornE_commerce.BAL.Dto.Currencies;
using OutbornE_commerce.BAL.Dto.ProductColors;
using OutbornE_commerce.BAL.Repositories.ProductColors;
using OutbornE_commerce.BAL.Repositories.ProductImages;
using OutbornE_commerce.DAL.Models;
using OutbornE_commerce.FilesManager;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace OutbornE_commerce.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductColorsController : ControllerBase
    {
        private readonly IProductColorRepository _productColorRepository;
        private readonly IProductImageRepository _productImageRepository;
        private readonly IFilesManager _filesManager;

        public ProductColorsController(IProductImageRepository productImageRepository,
            IProductColorRepository productColorRepository,
            IFilesManager filesManager)
        {
            _productImageRepository = productImageRepository;
            _productColorRepository = productColorRepository;
            _filesManager = filesManager;
        }
        [HttpGet]
        public async Task<IActionResult> GetAllProductColors(int pageNumber= 1, int pageSize= 10)
        {
            var items = await _productColorRepository.FindAllAsyncByPagination(null, pageNumber, pageSize, new string[] { "Color", "ProductImages" });

            var data = items.Data.Adapt<List<ProductColorDto>>();

            return Ok(new PaginationResponse<List<ProductColorDto>>
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
        public async Task<IActionResult> GetProductColorById(Guid id)
        {
            var productColor = await _productColorRepository.Find(p=>p.Id == id,true,new string[] {"Color","ProductImages"});
            var data = productColor.Adapt<ProductColorDto>();
            return Ok(new Response<ProductColorDto>
            {
                Data = data,
                IsError = false,
                Message = "",
                Status = (int)StatusCodeEnum.Ok
            });
        }
        [HttpPost]
        public async Task<IActionResult> CreateProductColor([FromForm] ProductColorForCreateDto model,CancellationToken cancellationToken)
        {
            var productColor = model.Adapt<ProductColor>();
            productColor.CreatedBy = "admin";
            productColor.ProductImages = new List<ProductImage>();
            if(model.Images != null)
            {
               List<FileModel> images = await _filesManager.UploadMultipleFile(model.Images,"Products");
                foreach (var image in images)
                {
                    var prodImage = new ProductImage
                    {
                        CreatedBy = "admin",
                        ImageUrl = image.Url,
                        CreatedOn = DateTime.Now,
                    };
                    productColor.ProductImages.Add(prodImage);
                }
            }

            var result =  await _productColorRepository.Create(productColor);
            await _productColorRepository.SaveAsync(cancellationToken);

            return Ok(new Response<Guid>
            {
                Data = result.Id,
                IsError = false,
                Message = "",
                Status = (int)StatusCodeEnum.Ok
            });
        }
        [HttpPut]
        public async Task<IActionResult> UpdateProductColor([FromForm]ProductColorDto model,CancellationToken cancellationToken)
        {
            var productColor = await _productColorRepository.Find(p=>p.Id == model.Id,false,new string[] {"ProductImages"});
             productColor = model.Adapt<ProductColor>();
            if (model.Images != null)
            {
                List<FileModel> images = await _filesManager.UploadMultipleFile(model.Images, "Products");
                foreach (var image in images)
                {
                    var prodImage = new ProductImage
                    {
                        CreatedBy = "admin",
                        ImageUrl = image.Url,
                        CreatedOn = DateTime.Now,
                    };
                    productColor.ProductImages.Add(prodImage);
                }
            }
            await _productImageRepository.DeleteRange(p=>p.ProductColorId == model.Id);
            productColor.UpdatedBy = "admin";
            productColor.UpdatedOn = DateTime.Now;
            _productColorRepository.Update(productColor);
            await _productColorRepository.SaveAsync(cancellationToken);
            return Ok(new Response<Guid>
            {
                Data = productColor.Id,
                IsError = false,
                Message = "",
                Status = (int)StatusCodeEnum.Ok
            });
        }
    }
}
