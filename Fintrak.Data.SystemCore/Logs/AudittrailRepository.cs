using Fintrak.Data.SystemCore.Interface;
using Fintrak.Model.SystemCore;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.Composition;

namespace Fintrak.Data.SystemCore
{
    [Export(typeof(IAudittrailRepository))]
    [PartCreationPolicy(CreationPolicy.NonShared)]
    public class AudittrailRepository : DataRepositoryBase<AuditLog>, IAudittrailRepository
    {
        public AudittrailRepository(SystemCoreDbContext context) : base(context)
        {
        }

        public async Task LogAuditAsync(AuditLog entity)
        {
            _context.DisableAuditLogging();
            _context.Set<AuditLog>().Add(entity);
            await _context.SaveChangesAsync();
        }

        public async Task<IEnumerable<AuditLog>> GetAuditLogsAsync(string? username, string? action, DateTime? startDate, DateTime? endDate)
        {
            var query = _context.Set<AuditLog>().AsQueryable();

            if (!string.IsNullOrEmpty(username))
            {
                query = query.Where(al => al.UserName == username);
            }

            if (!string.IsNullOrEmpty(action))
            {
                query = query.Where(al => al.Action == action);
            }

            if (startDate.HasValue)
            {
                query = query.Where(al => al.Timestamp >= startDate.Value);
            }

            if (endDate.HasValue)
            {
                query = query.Where(al => al.Timestamp <= endDate.Value);
            }

            return await query.ToListAsync();
        }
    }
}
