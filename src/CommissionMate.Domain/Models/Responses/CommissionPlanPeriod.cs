using Domain.Extensions;

namespace Domain.Models.Responses
{
    public class CommissionPlanPeriod
    {
        public string Year { get; set; } = string.Empty;

        public Period Period { get; set; }

        public string PeriodOfYear => $"{Year}-{Period.ConvertToString()}";

        public bool IsQtrFinished { get; set; }
    }
}
