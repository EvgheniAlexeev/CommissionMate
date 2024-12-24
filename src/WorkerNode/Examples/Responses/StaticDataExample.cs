using Domain.Extensions;
using Domain.Models;
using Domain.Models.Responses;

using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Abstractions;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Extensions;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Resolvers;

using Newtonsoft.Json.Serialization;

namespace WorkerNode.Examples.Responses
{
    public class StaticDataExample : OpenApiExample<StaticData>
    {
        public override IOpenApiExample<StaticData> Build(NamingStrategy namingStrategy = null)
        {
            var ex = new StaticData();
            ex.QuarterPeriods.AddRange(EnumExtension.EnumToDictionary<QuarterPeriod>());
            ex.PayoutPeriodTypes.AddRange(EnumExtension.EnumToDictionary<PayoutPeriodType>());
            ex.PayoutComponentTypes.AddRange(EnumExtension.EnumToDictionary<PayoutComponentType>());
            ex.Metrics.AddRange(EnumExtension.EnumToDictionary<Metric>());

            Examples.Add(OpenApiExampleResolver.Resolve(
                "StaticDataExample",
                ex,
                namingStrategy
            ));

            return this;
        }
    }
}
