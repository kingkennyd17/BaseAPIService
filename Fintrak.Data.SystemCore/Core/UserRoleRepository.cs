using Fintrak.Data.SystemCore.Interface;
using Fintrak.Model.SystemCore;
using Microsoft.EntityFrameworkCore;

namespace Fintrak.Data.SystemCore
{
    public class UserRoleRepository : DataRepositoryBase<UserRole>, IUserRoleRepository
    {
        public UserRoleRepository(SystemCoreDbContext context) : base(context)
        {

        }


        public async Task<IEnumerable<UserRoleInfo>> GetUserRoleInfo(string solutionName, string loginID, List<string> roleNames)
        {
            var query = from a in _context.UserRoleSet
                        join b in _context.UserSetupSet on a.UserId equals b.Id
                        join c in _context.RolesSet on a.RoleId equals c.Id
                        join d in _context.SolutionSet on c.SolutionId equals d.SolutionId
                        where d.Name == solutionName && b.UserName == loginID && roleNames.Contains(c.Name)
                        select new UserRoleInfo()
                        {
                            UserRole = a,
                            UserSetup = b,
                            Role = c,
                            Solution = d
                        };

            var result = await query.ToListAsync();
            return result;
        }

        public async Task<IEnumerable<UserRoleInfo>> GetUserRoleInfo()
        {
            var query = from a in _context.UserRoleSet
                        join b in _context.UserSetupSet on a.UserId equals b.Id
                        join c in _context.RolesSet on a.RoleId equals c.Id
                        join d in _context.SolutionSet on c.SolutionId equals d.SolutionId
                        select new UserRoleInfo()
                        {
                            UserRole = a,
                            UserSetup = b,
                            Role = c,
                            Solution = d
                        };

            var result = await query.ToListAsync();
            return result;
        }
    }
}
