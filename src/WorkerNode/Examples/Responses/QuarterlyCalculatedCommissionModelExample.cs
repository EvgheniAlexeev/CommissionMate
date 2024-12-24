using Domain.Extensions;
using Domain.Models;
using Domain.Models.Responses;

using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Abstractions;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Resolvers;

using Newtonsoft.Json.Serialization;

namespace WorkerNode.Examples.Responses
{
    public class QuarterlyCalculatedCommissionModelExample 
        : OpenApiExample<QuarterlyCalculatedCommissionModel>
    {
        public override IOpenApiExample<QuarterlyCalculatedCommissionModel> Build(NamingStrategy namingStrategy = null)
        {
            QuarterlyCalculatedCommissionModel ex = new()
            {
                EstimatedPayout = 6400,
                PerformancePercent = 80,
                QuarterPeriod = QuarterPeriod.Q1.ToDescription(),
                PayoutComponentType = PayoutComponentType.Software.ToDescription(),
                ExtraPayoutPercent = 3,
                GeneralPayoutPercent = 25,
                PerformanceFrom = 75,
                PerformanceTo = 99,
            };

            Examples.Add(OpenApiExampleResolver.Resolve(
                "QuarterlyCalculatedCommissionModelExample",
                ex,
                namingStrategy
            ));

            return this;
        }
    }
}
