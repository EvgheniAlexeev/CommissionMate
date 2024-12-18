using DataLayer.Repositories;

using Domain.Models.Requests;
using Domain.Models.Responses;

using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Attributes;
using Microsoft.Extensions.Logging;

using System.Net;

using WorkerNode.Authorization;
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

        [Authorize(UserRoles = [UserRoles.Sales, UserRoles.Admin])]
        [Function(nameof(GetUserAnnualPrime))]
        [OpenApiOperation(operationId: nameof(GetUserAnnualPrime), tags: [CommissionPlansTag])]
        [OpenApiResponseWithBody(
            statusCode: HttpStatusCode.OK,
            contentType: "application/json",
            bodyType: typeof(UserCommissionAnualPrimeModel),
            Description = "OK response with amount of the user's annual prime")]
        public HttpResponseData GetUserAnnualPrime(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get")] HttpRequestData req,
            FunctionContext executionContext)
        {
            var userContext = executionContext.Features.Get<UserContextFeature>()!;
            var response = _repository.GetUserCommissionAnualPrime(userContext.Email);
            return CreateJsonResponse(HttpStatusCode.OK, req, response);
        }


        [Authorize(UserRoles = [UserRoles.Sales, UserRoles.Admin])]
        [Function(nameof(GetCurrentPlan))]
        [OpenApiOperation(operationId: nameof(GetCurrentPlan), tags: [CommissionPlansTag])]
        [OpenApiResponseWithBody(
            statusCode: HttpStatusCode.OK,
            contentType: "application/json",
            bodyType: typeof(CommissionPlanHeaderModel),
            Description = "OK response with user's current commission plan header data")]
        public HttpResponseData GetCurrentPlan(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get")] HttpRequestData req,
            FunctionContext executionContext)
        {
            var userContext = executionContext.Features.Get<UserContextFeature>()!;
            var response = _repository.GetCurrentPlan(userContext.Email);
            return CreateJsonResponse(HttpStatusCode.OK, req, response);
        }

        [Authorize(UserRoles = [UserRoles.Sales, UserRoles.Admin])]
        [Function(nameof(GetUserCommissionPlans))]
        [OpenApiOperation(operationId: nameof(GetUserCommissionPlans), tags: [CommissionPlansTag])]
        [OpenApiResponseWithBody(
            statusCode: HttpStatusCode.OK,
            contentType: "application/json",
            bodyType: typeof(IEnumerable<CommissionPlanHeaderModel>),
            Description = "OK response with user's commission plans")]
        public HttpResponseData GetUserCommissionPlans(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get")] HttpRequestData req,
            FunctionContext executionContext)
        {
            var userContext = executionContext.Features.Get<UserContextFeature>()!;
            var response = _repository.GetUserPlans(userContext.Email);

            return CreateJsonResponse(HttpStatusCode.OK, req, response);
        }

        [Authorize(UserRoles = [UserRoles.Sales, UserRoles.Admin])]
        [Function(nameof(GetConcretePlan))]
        [OpenApiOperation(operationId: nameof(GetConcretePlan), tags: [CommissionPlansTag])]
        [OpenApiRequestBody(contentType: "application/json", bodyType: typeof(GetPlanHeaderModel), Description = "JSON payload with commission plan header information", Required = true)]
        [OpenApiResponseWithBody(
            statusCode: HttpStatusCode.OK,
            contentType: "application/json",
            bodyType: typeof(CommissionPlanHeaderModel),
            Description = "OK response with user's commission plan header data for a concrete year")]
        public HttpResponseData GetConcretePlan(
            [HttpTrigger(AuthorizationLevel.Anonymous, "post")] HttpRequestData req,
            FunctionContext executionContext)
        {
            var request = GetRequestBody<GetPlanHeaderModel>(req);
            var userContext = executionContext.Features.Get<UserContextFeature>()!;
            if (request == null)
            {
                return CreateBadRequestResponse(req);
            }

            var response = _repository.GetConcretePlan(userContext.Email, request);
            return CreateJsonResponse(HttpStatusCode.OK, req, response);
        }

        [Authorize(UserRoles = [UserRoles.Sales, UserRoles.Admin])]
        [Function(nameof(GetPlanDetails))]
        [OpenApiOperation(operationId: nameof(GetPlanDetails), tags: [CommissionPlansTag])]
        [OpenApiRequestBody(contentType: "application/json", bodyType: typeof(GetPlanDetailsModel), Description = "JSON payload with for a concrete commission plan period", Required = true)]
        [OpenApiResponseWithBody(
            statusCode: HttpStatusCode.OK,
            contentType: "application/json",
            bodyType: typeof(CommissionPlanDetailsByPeriodModel),
            Description = "OK response with user's commission plan header data for a concrete year")]
        public HttpResponseData GetPlanDetails(
            [HttpTrigger(AuthorizationLevel.Anonymous, "post")] HttpRequestData req,
            FunctionContext executionContext)
        {
            var request = GetRequestBody<GetPlanDetailsModel>(req);
            if (request == null)
            {
                return CreateBadRequestResponse(req);
            }
            var userContext = executionContext.Features.Get<UserContextFeature>()!;

            var response = _repository.GetPlanDetails(userContext.Email, request);
            return CreateJsonResponse(HttpStatusCode.OK, req, response);
        }

        [Authorize(UserRoles = [UserRoles.Sales, UserRoles.Admin])]
        [Function(nameof(GetPlanPayoutTables))]
        [OpenApiOperation(operationId: nameof(GetPlanPayoutTables), tags: [CommissionPlansTag])]
        [OpenApiRequestBody(contentType: "application/json", bodyType: typeof(GetPlanHeaderModel), Description = "JSON payload with commission plan header information", Required = true)]
        [OpenApiResponseWithBody(
            statusCode: HttpStatusCode.OK,
            contentType: "application/json",
            bodyType: typeof(IEnumerable<CommissionPlanPayoutModel>),
            Description = "OK response with payout plan Pay-out models (Quarterly and Annual)")]
        public HttpResponseData GetPlanPayoutTables(
            [HttpTrigger(AuthorizationLevel.Anonymous, "post")] HttpRequestData req,
            FunctionContext executionContext)
        {
            var userContext = executionContext.Features.Get<UserContextFeature>()!;
            var request = GetRequestBody<GetPlanHeaderModel>(req);
            if (request == null)
            {
                return CreateBadRequestResponse(req);
            }

            var response = _repository.GetCommissionPlanPayoutModel(request.FullName());
            return CreateJsonResponse(HttpStatusCode.OK, req, response);
        }

        [Authorize(UserRoles = [UserRoles.Sales, UserRoles.Admin])]
        [Function(nameof(CalculateCommissionQuarterly))]
        [OpenApiOperation(operationId: nameof(CalculateCommissionQuarterly), tags: [CommissionPlansTag])]
        [OpenApiRequestBody(contentType: "application/json", bodyType: typeof(GetQuarterlyCalculatedCommissionModel), Description = "JSON payload with gross profit and a component quota information", Required = true)]
        [OpenApiResponseWithBody(
            statusCode: HttpStatusCode.OK,
            contentType: "application/json",
            bodyType: typeof(QuarterlyCalculatedCommissionModel),
            Description = "OK response with quarterly calculated commission")]
        public HttpResponseData CalculateCommissionQuarterly(
            [HttpTrigger(AuthorizationLevel.Anonymous, "post")] HttpRequestData req,
            FunctionContext executionContext)
        {
            var userContext = executionContext.Features.Get<UserContextFeature>()!;
            var request = GetRequestBody<GetQuarterlyCalculatedCommissionModel>(req);
            if (request == null)
            {
                return CreateBadRequestResponse(req);
            }

            var response = _commissionProvider.CalculateQuarterCommission(request, userContext.Email);
            return CreateJsonResponse(HttpStatusCode.OK, req, response);
        }

        [Authorize(UserRoles = [UserRoles.Sales, UserRoles.Admin])]
        [Function(nameof(CalculateAnnualCommission))]
        [OpenApiOperation(operationId: nameof(CalculateAnnualCommission), tags: [CommissionPlansTag])]
        [OpenApiRequestBody(contentType: "application/json", bodyType: typeof(GetAnnualCalculatedCommissionModel), Description = "JSON payload with gross profit and annual component quota maps", Required = true)]
        [OpenApiResponseWithBody(
            statusCode: HttpStatusCode.OK,
            contentType: "application/json",
            bodyType: typeof(AnnualCalculatedCommissionModel),
            Description = "OK response with annually calculated commission and estimated pay-out balance")]
        public HttpResponseData CalculateAnnualCommission(
            [HttpTrigger(AuthorizationLevel.Anonymous, "post")] HttpRequestData req,
            FunctionContext executionContext)
        {
            var userContext = executionContext.Features.Get<UserContextFeature>()!;
            var request = GetRequestBody<GetAnnualCalculatedCommissionModel>(req);
            if (request == null)
            {
                return CreateBadRequestResponse(req);
            }

            var response = _commissionProvider.CalculateAnnualCommission(request, userContext.Email);
            return CreateJsonResponse(HttpStatusCode.OK, req, response);
        }
    }
}
