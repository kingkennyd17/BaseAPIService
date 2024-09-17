using Fintrak.Data.Interface;
using Fintrak.Data.SystemCore.Interface;
using Fintrak.Model.SystemCore;
using Fintrak.Shared.Common.Base;
using Fintrak.Shared.Common.Interface;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fintrak.Data.SystemCore
{
    [Export(typeof(IRoleRepository))]
    [PartCreationPolicy(CreationPolicy.NonShared)]
    public class ErrorLogRepository : DataRepositoryBase<ErrorLogs>, IErrorLogRepository
    {
        private readonly SystemCoreDbContext _context;

        public ErrorLogRepository(SystemCoreDbContext context) : base(context)
        {
            _context = context;
        }

        public async Task LogErrorAsync(ErrorLogs errorLog)
        {
            _context.DisableAuditLogging();
            _context.Set<ErrorLogs>().Add(errorLog);
            await _context.SaveChangesAsync();
        }

        public async Task<IEnumerable<ErrorLogs>> GetErrorLogsAsync(string? username, DateTime? startDate, DateTime? endDate)
        {
            var query = _context.Set<ErrorLogs>().AsQueryable();

            if (!string.IsNullOrEmpty(username))
            {
                query = query.Where(al => al.CreatedBy == username);
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
