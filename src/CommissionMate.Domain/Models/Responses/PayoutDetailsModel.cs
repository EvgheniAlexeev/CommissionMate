namespace Domain.Models.Responses
{
    public class PayoutDetailsModel
    {
        public decimal From { get; set; } = 0;

        public decimal To { get; set; } = 0;

        public decimal GeneralPayout { get; set; } = 0;

        public decimal ExtraPayout { get; set; } = 0;
    }
}
