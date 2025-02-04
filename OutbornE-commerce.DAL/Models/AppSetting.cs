using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OutbornE_commerce.DAL.Models
{
    public class AppSetting : BaseEntity
    {
        public string? PhonrNumber { get; set; }
        public string? Email { get; set; }
        public string? Address { get; set; }
        public string? FacebookUrl { get; set; }
        public string? InstaPayUsername { get; set; }
    }
}
