using Domain.Extensions;

namespace Domain.Models.Responses
{
    public class PayoutSourceSplitModel
    {
        public PayoutSourceType PayoutSourceType { get; set; }

        public string PayoutSourceTypeDescription => PayoutSourceType.ToDescription();

        public decimal Split { get; set; }
    }
}
