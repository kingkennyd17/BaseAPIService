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
    public class ExtractionController : ControllerBase
    {
        private readonly IExtractionProcessService _service;

        public ExtractionController(IExtractionProcessService service)
        {
            _service = service;
        }

        [HttpPost]
        [Route("updateextraction")]
        public async Task<IActionResult> UpdateExtraction([FromBody] Extraction extractionModel)
        {
            await _service.UpdateExtraction(extractionModel);

            return Ok();
        }

        [HttpPost]
        [Route("deleteextraction")]
        public async Task<IActionResult> DeleteExtraction([FromBody] int extractionId)
        {
            // not that calling the WCF service here will authenticate access to the data
            Extraction extraction = await _service.GetExtraction(extractionId);

            if (extraction != null)
            {
                await _service.DeleteExtraction(extractionId);

                return Ok();
            }
            else
                return BadRequest("No extraction found under that ID.");
        }

        [HttpGet]
        [Route("getextraction/{extractionId}")]
        public async Task<IActionResult> GetExtraction(HttpRequestMessage request, int extractionId)
        {
            Extraction extraction = await _service.GetExtraction(extractionId);

            // notice no need to create a seperate model object since Extraction entity will do just fine
            return Ok(extraction);
        }

        [HttpGet]
        [Route("getextractionwithchildren")]
        public async Task<IActionResult> GetExtractionWithChildren(HttpRequestMessage request, [FromQuery]int extractionId)
        {
            var extractionModel = new ExtractionModel();

            extractionModel.Extraction = await _service.GetExtraction(extractionId);
            extractionModel.ExtractionRoles = await _service.GetExtractionRoleByExtraction(extractionId);

            // notice no need to create a seperate model object since Extraction entity will do just fine
            return Ok(extractionModel);
        }

        [HttpGet]
        [Route("getextractions")]
        public async Task<IActionResult> GetAvailableExtractions(HttpRequestMessage request)
        {
            IEnumerable<ExtractionData> extractions = await _service.GetExtractions();

            return Ok(extractions);
        }
    }
}
