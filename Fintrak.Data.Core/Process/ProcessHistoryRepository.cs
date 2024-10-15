using Fintrak.Model.Core;
using System.ComponentModel.Composition;

namespace Fintrak.Data.Core
{
    [Export(typeof(IProcessHistoryRepository))]
    [PartCreationPolicy(CreationPolicy.NonShared)]
    public class ProcessHistoryRepository : DataRepositoryBase<ProcessHistory>, IProcessHistoryRepository
    {
        public ProcessHistoryRepository(CoreDbContext context) : base(context)
        {
        }

        public async Task<IEnumerable<ProcessHistory>> GetProcessHistorys(int defaultCount)
        {
            var query = (from e in _context.Set<ProcessHistory>().Take(defaultCount).OrderByDescending(c => c.CreatedOn)
                            select e);

            return query.ToArray();
        }
    }
}
