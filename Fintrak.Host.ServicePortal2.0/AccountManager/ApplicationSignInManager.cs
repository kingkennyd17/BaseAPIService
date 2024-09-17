using Fintrak.Data.SystemCore;
using Fintrak.Model.SystemCore;
using Fintrak.Model.SystemCore.Common;
using Fintrak.Model.SystemCore.Enum;
using Fintrak.Service.SystemCore;
using Fintrak.Service.SystemCore.Interface;
using Fintrak.Shared.Common.Helper;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;

namespace Fintrak.Host.ServicePortal2._0
{
    public class ApplicationSignInManager : SignInManager<UserSetup>
    {
        private readonly ISystemCoreManager _systemCoreManager;
        private readonly UserContextService _userContextService;

        public ApplicationSignInManager(
            UserManager<UserSetup> userManager,
            IHttpContextAccessor contextAccessor,
            IUserClaimsPrincipalFactory<UserSetup> claimsFactory,
            IOptions<IdentityOptions> optionsAccessor,
            ILogger<SignInManager<UserSetup>> logger,
            IAuthenticationSchemeProvider schemes,  // Add this parameter
            IUserConfirmation<UserSetup> confirmation,  // Add this parameter
            ISystemCoreManager systemCoreManager,
            UserContextService userContextService)
            : base(userManager, contextAccessor, claimsFactory, optionsAccessor, logger, schemes, confirmation)  // Pass parameters to the base constructor
        {
            _systemCoreManager = systemCoreManager;
            _userContextService = userContextService;
        }

        public override async Task SignInAsync(UserSetup user, AuthenticationProperties authenticationProperties, string authenticationMethod = null)
        {
            if (user.ApprovalStatus != ApprovalStatus.Approved)
            {
                return; // Or any custom response you want to give
            }
            await base.SignInAsync(user, authenticationProperties, authenticationMethod);

            var auditLog = new AuditLog
            {
                UserId = user.Id,
                UserName = user.UserName,
                Action = "Login",
                TableName = "AspNetUsers",
                Timestamp = DateTime.UtcNow,
                IpAddress = _userContextService.GetClientIpAddress(),
                UpdatedBy = user.UserName,
                CreatedBy = user.UserName,
                TenantId = user.TenantId
            };

            await _systemCoreManager.LogAuditAsync(auditLog);
            //await _context.SaveChangesAsync();
        }

        public override async Task<SignInResult> CheckPasswordSignInAsync(UserSetup user, string password, bool lockoutOnFailure)
        {
            if (user.ApprovalStatus != ApprovalStatus.Approved)
            {
                return SignInResult.NotAllowed; // Or any custom response you want to give
            }
            var result = await base.CheckPasswordSignInAsync(user, password, lockoutOnFailure);

            if (result.Succeeded)
            {
                var auditLog = new AuditLog
                {
                    UserId = user.Id,
                    UserName = user.UserName,
                    Action = "Login",
                    TableName = "AspNetUsers",
                    Timestamp = DateTime.UtcNow,
                    IpAddress = _userContextService.GetClientIpAddress(),
                    UpdatedBy = user.UserName,
                    CreatedBy = user.UserName,
                    TenantId = user.TenantId
                };

                await _systemCoreManager.LogAuditAsync(auditLog);
                //await _context.SaveChangesAsync();
            }

            return result;
        }

        public override async Task<SignInResult> PasswordSignInAsync(string userName, string password, bool isPersistent, bool lockoutOnFailure)
        {
            var user = await UserManager.FindByNameAsync(userName);

            if (user != null && user.ApprovalStatus != ApprovalStatus.Approved)
            {
                return SignInResult.NotAllowed;
            }

            return await base.PasswordSignInAsync(userName, password, isPersistent, lockoutOnFailure);
        }
    }

}
