using System.ComponentModel;

namespace Domain.Models
{
    public enum Metric
    {
        [Description("GP")]
        GP,

        [Description("Delivered Revenue")]
        DeliveredRevenue,
    }
}

