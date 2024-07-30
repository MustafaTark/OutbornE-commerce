using Microsoft.AspNetCore.Identity.UI.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OutbornE_commerce.BAL.EmailServices
{
    public interface IEmailSenderCustom : IEmailSender
    {
        Task SendEmailContactUsAsync(string email, string subject, string htmlMessage);
        Task SendEmailToListAsync(List<string> emails, string subject, string htmlMessage);
    }
}
