using Domain.Models.Responses;

namespace Domain.Models.Requests
{
    public class SetUserCommissionAnualPrimeRequestModel : UserCommissionAnualPrimeModel
    {
    }

    public class SetUserCommissionAnualPrimeModelDto : SetUserCommissionAnualPrimeRequestModel
    {
        public SetUserCommissionAnualPrimeModelDto(string assignedBy, string email, SetUserCommissionAnualPrimeRequestModel model)
        {
            AssignedBy = assignedBy;
            UserEmail = email;
            AnnualPrime = model.AnnualPrime;
            Currency = model.Currency;
        }

        public string UserEmail { get; set; } = string.Empty;

        public string AssignedBy { get; set; } = string.Empty;

        public DateTime AssignedOn { get; set; } = DateTime.UtcNow.Date;
    }
}
