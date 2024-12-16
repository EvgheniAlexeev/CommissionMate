using Domain.Models.Requests;
using Domain.Models.Responses;

namespace WorkerNode.Services
{
    public interface IUserRepository
    {
        IEnumerable<string> GetUserRoles(string email);

        CommissionPlanHeaderModel GetCurrentPlan(string email);

        CommissionPlanHeaderModel GetConcretePlan(string email, GetPlanHeaderModel header);

        IEnumerable<CommissionPlanHeaderModel> GetUserPlans(string email);

        CommissionPlanDetailsByPeriodModel GetPlanDetails(string email, GetPlanDetailsModel planDetails);

        IEnumerable<CommissionPlanPayoutModel> GetCommissionPlanPayoutModel(string fullPlanName);
    }

    public class UserRepository : IUserRepository
    {
        public IEnumerable<string> GetUserRoles(string email)
        {
            return ["sales", "admin"];
        }

        public CommissionPlanHeaderModel GetCurrentPlan(string email)
        {
            return new CommissionPlanHeaderModel
            {
                PlanName = "PLANB",
                AnnualPrime = 40000,
                Currency = "£",
                CreatedOn = DateTime.Now
            };
        }

        public CommissionPlanHeaderModel GetConcretePlan(string email, GetPlanHeaderModel header)
        {
            return new CommissionPlanHeaderModel
            {
                PlanName = $"{header.FullName}",
                AnnualPrime = 40000,
                Currency = "£"
            };
        }

        public IEnumerable<CommissionPlanHeaderModel> GetUserPlans(string email)
        {
            return [ new ()
                {
                    PlanName = "PLANB",
                    AnnualPrime = 40000,
                    CreatedOn = DateTime.Now,
                    Currency = "£"
                },
                new (){
                PlanName = "PLANA",
                    AnnualPrime = 40000,
                    CreatedOn = DateTime.Now.AddYears(-1),
                    Currency = "£"
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
                    Period = Period.Q1,
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
                    Performance = 90,
                    PayOut = 111,
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
                    Performance = 276,
                    PayOut = 111,
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
                    Performance = 83,
                    PayOut = 80,
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
                    PayoutTableType = PayoutTableType.Annual,
                    PayoutSourceSplits = [
                        new() {
                            PayoutSourceType = PayoutSourceType.Hardware,
                            Split = 50
                        },
                        new() {
                            PayoutSourceType = PayoutSourceType.Software,
                            Split = 50
                        }
                    ],
                    PayoutSources = [new()
                    {
                        PayoutSourceType = PayoutSourceType.Hardware,
                        PayoutDetails = [
                            new() {
                                From = 0,
                                To = 74,
                                GeneralPayout = 0
                            },
                            new() {
                                From = 75,
                                To = 99,
                                GeneralPayout = 50,
                                ExtraPayout = 2
                            },
                            new() {
                                From = 100,
                                To = 124,
                                GeneralPayout = 1,
                                ExtraPayout = 0
                            },
                            new() {
                                From = 125,
                                To = 149,
                                GeneralPayout = 125,
                                ExtraPayout = 2
                            },
                            new() {
                                From = 150,
                                To = int.MaxValue,
                                GeneralPayout = 175,
                                ExtraPayout = 0.5m
                            },
                        ]
                    },new()
                    {
                        PayoutSourceType = PayoutSourceType.Software,
                        PayoutDetails = [
                            new() {
                                From = 0,
                                To = 74,
                                GeneralPayout = 0
                            },
                            new() {
                                From = 75,
                                To = 99,
                                GeneralPayout = 25,
                                ExtraPayout = 3
                            },
                            new() {
                                From = 100,
                                To = 124,
                                GeneralPayout = 1,
                                ExtraPayout = 0
                            },
                            new() {
                                From = 125,
                                To = 149,
                                GeneralPayout = 125,
                                ExtraPayout = 2
                            },
                            new() {
                                From = 150,
                                To = int.MaxValue,
                                GeneralPayout = 175,
                                ExtraPayout = 0.5m
                            },
                        ]
                    },
                    ]
                },
                new () {
                    PayoutTableType = PayoutTableType.Quarterly,
                    PayoutSourceSplits = [
                        new() {
                            PayoutSourceType = PayoutSourceType.Hardware,
                            Split = 50
                        },
                        new() {
                            PayoutSourceType = PayoutSourceType.Software,
                            Split = 50
                        }
                    ],
                    PayoutSources = [new()
                    {
                        PayoutSourceType = PayoutSourceType.Hardware,
                        PayoutDetails = [
                            new() {
                                From = 0,
                                To = 74,
                                GeneralPayout = 0
                            },
                            new() {
                                From = 75,
                                To = 99,
                                GeneralPayout = 50,
                                ExtraPayout = 2
                            },
                            new() {
                                From = 100,
                                To = 149,
                                GeneralPayout = 1,
                                ExtraPayout = 0
                            },
                            new() {
                                From = 150,
                                To = int.MaxValue,
                                GeneralPayout = 0,
                                ExtraPayout = 0
                            },
                        ]
                    },new()
                    {
                        PayoutSourceType = PayoutSourceType.Software,
                        PayoutDetails = [
                            new() {
                                From = 0,
                                To = 74,
                                GeneralPayout = 0
                            },
                            new() {
                                From = 75,
                                To = 99,
                                GeneralPayout = 25,
                                ExtraPayout = 3
                            },
                            new() {
                                From = 100,
                                To = 149,
                                GeneralPayout = 1,
                                ExtraPayout = 0
                            },
                            new() {
                                From = 150,
                                To = int.MaxValue,
                                GeneralPayout = 0,
                                ExtraPayout = 0
                            },
                        ]
                    },
                    ]
                },
            ];
        }
    }
}
