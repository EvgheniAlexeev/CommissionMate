namespace Domain.Models.Requests
{
    public class GetPlanHeaderModel
    {
        public string Name { get; set; } = string.Empty;

        public DateTime CreatedAt { get; set; } = DateTime.MinValue;

        public string FullName => $"{Name}_{CreatedAt.Year.ToString()}";
    }
}
