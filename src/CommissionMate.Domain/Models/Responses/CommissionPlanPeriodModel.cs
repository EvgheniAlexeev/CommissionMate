using Domain.Extensions;

namespace Domain.Models.Responses
{
    public class CommissionPlanPeriodModel
    {
        public string Year { get; set; } = string.Empty;

        public QuarterPeriod Period { get; set; }

        public string PeriodOfYear => $"{Year}-{Period.ConvertToString()}";

        public bool IsQtrFinished { get; set; }
    }
}
