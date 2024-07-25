using Mapster;
using Microsoft.AspNetCore.Mvc;
using OutbornE_commerce.BAL.Dto;
using OutbornE_commerce.BAL.Dto.Brands;
using OutbornE_commerce.BAL.Dto.Categories;
using OutbornE_commerce.BAL.Repositories.Brands;
using OutbornE_commerce.BAL.Repositories.Categories;
using OutbornE_commerce.DAL.Enums;
using OutbornE_commerce.DAL.Models;
using OutbornE_commerce.FilesManager;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace OutbornE_commerce.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BrandsController : ControllerBase
    {
        private readonly IBrandRepository _brandRepository;
        private readonly IFilesManager _filesManager;

        public BrandsController(IFilesManager filesManager, IBrandRepository brandRepository)
        {
            _filesManager = filesManager;
            _brandRepository = brandRepository;
        }
        [HttpGet]
        public async Task<IActionResult> GetAllBrands()
        {
            var brands = await _brandRepository.FindAllAsync(null, false);
            var data = brands.Adapt<List<BrandDto>>();

             return Ok(new Response<List<BrandDto>>
            {
                Data = data,
                IsError = false,
                Message = $"",
                Status = (int)StatusCodeEnum.Ok
            });
        }
        [HttpGet("{id}")]
        public async Task<IActionResult> GetBrandById(Guid id)
        {
            var brand = await _brandRepository.Find(c => c.Id == id, false);
            if (brand == null)
            {
                return Ok(new Response<CityDto>
                {
                    Data = null,
                    IsError = true,
                    Message = $"",
                    Status = (int)StatusCodeEnum.NotFound
                });
            }
            var data = brand.Adapt<BrandDto>();
            return Ok(new Response<BrandDto>
            {
                Data = data,
                IsError = false,
                Message = $"",
                Status = (int)StatusCodeEnum.Ok
            });
        }

        [HttpPost]
        public async Task<IActionResult> CreateBrand([FromForm] BrandDto model, CancellationToken cancellationToken)
        {
            var brand = model.Adapt<Brand>();
            brand.CreatedBy = "admin";
            if (model.Image != null)
            {
                var fileModel = await _filesManager.UploadFile(model.Image, "Brands");
                brand.ImageUrl = fileModel!.Url;
            }
            var result = await _brandRepository.Create(brand);
            await _brandRepository.SaveAsync(cancellationToken);

            return Ok(new Response<Guid>
            {
                Data = result.Id,
                IsError = false,
                Message = $"",
                Status = (int)StatusCodeEnum.Ok
            }); 

        }

        [HttpPut]
        public async Task<IActionResult> UpdateBrand([FromForm] BrandDto model, CancellationToken cancellationToken)
        {
            var brand = await _brandRepository.Find(c => c.Id == model.Id, true);
            brand = model.Adapt<Brand>();
            brand.CreatedBy = "admin";

            if (model.Image != null)
            {
                var fileModel = await _filesManager.UploadFile(model.Image, "Brands");
                brand.ImageUrl = fileModel!.Url;
            }

            _brandRepository.Update(brand);
            await _brandRepository.SaveAsync(cancellationToken);

            return Ok(new Response<Guid>
            {
                Data = brand.Id,
                IsError = false,
                Message = $"",
                Status = (int)StatusCodeEnum.Ok
            }); ;
        }
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteBrand(Guid id, CancellationToken cancellationToken)
        {
            var brand = await _brandRepository.Find(c => c.Id == id, true);

            _brandRepository.Delete(brand);
            await _brandRepository.SaveAsync(cancellationToken);

            return Ok(new Response<Guid>
            {
                Data = id,
                IsError = false,
                Message = $"",
                Status = (int)StatusCodeEnum.Ok
            });
        }

        [HttpGet("subBrands/{brandId}")]
        public async Task<IActionResult> GetAllSubBrands(Guid brandId)
        {
            var subs = await _brandRepository.FindByCondition(s => s.ParentBrandId == brandId);
            var data = subs.Adapt<List<SubBrandDto>>();

            return Ok(new Response<List<SubBrandDto>>
            {
                Data = data,
                IsError = false,
                Message = $"",
                Status = (int)StatusCodeEnum.Ok
            }); ;
        }
        [HttpGet("subBrandById/{id}")]
        public async Task<IActionResult> GetSubBrandById(Guid id)
        {
            var sub = await _brandRepository.Find(s => s.Id == id, false);
            if (sub == null)
            {
                return NotFound();
            }
            var data = sub.Adapt<SubBrandDto>();

            return Ok(new Response<SubBrandDto>
            {
                Data = data,
                IsError = false,
                Message = $"",
                Status = (int)StatusCodeEnum.Ok
            });
        }
        [HttpPost("subBrand")]
        public async Task<IActionResult> CreateSubBrand([FromForm] SubBrandDto model, CancellationToken cancellationToken)
        {
            var subbrand = model.Adapt<Brand>();
            subbrand.CreatedBy = "admin";
            if (model.Image != null)
            {
                var fileModel = await _filesManager.UploadFile(model.Image, "Categories");
                subbrand.ImageUrl = fileModel!.Url;
            }
            var result = await _brandRepository.Create(subbrand);
            await _brandRepository.SaveAsync(cancellationToken);

            return Ok(new Response<Guid>
            {
                Data = result.Id,
                IsError = false,
                Message = $"",
                Status = (int)StatusCodeEnum.Ok
            });
        }

        [HttpPut("subcategory")]
        public async Task<IActionResult> UpdateSubBrand([FromForm] SubBrandDto model, CancellationToken cancellationToken)
        {
            var brand = await _brandRepository.Find(c => c.Id == model.Id, true);
            brand = model.Adapt<Brand>();
            brand.CreatedBy = "admin";

            if (model.Image != null)
            {
                var fileModel = await _filesManager.UploadFile(model.Image, "Categories");
                brand.ImageUrl = fileModel!.Url;
            }

            _brandRepository.Update(brand);
            await _brandRepository.SaveAsync(cancellationToken);

            return Ok(new Response<Guid>
            {
                Data = brand.Id,
                IsError = false,
                Message = $"",
                Status = (int)StatusCodeEnum.Ok
            });
        }
    }
}
