﻿using Domain.Models;
using Domain.Models.Requests;
using Domain.Models.Responses;

namespace DataLayer.Repositories
{
    public interface IUserRepository
    {
        IEnumerable<string> GetUserRoles(string email);

        UserCommissionAnualPrimeModel GetUserCommissionAnualPrime(string email);

        CommissionPlanHeaderModel GetCurrentPlan(string email);

        CommissionPlanHeaderModel GetConcretePlan(string email, string fullPlanName);

        IEnumerable<CommissionPlanHeaderModel> GetUserPlans(string email);

        CommissionPlanDetailsByPeriodModel GetPlanDetails(string email, GetPlanDetailsModel planDetails);

        IEnumerable<CommissionPlanPayoutModel> GetCommissionPlanPayoutModel(string fullPlanName);
    }

    public class UserRepositoryStub : IUserRepository
    {
        public IEnumerable<string> GetUserRoles(string email)
        {
            return ["sales", "admin"];
        }


        public UserCommissionAnualPrimeModel GetUserCommissionAnualPrime(string email)
        {
            return new()
            {
                AnnualPrime = 40000,
                Currency = "£",
            };
        }
        public CommissionPlanHeaderModel GetCurrentPlan(string email)
        {
            return new CommissionPlanHeaderModel
            {
                PlanName = "PLANB",
                CreatedOn = DateTime.Now
            };
        }

        public CommissionPlanHeaderModel GetConcretePlan(string email, string fullPlanName)
        {
            _ = int.TryParse($"{fullPlanName.Split('_')[1]}", out var year);

            return new CommissionPlanHeaderModel
            {
                PlanName = $"{fullPlanName.Split('_')[0]}",
                CreatedOn = DateTime.MinValue.AddYears(year)
            };
        }

        public IEnumerable<CommissionPlanHeaderModel> GetUserPlans(string email)
        {
            return [ new ()
                {
                    PlanName = "PLANB",
                    CreatedOn = DateTime.Now,
                },
                new (){
                PlanName = "PLANA",
                    CreatedOn = DateTime.Now.AddYears(-1),
                },
            ];
        }

        public CommissionPlanDetailsByPeriodModel GetPlanDetails(string email, GetPlanDetailsModel planDetails)
        {
            return new()
            {

                Period = new CommissionPlanPeriodModel
                {
                    Year = "2025",
                    Period = QuarterPeriod.Q1,
                    IsQtrFinished = true
                },
                Details = [new()
                {
                    EstimatedPayOut = 2700,
                    ActualPayOut = 3330,
                    ComponentType = ComponentType.Hardware,
                    Metric = Metric.GP,
                    TotalForQtr = 163274,
                    Quota = 180000,
                    PerformancePercent = 90,
                    PayOutPercent = 111,
                    ReviewedBy = "D Melody",
                    ApprovedBy = "M Dolan"
                },
                new() {
                    EstimatedPayOut = 8280,
                    ActualPayOut = 3330,
                    ComponentType = ComponentType.Software,
                    Metric = Metric.GP,
                    TotalForQtr = 497383,
                    Quota = 180000,
                    PerformancePercent = 276,
                    PayOutPercent = 111,
                    ReviewedBy = "D Melody",
                    ApprovedBy = "M Dolan"
                },
                new() {
                    EstimatedPayOut = 2490,
                    ActualPayOut = 2400,
                    ComponentType = ComponentType.Solutions,
                    Metric = Metric.DeliveredRevenue,
                    TotalForQtr = 50000,
                    Quota = 60000,
                    PerformancePercent = 83,
                    PayOutPercent = 80,
                    ReviewedBy = "D Melody",
                    ApprovedBy = "M Dolan"
                }
            ]
            };
        }

        public IEnumerable<CommissionPlanPayoutModel> GetCommissionPlanPayoutModel(string fullPlanName)
        {
            return [
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
        }
    }
}
