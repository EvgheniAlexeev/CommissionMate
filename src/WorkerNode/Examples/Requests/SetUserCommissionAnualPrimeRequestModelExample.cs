using Domain.Models.Requests;

using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Abstractions;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Resolvers;

using Newtonsoft.Json.Serialization;

namespace WorkerNode.Examples.Requests
{
    public class SetUserCommissionAnualPrimeRequestModelExample 
        : OpenApiExample<SetUserCommissionAnualPrimeRequestModel>
    {
        public override IOpenApiExample<SetUserCommissionAnualPrimeRequestModel> Build(NamingStrategy? namingStrategy = null)
        {
            var ex = new SetUserCommissionAnualPrimeRequestModel();
            ex.AnnualPrime = 1000;
            ex.Currency = "£";

            Examples.Add(OpenApiExampleResolver.Resolve(
                "SetUserCommissionAnualPrimeModel",
                ex,
                namingStrategy
            ));

            return this;
        }
    }
}
