﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OutbornE_commerce.BAL.Dto
{
    public class UserShowDto
    {
        public string FullName { get; set; }
        public string? ProfilePicture { get; set; }
        public string Email { get; set; }
        public string? PhoneNumber { get; set; }
    }
}
