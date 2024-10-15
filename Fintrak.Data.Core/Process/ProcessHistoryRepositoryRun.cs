using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using Fintrak.Model.Core;

namespace Fintrak.Data.Core
{
    [Export(typeof(IProcessHistoryRunRepository))]
    [PartCreationPolicy(CreationPolicy.NonShared)]
    public class ProcessHistoryRunRepository : DataRepositoryBase<ProcessHistoryRun>, IProcessHistoryRunRepository
    {
        public ProcessHistoryRunRepository(CoreDbContext context) : base(context)
        {
        }
        public async Task<IEnumerable<ProcessHistoryRun>> GetProcessHistoryRuns(int defaultCount)
        {
            var query = (from e in _context.Set<ProcessHistoryRun>().Take(defaultCount)
                            select e).Take(defaultCount);
            return query.ToArray();
        }
    }
}
