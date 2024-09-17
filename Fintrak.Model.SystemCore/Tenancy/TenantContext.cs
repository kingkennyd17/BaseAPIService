using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Fintrak.Model.SystemCore.Tenancy
{
    public class TenantContext
    {
        public string TenantId { get; set; }
        public int UserId { get; set; }
        public string UserName { get; set; }
        public string IPAddress { get; set; }
    }


    public interface ITenantProvider
    {
        string TenantId { get; }
        int UserId { get; }
        string UserName { get; }
        string IPAddress { get; }
    }

    public class TenantProvider : ITenantProvider
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        public TenantProvider(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        public string TenantId => _httpContextAccessor.HttpContext?.User.FindFirst("TenantId")?.Value ?? "DEF";
        public int UserId => Convert.ToInt32(_httpContextAccessor.HttpContext?.User?.Identity?.Name);
        public string UserName => _httpContextAccessor.HttpContext?.User?.FindFirstValue(ClaimTypes.NameIdentifier);
        public string IPAddress => _httpContextAccessor.HttpContext?.Connection?.RemoteIpAddress?.ToString();
    }
}
