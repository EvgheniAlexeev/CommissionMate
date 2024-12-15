using System.ComponentModel;

namespace Domain.Models.Responses
{
    public enum Metric
    {
        [Description("GP")]
        GP,

        [Description("Delivered Revenue")]
        DeliveredRevenue,
    }
}
