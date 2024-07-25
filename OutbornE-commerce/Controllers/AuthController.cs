using Mapster;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.WebUtilities;
using OutbornE_commerce.BAL.AuthServices;
using OutbornE_commerce.BAL.Dto;
using OutbornE_commerce.DAL.Enums;
using OutbornE_commerce.DAL.Models;
using OutbornE_commerce.Extensions;
using System.Text;

namespace OutbornE_commerce.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly UserManager<User> _userManager;
        private readonly IAuthService _authService;
        //private readonly IEmailService _emailService;
        public AuthController(
            UserManager<User> userManager, IAuthService authService)
        {
            _userManager = userManager;
            _authService = authService;
        }
        [HttpPost("registerUser")]
        public async Task<IActionResult> RegisterUser([FromBody] RegisterModel userForRegistration)
        {
            var checkByEmail = await _userManager.FindByEmailAsync(userForRegistration.Email);
            if(checkByEmail != null)
            {
                return BadRequest(new List<string> { "Email already Exist"});
            }
            var user = userForRegistration.Adapt<User>();
            var result = await _userManager.CreateAsync(user, userForRegistration.Password!);
            if (!result.Succeeded)
            {
                //foreach (var error in result.Errors)
                //{
                //    ModelState.TryAddModelError(error.Code, error.Description);
                //}
                return BadRequest(result.Errors.Select(e=>e.Description).ToList());
            }

            string Role = Enum.GetName(typeof(AccountTypeEnum), userForRegistration.AccountType)!;

            await _userManager.AddToRoleAsync(user, Role);
              return Ok(
                new
                {
                    UserId = await _userManager.GetUserIdAsync(user!)
                }
                );

        }
        [HttpPost("login")]

        public async Task<IActionResult> Authenticate([FromBody] UserForLoginDto user)
        {
            var validate = await _authService.ValidateUser(user);
            if (validate is not null)
                return Ok(validate);

            var userModel = await _userManager.FindByEmailAsync(user.Email!);
            var token = await _authService.CreateToken();
            var roles = await _userManager.GetRolesAsync(userModel!);

           var response =  userModel!.GetAuthResponse(roles.ToList(), "Success","تم بنجاح", (int)StatusCodeEnum.Ok, token);
            return Ok(response);
        }

        //[HttpPost("resetpassword")]
        //public async Task<IActionResult> ResetPassword(ResetPasswordModelDto model)
        //{
        //    var user = await _userManager.FindByEmailAsync(model.Email!);
        //    if (user == null)
        //        return NotFound($"Invalid Email Address");
        //    var decodedToken = Encoding.UTF8.GetString(WebEncoders.Base64UrlDecode(model.Token!));
        //    var result = await _userManager.ResetPasswordAsync(user, decodedToken, model.NewPassword!);
        //    if (result.Succeeded)
        //    {
        //        return StatusCodeEnum(201, "Password reset successful");
        //    }
        //    else
        //    {
        //        return BadRequest(result.Errors);
        //    }
        //}
        //[HttpPost("forgotpassword/{email}")]
        //public async Task<IActionResult> ForgotPassword(string email)
        //{
        //    if (string.IsNullOrEmpty(email))
        //    {
        //        return BadRequest("Email address cannot be null or empty.");
        //    }
        //    var user = await _userManager.FindByEmailAsync(email);
        //    if (user == null)
        //    {
        //        return NotFound($"Invalid Email Address");
        //    }
        //    var token = await _userManager.GeneratePasswordResetTokenAsync(user);
        //    var encodedToken = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(token));

        //    var callbackUrl = $"https://localhost:7187/api/Authentication/resetpassword?email={Uri.EscapeDataString(email)}&token={encodedToken}";

        //    // Send the password reset email with the callback URL
        //    try
        //    {
        //        await _emailService.SendPasswordResetEmailAsync(email, callbackUrl);
        //        return Ok();
        //    }
        //    catch (Exception ex)
        //    {
        //        return StatusCodeEnum(500, $"An error occurred while sending the password reset email. {ex.Message}");
        //    }
        //}

    }
}
