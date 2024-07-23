using Mapster;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using OutbornE_commerce.BAL.Dto.Colors;
using OutbornE_commerce.BAL.Dto.Sizes;
using OutbornE_commerce.BAL.Repositories.Sizes;
using OutbornE_commerce.DAL.Models;

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
        public async Task<IActionResult> GetAllSizes()
        {
            var sizes = await _sizeRepository.FindAllAsync(null, false);
            var sizeEntites = sizes.Adapt<List<SizeDto>>();
            return Ok(new { data = sizeEntites, message = "" });
        }
        [HttpGet("Clothing")]
        public async Task<IActionResult> GetAllClothingSizes()
        {
            var clothingSizes = await _sizeRepository.FindByCondition(t => t.Type == 0, null);
            var sizeEntites = clothingSizes.Adapt<List<SizeDto>>();
            return Ok(new { data = sizeEntites, message = "" });
        }
        [HttpGet("Shoes")]
        public async Task<IActionResult> GetAllShoesSizes()
        {
            var shoesSizes = await _sizeRepository.FindByCondition(t => (int)t.Type == 1, null);
            var sizeEntites = shoesSizes.Adapt<List<SizeDto>>();
            return Ok(new { data = sizeEntites, message = "" });
        }
        [HttpGet("Id")]
        public async Task<IActionResult> GetSizeById(Guid Id)
        {
            var size = await _sizeRepository.Find(c => c.Id == Id, false);
            if (size == null)
                return Ok(new { message = $"Size with Id: {size!.Id} doesn't exist in the database" });
            var sizeEntity = size.Adapt<SizeDto>();
            return Ok(new { data = sizeEntity, message = "" });
        }
        //[HttpPost]
        //public async Task<IActionResult> CreateSize([FromForm] SizeForCreationDto model, CancellationToken cancellationToken)
        //{
        //    var size = model.Adapt<Size>();
        //    size.CreatedBy = "admin";
        //    var result = await _sizeRepository.Create(size);
        //    await _sizeRepository.SaveAsync(cancellationToken);
        //    return Ok(new { data = result.Id, message = "" });
        //}
        //[HttpPut]
        //public async Task<IActionResult> UpdateSize([FromForm] SizeDto model, CancellationToken cancellationToken)
        //{
        //    var size = await _sizeRepository.Find(s => s.Id == model.Id, true);
        //    if (size == null)
        //        return Ok(new { message = $"Size with Id: {size!.Id} doesn't exist in the database" });
        //    size = model.Adapt<Size>();
        //    size.UpdatedBy = "admin";
        //    _sizeRepository.Update(size);
        //    await _sizeRepository.SaveAsync(cancellationToken);
        //    return Ok(new { data = size.Id, message = "" });
        //}
        [HttpDelete("Id")]
        public async Task<IActionResult> DeleteSize(Guid Id, CancellationToken cancellationToken)
        {
            var size = await _sizeRepository.Find(c => c.Id == Id, true);
            if (size == null)
                return Ok(new { message = $"Size with Id: {size!.Id} doesn't exist in the database" });
            _sizeRepository.Delete(size);
            await _sizeRepository.SaveAsync(cancellationToken);
            return Ok(new { data = Id, message = "" });
        }
    }
}
