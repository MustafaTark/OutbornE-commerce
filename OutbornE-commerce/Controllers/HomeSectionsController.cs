using Mapster;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using OutbornE_commerce.BAL.Dto.HomeSections;
using OutbornE_commerce.BAL.Repositories.HomeSections;
using OutbornE_commerce.DAL.Models;
using OutbornE_commerce.FilesManager;
using System.Reflection.PortableExecutable;
using static System.Collections.Specialized.BitVector32;

namespace OutbornE_commerce.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class HomeSectionsController : ControllerBase
    {
        private readonly IHomeSectionRepository _homeSectionRepository;
        private readonly IFilesManager _filesManager;

        public HomeSectionsController(IHomeSectionRepository homeSectionRepository, IFilesManager filesManager)
        {
            _homeSectionRepository = homeSectionRepository;
            _filesManager = filesManager;
        }
        [HttpGet]
        public async Task<IActionResult> GetHomeSection()
        {
            var homeSection = await _homeSectionRepository.FindAllAsync(null, false);
            var homeEntity = homeSection.Adapt<List<HomeSectionDto>>();
            return Ok(homeEntity);
        }
        [HttpPost]
        public async Task<IActionResult> CreateHomeSection([FromForm] HomeSectionForCreationDto model , CancellationToken cancellationToken)
        {
            var homeSection = await _homeSectionRepository.FindAllAsync(null, false);
            if (homeSection.Any())
                return Ok(new { message = "You have already inserted a home section" });
            var section = model.Adapt<HomeSection>();
            section.CreatedBy = "admin";
            if (section.ImageUrl != null)
            {
                var fileModel = await _filesManager.UploadFile(model.Image, "HomeSections");
                section.ImageUrl = fileModel!.Url;
            }
            var result = await _homeSectionRepository.Create(section);
            await _homeSectionRepository.SaveAsync(cancellationToken);
            return Ok(result.Id);
        }
        [HttpPut]
        public async Task<IActionResult> UpdateHomeSection([FromForm] HomeSectionDto model , CancellationToken cancellationToken)
        {
            var homeSection = await _homeSectionRepository.Find(s => s.Id == model.Id, false);
            if (homeSection == null)
                return Ok(new {message = $"Home Section with Id : {homeSection!.Id} doesn't exist in the database"});
            homeSection = model.Adapt<HomeSection>();
            homeSection.CreatedBy = "admin";
            if (homeSection.ImageUrl != null)
            {
                var fileModel = await _filesManager.UploadFile(model.Image, "HomeSections");
                homeSection.ImageUrl = fileModel!.Url;
            }
            _homeSectionRepository.Update(homeSection);
            await _homeSectionRepository.SaveAsync(cancellationToken);
            return Ok(homeSection.Id);
        }
        [HttpDelete("Id")]
        public async Task<IActionResult> DeleteHomeSection(Guid Id, CancellationToken cancellationToken)
        {
            var homeSection = await _homeSectionRepository.Find(h => h.Id == Id, false);
            if (homeSection == null)
                return Ok(new { message = $"Home Section with Id: {homeSection!.Id} doesn't exist in the database" });
            _homeSectionRepository.Delete(homeSection);
            await _homeSectionRepository.SaveAsync(cancellationToken);
            return Ok(homeSection.Id);
        }
    }
}
