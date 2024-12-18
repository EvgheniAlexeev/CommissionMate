using DataLayer.Repositories;

using Domain.Extensions;
using Domain.Models;
using Domain.Models.Requests;
using Domain.Models.Responses;

namespace WorkerNode.Providers
{
    public interface ICommissionProvider
    {
        QuarterlyCalculatedCommissionModel? CalculateQuarterCommission(
            GetQuarterlyCalculatedCommissionModel qcCommission,
            string email, GetPlanHeaderModel? plan = null);
    }

    public class CommissionProvider(IUserRepository userRepository) : ICommissionProvider
    {
        private readonly IUserRepository _userRepository = userRepository;
        private static readonly decimal OneHundred = 100;
        
        public QuarterlyCalculatedCommissionModel? CalculateQuarterCommission(
            GetQuarterlyCalculatedCommissionModel qcCommission, 
            string email, GetPlanHeaderModel? plan = null)
        {
            var fullPlanName = plan?.FullName() ?? _userRepository.GetCurrentPlan(email).FullPlanName();
            if (plan != null)
            {
                fullPlanName = _userRepository.GetConcretePlan(email, plan).FullPlanName();
            }
            
            var anualPrime = _userRepository.GetUserCommissionAnualPrime(email);
            var comPayoutPlan  = _userRepository.GetCommissionPlanPayoutModel(fullPlanName);

            var qComPayoutPlan = comPayoutPlan.Where(cp => cp.PayoutPeriodType == PayoutPeriodType.Quarterly).First();
            var performance = qcCommission.GrossProfit / qcCommission.QuarterlyComponentQuota * OneHundred;
            var payoutComponent = qComPayoutPlan.PayoutSources
                .Where(ps => ps.PayoutComponentType == qcCommission.PayoutComponentType)
                .First();

            var qComponentPrime = (anualPrime.AnnualPrime / 4)* payoutComponent.Split/ OneHundred;
            var payout = payoutComponent.PayoutDetails
                .Where(pd => pd.PerformanceFrom <= performance && performance <= pd.PerformanceTo)
                .FirstOrDefault();

            decimal payoutPercentage = 0;
            if (payout != null)
            {
                payoutPercentage = payout.GeneralPayout + (performance - payout.PerformanceFrom)* payout.ExtraPayout;
                return new()
                {
                    EstimatedPayout = qComponentPrime * payoutPercentage / OneHundred,
                    PerformancePercent = performance,
                    QuarterPeriod = qcCommission.QuarterPeriod.ToDescription(),
                    PayoutComponentType = qcCommission.PayoutComponentType.ToDescription(),
                    ExtraPayoutPercent = payout.ExtraPayout,
                    GeneralPayoutPercent = payout.GeneralPayout,
                    PerformanceFrom = payout.PerformanceFrom,
                    PerformanceTo = payout.PerformanceTo,
                };
            }

            return null;
        }
    }
}
