using Domain.Extensions;

namespace Domain.Models.Requests
{
    public class GetPlanDetailsRequestModel
    {
        public string FullPlanName { get; set; } = string.Empty;

        public QuarterPeriod PlanPeriod { get; set; }

        public string PlanPeriodName () => PlanPeriod.ToDescription();
    }
}
