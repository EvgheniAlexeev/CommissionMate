using Domain.Extensions;
using Domain.Models.Responses;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Domain.Models.Requests
{
    public class AddCommissionPlanDetailsRequestModel
    {
        [JsonPropertyName("componentType")]
        public ComponentType ComponentType { get; set; }

        [JsonPropertyName("metric")]
        public Metric Metric { get; set; }

        [JsonPropertyName("estimatedPayOut")]
        public decimal EstimatedPayOut { get; set; }

        [JsonPropertyName("actualPayOut")]
        public decimal ActualPayOut { get; set; }

        [JsonPropertyName("totalForQtr")]
        public decimal TotalForQtr { get; set; } = 0;

        [JsonPropertyName("quota")]
        public decimal Quota { get; set; } = 0;

        [JsonPropertyName("performancePercent")]
        public decimal PerformancePercent { get; set; } = 0;

        [JsonPropertyName("payOutPercent")]
        public decimal PayOutPercent { get; set; } = 0;

        [JsonPropertyName("reviewedBy")]
        public string ReviewedBy { get; set; } = string.Empty;

        [JsonPropertyName("approvedBy")]
        public string ApprovedBy { get; set; } = string.Empty;
    }
}
