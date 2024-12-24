namespace Domain.Models.Requests
{
    public class AssignCommissionPlanHeaderModelDto : AssignCommissionPlanHeaderRequestModel
    {
        public AssignCommissionPlanHeaderModelDto(string assignedBy, string userId, AssignCommissionPlanHeaderRequestModel model)
        {
            FullPlanName = model.FullPlanName;
            AssignedDate = model.AssignedDate;
            AssignedTo = userId;
            AssignedBy = assignedBy;
        }
        public string AssignedTo { get; set; } = string.Empty;

        public DateTime CreatedOn { get; set; } = DateTime.UtcNow.Date;

        public string AssignedBy { get; set; } = string.Empty;
    }

    public class AssignCommissionPlanHeaderRequestModel
    {
        public string FullPlanName { get; set; } = string.Empty;

        public DateTime AssignedDate { get; set; } = DateTime.UtcNow.Date;
    }
}
