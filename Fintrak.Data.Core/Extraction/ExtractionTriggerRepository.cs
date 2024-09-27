using Fintrak.Data.Core.Interface;
using Fintrak.Model.Core;
using Fintrak.Model.SystemCore;
using Fintrak.Model.SystemCore.Common;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.Composition;

namespace Fintrak.Data.Core
{
    [Export(typeof(IExtractionTriggerRepository))]
    [PartCreationPolicy(CreationPolicy.NonShared)]
    public class ExtractionTriggerRepository : DataRepositoryBase<ExtractionTrigger>, IExtractionTriggerRepository
    {
        public ExtractionTriggerRepository(CoreDbContext context) : base(context)
        {
        }

        public async Task<IEnumerable<ExtractionTriggerInfo>> GetExtractionTriggers()
        {
            var query = await (from a in _context.ExtractionTriggerSet
                        join b in _context.ExtractionSet on a.ExtractionId equals b.ExtractionId
                        join c in _context.SolutionRunDateSet on b.SolutionId equals c.SolutionId
                        where a.StartDate >= c.RunDate && a.EndDate <= c.RunDate
                        select new ExtractionTriggerInfo()
                        {
                            ExtractionTrigger = a,
                            Extraction = b
                        }).ToListAsync();

            return query;
        }

        public async Task<IEnumerable<ExtractionTriggerInfo>> GetExtractionTriggerByJob(string jobCode)
        {
            var query = await (from a in _context.ExtractionTriggerSet
                        join b in _context.ExtractionSet on a.ExtractionId equals b.ExtractionId
                        join c in _context.ExtractionJobSet on a.ExtractionJobId equals c.ExtractionJobId
                        where c.Code == jobCode
                        select new ExtractionTriggerInfo()
                        {
                            ExtractionTrigger = a,
                            Extraction = b,
                            ExtractionJob = c
                        }).ToListAsync();

            return query;
        }

        public async Task<IEnumerable<ExtractionTriggerInfo>> GetExtractionTriggerByExtraction(int extractionId)
        {
            var query = await (from a in _context.ExtractionTriggerSet
                        join b in _context.ExtractionSet on a.ExtractionId equals b.ExtractionId
                        join c in _context.ExtractionJobSet on a.ExtractionJobId equals c.ExtractionJobId
                        where a.ExtractionId == extractionId
                        select new ExtractionTriggerInfo()
                        {
                            ExtractionTrigger = a,
                            Extraction = b,
                            ExtractionJob = c
                        }).ToListAsync();

            return query;
        }

        public async Task<IEnumerable<ExtractionTriggerInfo>> GetExtractionTriggerByRunDate(DateTime startDate, DateTime endDate)
        {
            var query = await (from a in _context.ExtractionTriggerSet
                        join b in _context.ExtractionSet on a.ExtractionId equals b.ExtractionId
                        join c in _context.ExtractionJobSet on a.ExtractionJobId equals c.ExtractionJobId
                        where a.StartDate >= startDate.Date && a.EndDate <= endDate.Date
                        select new ExtractionTriggerInfo()
                        {
                            ExtractionTrigger = a,
                            Extraction = b,
                            ExtractionJob = c
                        }).ToListAsync();

            return query;
        }

        public async Task<IEnumerable<ExtractionTriggerInfo>> GetExtractionTriggerByRunTime(DateTime runTime)
        {
            var query = await (from a in _context.ExtractionTriggerSet
                        join b in _context.ExtractionSet on a.ExtractionId equals b.ExtractionId
                        join c in _context.ExtractionJobSet on a.ExtractionJobId equals c.ExtractionJobId
                        where a.RunTime == runTime
                        select new ExtractionTriggerInfo()
                        {
                            ExtractionTrigger = a,
                            Extraction = b,
                            ExtractionJob = c
                        }).ToListAsync();

            return query;
        }
    }
}
