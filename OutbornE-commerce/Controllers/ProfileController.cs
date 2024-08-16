using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using OutbornE_commerce.BAL.Dto.Profile;
using OutbornE_commerce.Extensions;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace OutbornE_commerce.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class ProfileController : ControllerBase
    {
        private readonly UserManager<User> _userManager;

        public ProfileController(UserManager<User> userManager)
        {
            _userManager = userManager;
        }
        [HttpGet]
        public async Task<IActionResult> GetProfile()
        {
            string userId = User.GetUserIdAPI();
            var user = await _userManager.FindByIdAsync(userId);
            var data = user.Adapt<ProfileDto>();
            return Ok(new Response<ProfileDto>
            {
                Data = data,
                IsError = false,
                Status = (int)StatusCodeEnum.Ok
            });
        }
        [HttpPut]
        public async Task<IActionResult> EditProfile([FromBody] ProfileDto model)
        {
            var user = await _userManager.FindByIdAsync(model.Id);
            user.Email= model.Email;
            user.CurrencyId = model.CurrencyId;
            user.FullName = model.FullName;

            if (user.PhoneNumber == model.PhoneNumber)
                user.PhoneNumberConfirmed = false;

            user.PhoneNumber = model.PhoneNumber;
            await _userManager.UpdateAsync(user);
            return Ok(new Response<string>
            {
                Data = model.Id,
                IsError = false,
                Status = (int)StatusCodeEnum.Ok
            });
        }
    }
}
