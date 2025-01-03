﻿using DataLayer.Repositories;

using Domain.Extensions;
using Domain.Models;
using Domain.Models.Requests;
using Domain.Models.Responses;

using FluentEmail.Core;

using System.Numerics;

namespace WorkerNode.Providers
{
    public interface ICommissionProvider
    {
        Task<QuarterlyCalculatedCommissionModel?> CalculateQuarterCommission(
            GetQuarterlyCalculatedCommissionRequestModel qcCommission,
            string email, string? planName = null);

        Task<AnnualCalculatedCommissionModel> CalculateAnnualCommission(
            GetAnnualCalculatedCommissionRequestModel request,
            string email,
            string? planName = null);
    }

    public class CommissionProvider(IUserRepository userRepository) : ICommissionProvider
    {
        private readonly IUserRepository _userRepository = userRepository;
        private static readonly decimal OneHundred = 100;

        public async Task<QuarterlyCalculatedCommissionModel?> CalculateQuarterCommission(
            GetQuarterlyCalculatedCommissionRequestModel qcCommission,
            string email, string? planName = null)
        {
            var fullPlanName = await GetFullPlanName(email, planName);
            var anualPrime = await _userRepository.GetUserCommissionAnualPrime(email);
            var comPayoutPlan = await _userRepository.GetCommissionPlanPayoutModel(fullPlanName);

            var qComPayoutPlan = comPayoutPlan.Where(cp => cp.PayoutPeriodType == PayoutPeriodType.Quarterly).Single();
            var performance = qcCommission.GrossProfit / qcCommission.QuarterlyComponentQuota * OneHundred;
            var payoutComponent = qComPayoutPlan.PayoutSources
                .Where(ps => ps.PayoutComponentType == qcCommission.PayoutComponentType)
                .Single();

            var qComponentPrime = (anualPrime.AnnualPrime / 4) * payoutComponent.Split / OneHundred;
            var payout = payoutComponent.PayoutDetails
                .Where(pd => pd.PerformanceFrom <= performance && performance <= pd.PerformanceTo)
                .FirstOrDefault();

            decimal payoutPercentage = 0;
            if (payout != null)
            {
                if (payout.IsLinearGeneralPayout)
                {
                    payout.GeneralPayout = performance;
                }

                payoutPercentage = payout.GeneralPayout + (performance - payout.PerformanceFrom) * payout.ExtraPayout;
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

        public async Task<AnnualCalculatedCommissionModel> CalculateAnnualCommission(
            GetAnnualCalculatedCommissionRequestModel acCommission,
            string email,
            string? planName = null)
        {
            var fullPlanName = await GetFullPlanName(email, planName);
            var userAnualPrime = await _userRepository.GetUserCommissionAnualPrime(email);
            var comPayoutPlan = await _userRepository.GetCommissionPlanPayoutModel(fullPlanName);

            var aComPayoutPlan = comPayoutPlan.Where(cp => cp.PayoutPeriodType == PayoutPeriodType.Annual).Single();
            var qComPayoutPlan = comPayoutPlan.Where(cp => cp.PayoutPeriodType == PayoutPeriodType.Quarterly).Single();

            var overralAnnualPerformance = acCommission.AnnualGrossProfit() / acCommission.AnnualComponentQuota() * OneHundred;
            var quarterlyPerformanceMap = acCommission.AnnualComponentQuarterGPMap
                .Select((kvp) =>
                    new { 
                        kvp.Key,
                        Value = kvp.Value / acCommission.AnnualComponentQuarterQuotaMap[kvp.Key] * OneHundred 
                    }).ToDictionary(item => item.Key, item => item.Value);

            var payoutComponentAnnual = aComPayoutPlan.PayoutSources
                .Where(ps => ps.PayoutComponentType == acCommission.PayoutComponentType)
                .Single();

            var aComponentPrime = (userAnualPrime.AnnualPrime) * payoutComponentAnnual.Split / OneHundred;
            var qComponentPrime = (userAnualPrime.AnnualPrime / 4) * payoutComponentAnnual.Split / OneHundred;

            var annualOverralPayout = payoutComponentAnnual.PayoutDetails
                .Where(pd => pd.PerformanceFrom <= overralAnnualPerformance && overralAnnualPerformance <= pd.PerformanceTo)
                .FirstOrDefault();

            var payoutComponentQuarterly = qComPayoutPlan.PayoutSources
                .Where(ps => ps.PayoutComponentType == acCommission.PayoutComponentType)
                .Single();
            var quarterlyPayoutMap = quarterlyPerformanceMap
                .Select((kvp) =>
                    new {
                        kvp.Key,
                        Value = payoutComponentQuarterly.PayoutDetails
                            .Where(pd => pd.PerformanceFrom <= kvp.Value && kvp.Value <= pd.PerformanceTo)
                            .FirstOrDefault()
                    }).ToDictionary(item => item.Key, item => item.Value);

            decimal payoutPercentage = 0;
            if (annualOverralPayout != null)
            {
                if (annualOverralPayout.IsLinearGeneralPayout)
                {
                    annualOverralPayout.GeneralPayout = overralAnnualPerformance;
                }

                payoutPercentage = annualOverralPayout.GeneralPayout + (overralAnnualPerformance - annualOverralPayout.PerformanceFrom) * annualOverralPayout.ExtraPayout;

                var aComponentPrimeVal = 0m;
                Dictionary<string, decimal> quarterlyEstimatedPayout= [];
                Dictionary<string, decimal> quarterlyPayoutPercent = [];

                foreach (var quarterlyPayout in quarterlyPayoutMap)
                {
                    if (quarterlyPayout.Value != null)
                    {
                        if (quarterlyPayout.Value.IsLinearGeneralPayout)
                        {
                            quarterlyPayout.Value.GeneralPayout = quarterlyPerformanceMap[quarterlyPayout.Key];
                        }

                        var qPercent = quarterlyPayout.Value.GeneralPayout + (quarterlyPerformanceMap[quarterlyPayout.Key] - quarterlyPayout.Value.PerformanceFrom) * quarterlyPayout.Value.ExtraPayout;

                        aComponentPrimeVal += qComponentPrime * qPercent / OneHundred;
                        quarterlyEstimatedPayout
                            .Add(quarterlyPayout.Key.ToDescription(), qComponentPrime * qPercent / OneHundred);
                        quarterlyPayoutPercent
                            .Add(quarterlyPayout.Key.ToDescription(), qPercent);
                    }
                }

                return new()
                {
                    EstimatedAnnualPayout = aComponentPrime * overralAnnualPerformance / OneHundred,
                    AnnualPerformancePercent = overralAnnualPerformance,
                    EstimatedPayoutBalance = aComponentPrime * overralAnnualPerformance / OneHundred - aComponentPrimeVal,
                    PayoutComponentType = acCommission.PayoutComponentType.ToDescription(),
                    ExtraPayoutPercent = annualOverralPayout.ExtraPayout,
                    GeneralPayoutPercent = annualOverralPayout.GeneralPayout,
                    PerformanceFrom = annualOverralPayout.PerformanceFrom,
                    PerformanceTo = annualOverralPayout.PerformanceTo,
                    QuarterlyEstimatedPayoutMap = quarterlyEstimatedPayout,
                    QuarterlyPayoutPercentMap = quarterlyPayoutPercent,
                };
            }

            return null;

        }

        private async Task<string> GetFullPlanName(string email, string? planName)
        {
            var fullPlanName = planName ?? (await _userRepository.GetCurrentPlan(email)).FullPlanName;
            if (planName != null)
            {
                fullPlanName = (await _userRepository.GetConcretePlan(fullPlanName)).FullPlanName;

            }
            return fullPlanName;
        }
    }
}
