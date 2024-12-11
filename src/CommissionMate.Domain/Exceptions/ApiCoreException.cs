namespace Domain.Exceptions
{
    public class ApiCoreException : KnownException
    {
        public ApiCoreException(Exception ex) {
            Message = ex.Message;
            InnerException = ex.InnerException;
            Source = ex.Source;
            StackTrace = ex.StackTrace;
            TargetSite = ex.TargetSite;
        }
    }
}
