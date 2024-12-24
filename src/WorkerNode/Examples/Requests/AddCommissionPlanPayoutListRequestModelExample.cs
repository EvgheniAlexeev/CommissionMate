using Domain.Models;
using Domain.Models.Requests;

using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Abstractions;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Resolvers;

using Newtonsoft.Json.Serialization;
    
namespace WorkerNode.Examples.Requests
{
    public class AddCommissionPlanPayoutListRequestModelExample : OpenApiExample<AddCommissionPlanPayoutListRequestModel>
    {
        public override IOpenApiExample<AddCommissionPlanPayoutListRequestModel> Build(NamingStrategy namingStrategy = null)
        {
            IEnumerable<AddCommissionPlanPayoutRequestModel> ex =
            [
                new () {
                    PayoutPeriodType = PayoutPeriodType.Annual,
                    PayoutSources = [new()
                    {
                        PayoutComponentType = PayoutComponentType.Hardware,
                        Split = 45,
                        PayoutDetails = [
                            new() {
                                PerformanceFrom = 0,
                                PerformanceTo = 74,
                                GeneralPayout = 0
                            },
                            new() {
                                PerformanceFrom = 75,
                                PerformanceTo = 99,
                                GeneralPayout = 25,
                                ExtraPayout = 3
                            },
                            new() {
                                PerformanceFrom = 100,
                                PerformanceTo = 124,
                                GeneralPayout = 1,
                                ExtraPayout = 0
                            },
                            new() {
                                PerformanceFrom = 125,
                                PerformanceTo = 149,
                                GeneralPayout = 125,
                                ExtraPayout = 2
                            },
                            new() {
                                PerformanceFrom = 150,
                                PerformanceTo = int.MaxValue,
                                GeneralPayout = 175,
                                ExtraPayout = 0.5m
                            },
                        ]
                    },new()
                    {
                        PayoutComponentType = PayoutComponentType.Software,
                        Split = 40,
                        PayoutDetails = [
                            new() {
                                PerformanceFrom = 0,
                                PerformanceTo = 74,
                                GeneralPayout = 0
                            },
                            new() {
                                PerformanceFrom = 75,
                                PerformanceTo = 99,
                                GeneralPayout = 25,
                                ExtraPayout = 3
                            },
                            new() {
                                PerformanceFrom = 100,
                                PerformanceTo = 124,
                                IsLinearGeneralPayout = true,
                                ExtraPayout = 0
                            },
                            new() {
                                PerformanceFrom = 125,
                                PerformanceTo = 149,
                                GeneralPayout = 125,
                                ExtraPayout = 2
                            },
                            new() {
                                PerformanceFrom = 150,
                                PerformanceTo = int.MaxValue,
                                GeneralPayout = 175,
                                ExtraPayout = 0.5m
                            },
                        ]
                    }
                    ,new()
                    {
                        PayoutComponentType = PayoutComponentType.IDS,
                        Split = 15,
                        PayoutDetails = [
                            new() {
                                PerformanceFrom = 0,
                                PerformanceTo = 49,
                                GeneralPayout = 0
                            },
                            new() {
                                PerformanceFrom = 50,
                                PerformanceTo = 99,
                                GeneralPayout = 25,
                                ExtraPayout = 2
                            },
                            new() {
                                PerformanceFrom = 100,
                                PerformanceTo = 149,
                                IsLinearGeneralPayout = true,
                                ExtraPayout = 0
                            },
                            new() {
                                PerformanceFrom = 149,
                                PerformanceTo = 199,
                                GeneralPayout = 150,
                                ExtraPayout = 3
                            },
                            new() {
                                PerformanceFrom = 200,
                                PerformanceTo = int.MaxValue,
                                GeneralPayout = 300,
                                ExtraPayout = 0.5m
                            },
                        ]
                    },
                    ]
                },
                new () {
                    PayoutPeriodType = PayoutPeriodType.Quarterly,
                    PayoutSources = [new()
                    {
                        PayoutComponentType = PayoutComponentType.Hardware,
                        Split = 45,
                        PayoutDetails = [
                            new() {
                                PerformanceFrom = 0,
                                PerformanceTo = 74,
                                GeneralPayout = 0
                            },
                            new() {
                                PerformanceFrom = 75,
                                PerformanceTo = 99,
                                GeneralPayout = 25,
                                ExtraPayout = 3
                            },
                            new() {
                                PerformanceFrom = 100,
                                PerformanceTo = 149,
                                GeneralPayout = 1,
                                ExtraPayout = 0
                            },
                            new() {
                                PerformanceFrom = 150,
                                PerformanceTo = int.MaxValue,
                                GeneralPayout = 0,
                                ExtraPayout = 0
                            },
                        ]
                    },new()
                    {
                        PayoutComponentType = PayoutComponentType.Software,
                        Split = 40,
                        PayoutDetails = [
                            new() {
                                PerformanceFrom = 0,
                                PerformanceTo = 74,
                                GeneralPayout = 0
                            },
                            new() {
                                PerformanceFrom = 75,
                                PerformanceTo = 99,
                                GeneralPayout = 25,
                                ExtraPayout = 3
                            },
                            new() {
                                PerformanceFrom = 100,
                                PerformanceTo = 149,
                                IsLinearGeneralPayout = true,
                                ExtraPayout = 0
                            },
                            new() {
                                PerformanceFrom = 150,
                                PerformanceTo = int.MaxValue,
                                GeneralPayout = 150,
                                ExtraPayout = 0
                            },
                        ]
                    },
                    new()
                    {
                        PayoutComponentType = PayoutComponentType.IDS,
                        Split = 15,
                        PayoutDetails = [
                            new() {
                                PerformanceFrom = 0,
                                PerformanceTo = 49,
                            },
                            new() {
                                PerformanceFrom = 50,
                                PerformanceTo = 99,
                                GeneralPayout = 0,
                                ExtraPayout = 2
                            },
                            new() {
                                PerformanceFrom = 100,
                                PerformanceTo = 149,
                                IsLinearGeneralPayout = true,
                            },
                            new() {
                                PerformanceFrom = 150,
                                PerformanceTo = int.MaxValue,
                                GeneralPayout = 150,
                            },
                        ]
                    },
                    ]
                },
            ];

            var example = new AddCommissionPlanPayoutListRequestModel()
            {
                AddCommissionPlanPayoutList = ex
            };

            Examples.Add(OpenApiExampleResolver.Resolve(
                "AddCommissionPlanPayoutRequestModelExample",
                example,
                namingStrategy
            ));

            return this;
        }
    }
}
