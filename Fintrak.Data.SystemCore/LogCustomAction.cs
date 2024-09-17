using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Fintrak.Model.SystemCore;
using System.Data.Entity;
using Fintrak.Shared.Common.Helper;

namespace Fintrak.Data.SystemCore
{
    public class LogCustomAction
    {
        private readonly UserContextService _userContextService;
        private readonly SystemCoreDbContext _context;
        public LogCustomAction(UserContextService userContextService, SystemCoreDbContext systemCoreDbContext)
        {
            _userContextService = userContextService;
            _context = systemCoreDbContext;
        }

        public async Task LogCustomActionAsync(string action, string tableName, string details)
        {
            var auditLog = new AuditLog
            {
                UserId = _userContextService.GetCurrentUserId(),
                UserName = _userContextService.GetCurrentUserName(),
                Action = action,
                TableName = tableName,
                Timestamp = DateTime.UtcNow,
                NewValues = details,
                IpAddress = _userContextService.GetClientIpAddress()
            };

            _context.AuditLogSet.Add(auditLog);
            await _context.SaveChangesAsync();
        }
    }
}
