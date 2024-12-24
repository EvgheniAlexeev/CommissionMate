using System.Text.Json.Serialization;

namespace Domain.Models.Requests
{
    public class PayoutDetailsRequestModel
    {
        [JsonPropertyName("performanceFrom")]
        public decimal PerformanceFrom { get; set; } = 0;

        [JsonPropertyName("performanceTo")]
        public decimal PerformanceTo { get; set; } = 0;

        [JsonPropertyName("generalPayout")]
        public decimal GeneralPayout { get; set; } = 0;

        [JsonPropertyName("isLinearGeneralPayout")]
        public bool IsLinearGeneralPayout = false;

        [JsonPropertyName("extraPayout")]
        public decimal ExtraPayout { get; set; } = 0;
    }
}
