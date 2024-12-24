using System.Text.Json.Serialization;

namespace Domain.Models.Requests
{
    public class AddCommissionPlanPayoutListRequestModelDto : AddCommissionPlanPayoutListRequestModel
    {
        public AddCommissionPlanPayoutListRequestModelDto(
            string addedBy, 
            AddCommissionPlanPayoutListRequestModel model)
        {
            AddedBy = addedBy;
            AddCommissionPlanPayoutList = model.AddCommissionPlanPayoutList;
        }

        public string AddedBy { get; } = string.Empty;

        public DateTime CreatedOn { get; } = DateTime.UtcNow.Date;
    }


    public class AddCommissionPlanPayoutListRequestModel
    {
        [JsonPropertyName("addCommissionPlanPayoutList")]
        public IEnumerable<AddCommissionPlanPayoutRequestModel> AddCommissionPlanPayoutList { get; set; } = [];
    }

    public class AddCommissionPlanPayoutRequestModel
    {
        [JsonPropertyName("payoutPeriodType")]
        public PayoutPeriodType PayoutPeriodType { get; set; }

        [JsonPropertyName("payoutSources")]
        public IEnumerable<PayoutSourceRequestModel> PayoutSources { get; set; } = [];
    }
}
