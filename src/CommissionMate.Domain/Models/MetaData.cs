namespace Domain.Models
{
    public record struct MetaData
    {
        public int TrxNo { get; set; }

        public DateTime DateTime { get; set; }

        public string ScriptLocation { get; set; }

        public DateTime DateTimeUtc { get; set; }
    }
}
