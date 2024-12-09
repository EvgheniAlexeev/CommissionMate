using Microsoft.Extensions.Logging;
using System.Net.Mail;

namespace Smtp.Infrastructure.Services
{
    public interface IEmailSenderService
    {
        void SendEmail(MailMessage message);
    }

    public class EmailSenderService : IEmailSenderService
    {
        private readonly ISmtpClient _emailService;
        private readonly ILogger<EmailSenderService> _logger;

        public EmailSenderService(ISmtpClient emailService, ILogger<EmailSenderService> logger)
        {
            _emailService = emailService;
            _logger = logger;
        }

        public void SendEmail(MailMessage message)
        {
            using var client = _emailService.Instance;
            try
            {
                client.Send(message);
            }
            catch (Exception ex)
            {
                _logger.LogError(message: ex.Message);

                throw;
            }
            finally { client.Dispose(); }
        }
    }
}
