namespace Domain.Models.Responses
{
    public class CommissionPlanDetailsByPeriodModel
    {
        public CommissionPlanPeriodModel? Period { get; set; }

        public IEnumerable<CommissionPlanDetailsModel>? Details { get; set; }
    }
}
