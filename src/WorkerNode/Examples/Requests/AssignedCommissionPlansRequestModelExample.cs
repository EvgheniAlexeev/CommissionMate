using Domain.Models.Responses;

using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Abstractions;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Resolvers;

using Newtonsoft.Json.Serialization;

namespace WorkerNode.Examples.Requests
{
    public class AssignedCommissionPlansRequestModelExample 
        : OpenApiExample<IEnumerable<AssignedCommissionPlansModel>>
    {
        public override IOpenApiExample<IEnumerable<AssignedCommissionPlansModel>> Build(NamingStrategy? namingStrategy = null)
        {
            var ex = new AssignedCommissionPlansModel();
            ex.PlanName = "PLANA";
            ex.CreatedOn = DateTime.UtcNow.Date;
            ex.AssignedDate = DateTime.UtcNow.Date;
            ex.AssignedBy = "S. Connor";
            ex.AutomateScriptUri = "https://automated.scripts.some_uri";

            List<CommissionPlanHeaderModel> exList = [ex];

            Examples.Add(OpenApiExampleResolver.Resolve(
                "AssignedCommissionPlansModel",
                exList,
                namingStrategy
            ));

            return this;
        }
    }
}
