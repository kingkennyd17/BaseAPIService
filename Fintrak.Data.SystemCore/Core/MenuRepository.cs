using Fintrak.Data.Interface;
using Fintrak.Data.SystemCore.Interface;
using Fintrak.Model.SystemCore;
using Fintrak.Model.SystemCore.Common;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Fintrak.Data.SystemCore
{
    public class MenuRepository : DataRepositoryBase<Menu>, IMenuRepository
    {
        private readonly SystemCoreDbContext _context;
        public MenuRepository(SystemCoreDbContext context) : base(context)
        {
            _context = context;
        }

        public async Task<IEnumerable<MenuInfo>> GetMenuInfoByLoginID(string loginID)
        {
            var roleQuery = from a in _context.MenuRoleSet
                            join b in _context.UserRoleSet on a.RoleId equals b.RoleId
                            join c in _context.UserSetupSet on b.UserId equals c.Id
                            where c.UserName == loginID
                            select a;

            var menuIds = await roleQuery.Select(c => c.MenuId).Distinct().ToListAsync();

            var query = from a in _context.MenuSet
                        join c in _context.MenuSet on a.ParentId equals c.MenuId into parents
                        from pt in parents.DefaultIfEmpty()

                        where a.Active && menuIds.Contains(a.MenuId)
                        select new MenuInfo()
                        {
                            Menu = a,
                            Parent = pt
                        };

            var result = await query.ToListAsync();
            return result;
        }

        public async Task<IEnumerable<MenuInfo>> GetMenuInfo()
        {
            var query = from a in _context.MenuSet
                        join c in _context.MenuSet on a.ParentId equals c.MenuId into parents
                        from pt in parents.DefaultIfEmpty()
                        select new MenuInfo()
                        {
                            Menu = a,
                            Parent = pt
                        };

            var result = await query.ToListAsync();
            return result;
        }
    }
}
