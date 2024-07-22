using Mapster;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using OutbornE_commerce.BAL.Dto.Categories;
using OutbornE_commerce.BAL.Repositories.Categories;
using OutbornE_commerce.DAL.Models;
using OutbornE_commerce.FilesManager;

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
            var categories = await _categoryRepository.GetAllAsync(null, false);
            var data = categories.Adapt<List<CategoryDto>>();
            return Ok(data);
        }
        [HttpGet("{id}")]
        public async Task<IActionResult> GetCategoryById(Guid id)
        {
            var category = await _categoryRepository.Find(c => c.Id == id, false);
            if (category == null)
            {
                return NotFound();
            }
            return Ok(category);
        }

        [HttpPost]
        public async Task<IActionResult> CreateCategory([FromForm] CategoryDto model)
        {
            var category = model.Adapt<Category>();
            category.CreatedBy = "admin";
            if(model.Image != null)
            {
               var fileModel =  await _filesManager.UploadFile(model.Image, "Categories");
                category.ImageUrl = fileModel!.Url;
            }
            var result = await _categoryRepository.Create(category);
            await _categoryRepository.SaveAsync();

            return Ok(result.Id);

        }

        [HttpPut]
        public async Task<IActionResult> UpdateCategory([FromForm] CategoryDto model)
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
            await _categoryRepository.SaveAsync();

            return Ok(category.Id);
        }
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCategory(Guid id)
        {
            var category = await _categoryRepository.Find(c => c.Id == id, true);

            _categoryRepository.Delete(category);
            await _categoryRepository.SaveAsync();

            return Ok(category.Id);
        }

        [HttpGet("subCategories/{categoryId}")]
        public async Task<IActionResult> GetAllSubCategories(Guid categoryId)
        {
            var subs = await _categoryRepository.FindAllByCondition(s => s.ParentCategoryId == categoryId);
            var data = subs.Adapt<List<SubCategoryDto>>();
            return Ok(data);
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
            return Ok(data);
        }
        [HttpPost("subcategory")]
        public async Task<IActionResult> CreateSubCategory([FromForm] SubCategoryDto model)
        {
            var subcategory = model.Adapt<Category>();
            subcategory.CreatedBy = "admin";
            if (model.Image != null)
            {
                var fileModel = await _filesManager.UploadFile(model.Image, "Categories");
                subcategory.ImageUrl = fileModel!.Url;
            }
            var result = await _categoryRepository.Create(subcategory);
            await _categoryRepository.SaveAsync();

            return Ok(result.Id);
        }

        [HttpPut("subcategory")]
        public async Task<IActionResult> UpdateSubCategory([FromForm] CategoryDto model)
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
            await _categoryRepository.SaveAsync();

            return Ok(category.Id);
        }
    }
}
