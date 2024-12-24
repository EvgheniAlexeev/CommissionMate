using System.Text.Json.Serialization;

namespace Domain.Models.Requests
{
    public class AddPlanDetailsRequestModelDto : AddPlanDetailsRequestModel
    {
        public AddPlanDetailsRequestModelDto(string createdBy, AddPlanDetailsRequestModel model)
        {
            Period = model.Period;
            Details = model.Details;
            CreatedBy = createdBy;
        }

        public DateTime CreatedOn { get; set; } = DateTime.UtcNow;

        public string CreatedBy { get; set; } = string.Empty;

    }

    public class AddPlanDetailsRequestModel
    {
        [JsonPropertyName("period")]
        public AddCommissionPlanPeriodRequestModel? Period { get; set; }

        [JsonPropertyName("details")]
        public IEnumerable<AddCommissionPlanDetailsRequestModel> Details { get; set; }
    }
}
