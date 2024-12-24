namespace Domain.Models.Responses
{
    public class AssignedCommissionPlansModel : CommissionPlanHeaderModel
    {
        public DateTime AssignedDate { get; set; } = DateTime.UtcNow;

        public string AssignedBy { get; set; } = string.Empty;
    }
}
