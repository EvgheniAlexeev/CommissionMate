using DataLayer.Repositories;

using Domain.Models.Requests;

using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Attributes;
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
    public class CommissionAdminFunc(
        ILogger<CommissionAdminFunc> logger,
        IApiClient apiClient,
        IUserRepository repository,
        ICommissionProvider commissionProvider) : BaseFunc
    {
        public const string AdminCommissionPlansTag = "Azure Function Commissions Calculator Admin Interface";
        private readonly ILogger<CommissionAdminFunc> _logger = logger;
        private readonly IApiClient _apiClient = apiClient;
        private readonly IUserRepository _repository = repository;
        private readonly ICommissionProvider _commissionProvider = commissionProvider;

        [Authorize(UserRoles = [UserRoles.Admin])]
        [Function(nameof(SetUserAnnualPrime))]
        [OpenApiOperation(operationId: nameof(SetUserAnnualPrime), tags: [AdminCommissionPlansTag])]
        [OpenApiParameter(
            name: "userId",
            In = ParameterLocation.Query,
            Required = true,
            Type = typeof(string),
            Example = typeof(UserIdPropertyExample),
            Description = "The ID of the user for whom the annual prime will be set.")]
        [OpenApiRequestBody(contentType: "application/json",
            bodyType: typeof(SetUserCommissionAnualPrimeRequestModel),
            Example = typeof(SetUserCommissionAnualPrimeRequestModelExample),
            Description = "Set annual prime to a user.",
            Required = true)]
        [OpenApiResponseWithoutBody(statusCode: HttpStatusCode.OK, Description = "OK on success.")]
        public async Task<HttpResponseData> SetUserAnnualPrime(
            [HttpTrigger(AuthorizationLevel.Anonymous, "post")] HttpRequestData req,
            string userId,
            FunctionContext executionContext)
        {
            var request = GetRequestBody<SetUserCommissionAnualPrimeRequestModel>(req);
            if (request == null)
            {
                return CreateBadRequestResponse(req);
            }
            var userContext = executionContext.Features.Get<UserContextFeature>()!;

            await _repository.SetUserAnnualPrime(userContext.Email, userId, request);
            return req.CreateResponse(HttpStatusCode.OK);
        }

        [Authorize(UserRoles = [UserRoles.Admin])]
        [Function(nameof(CreateCommissionPlan))]
        [OpenApiOperation(operationId: nameof(AssignPlanToUser), tags: [AdminCommissionPlansTag])]
        [OpenApiRequestBody(contentType: "application/json",
            bodyType: typeof(CreateCommissionPlanHeaderRequestModel),
            Example = typeof(CreateCommissionPlanHeaderRequestModelExample),
            Description = "Create a commissions plan (header)",
            Required = true)]
        [OpenApiResponseWithoutBody(statusCode: HttpStatusCode.OK, Description = "OK on success.")]
        public async Task<HttpResponseData> CreateCommissionPlan(
                    [HttpTrigger(AuthorizationLevel.Anonymous, "post")] HttpRequestData req,
                    string userId,
                    FunctionContext executionContext)
        {
            var request = GetRequestBody<CreateCommissionPlanHeaderRequestModel>(req);
            if (request == null)
            {
                return CreateBadRequestResponse(req);
            }

            var userContext = executionContext.Features.Get<UserContextFeature>()!;
            await _repository.CreateCommissionPlan(userContext.Email, request);
            return req.CreateResponse(HttpStatusCode.OK);
        }

        [Authorize(UserRoles = [UserRoles.Admin])]
        [Function(nameof(AssignPlanToUser))]
        [OpenApiOperation(operationId: nameof(AssignPlanToUser), tags: [AdminCommissionPlansTag])]
        [OpenApiParameter(
            name: "userId",
            In = ParameterLocation.Query,
            Required = true,
            Type = typeof(string),
            Example = typeof(UserIdPropertyExample),
            Description = "The ID of the user for whom the annual plan will be set.")]
        [OpenApiRequestBody(contentType: "application/json",
            bodyType: typeof(AssignCommissionPlanHeaderRequestModel),
            Example = typeof(AssignCommissionPlanHeaderRequestModelExample),
            Description = "Assign a commissions plan to a user",
            Required = true)]
        [OpenApiResponseWithoutBody(statusCode: HttpStatusCode.OK, Description = "OK on success.")]
        public async Task<HttpResponseData> AssignPlanToUser(
            [HttpTrigger(AuthorizationLevel.Anonymous, "post")] HttpRequestData req,
            string userId,
            FunctionContext executionContext)
        {
            var request = GetRequestBody<AssignCommissionPlanHeaderRequestModel>(req);
            if (request == null)
            {
                return CreateBadRequestResponse(req);
            }

            var userContext = executionContext.Features.Get<UserContextFeature>()!;
            await _repository.AssignPlanToUser(userContext.Email, userId, request);
            return req.CreateResponse(HttpStatusCode.OK);
        }

        [Authorize(UserRoles = [UserRoles.Sales, UserRoles.Admin])]
        [Function(nameof(AddPlanDetails))]
        [OpenApiOperation(operationId: nameof(AddPlanDetails), tags: [AdminCommissionPlansTag])]
        [OpenApiParameter(
            name: "fullPlanName",
            In = ParameterLocation.Query,
            Required = true,
            Type = typeof(string),
            Example = typeof(UserIdPropertyExample),
            Description = "The ID of the user for whom the annual plan will be set.")]
        [OpenApiRequestBody(contentType: "application/json", 
            bodyType: typeof(AddPlanDetailsRequestModel),
            Example = typeof(AddPlanDetailsRequestModelExample),
            Description = "JSON payload with details of a commission plan.", 
            Required = true)]
        [OpenApiResponseWithoutBody(statusCode: HttpStatusCode.OK, Description = "OK on success.")]
        public async Task<HttpResponseData> AddPlanDetails(
            [HttpTrigger(AuthorizationLevel.Anonymous, "post")] HttpRequestData req,
            string fullPlanName,
            FunctionContext executionContext)
        {
            var request = GetRequestBody<AddPlanDetailsRequestModel>(req);
            if (request == null)
            {
                return CreateBadRequestResponse(req);
            }
            var userContext = executionContext.Features.Get<UserContextFeature>()!;

            await _repository.AddPlanDetails(userContext.Email, fullPlanName, request);
            return req.CreateResponse(HttpStatusCode.OK);
        }

        [Authorize(UserRoles = [UserRoles.Sales, UserRoles.Admin])]
        [Function(nameof(AddPayoutTablesToPlan))]
        [OpenApiOperation(operationId: nameof(AddPayoutTablesToPlan), tags: [AdminCommissionPlansTag])]
        [OpenApiParameter(name: "fullPlanName",
            In = ParameterLocation.Query,
            Required = true,
            Type = typeof(string),
            Example = typeof(PlanNameExample),
            Description = $"The name of the commission plan."
            )]
        [OpenApiRequestBody(contentType: "application/json",
            bodyType: typeof(AddCommissionPlanPayoutListRequestModel),
            Example = typeof(AddCommissionPlanPayoutListRequestModelExample),
            Description = "JSON payload with array of pay-outs of a plan.",
            Required = true)]
        [OpenApiResponseWithoutBody(statusCode: HttpStatusCode.OK, Description = "OK on success.")]
        public async Task<HttpResponseData> AddPayoutTablesToPlan(
            [HttpTrigger(AuthorizationLevel.Anonymous, "post")] HttpRequestData req,
            string fullPlanName,
            FunctionContext executionContext)
        {
            var request = GetRequestBody<AddCommissionPlanPayoutListRequestModel>(req);
            if (request == null 
                || request.AddCommissionPlanPayoutList
                    .Where(pl => pl.PayoutPeriodType == Domain.Models.PayoutPeriodType.Quarterly)
                    .Count() > 1
                || request.AddCommissionPlanPayoutList
                    .Where(pl => pl.PayoutPeriodType == Domain.Models.PayoutPeriodType.Annual)
                    .Count() > 1
                || request.AddCommissionPlanPayoutList
                    .Where(pl => pl.PayoutPeriodType == Domain.Models.PayoutPeriodType.Annual)
                    .Select(d => d.PayoutSources).Count() !=
                   request.AddCommissionPlanPayoutList
                    .Where(pl => pl.PayoutPeriodType == Domain.Models.PayoutPeriodType.Quarterly)
                    .Select(d => d.PayoutSources).Count())
            {
                return CreateBadRequestResponse(req);
            }

            var userContext = executionContext.Features.Get<UserContextFeature>()!;

            await _repository.AddCommissionPlanPayoutToPlan(userContext.Email, fullPlanName,  request);
            return req.CreateResponse(HttpStatusCode.OK);
        }
    }
}
