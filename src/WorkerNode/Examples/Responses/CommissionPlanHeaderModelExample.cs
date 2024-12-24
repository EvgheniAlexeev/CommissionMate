using Domain.Models.Responses;

using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Abstractions;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Resolvers;

using Newtonsoft.Json.Serialization;

namespace WorkerNode.Examples.Responses
{
    public class CommissionPlanHeaderModelExample 
        : OpenApiExample<CommissionPlanHeaderModel>
    {
        public override IOpenApiExample<CommissionPlanHeaderModel> Build(NamingStrategy namingStrategy = null)
        {
            var ex = new CommissionPlanHeaderModel();
            ex.PlanName = "PLANA";
            ex.CreatedOn = DateTime.UtcNow.Date;
            ex.AutomateScriptUri = "https://automated.scripts.some_uri";

            if (!Examples.TryGetValue("CommissionPlanHeaderModelExample", out _))
            {
                Examples.Add(OpenApiExampleResolver.Resolve(
                    "CommissionPlanHeaderModelExample",
                    ex,
                    namingStrategy
                ));
            }

            return this;
        }
    }
}
