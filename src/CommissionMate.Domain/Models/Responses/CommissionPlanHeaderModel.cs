namespace Domain.Models.Responses
{
    public class CommissionPlanHeaderModel
    {
        public string PlanName { get; set; } = string.Empty;

        public string FullPlanName() => $"{PlanName}_{CreateOnYear}";

        public decimal AnnualPrime { get; set; } = 0;

        public string Currency { get; set; } = string.Empty;

        public DateTime CreatedOn { get; set; } = DateTime.MinValue;

        public string CreateOnYear() => CreatedOn.Year.ToString();

        public string AutomateScriptUri {get; set; } = string.Empty;
    }
}
