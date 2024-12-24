using DataLayer.Repositories;

using Domain.Extensions;
using Domain.Models;
using Domain.Models.Requests;
using Domain.Models.Responses;

using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Attributes;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Extensions;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;

using System.Net;

using WorkerNode.Authorization;
using WorkerNode.Examples.Properties;
using WorkerNode.Examples.Requests;
using WorkerNode.Examples.Responses;
using WorkerNode.Middleware;
using WorkerNode.Providers;
using WorkerNode.Services;

namespace WorkerNode
{
    public class CommissionPlansFunc(
        ILogger<AuthenticateFunc> logger,
        IApiClient apiClient,
        IUserRepository repository,
        ICommissionProvider commissionProvider) : BaseFunc
    {
        public const string CommissionPlansTag = "Azure Function Commissions Calculator";
        private readonly ILogger<AuthenticateFunc> _logger = logger;
        private readonly IApiClient _apiClient = apiClient;
        private readonly IUserRepository _repository = repository;
        private readonly ICommissionProvider _commissionProvider = commissionProvider;

        [Function(nameof(GetStaticData))]
        [OpenApiOperation(operationId: nameof(GetStaticData), tags: [CommissionPlansTag])]
        [OpenApiResponseWithBody(
            statusCode: HttpStatusCode.OK,
            contentType: "application/json",
            bodyType: typeof(StaticData),
            Example = typeof(StaticDataExample),
            Description = "OK response with bunch of available static types datasets ")]
        public async Task<HttpResponseData> GetStaticData(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get")] HttpRequestData req,
            FunctionContext executionContext)
        {
            var response = new StaticData();
            response.QuarterPeriods.AddRange(EnumExtension.EnumToDictionary<QuarterPeriod>());
            response.PayoutPeriodTypes.AddRange(EnumExtension.EnumToDictionary<PayoutPeriodType>());
            response.PayoutComponentTypes.AddRange(EnumExtension.EnumToDictionary<PayoutComponentType>());

            return CreateJsonResponse(HttpStatusCode.OK, req, response);
        }

        [Authorize(UserRoles = [UserRoles.Sales, UserRoles.Admin])]
        [Function(nameof(GetUserAnnualPrime))]
        [OpenApiOperation(operationId: nameof(GetUserAnnualPrime), tags: [CommissionPlansTag])]
        [OpenApiParameter(
            name: "userId",
            In = ParameterLocation.Query,
            Required = false,
            Type = typeof(string),
            Example = typeof(UserIdPropertyExample),
            Description = "The  ID of the user (optional) for whom the annual prime will be get by manager.")]
        [OpenApiResponseWithBody(
            statusCode: HttpStatusCode.OK,
            contentType: "application/json",
            bodyType: typeof(UserCommissionAnualPrimeModel),
            Example = typeof(UserCommissionAnualPrimeModelExample),
            Description = "OK response with amount of the user's annual prime")]
        public async Task<HttpResponseData> GetUserAnnualPrime(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get")] HttpRequestData req,
            string userId,
            FunctionContext executionContext)
        {
            var userContext = executionContext.Features.Get<UserContextFeature>()!;
            if (!string.IsNullOrEmpty(userId))
            {
                if (!userContext.Roles.Contains(UserRoles.Admin))
                {
                    return CreateUnauthorizedResponse(req, $"You are not a manager for {userId}");
                }

                var userAnnualPrime = await _repository.GetUserCommissionAnualPrime(userId);
                return CreateJsonResponse(HttpStatusCode.OK, req, userAnnualPrime);
            }

            var response = await _repository.GetUserCommissionAnualPrime(userContext.Email);
            return CreateJsonResponse(HttpStatusCode.OK, req, response);
        }

        [Authorize(UserRoles = [UserRoles.Sales, UserRoles.Admin])]
        [Function(nameof(GetCurrentUserPlan))]
        [OpenApiOperation(operationId: nameof(GetCurrentUserPlan), tags: [CommissionPlansTag])]
        [OpenApiResponseWithBody(
            statusCode: HttpStatusCode.OK,
            contentType: "application/json",
            bodyType: typeof(CommissionPlanHeaderModel),
            Example = typeof(CommissionPlanHeaderModelExample),
            Description = "OK response with user's current commission plan header data")]
        public async Task<HttpResponseData> GetCurrentUserPlan(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get")] HttpRequestData req,
            FunctionContext executionContext)
        {
            var userContext = executionContext.Features.Get<UserContextFeature>()!;
            var response = await _repository.GetCurrentPlan(userContext.Email);
            return CreateJsonResponse(HttpStatusCode.OK, req, response);
        }

        [Authorize(UserRoles = [UserRoles.Sales, UserRoles.Admin])]
        [Function(nameof(GetUserCommissionPlans))]
        [OpenApiOperation(operationId: nameof(GetUserCommissionPlans), tags: [CommissionPlansTag])]
        [OpenApiResponseWithBody(
            statusCode: HttpStatusCode.OK,
            contentType: "application/json",
            bodyType: typeof(IEnumerable<AssignedCommissionPlansModel>),
            Example = typeof(AssignedCommissionPlansRequestModelExample),
            Description = "OK response with user's commission plans")]
        public async Task<HttpResponseData> GetUserCommissionPlans(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get")] HttpRequestData req,
            FunctionContext executionContext)
        {
            var userContext = executionContext.Features.Get<UserContextFeature>()!;
            var response = await _repository.GetUserPlans(userContext.Email);

            return CreateJsonResponse(HttpStatusCode.OK, req, response);
        }

        [Authorize(UserRoles = [UserRoles.Sales, UserRoles.Admin])]
        [Function(nameof(GetPlanByName))]
        [OpenApiOperation(operationId: nameof(GetPlanByName), tags: [CommissionPlansTag])]
        [OpenApiParameter(name: "fullPlanName",
            In = ParameterLocation.Query,
            Required = true,
            Type = typeof(string),
            Example = typeof(PlanNameExample),
            Description = $"The name of the commission plan."
            )]
        [OpenApiResponseWithBody(
            statusCode: HttpStatusCode.OK,
            contentType: "application/json",
            bodyType: typeof(CommissionPlanHeaderModel),
            Example = typeof(CommissionPlanHeaderModelExample),
            Description = "OK response with user's commission plan header data for a concrete year")]
        public async Task<HttpResponseData> GetPlanByName(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get")] HttpRequestData req,
            string fullPlanName,
            FunctionContext executionContext)
        {
            var response = await _repository.GetConcretePlan(fullPlanName);
            return CreateJsonResponse(HttpStatusCode.OK, req, response);
        }

        [Authorize(UserRoles = [UserRoles.Sales, UserRoles.Admin])]
        [Function(nameof(GetPlanDetails))]
        [OpenApiOperation(operationId: nameof(GetPlanDetails), tags: [CommissionPlansTag])]
        [OpenApiRequestBody(contentType: "application/json", 
            bodyType: typeof(GetPlanDetailsRequestModel),
            Example = typeof(GetPlanDetailsRequestModelExample),
            Description = "JSON payload for a concrete commission plan period", 
            Required = true)]
        [OpenApiResponseWithBody(
            statusCode: HttpStatusCode.OK,
            contentType: "application/json",
            bodyType: typeof(CommissionPlanDetailsByPeriodModel),
            Example = typeof(CommissionPlanDetailsByPeriodModelExample),
            Description = "OK response with user's commission plan header data for a concrete year")]
        public async Task<HttpResponseData> GetPlanDetails(
            [HttpTrigger(AuthorizationLevel.Anonymous, "post")] HttpRequestData req,
            FunctionContext executionContext)
        {
            var request = GetRequestBody<GetPlanDetailsRequestModel>(req);
            if (request == null)
            {
                return CreateBadRequestResponse(req);
            }

            var response = await _repository.GetPlanDetails(request);
            return CreateJsonResponse(HttpStatusCode.OK, req, response);
        }

        [Authorize(UserRoles = [UserRoles.Sales, UserRoles.Admin])]
        [Function(nameof(GetPlanPayoutTables))]
        [OpenApiOperation(operationId: nameof(GetPlanPayoutTables), tags: [CommissionPlansTag])]
        [OpenApiParameter(name: "fullPlanName",
            In = ParameterLocation.Query,
            Required = true,
            Type = typeof(string),
            Example = typeof(PlanNameExample),
            Description = $"The name of the commission plan."
            )]
        [OpenApiResponseWithBody(
            statusCode: HttpStatusCode.OK,
            contentType: "application/json",
            bodyType: typeof(IEnumerable<CommissionPlanPayoutModel>),
            Example = typeof(CommissionPlanPayoutModelExample),
            Description = "OK response with payout plan Pay-out models (Quarterly and Annual)")]
        public async Task<HttpResponseData> GetPlanPayoutTables(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get")] HttpRequestData req,
            string fullPlanName,
            FunctionContext executionContext)
        {
            var userContext = executionContext.Features.Get<UserContextFeature>()!;
            var response = await _repository.GetCommissionPlanPayoutModel(fullPlanName);
            return CreateJsonResponse(HttpStatusCode.OK, req, response);
        }

        [Authorize(UserRoles = [UserRoles.Sales, UserRoles.Admin])]
        [Function(nameof(CalculateCommissionQuarterly))]
        [OpenApiOperation(operationId: nameof(CalculateCommissionQuarterly), tags: [CommissionPlansTag])]
        [OpenApiRequestBody(contentType: "application/json", 
            bodyType: typeof(GetQuarterlyCalculatedCommissionRequestModel), 
            Example = typeof(GetQuarterlyCalculatedCommissionRequestModelExample),
            Description = "JSON payload with gross profit and a component quota information", Required = true)]
        [OpenApiResponseWithBody(
            statusCode: HttpStatusCode.OK,
            contentType: "application/json",
            bodyType: typeof(QuarterlyCalculatedCommissionModel),
            Example = typeof(QuarterlyCalculatedCommissionModelExample),
            Description = "OK response with quarterly calculated commission")]
        public async Task<HttpResponseData> CalculateCommissionQuarterly(
            [HttpTrigger(AuthorizationLevel.Anonymous, "post")] HttpRequestData req,
            FunctionContext executionContext)
        {
            var userContext = executionContext.Features.Get<UserContextFeature>()!;
            var request = GetRequestBody<GetQuarterlyCalculatedCommissionRequestModel>(req);
            if (request == null)
            {
                return CreateBadRequestResponse(req);
            }

            var response = await _commissionProvider.CalculateQuarterCommission(request, userContext.Email);
            return CreateJsonResponse(HttpStatusCode.OK, req, response);
        }

        [Authorize(UserRoles = [UserRoles.Sales, UserRoles.Admin])]
        [Function(nameof(CalculateAnnualCommission))]
        [OpenApiOperation(operationId: nameof(CalculateAnnualCommission), tags: [CommissionPlansTag])]
        [OpenApiRequestBody(contentType: 
            "application/json", 
            bodyType: typeof(GetAnnualCalculatedCommissionRequestModel), 
            Description = "JSON payload with gross profit and annual component quota maps", 
            Example = typeof(GetAnnualCalculatedCommissionRequestModelExample),
            Required = true)]
        [OpenApiResponseWithBody(
            statusCode: HttpStatusCode.OK,
            contentType: "application/json",
            bodyType: typeof(AnnualCalculatedCommissionModel),
            Example = typeof(AnnualCalculatedCommissionModelExample),
            Description = "OK response with annually calculated commission and estimated pay-out balance")]
        public async Task<HttpResponseData> CalculateAnnualCommission(
            [HttpTrigger(AuthorizationLevel.Anonymous, "post")] HttpRequestData req,
            FunctionContext executionContext)
        {
            var userContext = executionContext.Features.Get<UserContextFeature>()!;
            var request = GetRequestBody<GetAnnualCalculatedCommissionRequestModel>(req);
            if (request == null)
            {
                return CreateBadRequestResponse(req);
            }

            var response = await _commissionProvider.CalculateAnnualCommission(request, userContext.Email);
            return CreateJsonResponse(HttpStatusCode.OK, req, response);
        }
    }
}
