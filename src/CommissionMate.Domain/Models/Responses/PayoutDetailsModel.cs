namespace Domain.Models.Responses
{
    public class PayoutDetailsModel
    {
        public decimal PerformanceFrom { get; set; } = 0;

        public decimal PerformanceTo { get; set; } = 0;

        public decimal GeneralPayout { get; set; } = 0;

        public bool IsLinearGeneralPayout = false;

        public decimal ExtraPayout { get; set; } = 0;
    }
}
