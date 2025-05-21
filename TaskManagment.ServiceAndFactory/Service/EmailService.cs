using MailKit.Net.Smtp;
using MailKit.Security;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using MimeKit;
using TaskManagment.Core.Settinges;
using System.Net;

namespace TaskManagment.ServiceAndFactore.Service
{
    public class EmailService(IOptions<MailSetting> mailSettings, ILogger<EmailService> logger) : IEmailSender
    {
        private readonly MailSetting _mailSettings = mailSettings.Value;
        private readonly ILogger<EmailService> _logger = logger;

        public async Task SendEmailAsync(string email, string subject, string htmlMessage)
        {
            try
            {
                var message = new MimeMessage
                {
                    Sender = MailboxAddress.Parse(_mailSettings.Mail),
                    Subject = subject
                };
                message.From.Add(MailboxAddress.Parse(_mailSettings.Mail));
                message.To.Add(MailboxAddress.Parse(email));

                var builder = new BodyBuilder
                {
                    HtmlBody = htmlMessage
                };

                message.Body = builder.ToMessageBody();

                string ipAddresses = "";
                try
                {
                    var addresses = await Dns.GetHostAddressesAsync(_mailSettings.Host);
                    ipAddresses = string.Join(", ", addresses.Select(a => a.ToString()));
                }
                catch (Exception dnsEx)
                {
                    _logger.LogWarning(dnsEx, "فشل في الحصول على عنوان IP للمضيف {Host}", _mailSettings.Host);
                }

                _logger.LogInformation("Connecting to SMTP server. Host: {Host}, Port: {Port}, IP(s): {IPs}",
                    _mailSettings.Host, _mailSettings.Port,
                    string.IsNullOrEmpty(ipAddresses) ? "Unavailable" : ipAddresses);

                using var smtp = new SmtpClient();

                smtp.Connect(_mailSettings.Host, _mailSettings.Port, SecureSocketOptions.SslOnConnect);
                smtp.Authenticate(_mailSettings.Mail, _mailSettings.Password);
                await smtp.SendAsync(message);
                smtp.Disconnect(true);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to send email to {email}", email);

                if (ex.InnerException != null)
                {
                    _logger.LogError("Inner Exception: {Inner}", ex.InnerException.Message);
                }

                throw;
            }
        }

    }

}
