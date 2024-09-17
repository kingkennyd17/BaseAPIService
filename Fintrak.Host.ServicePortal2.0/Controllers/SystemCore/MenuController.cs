using Fintrak.Model.SystemCore.Common;
using Fintrak.Service.SystemCore.Interface;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Fintrak.Host.ServicePortal2._0.Controllers.SystemCore
{
    [Route("api/[controller]")]
    [ApiController]
    public class MenuController : ControllerBase
    {
        private readonly IConfiguration _configuration;
        private readonly ISystemCoreManager _systemCoreManager;

        public MenuController(IConfiguration configuration, ISystemCoreManager systemCoreManager)
        {
            _configuration = configuration;
            _systemCoreManager = systemCoreManager;
        }

        [HttpGet]
        [Route("getmenusbylogin")]
        [Authorize(Roles = "AdminMaker, AdminChecker, IFRSAdministrator, FinstatAdministrator, IFRSUser, FinstatUser, TenantAdmin")]
        public async Task<IActionResult> GetErrorLogs()
        {
            var user = User?.FindFirstValue(ClaimTypes.NameIdentifier);
            var auditLogs = await _systemCoreManager.GetMenuByLoginID(user);
            return Ok(auditLogs);
        }
    }
}
