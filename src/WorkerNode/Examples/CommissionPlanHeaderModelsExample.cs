using Domain.Models.Responses;

using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Abstractions;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Resolvers;

using Newtonsoft.Json.Serialization;

namespace WorkerNode.Examples
{
    public class CommissionPlanHeaderModelsExample : OpenApiExample<IEnumerable<CommissionPlanHeaderModel>>
    {
        public override IOpenApiExample<IEnumerable<CommissionPlanHeaderModel>> Build(NamingStrategy namingStrategy = null)
        {
            var ex = new CommissionPlanHeaderModel();
            ex.PlanName = "PLANA";
            ex.CreatedOn = DateTime.UtcNow.Date;
            ex.AutomateScriptUri = "https://automated.scripts.some_uri";

            List<CommissionPlanHeaderModel> exList = [ex];

            Examples.Add(OpenApiExampleResolver.Resolve(
                "CommissionPlanHeaderModels",
                exList,
                namingStrategy
            ));

            return this;
        }
    }
}
