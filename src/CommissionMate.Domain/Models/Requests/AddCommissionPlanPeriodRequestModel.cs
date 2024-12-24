using Domain.Extensions;

using System.Text.Json.Serialization;

namespace Domain.Models.Requests
{
    public class AddCommissionPlanPeriodRequestModel
    {
        [JsonPropertyName("year")]
        public string Year { get; set; } = string.Empty;

        [JsonPropertyName("period")]
        public QuarterPeriod Period { get; set; }

        public string PeriodOfYear() => $"{Year}-{Period.ToDescription()}";

        [JsonPropertyName("isQtrFinished")]
        public bool IsQtrFinished { get; set; }
    }
}
