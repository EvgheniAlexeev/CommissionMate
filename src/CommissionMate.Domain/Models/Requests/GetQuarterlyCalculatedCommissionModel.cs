namespace Domain.Models.Requests
{
    public class GetQuarterlyCalculatedCommissionModel
    {
        public decimal GrossProfit { get; set; } = 0;

        public PayoutComponentType PayoutComponentType { get; set; }

        public QuarterPeriod QuarterPeriod { get; set; }

        public decimal QuarterlyComponentQuota { get; set; } = 0;
    }
}
