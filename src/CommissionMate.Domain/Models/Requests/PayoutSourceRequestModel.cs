using Domain.Models.Responses;

using System.Text.Json.Serialization;

namespace Domain.Models.Requests
{
    public class PayoutSourceRequestModel
    {
        [JsonPropertyName("payoutComponentType")]
        public PayoutComponentType PayoutComponentType { get; set; }

        [JsonPropertyName("split")]
        public decimal Split { get; set; }

        [JsonPropertyName("payoutDetails")]
        public IEnumerable<PayoutDetailsRequestModel> PayoutDetails { get; set; } = [];
    }
}
