using System.ComponentModel;

namespace Domain.Models.Responses
{
    public enum PayoutSourceType
    {
        Hardware,
        Software,
        [Description("Software & Hardware")]
        SH,
        KPI,
        [Description("Insight Delivered Solutions")]
        IDS,
    }
}
