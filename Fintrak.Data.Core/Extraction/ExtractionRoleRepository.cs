using Fintrak.Data.Core.Interface;
using Fintrak.Model.Core;
using Fintrak.Model.SystemCore;
using Fintrak.Model.SystemCore.Common;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.Composition;

namespace Fintrak.Data.Core
{
    [Export(typeof(IExtractionRoleRepository))]
    [PartCreationPolicy(CreationPolicy.NonShared)]
    public class ExtractionRoleRepository : DataRepositoryBase<ExtractionRole>, IExtractionRoleRepository
    {
        public ExtractionRoleRepository(CoreDbContext context) : base(context)
        {
        }

        public async Task<IEnumerable<ExtractionRoleInfo>> GetExtractionRoles()
        {
            var query = await (from a in _context.ExtractionRoleSet
                        join b in _context.ExtractionSet on a.ExtractionId equals b.ExtractionId

                        select new ExtractionRoleInfo()
                        {
                            ExtractionRole = a,
                            Extraction = b,

                        }).ToListAsync();

            return query;
        }

        public async Task<IEnumerable<ExtractionRoleInfo>> GetExtractionRoleByExtraction(int extractionId)
        {
            var query = await (from a in _context.ExtractionRoleSet
                        join b in _context.ExtractionSet on a.ExtractionId equals b.ExtractionId

                        where a.ExtractionId == extractionId
                        select new ExtractionRoleInfo()
                        {
                            ExtractionRole = a,
                            Extraction = b,

                        }).ToListAsync();

            return query;
        }
    }
}
