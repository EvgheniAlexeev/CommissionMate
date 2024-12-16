using Domain.Extensions;

namespace Domain.Models.Responses
{
    public class CommissionPlanPayoutModel
    {
        public PayoutTableType PayoutTableType { get; set; }

        public string PayoutTableTypeDescription => PayoutTableType.ToDescription();

        public IEnumerable<PayoutSourceModel>? PayoutSources { get; set;} 

        public IEnumerable<PayoutSourceSplitModel>? PayoutSourceSplits { get; set;}

        public bool IsValidPayout() =>
            PayoutSources != null 
            && PayoutSourceSplits != null 
            && PayoutSources.All(ps => PayoutSourceSplits.Select(pss => pss.PayoutSourceType).Contains(ps.PayoutSourceType));
    }
}
