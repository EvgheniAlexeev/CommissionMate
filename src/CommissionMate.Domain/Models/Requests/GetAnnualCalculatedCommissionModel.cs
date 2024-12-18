namespace Domain.Models.Requests
{
    public class GetAnnualCalculatedCommissionModel
    {
        public decimal AnnualGrossProfit() => AnnualComponentQuarterGPMap.Values.Sum();

        public PayoutComponentType PayoutComponentType { get; set; }

        public decimal AnnualComponentQuota() => AnnualComponentQuarterQuotaMap.Values.Sum();

        public Dictionary<QuarterPeriod, decimal> AnnualComponentQuarterGPMap { get; set; } = [];

        public Dictionary<QuarterPeriod, decimal> AnnualComponentQuarterQuotaMap { get; set; } = [];
    }
}
