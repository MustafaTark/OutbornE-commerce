﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OutbornE_commerce.BAL.Dto
{
	public class UserForLoginDto
	{
		public string EmailOrPhone { get; set; }
		public string Password { get; set; }
	}
}
