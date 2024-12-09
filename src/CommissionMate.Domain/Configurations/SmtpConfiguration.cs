namespace Domain.Configurations
{
    public class SmtpConfiguration
    {
        public string? DefaultFrom { get; set; }

        public string? Server { get; set; }

        public string? AccName { get; set; }

        public int Port { get; set; }

        public string? Password { get; set; }
    }
}
