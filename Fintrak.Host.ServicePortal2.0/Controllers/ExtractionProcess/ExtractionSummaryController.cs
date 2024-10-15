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
    public class ExtractionSummaryController : ControllerBase
    {
        private readonly IExtractionProcessService _service;

        public ExtractionSummaryController(IExtractionProcessService service)
        {
            _service = service;
        }

        [HttpPost]
        [Route("updateextractionsummary")]
        public async Task<IActionResult> UpdateExtractionSummary([FromBody] ExtractionSummary extractionSummaryModel)
        {
            await _service.UpdateExtractionSummary(extractionSummaryModel);

            return Ok();
        }

        [HttpPost]
        [Route("deleteextractionsummary")]
        public async Task<IActionResult> DeleteExtractionSummary([FromBody] int extractionSummaryId)
        {
            // not that calling the WCF service here will authenticate access to the data
            ExtractionSummary extractionSummary = await _service.GetExtractionSummary(extractionSummaryId);

            if (extractionSummary != null)
            {
                await _service.DeleteExtractionSummary(extractionSummaryId);

                return Ok();
            }
            else
                return BadRequest("No extraction summary found under that ID.");
        }

        [HttpGet]
        [Route("getextractionsummary/{extractionsummaryId}")]
        public async Task<IActionResult> GetExtractionSummary(int extractionSummaryId)
        {
            ExtractionSummary extractionSummary = await _service.GetExtractionSummary(extractionSummaryId);

            return Ok(extractionSummary);
        }

        [HttpGet]
        [Route("getextractionsummarys")]
        public async Task<IActionResult> GetAvailableUploads(HttpRequestMessage request)
        {
            IEnumerable<ExtractionSummary> extractionsummarys = await _service.GetAllExtractionSummarys();

            return Ok(extractionsummarys);
        }
    }
}
