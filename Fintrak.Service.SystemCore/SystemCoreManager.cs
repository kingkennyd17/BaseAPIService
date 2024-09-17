using Fintrak.Data.Interface;
using Fintrak.Data.SystemCore.Interface;
using Fintrak.Model.SystemCore;
using Fintrak.Model.SystemCore.Common;
using Fintrak.Service.SystemCore.Interface;
using Fintrak.Service.SystemCore.Interface.DataModel;
using Fintrak.Shared.Common.Interface;
using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;

namespace Fintrak.Service.SystemCore
{
    public class SystemCoreManager : ISystemCoreManager
    {
        public SystemCoreManager()
        {
        }

        public SystemCoreManager(IDataRepositoryFactory dataRepositoryFactory)
        {
            _DataRepositoryFactory = dataRepositoryFactory;
        }

        //[Import]
        IDataRepositoryFactory _DataRepositoryFactory;



        #region ErrorLog
        public async Task LogErrorAsync(ErrorLogs errorLog)
        {
            using (var scope = new TransactionScope(TransactionScopeOption.Required, TransactionScopeAsyncFlowOption.Enabled))
            {
                IErrorLogRepository errorLogRepository = _DataRepositoryFactory.GetRepository<IErrorLogRepository>();
                await errorLogRepository.LogErrorAsync(errorLog);
                scope.Complete();
            }

        }

        public async Task<IEnumerable<ErrorLogs>> GetErrorLogsAsync(string? username, DateTime? startDate, DateTime? endDate)
        {
            IErrorLogRepository repository = _DataRepositoryFactory.GetRepository<IErrorLogRepository>();
            return await repository.GetErrorLogsAsync(username, startDate, endDate);
        }
        #endregion

        #region Role
        public async Task<IEnumerable<Roles>> GetRoles()
        {
            using (var scope = new TransactionScope(TransactionScopeOption.Required, TransactionScopeAsyncFlowOption.Enabled))
            {
                IRoleRepository roleRepository = _DataRepositoryFactory.GetRepository<IRoleRepository>();
                var roles = await roleRepository.GetAllAsync();
                scope.Complete();
                return roles;
            }
        }
        #endregion

        #region AuditLog
        public async Task LogAuditAsync(AuditLog entity)
        {
            using (var scope = new TransactionScope(TransactionScopeOption.Required, TransactionScopeAsyncFlowOption.Enabled))
            {
                IAudittrailRepository repository = _DataRepositoryFactory.GetRepository<IAudittrailRepository>();
                await repository.LogAuditAsync(entity);
                scope.Complete();
            }
        }

        public async Task<IEnumerable<AuditLog>> GetAuditLogsAsync(string? username, string? action, DateTime? startDate, DateTime? endDate)
        {
            IAudittrailRepository repository = _DataRepositoryFactory.GetRepository<IAudittrailRepository>();
            return await repository.GetAuditLogsAsync(username, action, startDate, endDate);
        }
        #endregion

        #region UserSetup
        public async Task UserSetupUpdateTokenAsync(UserSetup entity)
        {
            using (var scope = new TransactionScope(TransactionScopeOption.Required, TransactionScopeAsyncFlowOption.Enabled))
            {
                IUserSetupRepository repository = _DataRepositoryFactory.GetRepository<IUserSetupRepository>();
                await repository.UserSetupUpdateTokenAsync(entity);
                scope.Complete();
            }
        }

        public async Task<IEnumerable<UserSetup>> GetAllUserSetupByTenant()
        {
            using (var scope = new TransactionScope(TransactionScopeOption.Required, TransactionScopeAsyncFlowOption.Enabled))
            {
                IUserSetupRepository repository = _DataRepositoryFactory.GetRepository<IUserSetupRepository>();
                var result = await repository.GetAllUserSetupByTenant();
                scope.Complete();
                return result;
            }

        }
        #endregion

        #region Menu
        public async Task<IEnumerable<MenuData>> GetMenuByLoginID(string loginID)
        {
            IMenuRepository menuRepository = _DataRepositoryFactory.GetRepository<IMenuRepository>();

            List<MenuData> menus = new List<MenuData>();
            IEnumerable<MenuInfo> result = await menuRepository.GetMenuInfoByLoginID(loginID);
            var menuInfos = result.Where(c => c.Menu.Active).OrderBy(c => c.Menu.ModuleId).OrderBy(c => c.Menu.Position).ToArray();
            
            foreach (var menuInfo in menuInfos)
            {
                menus.Add(
                    new MenuData
                    {
                        MenuId = menuInfo.Menu.EntityId,
                        Name = menuInfo.Menu.Name,
                        Alias = menuInfo.Menu.Alias,
                        Action = menuInfo.Menu.Action,
                        ActionUrl = menuInfo.Menu.ActionUrl,
                        Image = menuInfo.Menu.Image,
                        ImageUrl = menuInfo.Menu.ImageUrl,
                        //ModuleId = menuInfo.Module.EntityId,
                        //ModuleName = menuInfo.Module.Alias,
                        ParentId = menuInfo.Parent != null ? menuInfo.Parent.EntityId : 0,
                        ParentName = menuInfo.Parent != null ? menuInfo.Parent.Alias : string.Empty,
                        Position = menuInfo.Menu.Position,
                        Active = menuInfo.Menu.Active
                    });
            }

            return menus.ToArray();
        }
        #endregion
    }
}