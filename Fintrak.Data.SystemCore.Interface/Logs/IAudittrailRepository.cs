using Fintrak.Model.SystemCore;
using Fintrak.Shared.Common.Interface;
using Fintrak.Data.SystemCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fintrak.Data.Interface
{
    public interface IAudittrailRepository : IDataRepository<AuditLog>
    {
        Task LogAuditAsync(AuditLog errorLog);
        Task<IEnumerable<AuditLog>> GetAuditLogsAsync(string? username, string? action, DateTime? startDate, DateTime? endDate);
    }
}
