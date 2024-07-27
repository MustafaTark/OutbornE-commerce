using Mapster;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using OutbornE_commerce.BAL.Dto.Categories;
using OutbornE_commerce.BAL.Repositories.Categories;
using OutbornE_commerce.DAL.Models;
using OutbornE_commerce.FilesManager;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace OutbornE_commerce.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoriesController : ControllerBase
    {
        private readonly ICategoryRepository _categoryRepository;
        private readonly IFilesManager _filesManager;

        public CategoriesController(IFilesManager filesManager, ICategoryRepository categoryRepository)
        {
            _filesManager = filesManager;
            _categoryRepository = categoryRepository;
        }
        [HttpGet]
        public async Task<IActionResult> GetAllCateogries()
        {
            var categories = await _categoryRepository.FindAllAsync(null, false);
            var data = categories.Adapt<List<CategoryDto>>();
            return Ok(new Response<List<CategoryDto>>()
            {
                Data =data,
                IsError = false,
                Message = $"",
                Status = (int)StatusCodeEnum.Ok
            });
        }
        [HttpGet("{id}")]
        public async Task<IActionResult> GetCategoryById(Guid id)
        {
            var category = await _categoryRepository.Find(c => c.Id == id, true);
            if (category == null)
            {
                return Ok(new Response<CategoryDto>()
                {
                    Data = null,
                    IsError = true,
                    Message = $"",
                    Status = (int)StatusCodeEnum.NotFound
                });
            }
            var data = category.Adapt<CategoryDto>();
            return Ok(new Response<CategoryDto>()
            {
                Data = data,
                IsError = false,
                Message = $"",
                Status = (int)StatusCodeEnum.Ok
            });
        }

        [HttpPost]
        public async Task<IActionResult> CreateCategory([FromForm] CategoryDto model , CancellationToken cancellationToken)
        {
            var category = model.Adapt<Category>();
            category.CreatedBy = "admin";
            if(model.Image != null)
            {
               var fileModel =  await _filesManager.UploadFile(model.Image, "Categories");
                category.ImageUrl = fileModel!.Url;
            }
            var result = await _categoryRepository.Create(category);
            await _categoryRepository.SaveAsync(cancellationToken);

            return Ok(new Response<Guid> { Data = result.Id, IsError = false, Message = $"", Status = (int)StatusCodeEnum.Ok });

        }

        [HttpPut]
        public async Task<IActionResult> UpdateCategory([FromForm] CategoryDto model , CancellationToken cancellationToken)
        {
            var category = await _categoryRepository.Find(c => c.Id == model.Id, false);
            category = model.Adapt<Category>();
            category.CreatedBy = "admin";

            if (model.Image != null)
            {
                var fileModel = await _filesManager.UploadFile(model.Image, "Categories");
                category.ImageUrl = fileModel!.Url;
            }

            _categoryRepository.Update(category);
            await _categoryRepository.SaveAsync(cancellationToken);

            return Ok(new Response<Guid> { Data = category.Id, IsError = false, Message = $"", Status = (int)StatusCodeEnum.Ok });
        }
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCategory(Guid id,CancellationToken cancellationToken)
        {
            var category = await _categoryRepository.Find(c => c.Id == id, false);

            _categoryRepository.Delete(category);
            await _categoryRepository.SaveAsync(cancellationToken);

            return Ok(new Response<Guid> { Data = category.Id, IsError = false, Message = $"", Status = (int)StatusCodeEnum.Ok });
        }

        [HttpGet("subCategories/{categoryId}")]
        public async Task<IActionResult> GetAllSubCategories(Guid categoryId)
        {
            var subs = await _categoryRepository.FindByCondition(s => s.ParentCategoryId == categoryId);
            var data = subs.Adapt<List<SubCategoryDto>>();

            return Ok(new Response<List<SubCategoryDto>>()
            {
                Data = data,
                IsError = false,
                Message = $"",
                Status = (int)StatusCodeEnum.Ok
            });
        }
        [HttpGet("subCategoryById/{id}")]
        public async Task<IActionResult> GetSubCategoryById(Guid id)
        {
            var sub = await _categoryRepository.Find(s => s.Id == id, false);
            if (sub == null)
            {
                return NotFound();
            }
            var data = sub.Adapt<SubCategoryDto>();
            return Ok(new Response<SubCategoryDto>()
            {
                Data = data,
                IsError = false,
                Message = $"",
                Status = (int)StatusCodeEnum.Ok
            });
        }
        [HttpPost("subcategory")]
        public async Task<IActionResult> CreateSubCategory([FromForm] SubCategoryDto model, CancellationToken cancellationToken)
        {
            var subcategory = model.Adapt<Category>();
            subcategory.CreatedBy = "admin";
            if (model.Image != null)
            {
                var fileModel = await _filesManager.UploadFile(model.Image, "Categories");
                subcategory.ImageUrl = fileModel!.Url;
            }
            var result = await _categoryRepository.Create(subcategory);
            await _categoryRepository.SaveAsync(cancellationToken);

            return Ok(new Response<Guid>()
            {
                Data = result.Id,
                IsError = false,
                Message = $"",
                Status = (int)StatusCodeEnum.Ok
            });
        }

        [HttpPut("subcategory")]
        public async Task<IActionResult> UpdateSubCategory([FromForm] SubCategoryDto model, CancellationToken cancellationToken)
        {
            var category = await _categoryRepository.Find(c => c.Id == model.Id, true);
            category = model.Adapt<Category>();
            category.CreatedBy = "admin";

            if (model.Image != null)
            {
                var fileModel = await _filesManager.UploadFile(model.Image, "Categories");
                category.ImageUrl = fileModel!.Url;
            }

            _categoryRepository.Update(category);
            await _categoryRepository.SaveAsync(cancellationToken);

            return Ok(new Response<Guid>()
            {
                Data = category.Id,
                IsError = false,
                Message = $"",
                Status = (int)StatusCodeEnum.Ok
            });
        }
    }
}
