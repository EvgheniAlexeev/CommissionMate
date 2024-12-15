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

        CommissionPlanDtailsByPeriodModel GetPlanDetails(string email, GetPlanDetailsModel planDetails);
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
                PlanName = "PLANB_2025",
                AnnualPrime = 40000,
                Currency = "£"
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
                    CreatedAt = DateTime.Now,
                    Currency = "£"
                },
                new (){
                PlanName = "PLANA",
                    AnnualPrime = 40000,
                    CreatedAt = DateTime.Now.AddYears(-1),
                    Currency = "£"
                },
            ];
        }

        public CommissionPlanDtailsByPeriodModel GetPlanDetails(string email, GetPlanDetailsModel planDetails)
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
    }
}
