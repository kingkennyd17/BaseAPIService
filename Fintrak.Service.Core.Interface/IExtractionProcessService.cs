using Fintrak.Model.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fintrak.Service.Core.Interface
{
    public interface IExtractionProcessService
    {
        #region Extraction
        Task UpdateExtraction(Extraction extraction);
        Task DeleteExtraction(int extractionId);
        Task<Extraction> GetExtraction(int extractionId);
        Task<IEnumerable<Extraction>> GetAllExtractions();
        Task<IEnumerable<ExtractionData>> GetExtractions();
        Task<IEnumerable<ExtractionData>> GetExtractionByLogin(string loginID);
        Task<IEnumerable<ExtractionData>> GetExtractionBySolution(int solutionId, string loginID);
        #endregion Extraction

        #region ExtractionRole
        Task UpdateExtractionRole(ExtractionRole extractionRole);
        Task DeleteExtractionRole(int extractionRoleId);
        Task<ExtractionRole> GetExtractionRole(int extractionRoleId);
        Task<IEnumerable<ExtractionRole>> GetAllExtractionRoles();
        Task<IEnumerable<ExtractionRoleData>> GetExtractionRoles();
        Task<IEnumerable<ExtractionRoleData>> GetExtractionRoleByExtraction(int extractionId);
        #endregion ExtractionRole

        //#region PackageSetup
        //PackageSetup UpdatePackageSetup(PackageSetup packageSetup);
        //PackageSetup GetFirstPackageSetup();
        //#endregion PackageSetup

        //#region Process
        //Processes UpdateProcess(Processes process);
        //Task DeleteProcess(int processId);
        //Processes GetProcess(int processId);
        //Task<IEnumerable<Processes>> GetAllProcesses();
        //Task<IEnumerable<ProcessData>> GetProcesses();
        //Task<IEnumerable<ProcessData>> GetProcessBySolution(int solutionId, string loginID);
        //#endregion Process

        //#region ProcessRole
        //ProcessRole UpdateProcessRole(ProcessRole processRole);
        //Task DeleteProcessRole(int processRoleId);
        //ProcessRole GetProcessRole(int processRoleId);
        //Task<IEnumerable<ProcessRole>> GetAllProcessRoles();
        //Task<IEnumerable<ProcessRoleData>> GetProcessRoles();
        //Task<IEnumerable<ProcessRoleData>> GetProcessRoleByProcess(int processId);
        //#endregion ProcessRole

        #region ExtractionTrigger
        Task UpdateExtractionTrigger(ExtractionTrigger extractionTrigger);
        Task DeleteExtractionTrigger(int extractionTriggerId);
        Task<ExtractionTrigger> GetExtractionTrigger(int extractionTriggerId);
        Task<IEnumerable<ExtractionTrigger>> GetAllExtractionTriggers();
        Task<IEnumerable<ExtractionTriggerData>> GetExtractionTriggers();
        Task<IEnumerable<ExtractionTriggerData>> GetExtractionTriggerByExtraction(int extractionId);
        Task<IEnumerable<ExtractionTriggerData>> GetExtractionTriggerByJob(string jobCode);
        Task<IEnumerable<ExtractionTriggerData>> GetExtractionTriggerByRunDate(DateTime startDate, DateTime endDate);
        Task<IEnumerable<ExtractionTriggerData>> GetExtractionTriggerByRunTime(DateTime runTime);
        Task<IEnumerable<ExtractionTriggerData>> RunExtraction(int jobId, int[] extractionIds, DateTime startDate, DateTime endDate, DateTime runTime);
        Task<IEnumerable<ExtractionTriggerData>> CancelExtractions(DateTime startDate, DateTime endDate);
        Task<IEnumerable<ExtractionTriggerData>> CancelExtractionByCode(string code, DateTime startDate, DateTime endDate);
        #endregion ExtractionTrigger

        //#region ProcessTrigger
        //ProcessTrigger UpdateProcessTrigger(ProcessTrigger processTrigger);
        //Task DeleteProcessTrigger(int processTriggerId);
        //ProcessTrigger GetProcessTrigger(int processTriggerId);
        //Task<IEnumerable<ProcessTrigger>> GetAllProcessTriggers();
        //Task<IEnumerable<ProcessTriggerData>> GetProcessTriggers();
        //Task<IEnumerable<ProcessTriggerData>> GetProcessTriggerByProcess(int processId);
        //Task<IEnumerable<ProcessTriggerData>> GetProcessTriggerByRunDate();
        //Task<IEnumerable<ProcessTriggerData>> GetProcessTriggerByRunTime(DateTime runTime);
        //Task<IEnumerable<ProcessTriggerData>> GetProcessTriggerByJob(string jobCode);
        //Task<IEnumerable<ProcessTriggerData>> RunProcess(int jobId, int[] processIds, DateTime runTime);
        //Task<IEnumerable<ProcessTriggerData>> CancelProcesses(DateTime startDate, DateTime endDate);
        //Task<IEnumerable<ProcessTriggerData>> CancelProcessByCode(string code, DateTime startDate, DateTime endDate);
        //#endregion ProcessTrigger

        #region SolutionRunDate
        Task UpdateSolutionRunDate(SolutionRunDate solutionRunDate);
        //Task DeleteSolutionRunDate(int solutionRunDateId);
        Task<SolutionRunDate> GetSolutionRunDate(int solutionRunDateId);
        Task<IEnumerable<SolutionRunDate>> GetAllSolutionRunDates();
        Task<IEnumerable<SolutionRunDateData>> GetSolutionRunDates();
        Task<IEnumerable<SolutionRunDateData>> GetSolutionRunDateByLogin(string loginID);
        Task<string> GetSolutionRunDateByLoginByDefault(string loginID);
        Task<IEnumerable<SolutionRunDate>> GetRunDate();
        Task RestoreArchive(int solutionid, DateTime date);
        #endregion SolutionRunDate

        #region ClosedPeriod
        Task UpdateClosedPeriod(ClosedPeriod closedPeriod);
        Task DeleteClosedPeriod(int closedPeriodId);
        Task<ClosedPeriod> GetClosedPeriod(int closedPeriodId);
        Task<IEnumerable<ClosedPeriod>> GetAllClosedPeriods();
        Task<IEnumerable<ClosedPeriodData>> GetClosedPeriods();
        Task<IEnumerable<ClosedPeriodData>> GetClosedPeriodByLogin(string loginID);
        Task<ClosedPeriod> ClosePeriod(ClosedPeriod closedPeriod);
        #endregion ClosedPeriod

        #region ClosedPeriodTemplate
        Task UpdateClosedPeriodTemplate(ClosedPeriodTemplate closedPeriodTemplate);
        Task DeleteClosedPeriodTemplate(int closedPeriodTemplateId);
        Task<ClosedPeriodTemplate> GetClosedPeriodTemplate(int closedPeriodTemplateId);
        Task<IEnumerable<ClosedPeriodTemplate>> GetAllClosedPeriodTemplates();
        Task<IEnumerable<ClosedPeriodTemplateData>> GetClosedPeriodTemplates();
        Task<IEnumerable<ClosedPeriodTemplateData>> GetClosedPeriodTemplateByLogin(string loginID);
        #endregion ClosedPeriodTemplate

        #region ExtractionJob
        Task UpdateExtractionJob(ExtractionJob extractionJob);
        Task DeleteExtractionJob(int extractionJobId);
        Task<ExtractionJob> GetExtractionJob(int extractionJobId);
        Task<IEnumerable<ExtractionJob>> GetCurrentExtractionJobs();
        Task<IEnumerable<ExtractionJob>> GetExtractionJobByDate(DateTime startDate, DateTime endDate);
        Task<IEnumerable<ExtractionJob>> RunExtractionJob(int jobId, int[] extractionIds, DateTime startDate, DateTime endDate, DateTime runTime);
        Task<IEnumerable<ExtractionJob>> CancelExtractionJobByCode(string jobCode, DateTime startDate, DateTime endDate);
        Task ClearExtractionHistory(int solutionId);
        #endregion ExtractionJob

        //#region ProcessJob
        //ProcessJob UpdateProcessJob(ProcessJob processJob);
        //Task DeleteProcessJob(int processJobId);
        //ProcessJob GetProcessJob(int processJobId);
        //Task<IEnumerable<ProcessJob>> GetCurrentProcessJobs();
        //Task<IEnumerable<ProcessJob>> GetProcessJobByRunDate();
        //Task<IEnumerable<ProcessJob>> RunProcessJob(int jobId, int[] processIds, DateTime runTime);
        //Task<IEnumerable<ProcessJob>> CancelProcessJobByCode(string jobCode);
        //Task RestartService(string serviceName);
        //Task<string> GetServiceStatus(string serviceName);
        //Task ClearProcessHistory(int solutionId);
        //#endregion ProcessJob

        //#region Upload
        //Upload UpdateUpload(Upload upload);
        //Task DeleteUpload(int uploadId);
        //Upload GetUpload(int uploadId);
        //Task<IEnumerable<Upload>> GetUploadBySolution(int solutionId);
        //Task<IEnumerable<Upload>> GetAllUploads();
        //Task<IEnumerable<UploadData>> GetUploads();
        //UploadResult[] UploadCSV(int uploadId, string csvText);//, bool truncate, bool postUploadAction
        //UploadResult[] UploadCSVByCode(string uploadCode, string csvText);
        //UploadResult[] VerificationMsg(string sppVerify);
        //#endregion

        //#region UploadRole
        //UploadRole UpdateUploadRole(UploadRole uploadRole);
        //Task DeleteUploadRole(int uploadRoleId);
        //UploadRole GetUploadRole(int uploadRoleId);
        //Task<IEnumerable<UploadRole>> GetAllUploadRoles();
        //Task<IEnumerable<UploadRoleData>> GetUploadRoles();
        //Task<IEnumerable<UploadRoleData>> GetUploadRoleByUpload(int uploadId);
        //#endregion

        //#region CheckDataAvailability
        //Task<IEnumerable<CheckDataAvailability>> GetAllDataAvailability();
        //Task CheckDataAvailabilitybyRunDate(DateTime runDate);
        //#endregion

        //#region CheckifrsDataAvailability
        //Task<IEnumerable<CheckifrsDataAvailability>> GetAllifrsDataAvailability();
        //Task CheckifrsDataAvailabilitybyRunDate(DateTime runDate);
        //#endregion

        #region ExtractionSummary
        Task UpdateExtractionSummary(ExtractionSummary uploadRole);
        Task DeleteExtractionSummary(int uploadRoleId);
        Task<ExtractionSummary> GetExtractionSummary(int uploadRoleId);
        Task<IEnumerable<ExtractionSummary>> GetAllExtractionSummarys();
        #endregion

        //#region ProcessHistory
        //ProcessHistory UpdateProcessHistory(ProcessHistory processHistory);
        //Task DeleteProcessHistory(int processHistoryId);
        //ProcessHistory GetProcessHistory(int processHistoryId);
        //Task<IEnumerable<ProcessHistory>> GetProcessHistorys(int defaultCount);
        //Task<IEnumerable<ProcessHistory>> GetAllProcessHistory();
        //Task RunProcessHistory(int processhistoryrunId);
        //#endregion Process

        //#region ProcessHistoryRun
        //ProcessHistoryRun UpdateProcessHistoryRun(ProcessHistoryRun processHistoryRun);
        //Task DeleteProcessHistoryRun(int processHistoryRunId);
        //ProcessHistoryRun GetProcessHistoryRun(int processHistoryRunId);
        //Task<IEnumerable<ProcessHistoryRun>> GetProcessHistoryRuns(int defaultCount);
        //Task<IEnumerable<ProcessHistoryRun>> GetAllProcessHistoryRun();
        //#endregion ProcessHistoryRun
    }
}
