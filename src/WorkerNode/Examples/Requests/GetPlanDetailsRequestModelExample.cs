using Domain.Models;
using Domain.Models.Requests;

using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Abstractions;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Resolvers;

using Newtonsoft.Json.Serialization;

namespace WorkerNode.Examples.Requests
{
    public class GetPlanDetailsRequestModelExample 
        : OpenApiExample<GetPlanDetailsRequestModel>
    {
        public override IOpenApiExample<GetPlanDetailsRequestModel> Build(NamingStrategy? namingStrategy = null)
        {
            Examples.Add(OpenApiExampleResolver.Resolve("Sample",
                 new GetPlanDetailsRequestModel()
                 {
                     FullPlanName = $"PLANA_{DateTime.UtcNow.Year}",
                     PlanPeriod = QuarterPeriod.Q1,
                 },
                namingStrategy
            ));

            return this;
        }
    }
}
