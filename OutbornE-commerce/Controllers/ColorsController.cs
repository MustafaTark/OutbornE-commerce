using Mapster;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
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
        [HttpGet("GetStaticColor")]
        public async Task<IActionResult> GetStaticColor()
        {
            return Ok(new { hexa = "#6EACDA", Name = "testcolor" });
        }
        [HttpGet]
        public async Task<IActionResult> GetAllColors()
        {
            var colors = await _colorRepository.FindAllAsync(null, false);
            var colorEntites = colors.Adapt<List<ColorDto>>();
            return Ok(colorEntites);
        }
        [HttpGet("Id")]
        public async Task<IActionResult> GetColorById(Guid Id)
        {
            var color = await _colorRepository.Find(c => c.Id == Id , false);
            if (color == null)
                return Ok(new { message = $"Color with Id: {color!.Id} doesn't exist in the database" });
            var colorEntity = color.Adapt<ColorDto>();
            return Ok(colorEntity);
        }
        [HttpPost]
        public async Task<IActionResult> CreateColor([FromForm] ColorForCreationDto model , CancellationToken cancellationToken )
        {
            var color = model.Adapt<Color>();
            color.CreatedBy = "admin";
            var result = await _colorRepository.Create(color);
            await _colorRepository.SaveAsync(cancellationToken);
            return Ok(result.Id);
        }
        [HttpPut]
        public async Task<IActionResult> UpdateColor([FromForm] ColorDto model ,CancellationToken cancellationToken )
        {
            var color = await _colorRepository.Find(c => c.Id == model.Id , true);
            if (color == null)
                return Ok(new { message = $"Color with Id: {color!.Id} doesn't exist in the database" });
            color = model.Adapt<Color>();
            color.UpdatedBy = "admin";
            _colorRepository.Update(color);
            await _colorRepository.SaveAsync(cancellationToken) ;
            return Ok(color.Id);
        }
        [HttpDelete("Id")]
        public async Task<IActionResult> DeleteColor(Guid Id , CancellationToken cancellationToken)
        {
            var color = await _colorRepository.Find(c => c.Id == Id, true);
            if (color == null)
                return Ok(new { message = $"Color with Id: {color!.Id} doesn't exist in the database" });
            _colorRepository.Delete(color);
            await _colorRepository.SaveAsync(cancellationToken);
            return Ok(Id);
        }
    }
}
