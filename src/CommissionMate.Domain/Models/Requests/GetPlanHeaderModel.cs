namespace Domain.Models.Requests
{
    public class GetPlanHeaderModel
    {
        public string PlanName { get; set; } = string.Empty;

        public DateTime CreatedOn { get; set; } = DateTime.MinValue;

        public string FullName() { return $"{PlanName}_{CreatedOn.Year}"; }
    }
}
