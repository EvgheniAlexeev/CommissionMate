using Domain.Extensions;
using Domain.Models.Responses;

namespace Domain.Models.Requests
{
    public class GetPlanDetailsModel
    {
        public string FullPlanName { get; set; } = string.Empty;

        public Period PlanPeriod { get; set; }

        public string PlanPeriodName () => PlanPeriod.ToDescription();
    }
}
