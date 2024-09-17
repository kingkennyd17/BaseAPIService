using Fintrak.Model.SystemCore.Common;
using Fintrak.Service.SystemCore.Interface;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Fintrak.Host.ServicePortal2._0.Controllers.SystemCore
{
    [Route("api/[controller]")]
    [ApiController]
    public class ErrorLogController : ControllerBase
    {
        private readonly IConfiguration _configuration;
        private readonly ISystemCoreManager _systemCoreManager;

        public ErrorLogController(IConfiguration configuration, ISystemCoreManager systemCoreManager)
        {
            _configuration = configuration;
            _systemCoreManager = systemCoreManager;
        }

        [HttpGet]
        [Route("geterrorlog")]
        [Authorize(Roles = "AdminMaker, AdminChecker")]
        public async Task<IActionResult> GetErrorLogs([FromQuery] string? username, [FromQuery] DateTime? startDate, [FromQuery] DateTime? endDate)
        {
            var auditLogs = await _systemCoreManager.GetErrorLogsAsync(username, startDate, endDate);
            return Ok(auditLogs);
        }
    }
}
