namespace Domain.Models
{
    public class ConnectionErrorNotification(Exception ex, string methodName)
    {
        public Exception Exception { get; } = ex;

        public string SourceAppName => "CommissionMate Worker";

        public string TargetAppMethodName => methodName;
    }
}
