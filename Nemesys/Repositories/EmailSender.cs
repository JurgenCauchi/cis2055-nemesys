using Microsoft.Extensions.Options;
using Nemesys.Repositories;
using System.Net.Mail;
using System.Net;
using Microsoft.AspNetCore.Identity.UI.Services;
public class EmailSender : IEmailSender
{
    private readonly AuthMessageSenderOptions _options;
    private readonly ILogger<EmailSender> _logger;

    public EmailSender(IOptions<AuthMessageSenderOptions> options,
                      ILogger<EmailSender> logger)
    {
        _options = options.Value;
        _logger = logger;
    }

    public async Task SendEmailAsync(string email, string subject, string htmlMessage)
    {
        using (var client = new SmtpClient("smtp.gmail.com", 587))
        {
            // Critical Gmail-specific settings
            client.EnableSsl = true;
            client.UseDefaultCredentials = false; // Must be false for Gmail
            client.DeliveryMethod = SmtpDeliveryMethod.Network; // Explicit network delivery
            client.Credentials = new NetworkCredential(
                _options.Username,
                _options.Password
            );

            // Timeout settings
            client.Timeout = 10000; // 10 seconds

            var msg = new MailMessage
            {
                From = new MailAddress(_options.From),
                Subject = subject,
                Body = htmlMessage,
                IsBodyHtml = true,
                // Important headers for Gmail
                Headers =
            {
                { "X-Mailer", "ASP.NET Core" },
                { "X-Priority", "3" }
            }
            };
            msg.To.Add(email);

            try
            {
                await client.SendMailAsync(msg);
            }
            catch (SmtpException ex)
            {
                // Enhanced error logging
                Console.WriteLine($"SMTP Error: {ex.StatusCode}");
                Console.WriteLine($"Response: {ex.Message}");
                if (ex.InnerException != null)
                {
                    Console.WriteLine($"Inner: {ex.InnerException.Message}");
                }
                throw;
            }
        }
    }
}