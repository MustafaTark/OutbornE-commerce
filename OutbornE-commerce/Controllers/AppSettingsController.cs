using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using OutbornE_commerce.BAL.Repositories.AppSettings;

namespace OutbornE_commerce.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AppSettingsController : ControllerBase
    {
        private readonly IAppSettingRepository _appSettingRepository;

        public AppSettingsController(IAppSettingRepository appSettingRepository)
        {
            _appSettingRepository = appSettingRepository;
        }
        [HttpPost]
        public async Task<IActionResult> Update([FromBody] AppSettingDto model, CancellationToken cancellationToken)
        {
            var setting = await _appSettingRepository.Find(c => !c.IsDeleted);
            var entity = new AppSetting
            {
                PhonrNumber = model.PhonrNumber,
                InstaPayUsername = model.InstaPayUsername,
            };
            if (setting == null)
            {
                await _appSettingRepository.Create(entity);
                await _appSettingRepository.SaveAsync(cancellationToken);
            }
            else
            {
                entity.Id = setting.Id;
                _appSettingRepository.Update(entity);
            }

            return Ok(new Response<string>
            {
                Data = "",
                IsError = false,
                Status = (int)StatusCodeEnum.Ok,
            });
        }
        [HttpGet]
        public async Task<IActionResult> GetFirst()
        {
            var setting = await _appSettingRepository.Find(c => !c.IsDeleted);
            var data = setting.Adapt<AppSettingDto>();
            return Ok(new Response<AppSettingDto>
            {
                Data = data,
                IsError = false,
                Status = (int)StatusCodeEnum.Ok,
            });
        }
    }
}
