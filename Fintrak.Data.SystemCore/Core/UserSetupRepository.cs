using Fintrak.Data.SystemCore.Interface;
using Fintrak.Model.SystemCore;
using Fintrak.Model.SystemCore.Common;
using Fintrak.Shared.Common.Base;
using Fintrak.Shared.Common.Tenancy;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Fintrak.Data.SystemCore
{
    public class UserSetupRepository : DataRepositoryBase<UserSetup>, IUserSetupRepository
    {
        private readonly ITenantProvider _tenantProvider;
        public UserSetupRepository(SystemCoreDbContext context, ITenantProvider tenantProvider) : base(context)
        {
            _tenantProvider = tenantProvider;
        }

        public async Task UserSetupUpdateTokenAsync(UserSetup entity)
        {
            _context.DisableAuditLogging();
            _context.Set<UserSetup>().Update(entity);
            await _context.SaveChangesAsync();
        }

        public async Task<IEnumerable<UserSetup>> GetAllUserSetupByTenant()
        {
            var result = await _context.Set<UserSetup>().ToListAsync();
            return result.Where(e => e.TenantId == _tenantProvider.TenantId);
        }
    }
}
