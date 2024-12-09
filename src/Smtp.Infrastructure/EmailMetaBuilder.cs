using Domain;
using Domain.Models;

using Microsoft.Extensions.Logging;

using System.Net.Mail;

namespace Smtp.Infrastructure
{
    public interface IEmailMetaBuilder
    {
        MailMessage GetMessage(string subject, string body, bool isBodyHtml = true);

        EmailMessageDetails GetEmailMessageDetails(string subject, string body);

        string? SubjectTemplate { get; }
    }

    public class EmailMetaBuilder : IEmailMetaBuilder
    {
        protected char Separator = ';';
        private readonly IMessageTypes _msgTypes;
        private readonly ILogger<EmailMetaBuilder> _logger;
        private string? _from;
        private string? _to;
        private string? _cc;
        private string? _bcc;

        public EmailMetaBuilder(
            ILogger<EmailMetaBuilder> logger, 
            IMessageTypes msgTypes)
        {
            _logger = logger;
            _msgTypes = msgTypes;

            _from = msgTypes.Details(nameof(ConnectionIssue)).From;
            _to = msgTypes.Details(nameof(ConnectionIssue)).To;
            SubjectTemplate = msgTypes.Details(nameof(ConnectionIssue)).Subject;

            AppConstants.ValidateConstant(_from, nameof(ConnectionIssue.From));
            AppConstants.ValidateConstant(_to, nameof(ConnectionIssue.To));
            AppConstants.ValidateConstant(SubjectTemplate, nameof(ConnectionIssue.Subject));

            _cc = msgTypes.Details(nameof(ConnectionIssue)).Cc;
            _bcc = msgTypes.Details(nameof(ConnectionIssue)).Bcc;
            var separator = msgTypes.Details(nameof(ConnectionIssue)).Separator;

            if (!string.IsNullOrEmpty(separator))
            {
                Separator = separator.ToCharArray()[0];
            }
        }

        public string? SubjectTemplate { get; }

        public MailMessage GetMessage(string subject, string body, bool isBodyHtml = true) 
        {
            LogEmailDetails(subject, body);

            var msg = new MailMessage()
            {
                From = new MailAddress(_from!),
                Subject =subject,
                Body = body,
                IsBodyHtml = true
            };

            AddReceiverAddress(msg.To, _to!);
            AddReceiverAddress(msg.CC, _cc);
            AddReceiverAddress(msg.Bcc, _bcc);

            return msg;
        }

        public EmailMessageDetails GetEmailMessageDetails(string subject, string body)
        {
            LogEmailDetails(subject, body);

            var msg = new EmailMessageDetails()
            {
                From = _from!,
                Subject = subject,
                Body = body,
                To = _to!,
                Cc = _cc,
                Bcc = _bcc,
            };

            return msg;
        }

        private void LogEmailDetails(string subject, string body)
        {
            _logger.LogInformation($"Email subject: {subject}");
            _logger.LogInformation($"Email Recipients (To): {_to}");
            _logger.LogInformation($"Email Recipients (CC): {_cc}");
            _logger.LogInformation($"Email Recipients (Bcc): {_bcc}");
            _logger.LogInformation($"Email Body: {body}");
        }

        private void AddReceiverAddress(MailAddressCollection addressCollection, string? addresses)
        {
            if (string.IsNullOrEmpty(addresses))
            {
                return; 
            }

            foreach (var e in addresses!.Split(Separator))
            {
                if (string.IsNullOrEmpty(e))
                    continue;

                addressCollection.Add(e);
            }
        }
    }
}
