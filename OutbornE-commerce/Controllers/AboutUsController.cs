using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using OutbornE_commerce.BAL.Dto.AboutUs;
using OutbornE_commerce.BAL.Repositories.AboutUs;
using OutbornE_commerce.FilesManager;

namespace OutbornE_commerce.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AboutUsController : ControllerBase
    {
        private readonly IAboutUsRepository _aboutUsRepository;
        private readonly IFilesManager _filesManager;

        public AboutUsController(IAboutUsRepository aboutUsRepository, IFilesManager filesManager)
        {
            _aboutUsRepository = aboutUsRepository;
            _filesManager = filesManager;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllAboutUs(int pageNumber = 1, int pageSize = 10, string? searchTerm = null)
        {
            var items = new PagainationModel<IEnumerable<AboutUs>>();
            if (string.IsNullOrEmpty(searchTerm))
                items = await _aboutUsRepository.FindAllAsyncByPagination(null, pageNumber, pageSize);
            else
                items = await _aboutUsRepository.FindAllAsyncByPagination(a => (a.BackgroundColor.Contains(searchTerm)
                                                                                                        || a.Description1Ar.Contains(searchTerm)
                                                                                                        || a.Description1En.Contains(searchTerm)
                                                                                                        || a.Description2Ar.Contains(searchTerm)
                                                                                                        || a.Description2En.Contains(searchTerm)
                                                                                                        || a.TitleAr.Contains(searchTerm)
                                                                                                        || a.TitleEn.Contains(searchTerm)), pageNumber, pageSize);
            var data = items.Adapt<List<AboutUsDto>>();
            return Ok(new PaginationResponse<List<AboutUsDto>>
            {
                Data = data,
                IsError = false,
                PageNumber = pageNumber,
                PageSize = pageSize,
                Status = (int)StatusCodeEnum.Ok,
                TotalCount = items.TotalCount
            });
        }
        [HttpGet("All")]
        public async Task<IActionResult> GetAllAboutsUs()
        {
            var items = await _aboutUsRepository.FindAllAsync(null);
            var itemEntites = items.Adapt<List<AboutUsDto>>();
            return Ok(new Response<List<AboutUsDto>>
            {
                Data = itemEntites,
                IsError = false,
                Message = "",
                Status = (int)StatusCodeEnum.Ok
            });
        }
        [HttpPost]
        public async Task<IActionResult> CreateAboutUs([FromForm] AboutUsForCreationDto model, CancellationToken cancellationToken)
        {
            var item = model.Adapt<AboutUs>();
            item.CreatedBy = "admin";
            if (model.Image != null)
            {
                var fileModel = await _filesManager.UploadFile(model.Image, "AboutUs");
                item.ImageUrl = fileModel!.Url;
            }
            var result = await _aboutUsRepository.Create(item);
            await _aboutUsRepository.SaveAsync(cancellationToken);
            return Ok(new Response<Guid>
            {
                Data = result.Id,
                IsError = false,
                Message = "",
                Status = (int)StatusCodeEnum.Ok
            });
        }
        [HttpPut]
        public async Task<IActionResult> UpdateAboutUs([FromForm] AboutUsDto model, CancellationToken cancellationToken)
        {
            var item = await _aboutUsRepository.Find(a => a.Id == model.Id, true);
            if (item is null)
                return Ok(new Response<Guid>
                {
                    Data = model.Id,
                    IsError = true,
                    Message = "",
                    Status = (int)StatusCodeEnum.NotFound
                });
            item = model.Adapt<AboutUs>();
            item.CreatedBy = "admin";
            if (model.Image != null)
            {
                var fileModel = await _filesManager.UploadFile(model.Image, "AboutUs");
                item.ImageUrl = fileModel!.Url;
            }
            _aboutUsRepository.Update(item);
            await _aboutUsRepository.SaveAsync(cancellationToken);
            return Ok(new Response<Guid>
            {
                Data = item.Id,
                IsError = false,
                Message = "",
                Status = (int)StatusCodeEnum.Ok
            });
        }
        [HttpGet("{Id}")]
        public async Task<IActionResult> GetAboutUsById(Guid Id)
        {
            var item = await _aboutUsRepository.Find(a => a.Id == Id);
            if (item is null)
                return Ok(new Response<AboutUsDto>
                {
                    Data = null,
                    IsError = true,
                    Message = "",
                    Status = (int)StatusCodeEnum.NotFound
                });
            var itemEntity = item.Adapt<AboutUsDto>();
            return Ok(new Response<AboutUsDto>
            {
                Data = itemEntity,
                IsError = false,
                Message = "",
                Status = (int)StatusCodeEnum.Ok
            });
        }
        [HttpDelete("{Id}")]
        public async Task<IActionResult> DeleteAboutUs(Guid Id, CancellationToken cancellationToken)
        {
            var item = await _aboutUsRepository.Find(a => a.Id == Id);
            if (item is null)
                return Ok(new Response<AboutUsDto>
                {
                    Data = null,
                    IsError = true,
                    Message = "",
                    Status = (int)StatusCodeEnum.NotFound
                });
            _aboutUsRepository.Delete(item);
            await _aboutUsRepository.SaveAsync(cancellationToken);
            return Ok(new Response<Guid>
            {
                Data = item.Id,
                IsError = false,
                Message = "",
                Status = (int)StatusCodeEnum.Ok
            });
        }
    }
}
