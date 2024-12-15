namespace Domain.Models.Requests
{
    public class PlanHeader
    {
        public string PlanName { get; set; } = string.Empty;

        public DateTime CreatedAt { get; set; } = DateTime.MinValue;

        public string CreateOnYear => CreatedAt.Year.ToString();
    }
}
