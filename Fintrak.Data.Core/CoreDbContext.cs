using AuditManager.AuditInterface;
using Fintrak.Model.Core;
using Fintrak.Model.SystemCore;
using Fintrak.Shared.Common.Tenancy;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;

namespace Fintrak.Data.Core
{
    public class CoreDbContext : DbContext
    {
        private readonly ITenantProvider _tenantProvider;
        private readonly IAuditInterface _auditInterface;
        public CoreDbContext(ITenantProvider tenantProvider, IAuditInterface auditInterface)
        {
            _tenantProvider = tenantProvider;
            _auditInterface = auditInterface;
        }

        public DbSet<Extraction> ExtractionSet { get; set; }
        public DbSet<ExtractionRole> ExtractionRoleSet { get; set; }
        public DbSet<ExtractionJob> ExtractionJobSet { get; set; }
        public DbSet<ExtractionTrigger> ExtractionTriggerSet { get; set; }
        public DbSet<SolutionRunDate> SolutionRunDateSet { get; set; }
        public DbSet<DefaultUser> DefaultUserSet { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Extraction>().HasQueryFilter(e => e.TenantId == _tenantProvider.TenantId);
            modelBuilder.Entity<Extraction>().Ignore(e => e.EntityId).HasKey(e => e.ExtractionId);
            modelBuilder.Entity<Extraction>().Property(c => c.RowVersion).IsRowVersion();
            modelBuilder.Entity<Extraction>().ToTable("cor_extraction");

            modelBuilder.Entity<ExtractionJob>().HasQueryFilter(e => e.TenantId == _tenantProvider.TenantId);
            modelBuilder.Entity<ExtractionJob>().Ignore(e => e.EntityId).HasKey(e => e.ExtractionJobId);
            modelBuilder.Entity<ExtractionJob>().Property(c => c.RowVersion).IsRowVersion();
            modelBuilder.Entity<ExtractionJob>().ToTable("cor_extractionjob");

            modelBuilder.Entity<ExtractionRole>().HasQueryFilter(e => e.TenantId == _tenantProvider.TenantId);
            modelBuilder.Entity<ExtractionRole>().Ignore(e => e.EntityId).HasKey(e => e.ExtractionRoleId);
            modelBuilder.Entity<ExtractionRole>().Property(c => c.RowVersion).IsRowVersion();
            modelBuilder.Entity<ExtractionRole>().ToTable("cor_extractionrole");

            modelBuilder.Entity<ExtractionTrigger>().HasQueryFilter(e => e.TenantId == _tenantProvider.TenantId);
            modelBuilder.Entity<ExtractionTrigger>().Ignore(e => e.EntityId).HasKey(e => e.ExtractionTriggerId);
            modelBuilder.Entity<ExtractionTrigger>().Property(c => c.RowVersion).IsRowVersion();
            modelBuilder.Entity<ExtractionTrigger>().ToTable("cor_extractiontrigger");

            modelBuilder.Entity<ExtractionSummary>().HasQueryFilter(e => e.TenantId == _tenantProvider.TenantId);
            modelBuilder.Entity<ExtractionSummary>().Ignore(e => e.EntityId).HasKey(e => e.Id);
            modelBuilder.Entity<ExtractionSummary>().Property(c => c.RowVersion).IsRowVersion();
            modelBuilder.Entity<ExtractionSummary>().ToTable("cor_extractiontrigger");

            modelBuilder.Entity<SolutionRunDate>().HasQueryFilter(e => e.TenantId == _tenantProvider.TenantId);
            modelBuilder.Entity<SolutionRunDate>().Ignore(e => e.EntityId).HasKey(e => e.SolutionRunDateId);
            modelBuilder.Entity<SolutionRunDate>().Property(c => c.RowVersion).IsRowVersion();
            modelBuilder.Entity<SolutionRunDate>().ToTable("cor_solutionrundate");

            modelBuilder.Entity<DefaultUser>().HasQueryFilter(e => e.TenantId == _tenantProvider.TenantId);
            modelBuilder.Entity<DefaultUser>().Ignore(e => e.EntityId).HasKey(e => e.DefaultUserId);
            modelBuilder.Entity<DefaultUser>().Property(c => c.RowVersion).IsRowVersion();
            modelBuilder.Entity<DefaultUser>().ToTable("cor_defaultuser");
        }

        public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            await AddAudit();
            SetTenantId();

            return await base.SaveChangesAsync(cancellationToken);
        }

        private void SetTenantId()
        {
            // Get entries that are being added or modified
            var entries = ChangeTracker.Entries<ITenantEntity>()
                .Where(e => e.State == EntityState.Added || e.State == EntityState.Modified);

            foreach (var entry in entries)
            {
                entry.Entity.TenantId = _tenantProvider.TenantId;

                if (entry.State == EntityState.Added)
                {
                    //entry is added
                    entry.Entity.CreatedBy = _tenantProvider.UserName;
                    entry.Entity.CreatedOn = DateTime.Now;
                    entry.Entity.UpdatedBy = _tenantProvider.UserName;
                    entry.Entity.UpdatedOn = DateTime.Now;
                }
                else
                {
                    //entry is modified
                    entry.Entity.UpdatedBy = _tenantProvider.UserName;
                    entry.Entity.UpdatedOn = DateTime.Now;
                }
            }
        }

        private async Task AddAudit()
        {
            var entries = ChangeTracker.Entries()
                .Where(e => e.State == EntityState.Added || e.State == EntityState.Modified || e.State == EntityState.Deleted)
                .ToList();
            await _auditInterface.AddAudit(entries);
        }
    }
}
