using Domain.Models;
using Domain.Models.Requests;
using Domain.Models.Responses;

namespace DataLayer.Repositories
{
    public interface IUserRepository
    {
        Task<IEnumerable<string>> GetUserRoles(string email);

        Task<UserCommissionAnualPrimeModel?> GetUserCommissionAnualPrime(string email);

        Task<CommissionPlanHeaderModel> GetCurrentPlan(string email);

        Task<CommissionPlanHeaderModel> GetConcretePlan(string fullPlanName);

        Task<IEnumerable<AssignedCommissionPlansModel>> GetUserPlans(string email);

        Task<CommissionPlanDetailsByPeriodModel> GetPlanDetails(GetPlanDetailsRequestModel planDetails);

        Task<IEnumerable<CommissionPlanPayoutModel>> GetCommissionPlanPayoutModel(string fullPlanName);

        Task SetUserAnnualPrime(string assignedBy, string userID, SetUserCommissionAnualPrimeRequestModel userAnnualPrime);

        Task CreateCommissionPlan(string createdBy, CreateCommissionPlanHeaderRequestModel comPlanHeader);

        Task AssignPlanToUser(string assignedBy, string userId, AssignCommissionPlanHeaderRequestModel request);

        Task AddPlanDetails(string email, string fullPlanName, AddPlanDetailsRequestModel request);

        Task<bool> AreYouManagerFor(string email, string userId);
        Task AddCommissionPlanPayoutToPlan(string addedBy, string fullPlanName, AddCommissionPlanPayoutListRequestModel request);
    }

    public class UserRepositoryStub : IUserRepository
    {
        private static List<SetUserCommissionAnualPrimeModelDto> UserAnualPrimes { get; } = [];

        private static List<CreateCommissionPlanHeaderModelDto> CommissionPlanHeaders { get; } = [];

        private static List<AssignCommissionPlanHeaderModelDto> UserAssignedPlans { get; } = [];

        private static Dictionary<PlanPeriodRequestModel, AddPlanDetailsRequestModelDto> PlanDetails { get; } = [];

        private static Dictionary<string, AddCommissionPlanPayoutListRequestModelDto> CommissionPlanPayouts { get; } = [];

        public async Task<IEnumerable<string>> GetUserRoles(string email)
        {
            return ["sales", "admin"];
        }

        public async Task<UserCommissionAnualPrimeModel?> GetUserCommissionAnualPrime(string email)
        {
            var result = UserAnualPrimes.FirstOrDefault(x => string.Equals(x.UserEmail, email, StringComparison.OrdinalIgnoreCase));
            if (result == null)
            {
                UserAnualPrimes.Add(new(
                    "System",
                    email.ToLower(),
                    new()
                    {
                        AnnualPrime = 40000,
                        Currency = "£",
                    }));

                return UserAnualPrimes.FirstOrDefault(x => string.Equals(x.UserEmail, email, StringComparison.OrdinalIgnoreCase));
            }

            return result;
        }

        public async Task<CommissionPlanHeaderModel> GetCurrentPlan(string email)
        {
            var item = UserAssignedPlans
                .OrderByDescending(x => x.AssignedDate)
                .FirstOrDefault(x => x.AssignedTo == email);

            if (item == null)
            {
                UserAssignedPlans.Add(new AssignCommissionPlanHeaderModelDto(
                    "System",
                    email,
                    new()
                    {
                        AssignedDate = DateTime.UtcNow.Date,
                        FullPlanName = $"PLANB_{DateTime.UtcNow.Year}",
                    }));
            }

            return await GetConcretePlan(item.FullPlanName);
        }

        public async Task<CommissionPlanHeaderModel> GetConcretePlan(string fullPlanName)
        {
            _ = int.TryParse($"{fullPlanName.Split('_')[1]}", out var year);
            var result = CommissionPlanHeaders
                .FirstOrDefault(x => string.Equals($"{x.PlanName}_{year}", fullPlanName));

            if (result == null)
            {
                CommissionPlanHeaders.Add(new(
                    "System",
                    new()
                    {
                        PlanName = fullPlanName.Split('_')[0],
                        Year = year,
                        AutomateScriptUri = "https://automated.scripts.some_uri"
                    }));

                result = CommissionPlanHeaders.FirstOrDefault(x => $"{x.PlanName}_{year}" == fullPlanName);
            }

            return new()
            {
                CreatedOn = DateTime.MinValue.AddYears(result.Year).ToUniversalTime(),
                PlanName = result.PlanName,
                AutomateScriptUri = result.AutomateScriptUri,
            };
        }

        public async Task<IEnumerable<AssignedCommissionPlansModel>> GetUserPlans(string email)
        {
            return [ new ()
                {
                    PlanName = "PLANB",
                    CreatedOn = DateTime.UtcNow,
                    AssignedDate = DateTime.UtcNow,
                    AssignedBy = "S. Connor",
                },
                new (){
                PlanName = "PLANA",
                    CreatedOn = DateTime.UtcNow.AddYears(-1),
                    AssignedDate = DateTime.UtcNow.AddMonths(-3),
                    AssignedBy = "S. Connor",
                },
            ];
        }

        public async Task<CommissionPlanDetailsByPeriodModel> GetPlanDetails(GetPlanDetailsRequestModel request)
        {
            var plan = await GetConcretePlan(request.FullPlanName);
            var key = new PlanPeriodRequestModel
            {
                PlanName = request.FullPlanName,
                Period = new()
                {
                    Year = plan.CreateOnYear,
                    Period = request.PlanPeriod,
                }
            };

            var response = new CommissionPlanDetailsByPeriodModel
            {
                Period = new CommissionPlanPeriodModel
                {
                    Year = key.Period.Year,
                    Period = key.Period.Period,
                },
                Details = []
            };

            if (PlanDetails.TryGetValue(key, out var planDetails))
            {
                response.Period.IsQtrFinished = planDetails.Period!.IsQtrFinished;
                planDetails.Details.ToList().ForEach(x => response.Details.Append(new()
                {
                    ComponentType = x.ComponentType,
                    Metric = x.Metric,
                    EstimatedPayOut = x.EstimatedPayOut,
                    ActualPayOut = x.ActualPayOut,
                    TotalForQtr = x.TotalForQtr,
                    Quota = x.Quota,
                    PerformancePercent = x.PerformancePercent,
                    PayOutPercent = x.PayOutPercent,
                    ReviewedBy = x.ReviewedBy,
                    ApprovedBy = x.ApprovedBy
                }));

                return response;
            }
            else
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
        }

        public async Task<IEnumerable<CommissionPlanPayoutModel>> GetCommissionPlanPayoutModel(string fullPlanName)
        {
            if (!CommissionPlanPayouts.TryGetValue(fullPlanName, out var planPayouts))
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

            List<CommissionPlanPayoutModel> result = [];
            planPayouts.AddCommissionPlanPayoutList.ToList().ForEach(x => result.Add(new()
            {
                PayoutPeriodType = x.PayoutPeriodType,
                PayoutSources = x.PayoutSources.ToList().Select(y => new PayoutSourceModel()
                {
                    PayoutComponentType = y.PayoutComponentType,
                    Split = y.Split,
                    PayoutDetails = y.PayoutDetails.Select(z => new PayoutDetailsModel()
                    {
                        PerformanceFrom = z.PerformanceFrom,
                        PerformanceTo = z.PerformanceTo,
                        GeneralPayout = z.GeneralPayout,
                        ExtraPayout = z.ExtraPayout,
                        IsLinearGeneralPayout = z.IsLinearGeneralPayout
                    })
                })
            }));

            return result;
        }

        public async Task SetUserAnnualPrime(string assignedBy, string userId, SetUserCommissionAnualPrimeRequestModel userAnnualPrime)
        {
            var item = UserAnualPrimes
                .FirstOrDefault(x => string.Equals(x.UserEmail, userId, StringComparison.OrdinalIgnoreCase));
            if (item != null)
            {
                UserAnualPrimes.Remove(item);
            }

            UserAnualPrimes.Add(new SetUserCommissionAnualPrimeModelDto(assignedBy.ToLower(), userId.ToLower(), userAnnualPrime));
        }

        public async Task CreateCommissionPlan(string createdBy, CreateCommissionPlanHeaderRequestModel comPlanHeader)
        {
            var item = CommissionPlanHeaders
                .FirstOrDefault(x => string.Equals(x.PlanName, comPlanHeader.PlanName, StringComparison.Ordinal));
            if (item != null)
            {
                CommissionPlanHeaders.Remove(item);
            }

            CommissionPlanHeaders.Add(new CreateCommissionPlanHeaderModelDto(createdBy, comPlanHeader));
        }

        public async Task AssignPlanToUser(string assignedBy, string userId, AssignCommissionPlanHeaderRequestModel request)
        {
            var item = UserAssignedPlans
                .OrderByDescending(x => x.AssignedDate)
                .FirstOrDefault(x => x.FullPlanName == request.FullPlanName
                    && string.Equals(x.AssignedTo, userId, StringComparison.OrdinalIgnoreCase));

            if (item != null && item.AssignedDate.AddMonths(3) > item.AssignedDate)
            {
                UserAssignedPlans.Remove(item);
            }

            UserAssignedPlans.Add(new AssignCommissionPlanHeaderModelDto(assignedBy.ToLower(), userId.ToLower(), request));
        }

        public async Task AddPlanDetails(string email, string fullPlanName, AddPlanDetailsRequestModel request)
        {
            var key = new PlanPeriodRequestModel
            {
                PlanName = fullPlanName,
                Period = request.Period
            };

            if (PlanDetails.TryGetValue(key, out var planDetails))
            {
                PlanDetails.Remove(key);
            }


            PlanDetails.Add(key, new AddPlanDetailsRequestModelDto(email, request));
        }

        public async Task AddCommissionPlanPayoutToPlan(
            string addedBy, 
            string fullPlanName, 
            AddCommissionPlanPayoutListRequestModel request)
        {
            if (CommissionPlanPayouts.TryGetValue(fullPlanName, out var planPayouts))
            {
                CommissionPlanPayouts.Remove(fullPlanName);
            }

            CommissionPlanPayouts.Add(fullPlanName, new AddCommissionPlanPayoutListRequestModelDto(addedBy, request));
        }

        public async Task<bool> AreYouManagerFor(string email, string userId)
        {
            if (string.Equals(userId, "user.anothermanager@insight.com", StringComparison.OrdinalIgnoreCase))
            {
                return false;
            }

            return true;
        }
    }
}
