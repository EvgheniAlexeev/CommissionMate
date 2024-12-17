namespace Domain.Models.Responses
{
    public class CommissionPlanHeaderModel
    {
        public string PlanName { get; set; } = string.Empty;

        public string FullPlanName() => $"{PlanName}_{CreateOnYear}";

        public DateTime CreatedOn { get; set; } = DateTime.MinValue;

        public string CreateOnYear() => CreatedOn.Year.ToString();

        public string AutomateScriptUri {get; set; } = string.Empty;
    }
}
