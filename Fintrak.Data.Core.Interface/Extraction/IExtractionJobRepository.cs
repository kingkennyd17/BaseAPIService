using System;
using Fintrak.Model.Core;
using Fintrak.Shared.Common.Interface;

namespace Fintrak.Data.Core.Interface
{
    public interface IExtractionJobRepository : IDataRepository<ExtractionJob>
    {
        Task<IEnumerable<ExtractionJob>> GetExtractionJobByRunDate(DateTime startDate, DateTime endDate);
        Task ClearExtractionHistory(int solutionId);
    }
}
