using Fintrak.Shared.Common.Tenancy;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuditManager.AuditInterface
{
    public interface IAuditInterface
    {
        Task AddAudit(List<EntityEntry> entries);
    }
}
