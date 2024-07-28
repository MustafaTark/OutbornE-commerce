using Mapster;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using OutbornE_commerce.BAL.Dto.Currencies;
using OutbornE_commerce.BAL.Dto.Headers;
using OutbornE_commerce.BAL.Repositories.Headers;
using OutbornE_commerce.DAL.Models;
using OutbornE_commerce.FilesManager;

namespace OutbornE_commerce.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class HeadersController : ControllerBase
    {
        private readonly IHeaderRepository _headerRepository;
        private readonly IFilesManager _filesManager;

        public HeadersController(IHeaderRepository headerRepository, IFilesManager filesManager)
        {
            _headerRepository = headerRepository;
            _filesManager = filesManager;
        }
        [HttpGet]
        public async Task<IActionResult> GetAllHeaders(int pageNumber = 1, int pageSize = 10)
        {
            var items = await _headerRepository.FindAllAsyncByPagination(null, pageNumber, pageSize);

            var data = items.Data.Adapt<List<HeaderDto>>();

            return Ok(new PaginationResponse<List<HeaderDto>>
            {
                Data = data,
                IsError = false,
                Status = (int)StatusCodeEnum.Ok,
                PageNumber = pageNumber,
                PageSize = pageSize,
                TotalCount = items.TotalCount
            });
        }
        [HttpGet("Id")]
        public async Task<IActionResult>GetHeaderById(Guid Id)
        {
            var header = await _headerRepository.Find(i => i.Id == Id, false);
            var headerEntity = header.Adapt<HeaderDto>();
            //if (headerEntity == null)
            //    return Ok(new { message =$"Header with Id{header!.Id} doesn't exist in the database"});
            return Ok(new Response<HeaderDto>
            {
                Data = headerEntity,
                IsError = false,
                Message = "",
                Status = (int)(StatusCodeEnum.Ok)
            });
        }
        [HttpPost]
        public async Task<IActionResult> CreateHeader([FromForm] HeaderForCreationDto model , CancellationToken cancellationToken)
        {
            var header = model.Adapt<Header>();
            header.CreatedBy = "admin";
            if(header.ImageUrl != null)
            {
                var fileModel = await _filesManager.UploadFile(model.Image, "Headers");
                header.ImageUrl = fileModel!.Url;
            }
            var result = await _headerRepository.Create(header);
            await _headerRepository.SaveAsync(cancellationToken);
            return Ok(new Response<Guid>
            {
                Data = header.Id,
                IsError = false,
                Message = "",
                Status = (int)(StatusCodeEnum.Ok)
            });
        }
        [HttpPut]
        public async Task<IActionResult> UpdateHeader([FromForm] HeaderDto model, CancellationToken cancellationToken)
        {
            var header = await _headerRepository.Find(h => h.Id == model.Id, false);
            header = model.Adapt<Header>();
            header.CreatedBy = "admin";
            if (header.ImageUrl != null)
            {
                var fileModel = await _filesManager.UploadFile(model.Image, "Headers");
                header.ImageUrl = fileModel!.Url;
            }
           _headerRepository.Update(header);
            await _headerRepository.SaveAsync(cancellationToken);
            return Ok(new Response<Guid>
            {
                Data = header.Id,
                IsError = false,
                Message = "",
                Status = (int)(StatusCodeEnum.Ok)
            });
        }
        [HttpDelete("Id")]
        public async Task<IActionResult> DeleteHeader(Guid Id , CancellationToken cancellationToken)
        {
            var header = await _headerRepository.Find(h => h.Id==Id, false);
            if(header == null)
                return Ok(new { message = $"Header with Id{header!.Id} doesn't exist in the database" });
            _headerRepository.Delete(header);
            await _headerRepository.SaveAsync(cancellationToken);
            return Ok(new Response<Guid> { Data = header.Id, IsError = false, Message = "", Status = (int)(StatusCodeEnum.Ok) });
        }
    }
}
