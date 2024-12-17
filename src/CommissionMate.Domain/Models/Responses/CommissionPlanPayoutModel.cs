using Domain.Extensions;

namespace Domain.Models.Responses
{
    public class CommissionPlanPayoutModel
    {
        public PayoutTableType PayoutTableType { get; set; }

        public string PayoutTableTypeDescription() => PayoutTableType.ToDescription();

        public IEnumerable<PayoutSourceModel>? PayoutSources { get; set;} 
    }
}
