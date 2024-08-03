using Mapster;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using OutbornE_commerce.BAL.Dto.Colors;
using OutbornE_commerce.BAL.Dto.ProductColors;
using OutbornE_commerce.BAL.Dto.Sizes;
using OutbornE_commerce.BAL.Repositories.Sizes;
using OutbornE_commerce.DAL.Models;
using System.Net.Sockets;

namespace OutbornE_commerce.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SizesController : ControllerBase
    {
        private readonly ISizeRepository _sizeRepository;

        public SizesController(ISizeRepository sizeRepository)
        {
            _sizeRepository = sizeRepository;
        }
        [HttpGet]
        public async Task<IActionResult> GetAllSizes(int pageNumber = 1, int pageSize = 5,string? searchTerm = null)
        {
            var items = new PagainationModel<IEnumerable<Size>>();

            if (string.IsNullOrEmpty(searchTerm))
                items = await _sizeRepository.FindAllAsyncByPagination(null, pageNumber, pageSize);
            else
                items = await _sizeRepository
                                    .FindAllAsyncByPagination(b => (b.Name.Contains(searchTerm))
                                                               , pageNumber, pageSize);

            var data = items.Data.Adapt<List<SizeDto>>();

            return Ok(new PaginationResponse<List<SizeDto>>
            {
                Data = data,
                IsError = false,
                Status = (int)StatusCodeEnum.Ok,
                PageNumber = pageNumber,
                PageSize = pageSize,
                TotalCount = items.TotalCount
            });
        }
        [HttpGet("Clothing")]
        public async Task<IActionResult> GetAllClothingSizes()
        {
            var clothingSizes = await _sizeRepository.FindByCondition(t => t.Type == 0, null);
            var sizeEntites = clothingSizes.Adapt<List<SizeDto>>();
            return Ok(new Response<List<SizeDto>>
            {
                Data = sizeEntites,
                IsError = false,
                Status = (int)StatusCodeEnum.Ok
            });
        }
        [HttpGet("Shoes")]
        public async Task<IActionResult> GetAllShoesSizes()
        {
            var shoesSizes = await _sizeRepository.FindByCondition(t => (int)t.Type == 1, null);
            var sizeEntites = shoesSizes.Adapt<List<SizeDto>>();
            return Ok(new Response<List<SizeDto>>
            {
                Data = sizeEntites,
                IsError = false,
                Status = (int)StatusCodeEnum.Ok
            });
        }
        [HttpGet("{Id}")]
        public async Task<IActionResult> GetSizeById(Guid Id)
        {
            var size = await _sizeRepository.Find(c => c.Id == Id, false);
            if (size == null)
                return Ok(new { message = $"Size with Id: {size!.Id} doesn't exist in the database" });
            var sizeEntity = size.Adapt<SizeDto>();
            return Ok(new Response<SizeDto>
            {
                Data = sizeEntity,
                IsError = false,
                Status = (int)StatusCodeEnum.Ok
            });
        }
        [HttpPost("CreateSize")]
        public async Task<IActionResult> CreateSize([FromBody] SizeForCreationDto model, CancellationToken cancellationToken)
        {
            var size = model.Adapt<Size>();
            size.CreatedBy = "admin";
            var result = await _sizeRepository.Create(size);
            await _sizeRepository.SaveAsync(cancellationToken);
            return Ok(new Response<Guid>()
            {
                Data = size.Id,
                IsError = false,
                Status = (int)StatusCodeEnum.Ok

            });
        }
        [HttpPut("UpdateSize")]
        public async Task<IActionResult> UpdateSize([FromBody] SizeDto model, CancellationToken cancellationToken)
        {
            var size = await _sizeRepository.Find(s => s.Id == model.Id, false);
            if (size == null)
                return Ok(new { message = $"Size with Id: {size!.Id} doesn't exist in the database" });
            size = model.Adapt<Size>();
            size.UpdatedBy = "admin";
            _sizeRepository.Update(size);
            await _sizeRepository.SaveAsync(cancellationToken);
            return Ok(new Response<Guid>()
            {
                Data = size.Id,
                IsError = false,
                Status = (int)StatusCodeEnum.Ok

            });
        }
        [HttpDelete("{Id}")]
        public async Task<IActionResult> DeleteSize(Guid Id, CancellationToken cancellationToken)
        {
            var size = await _sizeRepository.Find(c => c.Id == Id, false);
            if (size == null)
                return Ok(new { message = $"Size with Id: {size!.Id} doesn't exist in the database" });
            _sizeRepository.Delete(size);
            await _sizeRepository.SaveAsync(cancellationToken);
            return Ok(new Response<Guid>()
            {
                Data = Id,
                IsError = false,
                Status = (int)StatusCodeEnum.Ok

            });
        }
    }
}
