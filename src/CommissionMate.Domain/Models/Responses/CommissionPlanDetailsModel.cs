using Domain.Extensions;

namespace Domain.Models.Responses
{
    public class CommissionPlanDetailsModel
    {
        public ComponentType ComponentType { get; set; }

        public string ComponentTypeValue => ComponentType.ToDescription();

        public Metric Metric { get; set; }

        public string MetricValue => Metric.ToDescription();

        public decimal EstimatedPayOut { get; set; }

        public decimal ActualPayOut { get; set; }

        public decimal TotalForQtr { get; set; } = 0;

        public decimal Quota { get; set; } = 0;

        public decimal Performance { get; set; } = 0;

        public decimal PayOut { get; set; } = 0;

        public string ReviewedBy { get; set; } = string.Empty;

        public string ApprovedBy { get; set; } = string.Empty;
    }
}
