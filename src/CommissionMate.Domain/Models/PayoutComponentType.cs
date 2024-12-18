using System.ComponentModel;

namespace Domain.Models
{
    public enum PayoutComponentType
    {
        Hardware = 1,
        Software = 2,
        [Description("Software & Hardware")]
        SH = 3,
        KPI = 4,
        [Description("Insight Delivered Solutions")]
        IDS = 5,
    }
}
