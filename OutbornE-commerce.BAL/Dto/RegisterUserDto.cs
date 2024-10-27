using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OutbornE_commerce.BAL.Dto
{
    public class RegisterUserDto
    {
        public string FullName { get; set; }
        [Required]
        public string Phone { get; set; }

        [Required]
        public string Password { get; set; }
    }
}
