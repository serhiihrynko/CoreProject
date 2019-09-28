using MailKit.Net.Smtp;
using MailKit.Security;
using Microsoft.Extensions.Options;
using MimeKit;
using System;
using System.Net;
using System.Threading.Tasks;

namespace API.Infrastructure.Email
{
    public class EmailService : IEmailService
    {
        private readonly EmailConfig _emailConfig;

        public EmailService(IOptions<EmailConfig> emailConfig)
        {
            _emailConfig = emailConfig.Value;
        }

        public async Task SendEmailAsync(String email, String subject, String message)
        {
            var emailMessage = new MimeMessage();

            emailMessage.From.Add(new MailboxAddress(_emailConfig.FromName, _emailConfig.FromAddress));
            emailMessage.To.Add(new MailboxAddress("", email));
            emailMessage.Subject = subject;
            emailMessage.Body = new TextPart(MimeKit.Text.TextFormat.Html) { Text = message };

            using (var client = new SmtpClient())
            {
                await client.ConnectAsync(
                    _emailConfig.MailServerAddress,
                    int.Parse(_emailConfig.MailServerPort),
                    SecureSocketOptions.Auto
                ).ConfigureAwait(false);

                await client.AuthenticateAsync(new NetworkCredential(_emailConfig.UserId, _emailConfig.UserPassword));

                await client.SendAsync(emailMessage)
                    .ConfigureAwait(false);

                await client.DisconnectAsync(true)
                    .ConfigureAwait(false);
            }
        }
    }
}
