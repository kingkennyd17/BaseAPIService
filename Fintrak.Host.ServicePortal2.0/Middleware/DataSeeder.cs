using Fintrak.Model.SystemCore;
using Fintrak.Model.SystemCore.Common;
using Fintrak.Model.SystemCore.Enum;
using Microsoft.AspNetCore.Identity;

namespace Fintrak.Host.ServicePortal2._0.Middleware
{
    public class DataSeeder
    {
        private readonly UserManager<UserSetup> _userManager;
        private readonly RoleManager<Roles> _roleManager;

        public DataSeeder(UserManager<UserSetup> userManager, RoleManager<Roles> roleManager)
        {
            _userManager = userManager;
            _roleManager = roleManager;
        }

        public async Task SeedAsync()
        {
            // Define roles with additional properties
            var roles = new[]
            {
            new { Name = "AdminMaker", Description = "Admin Maker Role", SolutionId = 1 },
            new { Name = "AdminChecker", Description = "Admin Checker Role", SolutionId = 1 },
            new { Name = "IFRSAdministrator", Description = "IFRS Administrator Role", SolutionId = 2 },
            new { Name = "FinstatAdministrator", Description = "Finstat Administrator Role", SolutionId = 3 },
            new { Name = "IFRSUser", Description = "IFRS User Role", SolutionId = 2 },
            new { Name = "FinstatUser", Description = "Finstat User Role", SolutionId = 3 },
            new { Name = "TenantAdmin", Description = "Role for creating New Tenant", SolutionId = 0 },
            };

            // Create roles if they do not exist
            foreach (var role in roles)
            {
                if (!await _roleManager.RoleExistsAsync(role.Name))
                {
                    await _roleManager.CreateAsync(new Roles() { Name = role.Name, Description = role.Description, SolutionId = role.SolutionId });
                }
            }

            // Create default users if they do not exist
            await CreateUserIfNotExists("fintrak", "fintrak@fintraksoftware.com", "@password", new[] { "AdminChecker", "AdminMaker", "TenantAdmin" }, "DEF", ApprovalStatus.Approved);
            await CreateUserIfNotExists("fintrakbusiness", "fintrakbusiness@fintraksoftware.com", "@password", new[] { "IFRSAdministrator", "FinstatAdministrator" }, "DEF", ApprovalStatus.Approved);
        }

        private async Task CreateUserIfNotExists(string username, string email, string password, string[] roles, string tenantId, ApprovalStatus status)
        {
            if (await _userManager.FindByNameAsync(username) == null)
            {
                var user = new UserSetup()
                {
                    UserName = username,
                    Email = email,
                    EmailConfirmed = true,
                    TenantId = tenantId,
                    ApprovalStatus = status
                };

                var result = await _userManager.CreateAsync(user, password);
                if (result.Succeeded)
                {
                    await _userManager.AddToRolesAsync(user, roles);
                }
                else
                {
                    throw new Exception($"Failed to create user {username}: {string.Join(", ", result.Errors.Select(e => e.Description))}");
                }
            }
        }
    }

}
