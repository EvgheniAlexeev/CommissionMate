using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Models.Responses
{
    public class QuarterlyCalculatedCommissionModel
    {
        public decimal PerformancePercent { get; set; } = 0;

        public decimal EstimatedPayout { get; set; } = 0;

        public string QuarterPeriod { get; set; } = string.Empty;

        public string PayoutComponentType { get; set; } = string.Empty;

        public decimal ExtraPayoutPercent { get; set; } = 0;

        public decimal GeneralPayoutPercent { get; set; } = 0;

        public decimal PerformanceFrom { get; set; } = 0;

        public decimal PerformanceTo { get; set; } = 0;
    }
}
