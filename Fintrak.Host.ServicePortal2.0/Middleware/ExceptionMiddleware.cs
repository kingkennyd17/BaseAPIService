using System.Net.Http;
using System.Net;
using System.Text.Json;
using Microsoft.Extensions.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using static Fintrak.Shared.Common.CustomErrorTypes;
using Fintrak.Data.Interface;
using Fintrak.Model.SystemCore;
using Fintrak.Service.SystemCore.Interface;
using Fintrak.Shared.Common.Helper;

namespace Fintrak.Host.ServicePortal
{
    public class ExceptionMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<ExceptionMiddleware> _logger;
        private readonly IHostEnvironment _env;

        public ExceptionMiddleware(RequestDelegate next, ILogger<ExceptionMiddleware> logger, 
            IHostEnvironment env)
        {
            _next = next;
            _logger = logger;
            _env = env;
        }

        public async Task InvokeAsync(HttpContext httpContext, ISystemCoreManager systemCoreManager)
        {
            try
            {
                await _next(httpContext);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An unexpected error occurred");
                await LogErrorAsync(ex, systemCoreManager, httpContext);
                await HandleExceptionAsync(httpContext, ex);
            }
        }

        private async Task LogErrorAsync(Exception exception, ISystemCoreManager systemCoreManager, HttpContext httpContext)
        {
            // Check if a user is logged in
            string tenantId = "DEF"; // Default value
            string createdBy = "auto";

            var errorLog = new ErrorLogs
            {
                ExceptionMessage = exception.InnerException?.Message ?? exception.Message,
                StackTrace = exception.StackTrace,
                Timestamp = DateTime.UtcNow,
                TenantId = tenantId,
                CreatedBy = createdBy,
                CreatedOn = DateTime.Now,
                UpdatedBy = createdBy,
                UpdatedOn = DateTime.Now,
            };

            await systemCoreManager.LogErrorAsync(errorLog);
        }

        private Task HandleExceptionAsync(HttpContext context, Exception exception)
        {
            context.Response.ContentType = "application/json";
            int statusCode;
            string message;

            switch (exception)
            {
                case NotFoundException notFoundException:
                    statusCode = (int)HttpStatusCode.NotFound;
                    message = notFoundException.Message;
                    break;
                case BadRequestException badRequestException:
                    statusCode = (int)HttpStatusCode.BadRequest;
                    message = badRequestException.Message;
                    break;
                default:
                    statusCode = (int)HttpStatusCode.InternalServerError;
                    message = "Internal Server Error";
                    break;
            }

            var response = new ErrorDetails(statusCode, message, _env.IsDevelopment() ? exception.StackTrace?.ToString() : null);

            return context.Response.WriteAsync(response.ToString());
        }


    }

    public class ErrorDetails
    {
        public int StatusCode { get; }
        public string Message { get; }
        public string? Details { get; }

        public ErrorDetails(int statusCode, string message, string? details = null)
        {
            StatusCode = statusCode;
            Message = message;
            Details = details;
        }

        public override string ToString()
        {
            return JsonSerializer.Serialize(this);
        }
    }

}