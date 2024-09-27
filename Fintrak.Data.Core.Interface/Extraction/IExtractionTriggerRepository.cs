using System;
using Fintrak.Model.Core;
using Fintrak.Shared.Common.Interface;

namespace Fintrak.Data.Core.Interface
{
    public interface IExtractionTriggerRepository : IDataRepository<ExtractionTrigger>
    {
        Task<IEnumerable<ExtractionTriggerInfo>> GetExtractionTriggers();
        Task<IEnumerable<ExtractionTriggerInfo>> GetExtractionTriggerByJob(string jobCode);
        Task<IEnumerable<ExtractionTriggerInfo>> GetExtractionTriggerByExtraction(int extractionId);
        Task<IEnumerable<ExtractionTriggerInfo>> GetExtractionTriggerByRunDate(DateTime startDate, DateTime endDate);
        Task<IEnumerable<ExtractionTriggerInfo>> GetExtractionTriggerByRunTime(DateTime runTime);
    }
}
