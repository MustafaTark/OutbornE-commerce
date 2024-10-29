using Mapster;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.EntityFrameworkCore;
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
        public AuthController(
            UserManager<User> userManager, IAuthService authService)
        {
            _userManager = userManager;
            _authService = authService;
        }
        [HttpPost("registerUser")]
        public async Task<IActionResult> RegisterUser([FromBody] RegisterUserDto userForRegistration)
        {
            var checkByPhone = await _userManager.Users.Where(p=>p.PhoneNumber == userForRegistration.Phone).FirstOrDefaultAsync();
            if(checkByPhone != null)
            {
                return Ok(new AuthResponseModel
                {
                    MessageEn = "Phone already Exist",
                    MessageAr = "هذا الرقم موجود من قبل",
                    IsError = true,
                    PhoneNumber = userForRegistration.Phone,
                    
                });
            }
            var user = userForRegistration.Adapt<User>();
            user.UserName = userForRegistration.Phone;
            user.PhoneNumber = userForRegistration.Phone;
            user.Email = userForRegistration.Phone;
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
                    PhoneNumber = userForRegistration.Phone,

                });
            
            }

         //   string Role = Enum.GetName(typeof(AccountTypeEnum), userForRegistration.AccountType)!;

            await _userManager.AddToRoleAsync(user, "User");
            return Ok(new AuthResponseModel
            {

                IsError = false,
                PhoneNumber = userForRegistration.Phone,
                Id = await _userManager.GetUserIdAsync(user!),
                AccountType = (int)AccountTypeEnum.User,
                

            });

        } 
        [HttpPost("registerAdmin")]
        public async Task<IActionResult> RegisterAdmin([FromBody] RegisterModel userForRegistration)
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
            user.UserName = userForRegistration.Email;
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

            await _userManager.AddToRoleAsync(user, "Admin");
            return Ok(new AuthResponseModel
            {

                IsError = false,
                Email = userForRegistration.Email,
                Id = await _userManager.GetUserIdAsync(user!),
                AccountType = (int) AccountTypeEnum.Admin,
            });

        }
        [HttpPost("login")]


        public async Task<IActionResult> Authenticate([FromBody] UserForLoginDto user)
        {
            var validate = await _authService.ValidateUser(user);
            if (validate is not null)
                return Ok(validate);

            var userModel = await _userManager.Users.Where(u=>u.PhoneNumber == user.EmailOrPhone).FirstOrDefaultAsync();
            var token = await _authService.CreateToken();
            var roles = await _userManager.GetRolesAsync(userModel!);

           var response =  userModel!.GetAuthResponse(roles.ToList(), "Success","تم بنجاح", (int)StatusCodeEnum.Ok, token);
            return Ok(response);
        }
        [HttpPost("loginAdmin")]
        public async Task<IActionResult> AuthenticateAdmin([FromBody] UserForLoginDto user)
        {
            var validate = await _authService.ValidateAdmin(user);
            if (validate is not null)
                return Ok(validate);

            var userModel = await _userManager.Users.Where(u=>u.Email == user.EmailOrPhone).FirstOrDefaultAsync();
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

            var callbackUrl = "1234";

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
