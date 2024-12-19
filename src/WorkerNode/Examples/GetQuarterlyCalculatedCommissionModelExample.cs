using Domain.Models;
using Domain.Models.Requests;

using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Abstractions;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Resolvers;

using Newtonsoft.Json.Serialization;

namespace WorkerNode.Examples
{
    public class GetQuarterlyCalculatedCommissionModelExample : OpenApiExample<GetQuarterlyCalculatedCommissionModel>
    {
        public override IOpenApiExample<GetQuarterlyCalculatedCommissionModel> Build(NamingStrategy namingStrategy = null)
        {
            var ex = new GetQuarterlyCalculatedCommissionModel();
            ex.QuarterPeriod = QuarterPeriod.Q1;
            ex.GrossProfit = 80000;
            ex.QuarterlyComponentQuota = 100000;
            ex.PayoutComponentType = PayoutComponentType.Software;

            Examples.Add(OpenApiExampleResolver.Resolve(
                "GetQuarterlyCalculatedCommissionModel",
                ex,
                namingStrategy
            ));

            return this;
        }
    }
}
