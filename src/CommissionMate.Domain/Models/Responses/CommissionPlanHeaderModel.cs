using Newtonsoft.Json;

namespace Domain.Models.Responses
{
    public class CommissionPlanHeaderModel
    {
        public string PlanName { get; set; } = string.Empty;

        public string FullPlanName => $"{PlanName}_{CreateOnYear}";

        /// <summary>
        /// Type of this property can be modified to int because we need only 'Year' part
        /// </summary>
        public DateTime CreatedOn { get; set; } = DateTime.UtcNow.Date;

        public string CreateOnYear => CreatedOn.Year.ToString();

        [Obsolete("This property may be deprecated in the future.")]
        [JsonProperty("automateScriptUri", Required = Required.AllowNull)]
        /// <summary>
        /// This property can be used to automate the commission plan. But it might be deprecated in the future.
        /// </summary>
        public string AutomateScriptUri {get; set; } = string.Empty;
    }
}
