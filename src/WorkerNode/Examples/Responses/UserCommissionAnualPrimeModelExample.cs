using Domain.Models.Responses;

using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Abstractions;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Resolvers;

using Newtonsoft.Json.Serialization;

namespace WorkerNode.Examples.Responses
{
    public class UserCommissionAnualPrimeModelExample 
        : OpenApiExample<UserCommissionAnualPrimeModel>
    {
        public override IOpenApiExample<UserCommissionAnualPrimeModel> Build(NamingStrategy namingStrategy = null)
        {
            var ex = new UserCommissionAnualPrimeModel();
            ex.AnnualPrime = 1000;
            ex.Currency = "£";

            Examples.Add(OpenApiExampleResolver.Resolve(
                "UserCommissionAnualPrimeModelExample",
                ex,
                namingStrategy
            ));

            return this;
        }
    }
}
