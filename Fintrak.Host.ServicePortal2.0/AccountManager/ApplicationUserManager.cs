using Fintrak.Data.SystemCore;
using Fintrak.Model.SystemCore;
using Fintrak.Model.SystemCore.Common;
using Fintrak.Service.SystemCore;
using Fintrak.Service.SystemCore.Interface;
using Fintrak.Shared.Common.Helper;
//using Microsoft.AspNet.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;

namespace Fintrak.Host.ServicePortal2._0
{
    public class ApplicationUserManager : UserManager<UserSetup>
    {
        private readonly ISystemCoreManager _systemCoreManager;
        private readonly SystemCoreDbContext _context;
        private readonly UserContextService _userContextService;

        public ApplicationUserManager(
            IUserStore<UserSetup> store,
            IOptions<IdentityOptions> optionsAccessor,
            IPasswordHasher<UserSetup> passwordHasher,
            IEnumerable<IUserValidator<UserSetup>> userValidators,
            IEnumerable<IPasswordValidator<UserSetup>> passwordValidators,
            ILookupNormalizer keyNormalizer,
            IdentityErrorDescriber errors,
            IServiceProvider services,
            ILogger<UserManager<UserSetup>> logger,
            ISystemCoreManager systemCoreManager,
            SystemCoreDbContext context,
            UserContextService userContextService)
            : base(store, optionsAccessor, passwordHasher, userValidators, passwordValidators, keyNormalizer, errors, services, logger)
        {
            _systemCoreManager = systemCoreManager;
            _userContextService = userContextService;
            _context = context;
        }

        public override async Task<IdentityResult> AddToRoleAsync(UserSetup user, string roleName)
        {
            _context.DisableAuditLogging();
            var result = await base.AddToRoleAsync(user, roleName);

            if (result.Succeeded)
            {
                var auditLog = new AuditLog
                {
                    UserId = _userContextService.GetCurrentUserId(),
                    UserName = _userContextService.GetCurrentUserName(),
                    Action = "AddRole",
                    TableName = "AspNetUserRoles",
                    NewValues = $"Added Role: {roleName} to {user.UserName}",
                    Timestamp = DateTime.UtcNow,
                    IpAddress = _userContextService.GetClientIpAddress(),
                    TenantId = user.TenantId
                };

                await _systemCoreManager.LogAuditAsync(auditLog);
                //await _context.SaveChangesAsync();
            }

            return result;
        }

        public override async Task<IdentityResult> RemoveFromRoleAsync(UserSetup user, string roleName)
        {
            _context.DisableAuditLogging();
            var result = await base.RemoveFromRoleAsync(user, roleName);

            if (result.Succeeded)
            {
                var auditLog = new AuditLog
                {
                    UserId = _userContextService.GetCurrentUserId(),
                    UserName = _userContextService.GetCurrentUserName(),
                    Action = "RemoveRole",
                    TableName = "AspNetUserRoles",
                    NewValues = $"Removed Role: {roleName} from {user.UserName}",
                    Timestamp = DateTime.UtcNow,
                    IpAddress = _userContextService.GetClientIpAddress(),
                    TenantId = user.TenantId
                };

                await _systemCoreManager.LogAuditAsync(auditLog);
                //await _context.SaveChangesAsync();
            }

            return result;
        }

        public override async Task<IdentityResult> AddToRolesAsync(UserSetup user, IEnumerable<string> roleNames)
        {
            _context.DisableAuditLogging();
            var result = await base.AddToRolesAsync(user, roleNames);

            if (result.Succeeded)
            {
                foreach (var roleName in roleNames)
                {
                    var auditLog = new AuditLog
                    {
                        UserId = user.Id,
                        UserName = user.UserName,
                        Action = "AddRole",
                        TableName = "AspNetUserRoles",
                        NewValues = $"Added Role: {roleName} to {user.UserName}",
                        Timestamp = DateTime.UtcNow,
                        IpAddress = _userContextService.GetClientIpAddress(),
                        TenantId = user.TenantId
                    };

                    await _systemCoreManager.LogAuditAsync(auditLog);
                }

                //await _context.SaveChangesAsync();
            }

            return result;
        }
        public override async Task<IdentityResult> RemoveFromRolesAsync(UserSetup user, IEnumerable<string> roleNames)
        {
            _context.DisableAuditLogging();
            var result = await base.AddToRolesAsync(user, roleNames);

            if (result.Succeeded)
            {
                foreach (var roleName in roleNames)
                {
                    var auditLog = new AuditLog
                    {
                        UserId = _userContextService.GetCurrentUserId(),
                        UserName = _userContextService.GetCurrentUserName(),
                        Action = "RemoveRole",
                        TableName = "AspNetUserRoles",
                        NewValues = $"Removed Role: {roleName} from {user.UserName}",
                        Timestamp = DateTime.UtcNow,
                        IpAddress = _userContextService.GetClientIpAddress(),
                        TenantId = user.TenantId
                    };

                    await _systemCoreManager.LogAuditAsync(auditLog);
                }

                //await _context.SaveChangesAsync();
            }
            return result;
        }

        public override async Task<IdentityResult> CreateAsync(UserSetup user, string password)
        {
            _context.DisableAuditLogging();
            if (!string.IsNullOrEmpty(user.TenantId))
                _context.CreateNewTenant();
            var result = await base.CreateAsync(user, password);

            if (result.Succeeded)
            {
                var auditLog = new AuditLog
                {
                    UserId = _userContextService.GetCurrentUserId() ?? user.Id,
                    UserName = _userContextService.GetCurrentUserName() ?? user.UserName,
                    Action = "Create User",
                    TableName = "cor_usersetup",
                    NewValues = $"Created User: {user.UserName}",
                    Timestamp = DateTime.UtcNow,
                    IpAddress = _userContextService.GetClientIpAddress(),
                    TenantId = user.TenantId
                };

                await _systemCoreManager.LogAuditAsync(auditLog);
                //await _context.SaveChangesAsync();
            }

            return result;
        }
    }

}
