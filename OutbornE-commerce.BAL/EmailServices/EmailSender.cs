using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using OutbornE_commerce.BAL.Repositories.SMTP_Server;

namespace OutbornE_commerce.BAL.EmailServices
{
    public class EmailSender : IEmailSenderCustom
    {
        private readonly EmailSettings _mailSettings;
        private readonly ISMTPRepository _sMTPRepository;

        public EmailSender(
            IOptions<EmailSettings> mailSettings, ISMTPRepository sMTPRepository)
        {
            _mailSettings = mailSettings.Value;
            _sMTPRepository = sMTPRepository;
        }

        public async Task SendEmailAsync(string email, string subject, string htmlMessage)
        {
            var serverData = (await _sMTPRepository.FindAllAsync(null)).FirstOrDefault();
            MailMessage message = new()
            {
                From = new MailAddress(serverData.Email!, serverData.Username),
                Body = $" {htmlMessage}",
                Subject = subject,
                IsBodyHtml = true
            };

            message.To.Add(email);

            SmtpClient smtpClient = new(serverData.Host)
            {
                Port = 587,
                Credentials = new NetworkCredential(serverData.Email, serverData.Password),
                EnableSsl = true,
            };

            await smtpClient.SendMailAsync(message);

            smtpClient.Dispose();
        }
        public async Task SendEmailToListAsync(List<string> emails, string subject, string htmlMessage)
        {
            try
            {
                var serverData = (await _sMTPRepository.FindAllAsync(null)).FirstOrDefault();
                MailMessage message = new()
                {
                    From = new MailAddress(serverData.Email!, serverData.Username),
                    Body = $" {htmlMessage}",
                    Subject = subject,
                    IsBodyHtml = true
                };
                foreach (var email in emails)
                {
                    message.To.Add(email);
                }

                SmtpClient smtpClient = new(serverData.Host)
                {
                    Port = 587,
                    Credentials = new NetworkCredential(serverData.Email, serverData.Password),
                    EnableSsl = true,
                };

                await smtpClient.SendMailAsync(message);

                smtpClient.Dispose();
            }
            catch (Exception ex)
            {

            }

        }

        public async Task SendEmailContactUsAsync(string email, string subject, string htmlMessage)
        {
            var serverData = (await _sMTPRepository.FindAllAsync(null)).FirstOrDefault();
            MailMessage message = new()
            {
                From = new MailAddress(serverData.Email!, serverData.Username),
                Body = $" From : {email}, \n Subject : {subject}, \n  Body : {htmlMessage} ",
                Subject = subject,
                IsBodyHtml = true
            };

            message.To.Add(serverData.Email);

            SmtpClient smtpClient = new(serverData.Host)
            {
                Port = _mailSettings.MailPort,
                Credentials = new NetworkCredential(serverData.Email, serverData.Password),
                EnableSsl = true,
            };

            await smtpClient.SendMailAsync(message);

            smtpClient.Dispose();
        }
    }
}
