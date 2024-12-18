namespace Domain.Models.Responses
{
    public class AnnualCalculatedCommissionModel
    {
        public decimal PerformancePercent { get; set; } = 0;

        public decimal EstimatedAnnualPayout { get; set; } = 0;

        public decimal EstimatedPayoutBalance { get; set; } = 0;

        public string PayoutComponentType { get; set; } = string.Empty;

        public decimal ExtraPayoutPercent { get; set; } = 0;

        public decimal GeneralPayoutPercent { get; set; } = 0;

        public decimal PerformanceFrom { get; set; } = 0;

        public decimal PerformanceTo { get; set; } = 0;

    }
}
