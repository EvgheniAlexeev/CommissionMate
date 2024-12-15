using Domain.Models.Requests;
using Domain.Models.Responses;

using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Attributes;
using Microsoft.Extensions.Logging;

using System.Net;

using WorkerNode.Authorization;
using WorkerNode.Middleware;
using WorkerNode.Services;

namespace WorkerNode
{
    public class CommissionPlansFunc(
        ILogger<AuthenticateFunc> logger,
        IApiClient apiClient,
        IUserRepository repository) : BaseFunc
    {
        public const string CommissionPlansTag = "Azure Function Commissions Calculator";
        private readonly ILogger<AuthenticateFunc> _logger = logger;
        private readonly IApiClient _apiClient = apiClient;
        private readonly IUserRepository _repository = repository;
     
        [Authorize(UserRoles = [UserRoles.Sales, UserRoles.Admin])]
        [Function(nameof(GetCurrentPlan))]
        [OpenApiOperation(operationId: nameof(GetCurrentPlan), tags: [CommissionPlansTag])]
        [OpenApiResponseWithBody(
            statusCode: HttpStatusCode.OK,
            contentType: "application/json",
            bodyType: typeof(CommissionPlanHeaderModel),
            Description = "OK response with user's current commission plan header data")]
        public HttpResponseData GetCurrentPlan(
            [HttpTrigger(AuthorizationLevel.User, "get")] HttpRequestData req,
            FunctionContext executionContext)
        {
            var userContext = executionContext.Features.Get<UserContextFeature>();
            if (userContext == null)
            {
                return CreateUnauthorizedResponse(req, "Unauthorized access!");
            }

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
            [HttpTrigger(AuthorizationLevel.User, "get")] HttpRequestData req,
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
            [HttpTrigger(AuthorizationLevel.User, "post")] HttpRequestData req,
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
            bodyType: typeof(CommissionPlanDtailsByPeriodModel),
            Description = "OK response with user's commission plan header data for a concrete year")]
        public HttpResponseData GetPlanDetails(
            [HttpTrigger(AuthorizationLevel.User, "post")] HttpRequestData req,
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
    }
}
