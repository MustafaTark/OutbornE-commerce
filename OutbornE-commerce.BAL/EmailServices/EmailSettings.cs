using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OutbornE_commerce.BAL.EmailServices
{
    public class EmailSettings
    {
        public string AdminEmail { get; set; }
        public string MailServer { get; set; }
        public int MailPort { get; set; }
        public string UserName { get; set; }
        public string SupportEmail { get; set; }
        public string Password { get; set; }
        public bool EnableSsl { get; set; }
        public string SalesEmail { get; set; }
    }
}
