using Domain.Models;
using Domain.Models.Requests;

using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Abstractions;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Resolvers;

using Newtonsoft.Json.Serialization;

namespace WorkerNode.Examples
{
    public class GetPlanDetailsModelExample : OpenApiExample<GetPlanDetailsModel>
    {
        public override IOpenApiExample<GetPlanDetailsModel> Build(NamingStrategy namingStrategy = null)
        {
            Examples.Add(OpenApiExampleResolver.Resolve("Sample",
                 new GetPlanDetailsModel()
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
