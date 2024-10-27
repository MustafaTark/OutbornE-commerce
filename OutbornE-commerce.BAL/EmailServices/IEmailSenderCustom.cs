
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OutbornE_commerce.BAL.EmailServices
{
    public interface IEmailSenderCustom 
        //: Microsoft.AspNetCore.Identity.UI.Services.IEmailSender
    {
        Task SendEmailContactUsAsync(string email, string subject, string htmlMessage);
        Task SendEmailToListAsync(List<string> emails, string subject, string htmlMessage);
    }
}
