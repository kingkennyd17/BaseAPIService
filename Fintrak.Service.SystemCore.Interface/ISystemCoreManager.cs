using Fintrak.Model.SystemCore;
using Fintrak.Model.SystemCore.Common;
using Fintrak.Service.SystemCore.Interface.DataModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fintrak.Service.SystemCore.Interface
{
    public interface ISystemCoreManager
    {
        #region Roles
        Task<IEnumerable<Roles>> GetRoles();
        #endregion

        #region ErrorLog
        Task LogErrorAsync(ErrorLogs errorLog);
        Task<IEnumerable<ErrorLogs>> GetErrorLogsAsync(string? username, DateTime? startDate, DateTime? endDate);
        #endregion

        #region Audittrail
        Task LogAuditAsync(AuditLog entity);
        Task<IEnumerable<AuditLog>> GetAuditLogsAsync(string? username, string? action, DateTime? startDate, DateTime? endDate);
        #endregion

        #region UserSetup
        Task UserSetupUpdateTokenAsync(UserSetup entity);
        Task<IEnumerable<UserSetup>> GetAllUserSetupByTenant();
        #endregion

        #region Menu
        Task<IEnumerable<MenuData>> GetMenuByLoginID(string loginID);
        #endregion

    }
}
