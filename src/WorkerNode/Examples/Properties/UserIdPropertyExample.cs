using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Abstractions;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Resolvers;

using Newtonsoft.Json.Serialization;

namespace WorkerNode.Examples.Properties
{
    public class UserIdPropertyExample : OpenApiExample<string>
    {
        public override IOpenApiExample<string> Build(NamingStrategy namingStrategy = null)
        {
            var ex = "firstname.lastname@insight.com";
            Examples.Add(OpenApiExampleResolver.Resolve(
                "UserIdExample",
                ex,
                namingStrategy
            ));

            return this;
        }
    }
}
