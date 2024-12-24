using Domain.Models.Responses;

using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Abstractions;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Resolvers;

using Newtonsoft.Json.Serialization;

namespace WorkerNode.Examples.Responses
{
    public class AnnualCalculatedCommissionModelExample 
        : OpenApiExample<AnnualCalculatedCommissionModel>
    {
        public override IOpenApiExample<AnnualCalculatedCommissionModel> Build(NamingStrategy namingStrategy = null)
        {
            var ex = new AnnualCalculatedCommissionModel
            {
                PayoutComponentType = "Software",
                AnnualPerformancePercent = 120,
                EstimatedAnnualPayout = 19200,
                EstimatedPayoutBalance = 6480,
                GeneralPayoutPercent = 120,
                ExtraPayoutPercent = 0,
                PerformanceFrom = 100,
                PerformanceTo = 124,
                QuarterlyEstimatedPayoutMap = new Dictionary<string, decimal>
                {
                    { "Q1", 1600 },
                    { "Q2", 0 },
                    { "Q3", 5120 },
                    { "Q4", 6000 }
                },
                QuarterlyPayoutPercentMap = new Dictionary<string, decimal>
                {
                    { "Q1", 40 },
                    { "Q2", 0 },
                    { "Q3", 128 },
                    { "Q4", 150 }
                }
            };

            Examples.Add(OpenApiExampleResolver.Resolve(
                "AnnualCalculatedCommissionModelExample",
                ex,
                namingStrategy
            ));

            return this;
        }
    }
}
