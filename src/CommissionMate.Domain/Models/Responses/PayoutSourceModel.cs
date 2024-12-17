using Domain.Extensions;

namespace Domain.Models.Responses
{
    public class PayoutSourceModel
    {
        public PayoutSourceType PayoutSourceType { get; set; }

        public string PayoutSourceTypeDescription() => PayoutSourceType.ToDescription();

        public decimal Split { get; set; }

        public IEnumerable<PayoutDetailsModel>? PayoutDetails { get; set; }
    }
}
