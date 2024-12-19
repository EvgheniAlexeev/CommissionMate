using Domain.Models;
using Domain.Models.Requests;

using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Abstractions;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Resolvers;

using Newtonsoft.Json.Serialization;

namespace WorkerNode.Examples
{
    public class GetAnnualCalculatedCommissionModelExample : OpenApiExample<GetAnnualCalculatedCommissionModel>
    {
        public override IOpenApiExample<GetAnnualCalculatedCommissionModel> Build(NamingStrategy namingStrategy = null)
        {
            var ex = new GetAnnualCalculatedCommissionModel
            {
                PayoutComponentType = PayoutComponentType.Software,
                AnnualComponentQuarterGPMap = new Dictionary<QuarterPeriod, decimal>
                {
                    { QuarterPeriod.Q1, 100000 },
                    { QuarterPeriod.Q2, 80000 },
                    { QuarterPeriod.Q3, 160000 },
                    { QuarterPeriod.Q4, 260000 }
                },
                AnnualComponentQuarterQuotaMap = new Dictionary<QuarterPeriod, decimal>
                {
                    { QuarterPeriod.Q1, 125000 },
                    { QuarterPeriod.Q2, 125000 },
                    { QuarterPeriod.Q3, 125000 },
                    { QuarterPeriod.Q4, 125000 }
                }
            };

            Examples.Add(OpenApiExampleResolver.Resolve(
                "CommissionPlanHeaderModelExample",
                ex,
                namingStrategy
            ));

            return this;
        }
    }
}
