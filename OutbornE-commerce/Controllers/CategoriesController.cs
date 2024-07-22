using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using OutbornE_commerce.BAL.Repositories.Categories;
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
    }
}
