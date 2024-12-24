using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Abstractions;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Resolvers;

using Newtonsoft.Json.Serialization;

namespace WorkerNode.Examples.Responses
{
    public class PlanNameExample 
        : OpenApiExample<string>
    {
        public override IOpenApiExample<string> Build(NamingStrategy namingStrategy = null)
        {
            var ex = $"PLANA_{DateTime.UtcNow.Year}";

            Examples.Add(OpenApiExampleResolver.Resolve(
                "PlanNameExample",
                ex,
                namingStrategy
            ));

            return this;
        }
    }
}
