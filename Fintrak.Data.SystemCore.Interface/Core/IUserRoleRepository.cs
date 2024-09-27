using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fintrak.Data.SystemCore.Interface
{
    public interface IUserRoleRepository
    {
        Task<IEnumerable<UserRoleInfo>> GetUserRoleInfo(string solutionName, string loginID, List<string> roleNames);
        Task<IEnumerable<UserRoleInfo>> GetUserRoleInfo();
    }
}
