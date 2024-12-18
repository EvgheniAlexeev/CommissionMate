using Domain.Extensions;

namespace Domain.Models.Requests
{
    public class GetPlanDetailsModel
    {
        public string FullPlanName { get; set; } = string.Empty;

        public QuarterPeriod PlanPeriod { get; set; }

        public string PlanPeriodName () => PlanPeriod.ToDescription();
    }
}
