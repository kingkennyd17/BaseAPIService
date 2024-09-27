using AuditManager.AuditInterface;
using Fintrak.Model.SystemCore;
using Fintrak.Model.SystemCore.Common;
using Fintrak.Shared.Common;
using Fintrak.Shared.Common.Helper;
using Fintrak.Shared.Common.Tenancy;
using Microsoft.AspNet.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Data;
using System.Security.AccessControl;
using System.Security.Principal;

namespace Fintrak.Data.SystemCore
{
    public class SystemCoreDbContext : IdentityDbContext<UserSetup, Roles, int, IdentityUserClaim<int>, 
        UserRole, IdentityUserLogin<int>, IdentityRoleClaim<int>, IdentityUserToken<int>>
    {
        //private readonly UserContextService _userContextService;
        private readonly ITenantProvider _tenantProvider;
        private readonly IAuditInterface _auditInterface;
        public SystemCoreDbContext(DbContextOptions<SystemCoreDbContext> options, UserContextService userContextService,
                                ITenantProvider tenantProvider, IAuditInterface auditInterface)
        : base(options)
        {
            //_userContextService = userContextService;
            _tenantProvider = tenantProvider;
            _auditInterface = auditInterface;
        }

        private bool _isAuditDisabled;
        private bool _createNewTenant;

        public void DisableAuditLogging()
        {
            _isAuditDisabled = true;
        }

        public void CreateNewTenant()
        {
            _createNewTenant = true;
        }

        public DbSet<ErrorLogs> ErrorLogsSet { get; set; }
        public DbSet<Roles> RolesSet { get; set; }
        public DbSet<UserSetup> UserSetupSet { get; set; }
        public DbSet<AuditLog> AuditLogSet { get; set; }
        public DbSet<Menu> MenuSet { get; set; }
        public DbSet<MenuRole> MenuRoleSet { get; set; }
        public DbSet<Module> ModuleSet { get; set; }
        public DbSet<Solution> SolutionSet { get; set; }
        public DbSet<UserRole> UserRoleSet { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<UserRole>().ToTable("AspNetUserRoles");

            modelBuilder.Entity<ErrorLogs>().HasQueryFilter(e => e.TenantId == _tenantProvider.TenantId);
            modelBuilder.Entity<ErrorLogs>().Ignore(e => e.EntityId).HasKey(e => e.Id);
            modelBuilder.Entity<ErrorLogs>().Property(c => c.RowVersion).IsRowVersion();
            modelBuilder.Entity<ErrorLogs>().ToTable("cor_errorlog");

            modelBuilder.Entity<Roles>().HasMany(r => r.UserRoles).WithOne(ur => ur.Role).HasForeignKey(ur => ur.RoleId);
            modelBuilder.Entity<Roles>().Ignore(e => e.EntityId).HasKey(e => e.Id);
            modelBuilder.Entity<Roles>().Property(c => c.RowVersion).IsRowVersion();
            modelBuilder.Entity<Roles>().ToTable("cor_role");

            modelBuilder.Entity<Menu>().HasOne(m => m.ParentMenu).WithMany(m => m.SubMenus).HasForeignKey(m => m.ParentId).OnDelete(DeleteBehavior.Restrict);
            modelBuilder.Entity<Menu>().Ignore(e => e.EntityId).HasKey(e => e.MenuId);
            modelBuilder.Entity<Menu>().Property(c => c.RowVersion).IsRowVersion();
            modelBuilder.Entity<Menu>().ToTable("cor_menu");

            modelBuilder.Entity<MenuRole>().HasOne(cm => cm.Menu).WithMany().HasForeignKey(cm => cm.MenuId);
            modelBuilder.Entity<MenuRole>().Ignore(e => e.EntityId).HasKey(e => e.MenuRoleId);
            modelBuilder.Entity<MenuRole>().Property(c => c.RowVersion).IsRowVersion();
            modelBuilder.Entity<MenuRole>().ToTable("cor_menurole");

            modelBuilder.Entity<Module>().Ignore(e => e.EntityId).HasKey(e => e.ModuleId);
            modelBuilder.Entity<Module>().Property(c => c.RowVersion).IsRowVersion();
            modelBuilder.Entity<Module>().ToTable("cor_module");

            modelBuilder.Entity<Solution>().Ignore(e => e.EntityId).HasKey(e => e.SolutionId);
            modelBuilder.Entity<Solution>().Property(c => c.RowVersion).IsRowVersion();
            modelBuilder.Entity<Solution>().ToTable("cor_solution");

            //modelBuilder.Entity<UserSetup>().HasQueryFilter(e => e.TenantId == _tenantProvider.TenantId);
            modelBuilder.Entity<UserSetup>().HasMany(u => u.UserRoles).WithOne(ur => ur.User).HasForeignKey(ur => ur.UserId);
            modelBuilder.Entity<UserSetup>().Ignore(e => e.EntityId).HasKey(e => e.Id);
            modelBuilder.Entity<UserSetup>().Property(c => c.RowVersion).IsRowVersion();
            modelBuilder.Entity<UserSetup>().ToTable("cor_usersetup");

            modelBuilder.Entity<AuditLog>().HasQueryFilter(e => e.TenantId == _tenantProvider.TenantId);
            modelBuilder.Entity<AuditLog>().Ignore(e => e.EntityId).HasKey(e => e.Id);
            modelBuilder.Entity<AuditLog>().Property(c => c.RowVersion).IsRowVersion();
            modelBuilder.Entity<AuditLog>().ToTable("cor_audittrail");
            // Configure other entities...

        }

        public override int SaveChanges()
        {
            if (!_isAuditDisabled)
            {
                AddAudit();
                SetTenantId();
            }
            if (!string.IsNullOrEmpty(_tenantProvider.UserName))
                SetTenantId();

            return base.SaveChanges();
        }

        public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            if (!_isAuditDisabled)
            {
                await AddAudit();
                //SetTenantId();
            }
            if (!string.IsNullOrEmpty(_tenantProvider.UserName))
                SetTenantId();

            return await base.SaveChangesAsync(cancellationToken);
        }

        private void SetTenantId()
        {
            // Get entries that are being added or modified
            var entries = ChangeTracker.Entries<ITenantEntity>()
                .Where(e => e.State == EntityState.Added || e.State == EntityState.Modified || e.State == EntityState.Deleted)
                .ToList();

            foreach (var entry in entries)
            {
                if (!_createNewTenant)
                    entry.Entity.TenantId = _tenantProvider.TenantId;

                if (entry.State == EntityState.Added)
                {
                    //entry is added
                    entry.Entity.CreatedBy = _tenantProvider.UserName;
                    entry.Entity.CreatedOn = DateTime.Now;
                    entry.Entity.UpdatedBy = _tenantProvider.UserName;
                    entry.Entity.UpdatedOn = DateTime.Now;
                }
                else if(entry.State == EntityState.Deleted)
                {
                    //Do nothing for now
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