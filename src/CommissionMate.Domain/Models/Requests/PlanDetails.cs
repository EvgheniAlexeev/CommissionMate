using Domain.Models.Responses;

namespace Domain.Models.Requests
{
    public class PlanDetails
    {
        public string PlanName { get; set; } = string.Empty;

        public Period PlanPeriod { get; set; }
    }
}
