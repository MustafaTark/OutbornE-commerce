using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using OutbornE_commerce.BAL.Dto;
using OutbornE_commerce.DAL.Enums;
using OutbornE_commerce.DAL.Models;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace OutbornE_commerce.BAL.AuthServices
{
	public class AuthService : IAuthService
	{
		private readonly UserManager<User> _userManager;
		private readonly IConfiguration _configuration;
		private User? _user;
		public AuthService(UserManager<User> userManager, IConfiguration configuration)
		{
			_userManager = userManager;
			_configuration = configuration;
		}
		public async Task<AuthResponseModel?> ValidateUser(UserForLoginDto userForAuth)
		{
			 _user = await _userManager.Users.Where(u=>u.PhoneNumber == userForAuth.EmailOrPhone).FirstOrDefaultAsync();
			if (_user == null || ! await _userManager.CheckPasswordAsync(_user, userForAuth.Password!))
			{
                return new AuthResponseModel
                {
                    PhoneNumber = userForAuth.EmailOrPhone,
                    IsError = true,
                    MessageAr = "رقم الهاتف او كلمة السر غير صحيحين",
                    MessageEn = "Phone or Password not correct",
					StatusCode = (int) StatusCodeEnum.BadRequest
                };
            }
			return null;
		}
		public async Task<string> CreateToken()
		{
			var signingCredentials = GetSigningCredentials();
			var claims = await GetClaims();
			var tokenOptions = GenerateTokenOptions(signingCredentials, claims);
			return new JwtSecurityTokenHandler().WriteToken(tokenOptions);
		}
		private SigningCredentials GetSigningCredentials()
		{
			var key = Encoding.UTF8.GetBytes("HQDshfnnystWB3Ff4tKeQx3d0aIR2uoEurrknFhsyjA");
			var secret = new SymmetricSecurityKey(key);
			return new SigningCredentials(secret, SecurityAlgorithms.HmacSha256);
		}
		private async Task<List<Claim>> GetClaims()
		{
			var claims = new List<Claim> {
				new Claim("uid", _user.Id!),
				new Claim("email", _user.PhoneNumber!),
			};
			var roles = await _userManager.GetRolesAsync(_user);
			foreach (var role in roles)
			{
				claims.Add(new Claim(ClaimTypes.Role, role));
			}
			return claims;
		}
		private JwtSecurityToken GenerateTokenOptions(SigningCredentials signingCredentials, List<Claim> claims)
		{
			var jwtSettings = _configuration.GetSection("JWT");
			var tokenOptions = new JwtSecurityToken(
				issuer: jwtSettings.GetSection("validIssuer").Value,
				audience: jwtSettings.GetSection("validAudience").Value,
				claims: claims,
				expires: DateTime.Now.AddMonths(Convert.ToInt32(jwtSettings.GetSection("expires").Value)),
				signingCredentials: signingCredentials
				);
			return tokenOptions;
		}
	}
}
