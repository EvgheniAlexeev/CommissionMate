using Domain.Models;

namespace Smtp.Infrastructure
{
    public interface IMessageTypes
    {
        EmailMessageDetails Details(string key);
    }

    public class MessageTypes : Dictionary<string, EmailMessageDetails>, IMessageTypes
    {
        public EmailMessageDetails Details(string key)
        {
            if (key == null)
            {
                throw new ArgumentNullException($"Empty keys are not allowed in {nameof(MessageTypes)} configuration section.");
            }

            if (this == null || this.Count == 0)
            {
                throw new ArgumentException($"Obligatory {nameof(MessageTypes)} configuration section is missed.");
            }

            return this[key];
        }
    }
}
