using Domain;
using Domain.Models;

using Microsoft.Extensions.Configuration;
using Microsoft.Identity.Abstractions;

using Smtp.Infrastructure;
using Smtp.Infrastructure.Services;

namespace WorkerNode.Services
{
    public interface IConnectionErrorNotificationService
    {
        Task SendConnectionErrorNotification(ConnectionErrorNotification message);
    }

    public class ConnectionErrorNotificationService(
            IConfiguration configuration,
            IEmailBodyBuilder emailBodyBuilder,
            IEmailSenderService emailSender,
            IDownstreamApi api,
            IEmailMetaBuilder emailMetaBuilder) : IConnectionErrorNotificationService
    {
        private static readonly string PathToTemplate = "Resources/EmailTemplates/ConnectionErrorNotificationTemplate.cshtml";
        private readonly IEmailMetaBuilder _emailMetaBuilder = emailMetaBuilder;
        private readonly IEmailBodyBuilder _emailBodyBuilder = emailBodyBuilder;
        private readonly IEmailSenderService _emailSender = emailSender;
        private readonly IDownstreamApi _laEmailSender = api;
        private readonly string _env = configuration.GetSection(AppConstants.Environment).Value!;

        public async Task SendConnectionErrorNotification(ConnectionErrorNotification message)
        {
            _emailBodyBuilder.PathToTemplate = PathToTemplate;
            var emailBody = await _emailBodyBuilder.BuildEmailBody(message);

            var subject = string.Format(_emailMetaBuilder.SubjectTemplate!, _env);

#if DEBUG
            var msg = _emailMetaBuilder.GetMessage(subject,
                emailBody!,
                isBodyHtml: true);
            _emailSender.SendEmail(msg);
#endif      
#if !DEBUG
            var msg = _emailMetaBuilder.GetEmailMessageDetails(subject, emailBody!);
            await _laEmailSender.PostForAppAsync(AppConstants.LaSendEmailNotification, msg);
#endif
        }
    }
}
