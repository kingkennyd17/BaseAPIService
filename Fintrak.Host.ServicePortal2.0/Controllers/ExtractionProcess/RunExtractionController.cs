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
    public class RunExtractionController : ControllerBase
    {
        private readonly IExtractionProcessService _service;

        public RunExtractionController(IExtractionProcessService service)
        {
            _service = service;
        }

        [HttpGet]
        [Route("getrunextractions")]
        public async Task<IActionResult> GetRunExtractions([FromQuery]int solutionId)
        {
            var runEntities = new List<RunExtractionSingleModel>();
            IEnumerable<ExtractionData> extractions = await _service.GetExtractionBySolution(solutionId, User?.FindFirstValue(ClaimTypes.NameIdentifier));
            var solutions = extractions.Select(c => c.SolutionName).Distinct();

            foreach (var solution in solutions)
            {

                foreach (var extraction in extractions)
                {
                    if (extraction.SolutionName == solution)
                    {
                        var runModel = new RunExtractionSingleModel();
                        runModel.SolutionId = solutionId;
                        runModel.SolutionName = solution;
                        runModel.ExtrationTitle = extraction.Title;
                        runModel.ExtractionId = extraction.ExtractionId;
                        runModel.CanRun = false;

                        runEntities.Add(runModel);
                    }
                }
            }
            return Ok(runEntities.ToArray());
        }

        [HttpGet]
        [Route("cancelextractionjob")]
        public async Task<IActionResult> CancelExtractions([FromQuery]string jobCode, [FromQuery] DateTime startDate, [FromQuery]DateTime endDate)
        {
            IEnumerable<ExtractionJob> jobs = await _service.CancelExtractionJobByCode(jobCode, startDate, endDate);

            return Ok(jobs.ToArray());
        }

        [HttpPost]
        [Route("checkextraction")]
        public async Task<IActionResult> CheckExtractions([FromQuery] DateTime startDate, [FromQuery] DateTime endDate, [FromBody] int[] extractionIds)
        {
            var message = string.Empty;

            IEnumerable<ExtractionTriggerData> extractionTriggers = (await _service.GetExtractionTriggerByRunDate(startDate, endDate)).Where(c => extractionIds.Contains(c.ExtractionId) && (c.Status == PackageStatus.New || c.Status == PackageStatus.Pending || c.Status == PackageStatus.Running)).ToArray();

            if (extractionTriggers.Count() > 0)
            {
                foreach (var trigger in extractionTriggers)
                {
                    if (trigger.Status == PackageStatus.New)
                        message += "The extraction template for " + trigger.ExtractionTitle + " has just been added for extraction.<br>";
                    else if (trigger.Status == PackageStatus.Pending)
                        message += "The extraction template for " + trigger.ExtractionTitle + " is still pending for extraction.<br>";
                    else if (trigger.Status == PackageStatus.Running)
                        message += "The extraction template for " + trigger.ExtractionTitle + " is currently running.<br>";
                }
            }
            else
                message = "Ok";

            return Ok(message);
        }

        [HttpPost]
        [Route("startextraction")]
        public async Task<IActionResult> RunExtractions([FromQuery]int jobId, [FromQuery]DateTime startDate, [FromQuery] DateTime endDate, [FromQuery]DateTime runTime, [FromBody] int[] extractionIds)
        {
            IEnumerable<ExtractionJob> jobs = await _service.RunExtractionJob(jobId, extractionIds, startDate, endDate, runTime);

            return Ok(jobs);
        }

        [HttpGet]
        [Route("getextractiontriggers")]
        public async Task<IActionResult> GetExtractionTriggers([FromQuery] string jobCode)
        {
            IEnumerable<ExtractionTriggerData> extractionTriggers = await _service.GetExtractionTriggerByJob(jobCode);

            return Ok(extractionTriggers);
        }

        [HttpGet]
        [Route("getcurrentjobs")]
        public async Task<IActionResult> GetCurrentExtractionTriggers()
        {
            IEnumerable<ExtractionJob> jobs = await _service.GetCurrentExtractionJobs();

            return Ok(jobs);
        }

        [HttpGet]
        [Route("getjobs")]
        public async Task<IActionResult> GetCurrentExtractionTriggers([FromQuery]DateTime startDate, [FromQuery]DateTime endDate)
        {
            IEnumerable<ExtractionJob> jobs = await _service.GetExtractionJobByDate(startDate, endDate);

            return Ok(jobs);
        }

        [HttpPost]
        [Route("updateextractionjob")]
        public async Task<IActionResult> UpdateExtractionJob([FromBody] ExtractionJob extractionJobModel)
        {
            extractionJobModel.UserName = User?.FindFirstValue(ClaimTypes.NameIdentifier);
            await _service.UpdateExtractionJob(extractionJobModel);

            return Ok();
        }

        [HttpGet]
        [Route("clearextractionhistory")]
        public async Task<IActionResult> ClearExtractionHistory([FromQuery]int solutionId)
        {
            //Cancel Job
            var message = string.Empty;

            await _service.ClearExtractionHistory(solutionId);

            message = "Ok";

            return Ok(message);
        }
    }
}
