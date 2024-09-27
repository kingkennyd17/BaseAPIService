using Fintrak.Shared.Common.Tenancy;
using System.Security.Claims;

namespace Fintrak.Host.ServicePortal2._0.Middleware
{
    public class TenantMiddleware
    {
        private readonly RequestDelegate _next;

        public TenantMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context, TenantContext tenantContext)
        {
            var tenantIdClaim = context.User?.FindFirst("TenantId");
            if (tenantIdClaim != null)
            {
                tenantContext.TenantId = tenantIdClaim.Value;
                tenantContext.UserId = Convert.ToInt32(context?.User?.Identity?.Name);
                tenantContext.UserName = context?.User?.FindFirstValue(ClaimTypes.NameIdentifier);
                tenantContext.IPAddress = context?.Connection?.RemoteIpAddress?.ToString();
            }
            else
                tenantContext.TenantId = "DEF";

            await _next(context);
        }
    }


}
