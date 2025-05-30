
using MailKit.Net.Smtp;
using MailKit.Security;
using InstagramProjectBack.Models;
using Microsoft.Extensions.Options;
using MimeKit;

namespace InstagramProjectBack.Services
{
    public class EmailService
    {
        private readonly EmailSettings _emailSettings;
        public EmailService(IOptions<EmailSettings> settings)
        {
            _emailSettings = settings.Value;
        }
        public async Task SendEmail(string to,string token)
        {
            string verificationLink = $"http://localhost:5150/api/Auth/verify?token={token}";
            var message = new MimeMessage();
            string from = _emailSettings.Username;
            string fromName = from.Split("@")[0];
            string toName = to.Split("@")[0];
            message.From.Add(new MailboxAddress(fromName, from));
            message.To.Add(new MailboxAddress(toName, to));
            message.Subject = "Verify Email";
            message.Body = new TextPart("plain")
            {
                Text = $"please click the link to verify your account: {verificationLink} "
            };
            using var client = new SmtpClient();
            await client.ConnectAsync(_emailSettings.Host, _emailSettings.Port, SecureSocketOptions.StartTls);
            await client.AuthenticateAsync(_emailSettings.Username, _emailSettings.Password);
            await client.SendAsync(message);
            await client.DisconnectAsync(true);
        }
    }
}