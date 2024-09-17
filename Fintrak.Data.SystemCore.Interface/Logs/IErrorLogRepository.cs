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
    public interface IErrorLogRepository : IDataRepository<ErrorLogs>
    {
        Task LogErrorAsync(ErrorLogs errorLog);
        Task<IEnumerable<ErrorLogs>> GetErrorLogsAsync(string? username, DateTime? startDate, DateTime? endDate);
    }
}
