using Domain.Extensions;

namespace Domain.Models.Responses
{
    public class PayoutSourceModel
    {
        public PayoutComponentType PayoutComponentType { get; set; }

        public string PayoutComponentTypeDescription() => PayoutComponentType.ToDescription();

        public decimal Split { get; set; }

        public IEnumerable<PayoutDetailsModel> PayoutDetails { get; set; } = [];
    }
}
