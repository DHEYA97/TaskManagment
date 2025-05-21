using Hangfire;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.Extensions.Logging;
using TaskManagment.Core.Abstractions.Const;
using TaskManagment.Core.Helpers;
using TaskManagment.Core.Service;

namespace TaskManagment.ServiceAndFactory.Service
{
    public class EmailSenderService(
        ILogger<EmailSenderService> logger,
        IEmailSender emailSender,
        IHttpContextAccessor httpContextAccessor
    ) : IEmailSenderService
    {
        private readonly ILogger<EmailSenderService> _logger = logger;
        private readonly IEmailSender _emailSender = emailSender;
        private readonly IHttpContextAccessor _httpContextAccessor = httpContextAccessor;

        //private Dictionary<string, string> BuildRegistrationTemplateModel(BaseTemplateViewModel model, string qrCodePath)
        //{
        //    var origin = _httpContextAccessor.HttpContext?.Request.Headers.Origin.ToString();

        //    return new Dictionary<string, string>
        //    {
        //        {"{{name}}", model.Name},
        //        {"{{qrCodeImg}}", $"{origin}/images/{qrCodePath}"},
        //        {"{{emailHeaderImg}}", $"{origin}/images/{ImgName.EmailHeader}"},
        //        {"{{emailFooterImg}}", $"{origin}/images/{ImgName.EmailFooter}"}
        //    };
        //}

        //private Task<string?> ScheduleOrSendEmailAsync(string email, string title, string templateName, Dictionary<string, string> model, DateTime? scheduleDate = null)
        //{
        //    var emailBody = EmailBodyBuilder.GenerateEmailBody(templateName, model);

        //    if (scheduleDate.HasValue)
        //    {
        //        _logger.LogInformation("Scheduled Email To: {email} at {date}", email, scheduleDate);
        //        var jobId = BackgroundJob.Schedule(() => _emailSender.SendEmailAsync(email, title, emailBody), scheduleDate.Value);
        //        return Task.FromResult<string?>(jobId);
        //    }
        //    else
        //    {
        //        _logger.LogInformation("Send Email To: {email}", email);
        //        var jobId = BackgroundJob.Enqueue(() => _emailSender.SendEmailAsync(email, title, emailBody));
        //        return Task.FromResult<string?>(jobId);
        //    }
        //}

        //public async Task<string?> SendRegistrationEmailInBackgroundAsync(BaseTemplateViewModel newRegister, string qrCodePath, DateTime? sendDate = null)
        //{
        //    try
        //    {
        //        var model = BuildRegistrationTemplateModel(newRegister, qrCodePath);
        //        return await ScheduleOrSendEmailAsync(
        //            newRegister.Email,
        //            sendDate.HasValue ? "TaskManagment: Reminder" : "TaskManagment: Event Reminder",
        //            "RegistrationEmail",
        //            model,
        //            sendDate
        //        );
        //    }
        //    catch (Exception ex)
        //    {
        //        _logger.LogError(ex, "فشل إرسال البريد الإلكتروني إلى {Email}", newRegister.Email);
        //        return null;
        //    }
        //}
    
    
    }
}
