
namespace Domain.Models.Responses
{
    public class AnnualCalculatedCommissionModel
    {
        public string PayoutComponentType { get; set; } = string.Empty;

        public decimal AnnualPerformancePercent { get; set; } = 0;

        public decimal EstimatedAnnualPayout { get; set; } = 0;

        public decimal EstimatedPayoutBalance { get; set; } = 0;

        public decimal GeneralPayoutPercent { get; set; } = 0;

        public decimal ExtraPayoutPercent { get; set; } = 0;

        public decimal PerformanceFrom { get; set; } = 0;

        public decimal PerformanceTo { get; set; } = 0;

        public Dictionary<string, decimal> QuarterlyEstimatedPayoutMap { get; set; } = [];

        public Dictionary<string, decimal> QuarterlyPayoutPercentMap { get; set; } = [];
    }
}
