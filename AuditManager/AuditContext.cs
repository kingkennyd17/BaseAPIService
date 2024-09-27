using Fintrak.Model.SystemCore;
using Fintrak.Shared.Common.Tenancy;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;

namespace AuditManager
{
    public class AuditContext : DbContext
    {
        private readonly ITenantProvider _tenantProvider;
        public AuditContext(ITenantProvider tenantProvider)
        {
            _tenantProvider = tenantProvider;
        }

        public DbSet<AuditLog> AuditLogSet { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<AuditLog>().HasQueryFilter(e => e.TenantId == _tenantProvider.TenantId);
            modelBuilder.Entity<AuditLog>().Ignore(e => e.EntityId).HasKey(e => e.Id);
            modelBuilder.Entity<AuditLog>().Property(c => c.RowVersion).IsRowVersion();
            modelBuilder.Entity<AuditLog>().ToTable("cor_audittrail");
        }

        
    }
}