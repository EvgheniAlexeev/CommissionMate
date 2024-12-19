namespace Domain.Models.Responses
{
    public class StaticData
    {
        public Dictionary<int, string> QuarterPeriods { get; set; } = [];

        public Dictionary<int, string> PayoutPeriodTypes { get; set; } = [];

        public Dictionary<int, string> PayoutComponentTypes { get; set; } = [];

        public Dictionary<int, string> Metrics { get; set; } = [];
    }
}
