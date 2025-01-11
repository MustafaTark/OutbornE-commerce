using Mapster;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using OutbornE_commerce.BAL.Dto;
using OutbornE_commerce.BAL.Dto.Brands;
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
        public async Task<IActionResult> GetAllCateogries(int pageNumber = 1, int pageSize = 10, string? searchTerm = null)
        {
            var items = new PagainationModel<IEnumerable<Category>>();
            if (string.IsNullOrEmpty(searchTerm))
                items = await _categoryRepository.FindAllAsyncByPagination(null, pageNumber, pageSize);
            else
                items = await _categoryRepository
                                    .FindAllAsyncByPagination(b => (b.NameAr.Contains(searchTerm)
                                                               || b.NameEn.Contains(searchTerm))
                                   , pageNumber, pageSize);
            var data = items.Data.Adapt<List<CategoryDto>>();

            return Ok(new PaginationResponse<List<CategoryDto>>
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
        public async Task<IActionResult> CreateCategory([FromForm] CategoryForEdit model, CancellationToken cancellationToken)
        {
            var category = model.Adapt<Category>();
            category.CreatedBy = "admin";
            if (model.Image != null)
            {
                var fileModel = await _filesManager.UploadFile(model.Image, "Categories");
                category.ImageUrl = fileModel!.Url;
            }
            var result = await _categoryRepository.Create(category);
            await _categoryRepository.SaveAsync(cancellationToken);

            return Ok(new Response<Guid> { Data = result.Id, IsError = false, Message = $"", Status = (int)StatusCodeEnum.Ok });

        }

        [HttpPut]
        public async Task<IActionResult> UpdateCategory([FromForm] CategoryForEdit model, CancellationToken cancellationToken)
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
        public async Task<IActionResult> DeleteCategory(Guid id, CancellationToken cancellationToken)
        {
            var category = await _categoryRepository.Find(c => c.Id == id, false);

            _categoryRepository.Delete(category);
            await _categoryRepository.SaveAsync(cancellationToken);

            return Ok(new Response<Guid> { Data = category.Id, IsError = false, Message = $"", Status = (int)StatusCodeEnum.Ok });
        }

        [HttpGet("subCategories/{categoryId}")]
        public async Task<IActionResult> GetAllSubCategories(Guid categoryId, int pageNumber = 1, int pageSize = 10, string? searchTerm = null)
        {
            var items = new PagainationModel<IEnumerable<Category>>();

            if (string.IsNullOrEmpty(searchTerm))
                items = await _categoryRepository.FindAllAsyncByPagination(b => b.ParentCategoryId == categoryId, pageNumber, pageSize, new string[] { "ParentCategory" });
            else
                items = await _categoryRepository
                                   .FindAllAsyncByPagination(b => b.ParentCategoryId == categoryId && (b.NameAr.Contains(searchTerm)
                                                              || b.NameEn.Contains(searchTerm))
                                                              , pageNumber, pageSize, new string[] { "ParentCategory" });

            var data = items.Data.Adapt<List<SubCategoryDto>>();

            return Ok(new PaginationResponse<List<SubCategoryDto>>
            {
                Data = data,
                IsError = false,
                Status = (int)StatusCodeEnum.Ok,
                PageNumber = pageNumber,
                PageSize = pageSize,
                TotalCount = items.TotalCount
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
        public async Task<IActionResult> CreateSubCategory([FromForm] SubCategoryForEdit model, CancellationToken cancellationToken)
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
        public async Task<IActionResult> UpdateSubCategory([FromForm] SubCategoryForEdit model, CancellationToken cancellationToken)
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
        [HttpGet("allMainCategories")]
        public async Task<IActionResult> GetAllMainCategories()
        {
            var categories = await _categoryRepository.FindByCondition(c => c.ParentCategoryId == null);
            var data = categories.Adapt<List<CategoryDto>>();

            return Ok(new Response<List<CategoryDto>>
            {
                Data = data,
                IsError = false,
                Message = "",
                Status = (int)StatusCodeEnum.Ok
            });
        }
        [HttpGet("allSubCategories")]
        public async Task<IActionResult> GetAllSubCategories(Guid categoryId)
        {
            var categories = await _categoryRepository.FindByCondition(c => c.ParentCategoryId == categoryId);
            var data = categories.Adapt<List<CategoryDto>>();
            return Ok(new Response<List<CategoryDto>>
            {
                Data = data,
                IsError = false,
                Message = "",
                Status = (int)StatusCodeEnum.Ok
            });
        }
    }
}
