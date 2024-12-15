namespace Domain.Models.Responses
{
    public class CommissionPlanDtailsByPeriod
    {
        public CommissionPlanPeriod? Period { get; set; }

        public IEnumerable<CommissionPlanDetails>? Details { get; set; }
    }
}
