using FluentEmail.Core;

namespace Smtp.Infrastructure
{
    public interface IEmailBodyBuilder
    {
        string PathToTemplate { get; set; }

        Task<string> BuildEmailBody<T>(T model);
    }

    public class EmailBodyBuilder : IEmailBodyBuilder
    {
        private readonly IFluentEmail _fluentEmail;

        public string PathToTemplate { get; set; }

        public EmailBodyBuilder(IFluentEmail fluentEmail)
        {
            _fluentEmail = fluentEmail;
        }

        public Task<string> BuildEmailBody<T>(T model)
        {
            if (string.IsNullOrEmpty(PathToTemplate)) {
                throw new ArgumentException("Path to email template cannot be empty");
            }

            var email = _fluentEmail.UsingTemplateFromFile(
                $"{Directory.GetCurrentDirectory()}/{PathToTemplate!}",
                model);

            return Task.FromResult(email.Data.Body);
        }
    }
}
