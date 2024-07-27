using Mapster;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using OutbornE_commerce.BAL.Dto;
using OutbornE_commerce.BAL.Dto.Colors;
using OutbornE_commerce.BAL.Repositories.Colors;
using OutbornE_commerce.DAL.Models;
using System.Reflection.PortableExecutable;

namespace OutbornE_commerce.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ColorsController : ControllerBase
    {
        private readonly IColorRepository _colorRepository;
        public ColorsController(IColorRepository colorRepository)
        {
            _colorRepository = colorRepository;
        }
        [HttpGet("StaticColor")]
        public IActionResult GetStaticColor()
        {
            return Ok(new ColorDto()
            {
                Hexadecimal = "#CCC",
                NameAr = "رمادي",
                NameEn = "Grey"
            });
        }
        [HttpGet]
        public async Task<IActionResult> GetAllColors()
        {
            var colors = await _colorRepository.FindAllAsync(null, false);
            var colorEntites = colors.Adapt<List<ColorDto>>();
            return Ok(new { data = colorEntites, message = ""});
        }
        [HttpGet("Id")]
        public async Task<IActionResult> GetColorById(Guid Id)
        {
            var color = await _colorRepository.Find(c => c.Id == Id, false);
            if (color == null)
                return Ok(new Response<ColorDto>
                {
                    Data = null,
                    Status = (int)StatusCodeEnum.NotFound,
                    IsError = true,
                    Message = $"Color with Id: {Id} doesn't exist in the database"
                }); ;
            var colorEntity = color.Adapt<ColorDto>();
            return Ok(new Response<ColorDto>
            {
                Data = colorEntity,
                Status = (int)StatusCodeEnum.Ok, // Success  
                IsError = false,
                Message = ""
            });
        }
        [HttpPost]
        public async Task<IActionResult> CreateColor([FromBody] ColorForCreationDto model , CancellationToken cancellationToken )
        {
            var color = model.Adapt<Color>();
            color.CreatedBy = "admin";
            var result = await _colorRepository.Create(color);
            await _colorRepository.SaveAsync(cancellationToken);
            return Ok(new Response<Guid>
            {
                Data = result.Id,
                Status = (int)StatusCodeEnum.Ok,
                IsError = false,
                Message = ""
            });
        }
        [HttpPut]
        public async Task<IActionResult> UpdateColor([FromBody] ColorDto model ,CancellationToken cancellationToken )
        {
            var color = await _colorRepository.Find(c => c.Id == model.Id , false);
            if (color == null)
                return Ok(new { message = $"Color with Id: doesn't exist in the database" });
            color = model.Adapt<Color>();
            color.UpdatedBy = "admin";
            _colorRepository.Update(color);
            await _colorRepository.SaveAsync(cancellationToken) ;
            return Ok(new Response<Guid>
            {
                Data = color.Id,
                Status = (int)StatusCodeEnum.Ok,
                IsError = false,
                Message = ""
            });
        }
        [HttpDelete("Id")]
        public async Task<IActionResult> DeleteColor(Guid Id , CancellationToken cancellationToken)
        {
            var color = await _colorRepository.Find(c => c.Id == Id, false);
            if (color == null)
                return Ok(new { message = $"Color with Id: doesn't exist in the database" });
            _colorRepository.Delete(color);
            await _colorRepository.SaveAsync(cancellationToken);
            return Ok(new Response<Guid>
            {
                Data = color.Id,
                Status = (int)StatusCodeEnum.Ok,
                IsError = false,
                Message = ""
            });
        }
    }
}
