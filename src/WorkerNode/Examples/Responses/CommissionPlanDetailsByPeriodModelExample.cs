using Domain.Models;
using Domain.Models.Responses;

using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Abstractions;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Resolvers;

using Newtonsoft.Json.Serialization;

namespace WorkerNode.Examples.Responses
{
    public class CommissionPlanDetailsByPeriodModelExample 
        : OpenApiExample<CommissionPlanDetailsByPeriodModel>
    {
        public override IOpenApiExample<CommissionPlanDetailsByPeriodModel> Build(NamingStrategy namingStrategy = null)
        {
            var ex = new CommissionPlanDetailsByPeriodModel();
            ex.Period = new()
            {
                IsQtrFinished = true,
                Period = QuarterPeriod.Q1,
                Year = DateTime.UtcNow.Year.ToString(),
            };
            ex.Details = [new () {
                ActualPayOut = 10000,
                EstimatedPayOut = 15000,
                PayOutPercent = 110,
                PerformancePercent = 90,
                ApprovedBy = "J Smith",
                ReviewedBy = "S Connor",
                ComponentType = ComponentType.Software,
                Quota = 100000,
                TotalForQtr = 160000,
                Metric = Metric.GP
            }];

            Examples.Add(OpenApiExampleResolver.Resolve(
                "CommissionPlanDetailsByPeriodModelExample",
                ex,
                namingStrategy
            ));

            return this;
        }
    }
}
