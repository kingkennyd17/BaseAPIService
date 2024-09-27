using AuditManager.AuditInterface;
using Fintrak.Model.SystemCore;
using Fintrak.Shared.Common.Tenancy;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuditManager.AuditManager
{
    public class AuditManager : IAuditInterface
    {
        private readonly ITenantProvider _tenantProvider;
        private readonly AuditContext _auditContext;
        public AuditManager(ITenantProvider tenantProvider, AuditContext auditContext)
        {
            _tenantProvider = tenantProvider;
            _auditContext = auditContext;
        }

        public async Task AddAudit(List<EntityEntry> entries)
        {

            foreach (var entry in entries)
            {
                var auditLog = new AuditLog
                {
                    UserId = _tenantProvider.UserId,
                    UserName = _tenantProvider.UserName,
                    TableName = entry.Entity.GetType().Name,
                    Timestamp = DateTime.UtcNow,
                    IpAddress = _tenantProvider.IPAddress
                };

                if (entry.State == EntityState.Added)
                {
                    auditLog.Action = "Added";
                    auditLog.NewValues = JsonConvert.SerializeObject(entry.CurrentValues.ToObject());
                }
                else if (entry.State == EntityState.Modified)
                {
                    auditLog.Action = "Modified";
                    auditLog.OldValues = JsonConvert.SerializeObject(entry.OriginalValues.ToObject());
                    auditLog.NewValues = JsonConvert.SerializeObject(entry.CurrentValues.ToObject());
                    auditLog.AffectedColumns = string.Join(", ", entry.Properties.Where(p => p.IsModified).Select(p => p.Metadata.Name));
                }
                else if (entry.State == EntityState.Deleted)
                {
                    auditLog.Action = "Deleted";
                    auditLog.OldValues = JsonConvert.SerializeObject(entry.OriginalValues.ToObject());
                }

                await _auditContext.AuditLogSet.AddAsync(auditLog);
            }
        }
    }
}
