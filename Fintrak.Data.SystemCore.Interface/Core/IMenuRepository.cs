using Fintrak.Model.SystemCore;
using Fintrak.Model.SystemCore.Common;
using Fintrak.Shared.Common.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fintrak.Data.SystemCore.Interface
{
    public interface IMenuRepository : IDataRepository<Menu>
    {
        Task<IEnumerable<MenuInfo>> GetMenuInfoByLoginID(string loginID);
        Task<IEnumerable<MenuInfo>> GetMenuInfo();
    }
}
