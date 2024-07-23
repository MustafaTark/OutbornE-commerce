using Mapster;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
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
        public async Task<IActionResult> GetAllHeaders()
        {
            var header = await _headerRepository.FindAllAsync(null, false);
            var headerEntites = header.Adapt<HeaderDto>();
            if(headerEntites == null) 
                return Ok(new {Message = "No Headers are inserted in the database yet ! "});
            return Ok(new {data = headerEntites , Message = ""});
        }
        [HttpGet("Id")]
        public async Task<IActionResult>GetHeaderById(Guid Id)
        {
            var header = await _headerRepository.Find(i => i.Id == Id, false);
            var headerEntity = header.Adapt<HeaderDto>();
            if (headerEntity == null)
                return Ok(new { message =$"Header with Id{header!.Id} doesn't exist in the database"});
            return Ok(new { data = headerEntity, Message = "" });
        }
        [HttpPost]
        public async Task<IActionResult> CreateHeader([FromForm] HeaderDto model , CancellationToken cancellationToken)
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
            return Ok(new { data = result.Id  , message = ""});
        }
        [HttpPut]
        public async Task<IActionResult> UpdateHeader([FromForm] HeaderDto model, CancellationToken cancellationToken)
        {
            var header = await _headerRepository.Find(h => h.Id == model.Id, true);
            header = model.Adapt<Header>();
            header.CreatedBy = "admin";
            if (header.ImageUrl != null)
            {
                var fileModel = await _filesManager.UploadFile(model.Image, "Headers");
                header.ImageUrl = fileModel!.Url;
            }
           _headerRepository.Update(header);
            await _headerRepository.SaveAsync(cancellationToken);
            return Ok(new { data = header.Id, message = "" });
        }
        [HttpDelete("Id")]
        public async Task<IActionResult> DeleteHeader(Guid Id , CancellationToken cancellationToken)
        {
            var header = await _headerRepository.Find(h => h.Id==Id, true);
            if(header == null)
                return Ok(new { message = $"Header with Id{header!.Id} doesn't exist in the database" });
            _headerRepository.Delete(header);
            await _headerRepository.SaveAsync(cancellationToken);
            return Ok(new { data = header.Id, message = "" });
        }
    }
}
