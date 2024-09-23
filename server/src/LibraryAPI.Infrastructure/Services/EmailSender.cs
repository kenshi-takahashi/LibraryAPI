using Google.Apis.Gmail.v1;
using LibraryAPI.Application.Interfaces;
using LibraryAPI.Application.Common;
using MimeKit;
using Microsoft.Extensions.Options;

namespace LibraryAPI.Application.Services
{
    public class EmailSender : IEmailSender
    {
        private readonly GmailService _gmailService;
        private readonly EmailSettings _emailSettings;

        public EmailSender(GmailService gmailService, IOptions<EmailSettings> emailSettings)
        {
            _gmailService = gmailService;
            _emailSettings = emailSettings.Value;
        }

        public async Task SendEmailAsync(string to, string subject, string body)
        {
            var emailMessage = new MimeMessage();
            emailMessage.From.Add(new MailboxAddress("Library", _emailSettings.FromAddress));
            emailMessage.To.Add(new MailboxAddress("", to));
            emailMessage.Subject = subject;
            emailMessage.Body = new TextPart("plain") { Text = body };

            using (var stream = new MemoryStream())
            {
                emailMessage.WriteTo(stream);
                var result = Convert.ToBase64String(stream.GetBuffer(), 0, (int)stream.Length);
                var message = new Google.Apis.Gmail.v1.Data.Message { Raw = result };
                await _gmailService.Users.Messages.Send(message, "me").ExecuteAsync();
            }
        }
    }
}
