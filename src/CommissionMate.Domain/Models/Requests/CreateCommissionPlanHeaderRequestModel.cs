namespace Domain.Models.Requests
{
    public class CreateCommissionPlanHeaderModelDto : CreateCommissionPlanHeaderRequestModel
    {
        public CreateCommissionPlanHeaderModelDto(string createdBy, CreateCommissionPlanHeaderRequestModel model)
        {
            PlanName = model.PlanName;
            Year = model.Year;
            CreatedBy = createdBy;
        }

        public DateTime CreatedOn { get; set; } = DateTime.UtcNow.Date;

        public string CreatedBy { get; set; } = string.Empty;
    }

    public class CreateCommissionPlanHeaderRequestModel
    {
        public string PlanName { get; set; } = string.Empty;

        public int Year { get; set; } = DateTime.UtcNow.Year;

        public string AutomateScriptUri { get; set; } = string.Empty;
    }
}
