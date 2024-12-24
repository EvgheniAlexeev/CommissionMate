using Domain.Models.Requests;

using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Abstractions;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Resolvers;

using Newtonsoft.Json.Serialization;

namespace WorkerNode.Examples.Requests
{
    internal class CreateCommissionPlanHeaderRequestModelExample 
        : OpenApiExample<CreateCommissionPlanHeaderRequestModel>
    {
        public override IOpenApiExample<CreateCommissionPlanHeaderRequestModel> Build(NamingStrategy? namingStrategy = null)
        {
            var ex = new CreateCommissionPlanHeaderRequestModel();
            ex.AutomateScriptUri = "https://automated.scripts.some_uri";
            ex.PlanName = "PLANA";
            ex.Year = DateTime.UtcNow.Year;

            Examples.Add(OpenApiExampleResolver.Resolve(
                "CreateCommissionPlanHeaderModel",
                ex,
                namingStrategy
            ));

            return this;
        }
    }
}
