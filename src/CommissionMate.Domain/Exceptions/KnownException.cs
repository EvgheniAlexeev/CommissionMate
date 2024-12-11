using System.Reflection;

namespace Domain.Exceptions
{
    public abstract class KnownException : Exception
    {
        public new string? Message { get; set; }

        public new  Exception? InnerException { get; set; }

        public new string? StackTrace { get; set; }

        public new string? Source { get; set; }

        public new MethodBase? TargetSite { get; set; }
    }
}
