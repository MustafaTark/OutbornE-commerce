using Mapster;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.WebUtilities;
using OutbornE_commerce.BAL.AuthServices;
using OutbornE_commerce.BAL.Dto;
using OutbornE_commerce.BAL.Repositories.Currencies;
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
        private readonly ICurrencyRepository _currencyRepository;
        //private readonly IEmailService _emailService;
        public AuthController(
            UserManager<User> userManager, IAuthService authService, ICurrencyRepository currencyRepository)
        {
            _userManager = userManager;
            _authService = authService;
            _currencyRepository = currencyRepository;
        }
        [HttpPost("registerUser")]
        public async Task<IActionResult> RegisterUser([FromBody] RegisterModel userForRegistration)
        {
            var checkByEmail = await _userManager.FindByEmailAsync(userForRegistration.Email);
            if(checkByEmail != null)
            {
                return Ok(new AuthResponseModel
                {
                    MessageEn = "Email already Exist",
                    MessageAr = "هذا البريد موجود من قبل",
                    IsError = true,
                    Email = userForRegistration.Email,
                    
                });
            }
            var user = userForRegistration.Adapt<User>();
            var currency = await _currencyRepository.Find(c => c.IsDeafult);
            if(currency != null)
            {
                user.CurrencyId = currency.Id;
            }
           
            var result = await _userManager.CreateAsync(user, userForRegistration.Password!);
            if (!result.Succeeded)
            {
                //foreach (var error in result.Errors)
                //{
                //    ModelState.TryAddModelError(error.Code, error.Description);
                //}
                return Ok(new AuthResponseModel
                {
                    MessageEn = "Someting Wrong",
                    MessageAr = "حدث خطأ",
                    IsError = true,
                    Email = userForRegistration.Email,

                });
            
        }

            string Role = Enum.GetName(typeof(AccountTypeEnum), userForRegistration.AccountType)!;

            await _userManager.AddToRoleAsync(user, Role);
            return Ok(new AuthResponseModel
            {

                IsError = false,
                Email = userForRegistration.Email,
                Id = await _userManager.GetUserIdAsync(user!),
                AccountType = userForRegistration.AccountType,
                

            });

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

        [HttpPost("resetpassword")]
        [Authorize]
        public async Task<IActionResult> ResetPassword(ResetPasswordModelDto model)
        {
            string userId = User.GetUserIdAPI();
            var user = await _userManager.FindByEmailAsync(userId);
            if (user is null || !await _userManager.CheckPasswordAsync(user, model.OldPassword))
            {
                return Ok(new Response<string>
                {
                    Data = null,
                    IsError = true,
                    Message = "The Old Password Incorrect",
                    MessageAr = "كلمة المرور القديمة خاطئة",
                    Status = (int)StatusCodeEnum.BadRequest
                });
            }
            var hashPassword = _userManager.PasswordHasher.HashPassword(user, model.NewPassword);
            user.PasswordHash = hashPassword;
            var res = await _userManager.UpdateAsync(user);
            if (res.Succeeded)
            {
                return Ok(new Response<string>
                {
                    Data = user.Id,
                    IsError = false,
                    Status = (int)StatusCodeEnum.Ok
                });
            }
            else
            {
                return Ok(new Response<string>
                {
                    Data = null,
                    IsError = true,
                    Message = "Something went wrong",
                    MessageAr = "حدث خطأ",
                    Status = (int)StatusCodeEnum.ServerError
                });
            }
        }
        [HttpPost("forgotpassword/{email}")]
        public async Task<IActionResult> ForgotPassword(string email)
        {
            if (string.IsNullOrEmpty(email))
            {
                return BadRequest("Email address cannot be null or empty.");
            }
            var user = await _userManager.FindByEmailAsync(email);
            if (user == null)
            {
                return NotFound($"Invalid Email Address");
            }
            var token = await _userManager.GeneratePasswordResetTokenAsync(user);
            var encodedToken = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(token));

            var callbackUrl = $"https://localhost:7187/api/Authentication/resetpassword?email={Uri.EscapeDataString(email)}&token={encodedToken}";

            // Send the password reset email with the callback URL
            try
            {
                //await _emailService.SendPasswordResetEmailAsync(email, callbackUrl);
                return Ok(new Response<string>
                {
                    Data = callbackUrl,
                    IsError = false,
                    Status = (int)StatusCodeEnum.Ok
                });
            }
            catch (Exception ex)
            {
                return Ok(new Response<string>
                {
                    Data = user.Id,
                    IsError = false,
                    Status = (int)StatusCodeEnum.BadRequest
                });
            }
        }

    }
}
