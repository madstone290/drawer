using Drawer.Application.Services.Authentication;
using MailKit.Net.Smtp;
using MailKit.Security;
using MimeKit;
using MimeKit.Text;
using NETCore.MailKit.Core;
using NETCore.MailKit.Infrastructure.Internal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Drawer.Infrastructure.Authentication
{
    public class EmailSender : IEmailSender
    {
        private readonly MailKitOptions _mailKitOptions;

        public EmailSender(MailKitOptions mailKitOptions)
        {
            _mailKitOptions = mailKitOptions;
        }

        public async Task SendEmailAsync(string emailTo, string subject, string text, bool isHtml)
        {
            var email = new MimeMessage();
            email.From.Add(MailboxAddress.Parse(_mailKitOptions.SenderEmail));
            email.To.Add(MailboxAddress.Parse(emailTo));
            email.Subject = subject;
            var textFormat = isHtml ? TextFormat.Html : TextFormat.Plain;
            email.Body = new TextPart(textFormat) { Text = text };

            // todo 스팸필터 회피하도록 설정
            using var smtp = new SmtpClient();
            await smtp.ConnectAsync(_mailKitOptions.Server, _mailKitOptions.Port, SecureSocketOptions.StartTls);
            await smtp.AuthenticateAsync(_mailKitOptions.Account, _mailKitOptions.Password); 
            var formText = await smtp.SendAsync(email);
            Console.WriteLine(formText);
            await smtp.DisconnectAsync(true);
        }
    }
}
