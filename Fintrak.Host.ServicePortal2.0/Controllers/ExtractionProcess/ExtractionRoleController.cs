using Fintrak.Host.ServicePortal2._0.Models;
using Fintrak.Model.Core;
using Fintrak.Model.Core.Enum;
using Fintrak.Model.SystemCore.Common;
using Fintrak.Service.Core.Interface;
using Fintrak.Service.SystemCore.Interface;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Fintrak.Host.ServicePortal2._0.Controllers.SystemCore
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = "IFRSAdministrator, FinstatAdministrator, IFRSUser, FinstatUser")]
    public class ExtractionRoleController : ControllerBase
    {
        private readonly IExtractionProcessService _service;

        public ExtractionRoleController(IExtractionProcessService service)
        {
            _service = service;
        }

        [HttpPost]
        [Route("updateextractionrole")]
        public async Task<IActionResult> UpdateExtractionRole([FromBody] ExtractionRole extractionroleModel)
        {
            await _service.UpdateExtractionRole(extractionroleModel);

            return Ok();
        }

        [HttpPost]
        [Route("deleteextractionrole")]
        public async Task<IActionResult> DeleteExtractionRole([FromBody] int extractionroleId)
        {
            // not that calling the WCF service here will authenticate access to the data
            ExtractionRole extractionrole = await _service.GetExtractionRole(extractionroleId);

            if (extractionrole != null)
            {
                await _service.DeleteExtractionRole(extractionroleId);

                return Ok();
            }
            else
                return BadRequest("No extractionrole found under that ID.");
        }

        [HttpGet]
        [Route("getextractionrole/{extractionroleId}")]
        public async Task<IActionResult> GetExtractionRole(int extractionroleId)
        {
            ExtractionRole extractionrole = await _service.GetExtractionRole(extractionroleId);

            return Ok(extractionrole);
        }

        [HttpGet]
        [Route("availableextractionroles")]
        public async Task<IActionResult> GetAvailableExtractionRoles(HttpRequestMessage request)
        {
            IEnumerable<ExtractionRoleData> extractionroles = await _service.GetExtractionRoles();

            return Ok(extractionroles);
        }
    }
}
