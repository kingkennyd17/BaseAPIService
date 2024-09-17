using Fintrak.Data.SystemCore;
using Fintrak.Model.SystemCore;
using Fintrak.Model.SystemCore.Common;
using Fintrak.Service.SystemCore;
using Fintrak.Service.SystemCore.Interface;
using Fintrak.Shared.Common.Helper;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;

namespace Fintrak.Host.ServicePortal2._0
{
    public class ApplicationRoleManager : RoleManager<Roles>
    {
        private readonly SystemCoreDbContext _context;
        private readonly UserContextService _userContextService;

        public ApplicationRoleManager(
            IRoleStore<Roles> store,
            IEnumerable<IRoleValidator<Roles>> roleValidators,
            ILookupNormalizer keyNormalizer,
            IdentityErrorDescriber errors,
            ILogger<RoleManager<Roles>> logger,
            SystemCoreDbContext context,
            UserContextService userContextService)
            : base(store, roleValidators, keyNormalizer, errors, logger)
        {
            _context = context;
            _userContextService = userContextService;
        }

        public override async Task<IdentityResult> CreateAsync(Roles role)
        {
            _context.DisableAuditLogging();
            var result = await base.CreateAsync(role);
            return result;
        }
    }


}
