using Domain.Models.Requests;

using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Abstractions;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Resolvers;

using Newtonsoft.Json.Serialization;

namespace WorkerNode.Examples.Requests
{
    public class AssignCommissionPlanHeaderRequestModelExample 
        : OpenApiExample<AssignCommissionPlanHeaderRequestModel>
    {
        public override IOpenApiExample<AssignCommissionPlanHeaderRequestModel> Build(NamingStrategy? namingStrategy = null)
        {
            var ex = new AssignCommissionPlanHeaderRequestModel();
            ex.FullPlanName = "PLANA_2025";
            ex.AssignedDate = DateTime.UtcNow.Date;

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
