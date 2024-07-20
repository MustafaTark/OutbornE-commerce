using OutbornE_commerce.BAL.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OutbornE_commerce.BAL.AuthServices
{
	public interface IAuthService
	{
		Task<bool> ValidateUser(UserForLoginDto userForAuth);
		Task<string> CreateToken();
	}
}
