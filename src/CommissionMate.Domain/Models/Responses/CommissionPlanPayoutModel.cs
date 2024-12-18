using Domain.Extensions;

namespace Domain.Models.Responses
{
    public class CommissionPlanPayoutModel
    {
        public PayoutPeriodType PayoutPeriodType { get; set; }

        public string PayoutTableTypeDescription() => PayoutPeriodType.ToDescription();

        public IEnumerable<PayoutSourceModel> PayoutSources { get; set;} = [];
    }
}
