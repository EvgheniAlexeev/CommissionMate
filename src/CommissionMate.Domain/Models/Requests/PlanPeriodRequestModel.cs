namespace Domain.Models.Requests
{
    public class PlanPeriodRequestModel
    {
        public string PlanName { get; set; } = string.Empty;
        public AddCommissionPlanPeriodRequestModel? Period { get; set; }
    }
}
