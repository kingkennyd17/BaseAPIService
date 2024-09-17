using Fintrak.Model.SystemCore.Common;
using Fintrak.Service.SystemCore.Interface;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Fintrak.Host.ServicePortal2._0.Controllers.SystemCore
{
    [Route("api/[controller]")]
    [ApiController]
    public class AudittrailController : ControllerBase
    {
        private readonly IConfiguration _configuration;
        private readonly ISystemCoreManager _systemCoreManager;

        public AudittrailController(IConfiguration configuration, ISystemCoreManager systemCoreManager)
        {
            _configuration = configuration;
            _systemCoreManager = systemCoreManager;
        }

        [HttpGet]
        [Route("getaudittrail")]
        [Authorize(Roles = "AdminMaker, AdminChecker")]
        public async Task<IActionResult> GetAuditLogs([FromQuery] string? username, [FromQuery] string? action, [FromQuery] DateTime? startDate, [FromQuery] DateTime? endDate)
        {
            var auditLogs = await _systemCoreManager.GetAuditLogsAsync(username, action, startDate, endDate);
            return Ok(auditLogs);
        }
    }
}
