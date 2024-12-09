using Domain;
using Domain.Configurations;

using Microsoft.Extensions.Options;

namespace Smtp.Infrastructure
{
    public interface ISmtpClient
    {
        public System.Net.Mail.SmtpClient Instance { get; }
    }

    public class SmtpClient : ISmtpClient, IDisposable
    {
        private readonly IOptions<SmtpConfiguration> _smtpConfig;

        public System.Net.Mail.SmtpClient Instance
        {
            get
            {
                return new(_smtpConfig.Value!.Server)
                {
                    Port = _smtpConfig.Value!.Port,
                    Credentials = new System.Net.NetworkCredential(_smtpConfig.Value!.AccName!, _smtpConfig.Value!.Password!),
#if DEBUG
                    EnableSsl = false
#endif
                };
            }
        }

        public SmtpClient(IOptions<SmtpConfiguration> smtpConfig)
        {
            AppConstants.ValidateConstant(smtpConfig.Value?.Server, nameof(SmtpConfiguration.Server));
            AppConstants.ValidateConstant(smtpConfig.Value?.AccName, nameof(SmtpConfiguration.AccName));
            AppConstants.ValidateConstant(smtpConfig.Value?.Server, nameof(SmtpConfiguration.Password));

            _smtpConfig = smtpConfig;
        }

        public void Dispose()
        {
            Instance.Dispose();
            GC.SuppressFinalize(this);
        }
    }
}
