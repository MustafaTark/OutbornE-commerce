﻿using Mapster;
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
        public async Task<IActionResult> GetAllBrands(int pageNumber = 1 ,int pageSize = 10,string? searchTerm= null)
        {
            // var brands = await _brandRepository.FindAllAsync(null, false);
            var brands = new PagainationModel<IEnumerable<Brand>>();
            if(searchTerm  == null)
               brands = await _brandRepository.FindAllAsyncByPagination(null,pageNumber,pageSize);
            else
                brands = await _brandRepository
                                  .FindAllAsyncByPagination(b=>b.NameAr.Contains(searchTerm)
                                                             ||b.NameEn.Contains(searchTerm)
                                 ,pageNumber,pageSize);

            var data = brands.Data.Adapt<List<BrandDto>>();

             return Ok(new PaginationResponse<List<BrandDto>>
            {
                Data = data,
                IsError = false,
                Status = (int)StatusCodeEnum.Ok,
                PageNumber = pageNumber,
                PageSize = pageSize,
                TotalCount = brands.TotalCount
            });
        }
        [HttpGet("{id}")]
        public async Task<IActionResult> GetBrandById(Guid id)
        {
            var brand = await _brandRepository.Find(c => c.Id == id, false);
            if (brand == null)
            {
                return Ok(new Response<BrandDto>
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
            var brand = await _brandRepository.Find(c => c.Id == model.Id, false);
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
            var brand = await _brandRepository.Find(c => c.Id == id, false);

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
    }
}
