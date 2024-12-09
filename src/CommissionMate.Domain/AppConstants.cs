namespace Domain
{
    public static class AppConstants
    {
        public static string PowerAutomateApiGetCommissions => nameof(PowerAutomateApiGetCommissions);
        public static string PowerAutomateApiGetCommissionsSection => "PowerAutomateApi:Endpoints:GetCommissions";

        public static string PowerAutomateApiGetWeatherForecast => nameof(PowerAutomateApiGetWeatherForecast);
        public static string PowerAutomateApiGetWeatherForecastSection => "PowerAutomateApi:Endpoints:GetWeatherForecast";

        public static string MicrosoftGraphSection => "MicrosoftGraph";
        public static string MicrosoftGraphAcquireTokenSection => "MicrosoftGraph:AcquireTokenOptions";
        public static string ApplicationinsightsConnectionStringSection => "Logging:ApplicationInsights:ConnectionString";
        public static string LaSendEmailNotification => nameof(LaSendEmailNotification);
        public static string LaSendEmailNotificationSection => "LogicalApps:SendEmailNotification";

        public static string Environment = nameof(Environment);

        public static string SupplierNetworkApps = nameof(SupplierNetworkApps);

        public static string InviteTo = nameof(InviteTo);

        public static void ValidateConstant(string? constant, string constantName)
        {
            if (string.IsNullOrEmpty(constant))
            {
                throw new ArgumentException($"Constant: {constantName} value is not set.");
            }
        }
    }
}
