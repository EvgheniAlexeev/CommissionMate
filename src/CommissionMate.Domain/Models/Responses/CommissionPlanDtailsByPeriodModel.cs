namespace Domain.Models.Responses
{
    public class CommissionPlanDtailsByPeriodModel
    {
        public CommissionPlanPeriodModel? Period { get; set; }

        public IEnumerable<CommissionPlanDetailsModel>? Details { get; set; }
    }
}
