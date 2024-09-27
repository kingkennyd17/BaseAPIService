using Fintrak.Data.Core.Interface;
using Fintrak.Data.SystemCore.Interface;
using Fintrak.Host.ServicePortal2._0.Models;
using Fintrak.Model.Core;
using Fintrak.Model.Core.Enum;
using Fintrak.Model.SystemCore;
using Fintrak.Model.SystemCore.Common;
using Fintrak.Service.Core.Interface;
using Fintrak.Shared.Common.Interface;
using Fintrak.Shared.Common.ServiceModel;
using Fintrak.Shared.Common.Utils;
using Microsoft.AspNetCore.Http;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using System.ServiceModel;
using System.ServiceProcess;
using System.Configuration.Install;
using System.Transactions;
using static Fintrak.Shared.Common.CustomErrorTypes;
using Fintrak.Shared.Common.Data;

namespace Fintrak.Service.Core
{
    public class ExtractionProcessManager : ManagerBase, ICoreManager
    {
        private readonly string _connectionString;
        private readonly IConfiguration _configuration;
        public ExtractionProcessManager(IDataRepositoryFactory dataRepositoryFactory, IConfiguration configuration,
            IHttpContextAccessor httpContextAccessor) : base(httpContextAccessor)
        {
            _DataRepositoryFactory = dataRepositoryFactory;
            _configuration = configuration;
            _connectionString = configuration.GetConnectionString("DefaultConnection");
        }

        IDataRepositoryFactory _DataRepositoryFactory;


        #region Extraction operations
        public async Task UpdateExtraction(Extraction extraction)
        {
            using (var scope = new TransactionScope(TransactionScopeOption.Required, TransactionScopeAsyncFlowOption.Enabled))
            {
                IExtractionRepository extractionRepository = _DataRepositoryFactory.GetRepository<IExtractionRepository>();

                if (extraction.ExtractionId == 0)
                    await extractionRepository.AddAsync(extraction);
                else
                    await extractionRepository.UpdateAsync(extraction);

                scope.Complete();
            }
        }

        public async Task DeleteExtraction(int extractionId)
        {
            using (var scope = new TransactionScope(TransactionScopeOption.Required, TransactionScopeAsyncFlowOption.Enabled))
            {
                IExtractionRepository extractionRepository = _DataRepositoryFactory.GetRepository<IExtractionRepository>();

                await extractionRepository.DeleteAsync(extractionId);

                scope.Complete();
            }
        }

        public async Task<Extraction> GetExtraction(int extractionId)
        {
            IExtractionRepository extractionRepository = _DataRepositoryFactory.GetRepository<IExtractionRepository>();

            Extraction extractionEntity = await extractionRepository.GetByIdAsync(extractionId);
            return extractionEntity;
        }

        public async Task<IEnumerable<Extraction>> GetAllExtractions()
        {
            IExtractionRepository extractionRepository = _DataRepositoryFactory.GetRepository<IExtractionRepository>();

            IEnumerable<Extraction> extractions = await extractionRepository.GetAllAsync();

            return extractions.OrderBy(c => c.Position).ToArray();
        }

        public async Task<IEnumerable<ExtractionData>> GetExtractions()
        {
            IExtractionRepository extractionRepository = _DataRepositoryFactory.GetRepository<IExtractionRepository>();

            var solutions = await GetSolutions();
            var solutionIds = solutions.Where(c => c.Active).Select(c => c.EntityId).Distinct();

            List<ExtractionData> extractionDatas = new List<ExtractionData>();
            IEnumerable<Extraction> extractions = await extractionRepository.GetAllAsync();
            extractions = extractions.Where(c => solutionIds.Contains(c.SolutionId)).OrderBy(c => c.Position).ToArray();

            foreach (var extractionInfo in extractions)
            {
                extractionDatas.Add(
                    new ExtractionData
                    {
                        ExtractionId = extractionInfo.EntityId,
                        Title = extractionInfo.Title,
                        RunType = extractionInfo.RunType,
                        RunTypeName = extractionInfo.RunType.ToString(),
                        PackageName = extractionInfo.PackageName,
                        PackagePath = extractionInfo.PackagePath,
                        ProcedureName = extractionInfo.ProcedureName,
                        ScriptText = extractionInfo.ScriptText,
                        NeedArchiveAction = extractionInfo.NeedArchiveAction,
                        SolutionId = extractionInfo.SolutionId,
                        Position = extractionInfo.Position,
                        SolutionName = GetSolutionName(solutions, extractionInfo.SolutionId),
                        Active = extractionInfo.Active
                    });
            }

            return extractionDatas.ToArray();
        }

        public async Task<IEnumerable<ExtractionData>> GetExtractionByLogin(string loginID)
        {
            IExtractionRepository extractionRepository = _DataRepositoryFactory.GetRepository<IExtractionRepository>();
            IExtractionRoleRepository extractionRoleRepository = _DataRepositoryFactory.GetRepository<IExtractionRoleRepository>();
            IUserRoleRepository userRoleRepository = _DataRepositoryFactory.GetRepository<IUserRoleRepository>();

            var roleIds = (await userRoleRepository.GetUserRoleInfo()).Where(c => c.UserSetup.UserName == loginID).Select(c => c.Role.Id).Distinct();

            var extractionIds = (await extractionRoleRepository.GetAllAsync()).Where(c => roleIds.Contains(c.RoleId)).Select(c => c.ExtractionId).Distinct();

            var solutions = await GetSolutions();
            var solutionIds = solutions.Where(c => c.Active).Select(c => c.EntityId).Distinct();

            List<ExtractionData> extractions = new List<ExtractionData>();
            IEnumerable<Extraction> extractionInfos = (await extractionRepository.GetAllAsync()).Where(c => extractionIds.Contains(c.ExtractionId) && solutionIds.Contains(c.SolutionId)).OrderBy(c => c.Position).ToArray();

            foreach (var extractionInfo in extractionInfos)
            {
                extractions.Add(
                    new ExtractionData
                    {
                        ExtractionId = extractionInfo.EntityId,
                        Title = extractionInfo.Title,
                        RunType = extractionInfo.RunType,
                        RunTypeName = extractionInfo.RunType.ToString(),
                        PackageName = extractionInfo.PackageName,
                        PackagePath = extractionInfo.PackagePath,
                        ProcedureName = extractionInfo.ProcedureName,
                        ScriptText = extractionInfo.ScriptText,
                        NeedArchiveAction = extractionInfo.NeedArchiveAction,
                        SolutionId = extractionInfo.SolutionId,
                        Position = extractionInfo.Position,
                        SolutionName = GetSolutionName(solutions, extractionInfo.SolutionId),
                        Active = extractionInfo.Active
                    });
            }

            return extractions.ToArray();
        }

        public async Task<IEnumerable<ExtractionData>> GetExtractionBySolution(int solutionId, string loginID)
        {
            IExtractionRepository extractionRepository = _DataRepositoryFactory.GetRepository<IExtractionRepository>();
            IExtractionRoleRepository extractionRoleRepository = _DataRepositoryFactory.GetRepository<IExtractionRoleRepository>();
            IUserRoleRepository userRoleRepository = _DataRepositoryFactory.GetRepository<IUserRoleRepository>();

            var roleIds = (await userRoleRepository.GetUserRoleInfo()).Where(c => c.UserSetup.UserName == loginID).Select(c => c.Role.Id).Distinct();

            var extractionIds = (await extractionRoleRepository.GetAllAsync()).Where(c => roleIds.Contains(c.RoleId)).Select(c => c.ExtractionId).Distinct();

            var solutions = await GetSolutions();
            var solutionIds = solutions.Where(c => c.SolutionId == solutionId && c.Active).Select(c => c.EntityId).Distinct();

            List<ExtractionData> extractions = new List<ExtractionData>();
            IEnumerable<Extraction> extractionInfos = (await extractionRepository.GetAllAsync()).Where(c => extractionIds.Contains(c.ExtractionId) && solutionIds.Contains(c.SolutionId)).OrderBy(c => c.Position).ToArray();

            foreach (var extractionInfo in extractionInfos)
            {
                extractions.Add(
                    new ExtractionData
                    {
                        ExtractionId = extractionInfo.EntityId,
                        Title = extractionInfo.Title,
                        RunType = extractionInfo.RunType,
                        RunTypeName = extractionInfo.RunType.ToString(),
                        PackageName = extractionInfo.PackageName,
                        PackagePath = extractionInfo.PackagePath,
                        ProcedureName = extractionInfo.ProcedureName,
                        ScriptText = extractionInfo.ScriptText,
                        NeedArchiveAction = extractionInfo.NeedArchiveAction,
                        SolutionId = extractionInfo.SolutionId,
                        Position = extractionInfo.Position,
                        SolutionName = GetSolutionName(solutions, extractionInfo.SolutionId),
                        Active = extractionInfo.Active
                    });
            }

            return extractions.ToArray();
        }

        #endregion

        #region ExtractionRole operations
        public async Task UpdateExtractionRole(ExtractionRole extractionRole)
        {
            using (var scope = new TransactionScope(TransactionScopeOption.Required, TransactionScopeAsyncFlowOption.Enabled))
            {
                IExtractionRoleRepository extractionRoleRepository = _DataRepositoryFactory.GetRepository<IExtractionRoleRepository>();

                if (extractionRole.ExtractionId == 0)
                    await extractionRoleRepository.AddAsync(extractionRole);
                else
                    await extractionRoleRepository.UpdateAsync(extractionRole);

                scope.Complete();
            }
        }

        public async Task DeleteExtractionRole(int extractionRoleId)
        {
            using (var scope = new TransactionScope(TransactionScopeOption.Required, TransactionScopeAsyncFlowOption.Enabled))
            {
                IExtractionRoleRepository extractionRoleRepository = _DataRepositoryFactory.GetRepository<IExtractionRoleRepository>();

                await extractionRoleRepository.DeleteAsync(extractionRoleId);

                scope.Complete();
            }            
        }

        public async Task<ExtractionRole> GetExtractionRole(int extractionRoleId)
        {
            IExtractionRoleRepository extractionRoleRepository = _DataRepositoryFactory.GetRepository<IExtractionRoleRepository>();

            ExtractionRole extractionRoleEntity = await extractionRoleRepository.GetByIdAsync(extractionRoleId);

            return extractionRoleEntity;
        }

        public async Task<IEnumerable<ExtractionRole>> GetAllExtractionRoles()
        {
            IExtractionRoleRepository extractionRoleRepository = _DataRepositoryFactory.GetRepository<IExtractionRoleRepository>();

            IEnumerable<ExtractionRole> extractionRoles = await extractionRoleRepository.GetAllAsync();

            return extractionRoles.ToArray();
        }

        public async Task<IEnumerable<ExtractionRoleData>> GetExtractionRoles()
        {
            IExtractionRoleRepository extractionRoleRepository = _DataRepositoryFactory.GetRepository<IExtractionRoleRepository>();

            var solutions = await GetSolutions();
            var solutionIds = solutions.Where(c => c.Active).Select(c => c.EntityId).Distinct();

            var roles = await GetRoles();

            List<ExtractionRoleData> extractionRoles = new List<ExtractionRoleData>();
            IEnumerable<ExtractionRoleInfo> extractionRoleInfos = (await extractionRoleRepository.GetExtractionRoles()).Where(c => solutionIds.Contains(c.Extraction.SolutionId)).ToArray();

            foreach (var extractionRoleInfo in extractionRoleInfos)
            {
                extractionRoles.Add(
                    new ExtractionRoleData
                    {
                        ExtractionRoleId = extractionRoleInfo.ExtractionRole.EntityId,
                        RoleId = extractionRoleInfo.ExtractionRole.RoleId,
                        RoleName = GetRoleName(roles, extractionRoleInfo.ExtractionRole.RoleId),
                        ExtractionId = extractionRoleInfo.Extraction.EntityId,
                        ExtractionName = extractionRoleInfo.Extraction.Title,
                        SolutionId = extractionRoleInfo.Extraction.SolutionId,
                        SolutionName = GetSolutionName(solutions, extractionRoleInfo.Extraction.SolutionId),
                        Active = extractionRoleInfo.ExtractionRole.Active
                    });
            }

            return extractionRoles.ToArray();
        }

        public async Task<IEnumerable<ExtractionRoleData>> GetExtractionRoleByExtraction(int extractionId)
        {
            IExtractionRoleRepository extractionRoleRepository = _DataRepositoryFactory.GetRepository<IExtractionRoleRepository>();

            ISolutionRepository solutionRepository = _DataRepositoryFactory.GetRepository<ISolutionRepository>();

            var solutions = await GetSolutions();
            var solutionIds = solutions.Where(c => c.Active).Select(c => c.EntityId).Distinct();

            var roles = await GetRoles();

            List<ExtractionRoleData> extractionRoles = new List<ExtractionRoleData>();
            IEnumerable<ExtractionRoleInfo> extractionRoleInfos = (await extractionRoleRepository.GetExtractionRoleByExtraction(extractionId)).Where(c => solutionIds.Contains(c.Extraction.SolutionId)).ToArray();

            foreach (var extractionRoleInfo in extractionRoleInfos)
            {
                extractionRoles.Add(
                    new ExtractionRoleData
                    {
                        ExtractionRoleId = extractionRoleInfo.ExtractionRole.EntityId,
                        RoleId = extractionRoleInfo.ExtractionRole.RoleId,
                        RoleName = GetRoleName(roles, extractionRoleInfo.ExtractionRole.RoleId),
                        ExtractionId = extractionRoleInfo.Extraction.EntityId,
                        ExtractionName = extractionRoleInfo.Extraction.Title,
                        SolutionId = extractionRoleInfo.Extraction.SolutionId,
                        SolutionName = GetSolutionName(solutions, extractionRoleInfo.Extraction.SolutionId),
                        LongDescription = GetRoleName(roles, extractionRoleInfo.ExtractionRole.RoleId) + ' ' + GetSolutionName(solutions, extractionRoleInfo.Extraction.SolutionId),
                        Active = extractionRoleInfo.ExtractionRole.Active
                    });
            }

            return extractionRoles.ToArray();
        }

        #endregion

        #region ExtractionTrigger operations

        public async Task UpdateExtractionTrigger(ExtractionTrigger extractionTrigger)
        {
            using (var scope = new TransactionScope(TransactionScopeOption.Required, TransactionScopeAsyncFlowOption.Enabled))
            {
                IExtractionTriggerRepository extractionTriggerRepository = _DataRepositoryFactory.GetRepository<IExtractionTriggerRepository>();

                if (extractionTrigger.ExtractionTriggerId == 0)
                    await extractionTriggerRepository.AddAsync(extractionTrigger);
                else
                    await extractionTriggerRepository.UpdateAsync(extractionTrigger);

                scope.Complete();
            }
        }

        public async Task DeleteExtractionTrigger(int extractionTriggerId)
        {
            using (var scope = new TransactionScope(TransactionScopeOption.Required, TransactionScopeAsyncFlowOption.Enabled))
            {
                IExtractionTriggerRepository extractionTriggerRepository = _DataRepositoryFactory.GetRepository<IExtractionTriggerRepository>();

                await extractionTriggerRepository.DeleteAsync(extractionTriggerId);

                scope.Complete();
            }
        }

        public async Task<ExtractionTrigger> GetExtractionTrigger(int extractionTriggerId)
        {
            IExtractionTriggerRepository extractionTriggerRepository = _DataRepositoryFactory.GetRepository<IExtractionTriggerRepository>();

            ExtractionTrigger extractionTriggerEntity = await extractionTriggerRepository.GetByIdAsync(extractionTriggerId);

            return extractionTriggerEntity;
        }

        public async Task<IEnumerable<ExtractionTrigger>> GetAllExtractionTriggers()
        {
            IExtractionTriggerRepository extractionTriggerRepository = _DataRepositoryFactory.GetRepository<IExtractionTriggerRepository>();

            IEnumerable<ExtractionTrigger> extractionTriggers = await extractionTriggerRepository.GetAllAsync();

            return extractionTriggers.ToArray();
        }

        public async Task<IEnumerable<ExtractionTriggerData>> GetExtractionTriggers()
        {
            IExtractionTriggerRepository extractionTriggerRepository = _DataRepositoryFactory.GetRepository<IExtractionTriggerRepository>();

            List<ExtractionTriggerData> extractionTriggers = new List<ExtractionTriggerData>();
            IEnumerable<ExtractionTriggerInfo> extractionTriggerInfos = (await extractionTriggerRepository.GetExtractionTriggers()).OrderBy(c => c.Extraction.Title).ToArray();

            foreach (var extractionTriggerInfo in extractionTriggerInfos)
            {
                extractionTriggers.Add(
                    new ExtractionTriggerData
                    {
                        ExtractionId = extractionTriggerInfo.Extraction.EntityId,
                        ExtractionTitle = extractionTriggerInfo.Extraction.Title,
                        Code = extractionTriggerInfo.ExtractionTrigger.Code,
                        Status = extractionTriggerInfo.ExtractionTrigger.Status,
                        StatusName = extractionTriggerInfo.ExtractionTrigger.Status.ToString(),
                        Remark = extractionTriggerInfo.ExtractionTrigger.Remark,
                        UserName = extractionTriggerInfo.ExtractionTrigger.UserName,
                        StartDate = extractionTriggerInfo.ExtractionTrigger.StartDate,
                        EndDate = extractionTriggerInfo.ExtractionTrigger.EndDate,
                        RunTime = extractionTriggerInfo.ExtractionTrigger.RunTime,
                        NeedToArchive = extractionTriggerInfo.ExtractionTrigger.NeedToArchive,
                        Active = extractionTriggerInfo.ExtractionTrigger.Active
                    });
            }

            return extractionTriggers.ToArray();
        }

        public async Task<IEnumerable<ExtractionTriggerData>> GetExtractionTriggerByExtraction(int extractionId)
        {
            IExtractionTriggerRepository extractionTriggerRepository = _DataRepositoryFactory.GetRepository<IExtractionTriggerRepository>();


            List<ExtractionTriggerData> extractionTriggers = new List<ExtractionTriggerData>();
            IEnumerable<ExtractionTriggerInfo> extractionTriggerInfos = (await extractionTriggerRepository.GetExtractionTriggerByExtraction(extractionId)).OrderByDescending(c => c.ExtractionTrigger.ExtractionTriggerId).ToArray();

            foreach (var extractionTriggerInfo in extractionTriggerInfos)
            {
                extractionTriggers.Add(
                    new ExtractionTriggerData
                    {
                        ExtractionId = extractionTriggerInfo.Extraction.EntityId,
                        ExtractionTitle = extractionTriggerInfo.Extraction.Title,
                        Code = extractionTriggerInfo.ExtractionTrigger.Code,
                        Status = extractionTriggerInfo.ExtractionTrigger.Status,
                        StatusName = extractionTriggerInfo.ExtractionTrigger.Status.ToString(),
                        Remark = extractionTriggerInfo.ExtractionTrigger.Remark,
                        UserName = extractionTriggerInfo.ExtractionTrigger.UserName,
                        StartDate = extractionTriggerInfo.ExtractionTrigger.StartDate,
                        EndDate = extractionTriggerInfo.ExtractionTrigger.EndDate,
                        RunTime = extractionTriggerInfo.ExtractionTrigger.RunTime,
                        NeedToArchive = extractionTriggerInfo.ExtractionTrigger.NeedToArchive,
                        Active = extractionTriggerInfo.ExtractionTrigger.Active
                    });
            }

            return extractionTriggers.ToArray();
        }

        public async Task<IEnumerable<ExtractionTriggerData>> GetExtractionTriggerByJob(string jobCode)
        {
            IExtractionTriggerRepository extractionTriggerRepository = _DataRepositoryFactory.GetRepository<IExtractionTriggerRepository>();

            List<ExtractionTriggerData> extractionTriggers = new List<ExtractionTriggerData>();
            IEnumerable<ExtractionTriggerInfo> extractionTriggerInfos = (await extractionTriggerRepository.GetExtractionTriggerByJob(jobCode)).OrderByDescending(c => c.ExtractionTrigger.ExtractionTriggerId).ToArray();

            foreach (var extractionTriggerInfo in extractionTriggerInfos)
            {
                extractionTriggers.Add(
                    new ExtractionTriggerData
                    {
                        ExtractionId = extractionTriggerInfo.Extraction.EntityId,
                        ExtractionTitle = extractionTriggerInfo.Extraction.Title,
                        Code = extractionTriggerInfo.ExtractionTrigger.Code,
                        Status = extractionTriggerInfo.ExtractionTrigger.Status,
                        StatusName = extractionTriggerInfo.ExtractionTrigger.Status.ToString(),
                        Remark = extractionTriggerInfo.ExtractionTrigger.Remark,
                        UserName = extractionTriggerInfo.ExtractionTrigger.UserName,
                        StartDate = extractionTriggerInfo.ExtractionTrigger.StartDate,
                        EndDate = extractionTriggerInfo.ExtractionTrigger.EndDate,
                        RunTime = extractionTriggerInfo.ExtractionTrigger.RunTime,
                        NeedToArchive = extractionTriggerInfo.ExtractionTrigger.NeedToArchive,
                        Active = extractionTriggerInfo.ExtractionTrigger.Active
                    });
            }

            return extractionTriggers.ToArray();
        }

        public async Task<IEnumerable<ExtractionTriggerData>> GetExtractionTriggerByRunDate(DateTime startDate, DateTime endDate)
        {
            IExtractionTriggerRepository extractionTriggerRepository = _DataRepositoryFactory.GetRepository<IExtractionTriggerRepository>();

            List<ExtractionTriggerData> extractionTriggers = new List<ExtractionTriggerData>();
            IEnumerable<ExtractionTriggerInfo> extractionTriggerInfos = (await extractionTriggerRepository.GetExtractionTriggerByRunDate(startDate, endDate)).Where(c => c.ExtractionTrigger.Status != PackageStatus.Cancel).OrderByDescending(c => c.ExtractionTrigger.ExtractionTriggerId).ToArray();

            foreach (var extractionTriggerInfo in extractionTriggerInfos)
            {
                extractionTriggers.Add(
                    new ExtractionTriggerData
                    {
                        ExtractionId = extractionTriggerInfo.Extraction.EntityId,
                        ExtractionTitle = extractionTriggerInfo.Extraction.Title,
                        Code = extractionTriggerInfo.ExtractionTrigger.Code,
                        Status = extractionTriggerInfo.ExtractionTrigger.Status,
                        StatusName = extractionTriggerInfo.ExtractionTrigger.Status.ToString(),
                        Remark = extractionTriggerInfo.ExtractionTrigger.Remark,
                        UserName = extractionTriggerInfo.ExtractionTrigger.UserName,
                        StartDate = extractionTriggerInfo.ExtractionTrigger.StartDate,
                        EndDate = extractionTriggerInfo.ExtractionTrigger.EndDate,
                        RunTime = extractionTriggerInfo.ExtractionTrigger.RunTime,
                        NeedToArchive = extractionTriggerInfo.ExtractionTrigger.NeedToArchive,
                        Active = extractionTriggerInfo.ExtractionTrigger.Active
                    });
            }

            return extractionTriggers.ToArray();
        }

        public async Task<IEnumerable<ExtractionTriggerData>> GetExtractionTriggerByRunTime(DateTime runTime)
        {
            IExtractionTriggerRepository extractionTriggerRepository = _DataRepositoryFactory.GetRepository<IExtractionTriggerRepository>();
                
            List<ExtractionTriggerData> extractionTriggers = new List<ExtractionTriggerData>();
            IEnumerable<ExtractionTriggerInfo> extractionTriggerInfos = (await extractionTriggerRepository.GetExtractionTriggerByRunTime(runTime)).OrderBy(c => c.Extraction.Title).ToArray();

            foreach (var extractionTriggerInfo in extractionTriggerInfos)
            {
                extractionTriggers.Add(
                    new ExtractionTriggerData
                    {
                        ExtractionId = extractionTriggerInfo.Extraction.EntityId,
                        ExtractionTitle = extractionTriggerInfo.Extraction.Title,
                        Code = extractionTriggerInfo.ExtractionTrigger.Code,
                        Status = extractionTriggerInfo.ExtractionTrigger.Status,
                        StatusName = extractionTriggerInfo.ExtractionTrigger.Status.ToString(),
                        Remark = extractionTriggerInfo.ExtractionTrigger.Remark,
                        UserName = extractionTriggerInfo.ExtractionTrigger.UserName,
                        StartDate = extractionTriggerInfo.ExtractionTrigger.StartDate,
                        EndDate = extractionTriggerInfo.ExtractionTrigger.EndDate,
                        RunTime = extractionTriggerInfo.ExtractionTrigger.RunTime,
                        NeedToArchive = extractionTriggerInfo.ExtractionTrigger.NeedToArchive,
                        Active = extractionTriggerInfo.ExtractionTrigger.Active
                    });
            }

            return extractionTriggers.ToArray();
        }

        public async Task<IEnumerable<ExtractionTriggerData>> RunExtraction(int jobId, int[] extractionIds, DateTime startDate, DateTime endDate, DateTime runTime)
        {
            using (var scope = new TransactionScope(TransactionScopeOption.Required, TransactionScopeAsyncFlowOption.Enabled))
            {
                IExtractionRepository extractionRepository = _DataRepositoryFactory.GetRepository<IExtractionRepository>();
                var extractions = (await extractionRepository.GetAllAsync()).Where(c => extractionIds.Contains(c.EntityId));

                foreach (var ext in extractions)
                {
                    var needToArchive = false;
                    if (string.IsNullOrEmpty(ext.NeedArchiveAction))
                    {
                        var result = await SqlDataManager.RunProcedureWithMessage(_connectionString, ext.NeedArchiveAction, new List<SqlParameter>().ToArray());

                        if (result == "true" || result == "false")
                            needToArchive = bool.Parse(result);
                    }

                    var trigger = new ExtractionTrigger()
                    {
                        ExtractionJobId = jobId,
                        ExtractionId = ext.EntityId,
                        Code = UniqueKeyGenerator.RNGCharacterMask(6, 8),
                        Status = PackageStatus.New,
                        StartDate = startDate,
                        EndDate = endDate,
                        RunTime = runTime,
                        Remark = "Not Started",
                        UserName = LoggedInUsername,
                        NeedToArchive = needToArchive,
                        Active = true,
                        Deleted = false,
                        CreatedBy = "",
                        CreatedOn = DateTime.Now,
                        UpdatedBy = "",
                        UpdatedOn = DateTime.Now
                    };

                    await UpdateExtractionTrigger(trigger);
                }

                var job = await GetExtractionJob(jobId);

                return await GetExtractionTriggerByJob(job.Code);
            }
        }

        public async Task<IEnumerable<ExtractionTriggerData>> CancelExtractions(DateTime startDate, DateTime endDate)
        {
            using (var scope = new TransactionScope(TransactionScopeOption.Required, TransactionScopeAsyncFlowOption.Enabled))
            {
                IExtractionTriggerRepository extractionTriggerRepository = _DataRepositoryFactory.GetRepository<IExtractionTriggerRepository>();

                var triggers = (await extractionTriggerRepository.GetAllAsync()).Where(c => c.StartDate >= startDate && c.EndDate <= endDate && c.UserName == LoggedInUsername);

                using (TransactionScope ts = new TransactionScope())
                {
                    foreach (var trigger in triggers)
                    {
                        if (trigger.Status != PackageStatus.Done || trigger.Status != PackageStatus.Cancel || trigger.Status == PackageStatus.Fail)
                        {
                            //var dbTrigger = extractionTriggerRepository.Get(trigger.ExtractionTriggerId);
                            trigger.Status = PackageStatus.Cancel;
                            trigger.Remark = "Extraction has been cancel.";
                            await UpdateExtractionTrigger(trigger);
                        }
                    }

                    ts.Complete();
                }

                return await GetExtractionTriggerByRunDate(startDate, endDate);
            }
        }

        public async Task<IEnumerable<ExtractionTriggerData>> CancelExtractionByCode(string code, DateTime startDate, DateTime endDate)
        {
            using (var scope = new TransactionScope(TransactionScopeOption.Required, TransactionScopeAsyncFlowOption.Enabled))
            {
                IExtractionTriggerRepository extractionTriggerRepository = _DataRepositoryFactory.GetRepository<IExtractionTriggerRepository>();

                var triggers = (await extractionTriggerRepository.GetAllAsync()).Where(c => c.StartDate >= startDate && c.EndDate <= endDate && c.UserName == LoggedInUsername && c.Code == code);

                using (TransactionScope ts = new TransactionScope())
                {
                    foreach (var trigger in triggers)
                    {
                        if (trigger.Status != PackageStatus.Done || trigger.Status != PackageStatus.Cancel || trigger.Status == PackageStatus.Fail)
                        {
                            //var dbTrigger = extractionTriggerRepository.Get(trigger.ExtractionTriggerId);
                            trigger.Status = PackageStatus.Cancel;
                            trigger.Remark = "Extraction has been cancel.";
                            await UpdateExtractionTrigger(trigger);
                        }
                    }

                    ts.Complete();
                }

                return await GetExtractionTriggerByRunDate(startDate, endDate);
            }
        }

        #endregion

        #region ExtractionSummary operations
        public async Task UpdateExtractionSummary(ExtractionSummary extractionSummary)
        {
            using (var scope = new TransactionScope(TransactionScopeOption.Required, TransactionScopeAsyncFlowOption.Enabled))
            {
                IExtractionSummaryRepository extractionSummaryRepository = _DataRepositoryFactory.GetRepository<IExtractionSummaryRepository>();

                if (extractionSummary.Id == 0)
                    await extractionSummaryRepository.AddAsync(extractionSummary);
                else
                    await extractionSummaryRepository.UpdateAsync(extractionSummary);

                scope.Complete();
            }
        }

        public async Task DeleteExtractionSummary(int extractionSummaryId)
        {
            using (var scope = new TransactionScope(TransactionScopeOption.Required, TransactionScopeAsyncFlowOption.Enabled))
            {
                IExtractionSummaryRepository extractionSummaryRepository = _DataRepositoryFactory.GetRepository<IExtractionSummaryRepository>();

                await extractionSummaryRepository.DeleteAsync(extractionSummaryId);

                scope.Complete();
            }
        }

        public async Task<ExtractionSummary> GetExtractionSummary(int extractionSummaryId)
        {
            IExtractionSummaryRepository extractionSummaryRepository = _DataRepositoryFactory.GetRepository<IExtractionSummaryRepository>();

            ExtractionSummary extractionSummaryEntity = await extractionSummaryRepository.GetByIdAsync(extractionSummaryId);

            return extractionSummaryEntity;
        }

        public async Task<IEnumerable<ExtractionSummary>> GetAllExtractionSummarys()
        {
            IExtractionSummaryRepository extractionSummaryRepository = _DataRepositoryFactory.GetRepository<IExtractionSummaryRepository>();

            IEnumerable<ExtractionSummary> extractionSummarys = await extractionSummaryRepository.GetAllAsync();

            return extractionSummarys.ToArray();
        }

        #endregion

        #region ExtractionJob operations
        public async Task UpdateExtractionJob(ExtractionJob extractionJob)
        {
            using (var scope = new TransactionScope(TransactionScopeOption.Required, TransactionScopeAsyncFlowOption.Enabled))
            {
                IExtractionJobRepository extractionJobRepository = _DataRepositoryFactory.GetRepository<IExtractionJobRepository>();

                if (extractionJob.ExtractionJobId == 0)
                {
                    extractionJob.Code = UniqueKeyGenerator.RNGCharacterMask(6, 8);
                    extractionJob.Status = PackageStatus.New;
                    extractionJob.Active = true;
                    await extractionJobRepository.AddAsync(extractionJob);
                }
                else
                    await extractionJobRepository.UpdateAsync(extractionJob);

                scope.Complete();
            }
        }

        public async Task DeleteExtractionJob(int extractionJobId)
        {
            using (var scope = new TransactionScope(TransactionScopeOption.Required, TransactionScopeAsyncFlowOption.Enabled))
            {
                IExtractionJobRepository extractionJobRepository = _DataRepositoryFactory.GetRepository<IExtractionJobRepository>();

                await extractionJobRepository.DeleteAsync(extractionJobId);

                scope.Complete();
            }
        }

        public async Task<ExtractionJob> GetExtractionJob(int extractionJobId)
        {
            IExtractionJobRepository extractionJobRepository = _DataRepositoryFactory.GetRepository<IExtractionJobRepository>();

            ExtractionJob extractionJobEntity = await extractionJobRepository.GetByIdAsync(extractionJobId);

            return extractionJobEntity;
        }

        public async Task<IEnumerable<ExtractionJob>> GetCurrentExtractionJobs()
        {
            IExtractionJobRepository extractionJobRepository = _DataRepositoryFactory.GetRepository<IExtractionJobRepository>();

            IEnumerable<ExtractionJob> extractionJobs = (await extractionJobRepository.GetAllAsync()).Where(c => c.Status == PackageStatus.New || c.Status == PackageStatus.Pending || c.Status == PackageStatus.Running).ToArray();

            return extractionJobs.ToArray();
        }

        public async Task<IEnumerable<ExtractionJob>> GetExtractionJobByDate(DateTime startDate, DateTime endDate)
        {
            IExtractionJobRepository extractionJobRepository = _DataRepositoryFactory.GetRepository<IExtractionJobRepository>();

            IEnumerable<ExtractionJob> extractionJobs = (await extractionJobRepository.GetAllAsync()).Where(c => c.StartDate >= startDate && c.EndDate <= endDate).OrderByDescending(c => c.ExtractionJobId).ToArray();

            return extractionJobs.ToArray();
        }

        public async Task<IEnumerable<ExtractionJob>> RunExtractionJob(int jobId, int[] extractionIds, DateTime startDate, DateTime endDate, DateTime runTime)
        {
            using (var scope = new TransactionScope(TransactionScopeOption.Required, TransactionScopeAsyncFlowOption.Enabled))
            {
                foreach (var id in extractionIds)
                {
                    var trigger = new ExtractionTrigger()
                    {
                        ExtractionJobId = jobId,
                        ExtractionId = id,
                        Code = UniqueKeyGenerator.RNGCharacterMask(6, 8),
                        Status = PackageStatus.New,
                        StartDate = startDate,
                        EndDate = endDate,
                        RunTime = runTime,
                        Remark = "Not Started",
                        UserName = LoggedInUsername,
                        Active = true,
                        Deleted = false,
                        CreatedBy = LoggedInUsername,
                        CreatedOn = DateTime.Now,
                        UpdatedBy = LoggedInUsername,
                        UpdatedOn = DateTime.Now
                    };

                    await UpdateExtractionTrigger(trigger);
                }
                scope.Complete();
            }

            return await GetExtractionJobByDate(startDate, endDate);
        }

        public async Task<IEnumerable<ExtractionJob>> CancelExtractionJobByCode(string jobCode, DateTime startDate, DateTime endDate)
        {
            IExtractionJobRepository extractionJobRepository = _DataRepositoryFactory.GetRepository<IExtractionJobRepository>();
            IExtractionTriggerRepository extractionTriggerJobRepository = _DataRepositoryFactory.GetRepository<IExtractionTriggerRepository>();

            ExtractionJob initiators = (await extractionJobRepository.GetAllAsync()).Where(c => c.Code == jobCode).FirstOrDefault();
            string initiatorName = initiators.UserName;

            if (initiatorName != LoggedInUsername)
            {
                NotFoundException ex = new NotFoundException(string.Format("You can NOT cancel Extraction not initiated by You"));
                throw new FaultException<NotFoundException>(ex);
            }
            var jobs = (await extractionJobRepository.GetAllAsync()).Where(c => c.UserName == LoggedInUsername && c.Code == jobCode);
            using (var scope = new TransactionScope(TransactionScopeOption.Required, TransactionScopeAsyncFlowOption.Enabled))
            {
                foreach (var job in jobs)
                {
                    if (job.Status != PackageStatus.Done || job.Status != PackageStatus.Cancel || job.Status == PackageStatus.Fail)
                    {
                        //var dbJob = extractionJobRepository.Get(job.ExtractionJobId);
                        job.Status = PackageStatus.Cancel;
                        job.Remark = "Extraction has been cancel.";
                        await UpdateExtractionJob(job);

                        var triggers = await extractionTriggerJobRepository.GetExtractionTriggerByJob(job.Code);
                        foreach (var trigger in triggers)
                        {
                            if (trigger.ExtractionTrigger.Status != PackageStatus.New || trigger.ExtractionTrigger.Status != PackageStatus.Pending || trigger.ExtractionTrigger.Status != PackageStatus.Running)
                            {
                                trigger.ExtractionTrigger.Status = PackageStatus.Cancel;
                                trigger.ExtractionTrigger.Remark = "Extraction has been cancel.";
                                await UpdateExtractionTrigger(trigger.ExtractionTrigger);
                            }
                        }
                    }
                }

                scope.Complete();
            }

            return await GetExtractionJobByDate(startDate, endDate);
        }

        public async Task ClearExtractionHistory(int solutionId)
        {
            using (var con = new SqlConnection(_connectionString))
            {
                var cmd = new SqlCommand("spp_ifrs_getextractionservice_2delete", con);
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.CommandTimeout = 0;
                cmd.Parameters.Add(new SqlParameter
                {
                    ParameterName = "solutionId",
                    Value = solutionId,
                });

                await con.OpenAsync();
                var myRefNo = new ReferenceNoModel();
                SqlDataReader reader = await cmd.ExecuteReaderAsync();
                while (reader.Read())
                {

                    if (reader["Code"] != DBNull.Value)
                        myRefNo.RefNo = reader["Code"].ToString();
                    await ForceServiceDelete(myRefNo.RefNo, "spp_clear_extraction_history");
                }
                await con.CloseAsync();
            }
        }

        #endregion

        #region ProcessJob operations
        public async Task ForceServiceDelete(string serviceName, string sppName)
        {
            string serviceServerName = GetServiceServerName();

            var status = DoesServiceExist(serviceName, serviceServerName);
            if (status == true)
            {
                ServiceController sc = new ServiceController(serviceName, serviceServerName);
                if (sc.Status.Equals(ServiceControllerStatus.Running) || sc.Status.Equals(ServiceControllerStatus.StopPending) || sc.Status.Equals(ServiceControllerStatus.Paused))
                {
                    sc.Stop();
                    sc.WaitForStatus(ServiceControllerStatus.Stopped);

                }
                sc.Refresh();
                ServiceInstaller serviceInstallerObj = new ServiceInstaller();
                InstallContext context = new InstallContext("<<log file path>>", null);
                serviceInstallerObj.Context = context;
                serviceInstallerObj.ServiceName = serviceName;
                serviceInstallerObj.Uninstall(null);

            }
            await RemovefromDbase(serviceName, sppName);
        }

        bool DoesServiceExist(string serviceName, string serviceServerName)
        {
            ServiceController[] services = ServiceController.GetServices(serviceServerName);
            var service = services.FirstOrDefault(s => s.ServiceName == serviceName);
            return service != null;
        }

        public async Task RemovefromDbase(string serviceName, string sppName)
        {
            int status = 0;

            using (var con = new SqlConnection(_connectionString))
            {

                var cmd = new SqlCommand(sppName, con);
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.CommandTimeout = 0;
                cmd.Parameters.Add(new SqlParameter
                {
                    ParameterName = "ServiceCode",
                    Value = serviceName,
                });
                con.Open();
                status = await cmd.ExecuteNonQueryAsync();
                con.Close();
            }
        }
        #endregion

        #region SolutionRunDate operations
        public async Task UpdateSolutionRunDate(SolutionRunDate solutionRunDate)
        {
            using (var scope = new TransactionScope(TransactionScopeOption.Required, TransactionScopeAsyncFlowOption.Enabled))
            {
                IClosedPeriodRepository closedPeriodRepository = _DataRepositoryFactory.GetRepository<IClosedPeriodRepository>();
                ISolutionRunDateRepository solutionRunDateRepository = _DataRepositoryFactory.GetRepository<ISolutionRunDateRepository>();
                ISolutionRepository solutionRepository = _DataRepositoryFactory.GetRepository<ISolutionRepository>();


                var closedPeriod = (await closedPeriodRepository.GetAllAsync()).Where(c => c.SolutionId == solutionRunDate.SolutionId && c.Status).FirstOrDefault();
                //Retrieve excemption solution id
                var solution = (await solutionRepository.GetAllAsync()).Where(c => c.Name == "FIN_IFRS");

                solutionRunDate.Active = true;

                if (solution != null)
                {
                    if (closedPeriod == null)
                    {
                        NotFoundException ex = new NotFoundException(string.Format("Active closed period not in database"));
                        throw new FaultException<NotFoundException>(ex);
                    }
                }

                SolutionRunDate updatedEntity = null;

                if (solution == null)
                {
                    if (solutionRunDate.RunDate.Date < closedPeriod.Date.FirstOfMonth().Date || solutionRunDate.RunDate.Date > closedPeriod.Date.LastOfMonth().Date)
                    {
                        NotFoundException ex = new NotFoundException(string.Format("Run date must be within active closed period."));
                        throw new FaultException<NotFoundException>(ex);
                    }
                }

                if (solutionRunDate.SolutionRunDateId == 0)
                    await solutionRunDateRepository.AddAsync(solutionRunDate);
                else if (solutionRunDate.SolutionId == 2)
                {
                    await AchiveRecord();
                    await solutionRunDateRepository.UpdateAsync(solutionRunDate);
                }
                else
                {
                    await solutionRunDateRepository.UpdateAsync(solutionRunDate);
                }

                scope.Complete();
            }
        }

        public void DeleteSolutionRunDate(int solutionRunDateId)
        {
            using (var scope = new TransactionScope(TransactionScopeOption.Required, TransactionScopeAsyncFlowOption.Enabled))
            {
                ISolutionRunDateRepository solutionRunDateRepository = _DataRepositoryFactory.GetRepository<ISolutionRunDateRepository>();

                solutionRunDateRepository.DeleteAsync(solutionRunDateId);

                scope.Complete();
            }
        }

        public async Task<SolutionRunDate> GetSolutionRunDate(int solutionRunDateId)
        {
            ISolutionRunDateRepository solutionRunDateRepository = _DataRepositoryFactory.GetRepository<ISolutionRunDateRepository>();

            SolutionRunDate solutionRunDateEntity = await solutionRunDateRepository.GetByIdAsync(solutionRunDateId);
            if (solutionRunDateEntity == null)
            {
                NotFoundException ex = new NotFoundException(string.Format("SolutionRunDate with ID of {0} is not in database", solutionRunDateId));
                throw new FaultException<NotFoundException>(ex);
            }

            return solutionRunDateEntity;
        }

        public async Task<IEnumerable<SolutionRunDate>> GetAllSolutionRunDates()
        {
            ISolutionRunDateRepository solutionRunDateRepository = _DataRepositoryFactory.GetRepository<ISolutionRunDateRepository>();

            IEnumerable<SolutionRunDate> solutionRunDates = await solutionRunDateRepository.GetAllAsync();

            return solutionRunDates.ToArray();
        }

        public async Task<IEnumerable<SolutionRunDateData>> GetSolutionRunDates()
        {
            ISolutionRunDateRepository solutionRunDateRepository = _DataRepositoryFactory.GetRepository<ISolutionRunDateRepository>();

            ISolutionRepository solutionRepository = _DataRepositoryFactory.GetRepository<ISolutionRepository>();

            var solutions = await GetSolutions();
            var solutionIds = solutions.Where(c => c.Active).Select(c => c.EntityId).Distinct();

            List<SolutionRunDateData> solutionRunDates = new List<SolutionRunDateData>();
            IEnumerable<SolutionRunDate> solutionRunDateInfos = (await solutionRunDateRepository.GetAllAsync()).Where(c => solutionIds.Contains(c.SolutionId)).ToArray();

            foreach (var solutionRunDateInfo in solutionRunDateInfos)
            {
                solutionRunDates.Add(
                    new SolutionRunDateData
                    {
                        SolutionRunDateId = solutionRunDateInfo.EntityId,
                        RunDate = solutionRunDateInfo.RunDate,
                        Month = solutionRunDateInfo.RunDate.Month,
                        Year = solutionRunDateInfo.RunDate.Year,
                        SolutionId = solutionRunDateInfo.SolutionId,
                        SolutionName = GetSolutionName(solutions, solutionRunDateInfo.SolutionId),
                        Active = solutionRunDateInfo.Active
                    });
            }

            return solutionRunDates.ToArray();
        }

        public async Task<IEnumerable<SolutionRunDateData>> GetSolutionRunDateByLogin(string loginID)
        {
            IUserRoleRepository userRoleRepository = _DataRepositoryFactory.GetRepository<IUserRoleRepository>();
            ISolutionRunDateRepository solutionRunDateRepository = _DataRepositoryFactory.GetRepository<ISolutionRunDateRepository>();

            var solutionIds = (await userRoleRepository.GetUserRoleInfo()).Where(c => c.UserSetup.UserName == loginID).Select(c => c.Solution.SolutionId).Distinct();

            var solutions = await GetSolutions();
            var solutionId2s = solutions.Where(c => c.Active).Select(c => c.EntityId).Distinct();

            List<SolutionRunDateData> solutionRunDates = new List<SolutionRunDateData>();

            IEnumerable<SolutionRunDate> solutionRunDateInfos = (await solutionRunDateRepository.GetAllAsync()).Where(c => solutionIds.Contains(c.SolutionId) && solutionId2s.Contains(c.SolutionId)).ToArray();

            foreach (var solutionRunDateInfo in solutionRunDateInfos)
            {
                solutionRunDates.Add(
                    new SolutionRunDateData
                    {
                        SolutionRunDateId = solutionRunDateInfo.EntityId,
                        RunDate = solutionRunDateInfo.RunDate,
                        Month = solutionRunDateInfo.RunDate.Month,
                        Year = solutionRunDateInfo.RunDate.Year,
                        SolutionId = solutionRunDateInfo.SolutionId,
                        SolutionName = GetSolutionName(solutions, solutionRunDateInfo.SolutionId),
                        Active = solutionRunDateInfo.Active
                    });
            }

            return solutionRunDates.ToArray();
        }

        public async Task<string> GetSolutionRunDateByLoginByDefault(string loginID)
        {
            IDefaultUserRepository defaultuserRepository = _DataRepositoryFactory.GetRepository<IDefaultUserRepository>();
            ISolutionRunDateRepository solutionRunDateRepository = _DataRepositoryFactory.GetRepository<ISolutionRunDateRepository>();

            var solutionIds = (await defaultuserRepository.GetAllAsync()).Where(c => c.LoginID == loginID).FirstOrDefault();

            int solid = 0;
            if (solutionIds == null)
            {
                solid = 1;
            }
            else
            {
                solid = solutionIds.SolutionId;
            }

            var solutionIdrun = (await solutionRunDateRepository.GetAllAsync()).Where(c => c.SolutionId == solid).FirstOrDefault();
            object rundate;
            if (solutionIdrun == null)
            {
                rundate = DateTime.Now;

            }
            else
            {
                rundate = solutionIdrun.RunDate;
            }

            return rundate.ToString();

        }


        public async Task RestoreArchive(int solutionid, DateTime date)
        {
            int status = 0;

            using (var con = new SqlConnection(_connectionString))
            {
                var cmd = new SqlCommand("spp_restore_archived_data", con);
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.CommandTimeout = 0;
                cmd.Parameters.Add(new SqlParameter
                {
                    ParameterName = "SolutionId",
                    Value = solutionid,
                });
                cmd.Parameters.Add(new SqlParameter
                {
                    ParameterName = "AchRunDate",
                    Value = date,
                });

                await con.OpenAsync();

                status = await cmd.ExecuteNonQueryAsync();

                await con.CloseAsync();
            }
        }

        public async Task AchiveRecord()
        {
            using (var con = new SqlConnection(_connectionString))
            {
                var cmd = new SqlCommand("spp_ArchiveRecord", con);
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.CommandTimeout = 0;

                await con.OpenAsync();

                await cmd.ExecuteNonQueryAsync();

                await con.CloseAsync();
            }
        }

        public async Task<IEnumerable<SolutionRunDate>> GetRunDate()
        {
            ISolutionRunDateRepository solutionrundateRepository = _DataRepositoryFactory.GetRepository<ISolutionRunDateRepository>();
            IEnumerable<SolutionRunDate> solutionrundates = await solutionrundateRepository.GetAllAsync();

            return solutionrundates.ToArray();
        }

        #endregion

        #region ClosedPeriod operations

        public async Task UpdateClosedPeriod(ClosedPeriod closedPeriod)
        {
            using (var scope = new TransactionScope(TransactionScopeOption.Required, TransactionScopeAsyncFlowOption.Enabled))
            {
                IClosedPeriodRepository closedPeriodRepository = _DataRepositoryFactory.GetRepository<IClosedPeriodRepository>();

                if (closedPeriod.ClosedPeriodId == 0)
                    await closedPeriodRepository.AddAsync(closedPeriod);
                else
                    await closedPeriodRepository.UpdateAsync(closedPeriod);

                scope.Complete();
            }
        }

        public async Task DeleteClosedPeriod(int closedPeriodId)
        {
            using (var scope = new TransactionScope(TransactionScopeOption.Required, TransactionScopeAsyncFlowOption.Enabled))
            {
                IClosedPeriodRepository closedPeriodRepository = _DataRepositoryFactory.GetRepository<IClosedPeriodRepository>();

                await closedPeriodRepository.DeleteAsync(closedPeriodId);

                scope.Complete();
            }
        }

        public async Task<ClosedPeriod> GetClosedPeriod(int closedPeriodId)
        {
            IClosedPeriodRepository closedPeriodRepository = _DataRepositoryFactory.GetRepository<IClosedPeriodRepository>();

            ClosedPeriod closedPeriodEntity = await closedPeriodRepository.GetByIdAsync(closedPeriodId);
            if (closedPeriodEntity == null)
            {
                NotFoundException ex = new NotFoundException(string.Format("ClosedPeriod with ID of {0} is not in database", closedPeriodId));
                throw new FaultException<NotFoundException>(ex);
            }

            return closedPeriodEntity;
        }

        public async Task<IEnumerable<ClosedPeriod>> GetAllClosedPeriods()
        {
            IClosedPeriodRepository closedPeriodRepository = _DataRepositoryFactory.GetRepository<IClosedPeriodRepository>();

            IEnumerable<ClosedPeriod> closedPeriods = await closedPeriodRepository.GetAllAsync();

            return closedPeriods.ToArray();
        }

        public async Task<IEnumerable<ClosedPeriodData>> GetClosedPeriods()
        {
            IClosedPeriodRepository closedPeriodRepository = _DataRepositoryFactory.GetRepository<IClosedPeriodRepository>();

            var solutions = await GetSolutions();
            var solutionIds = solutions.Where(c => c.Active).Select(c => c.EntityId).Distinct();

            List<ClosedPeriodData> closedPeriods = new List<ClosedPeriodData>();
            //  IEnumerable<ClosedPeriodInfo> closedPeriodInfos = closedPeriodRepository.GetClosedPeriods().Where(c => solutionIds.Contains(c.ClosedPeriod.SolutionId)).ToArray();
            IEnumerable<ClosedPeriod> closedPeriodInfos = (await closedPeriodRepository.GetAllAsync()).Where(c => solutionIds.Contains(c.SolutionId)).OrderByDescending(c => c.Status).ThenByDescending(c => c.Date).ToArray();
            foreach (var closedPeriodInfo in closedPeriodInfos)
            {
                closedPeriods.Add(
                    new ClosedPeriodData
                    {
                        ClosedPeriodId = closedPeriodInfo.EntityId,
                        Date = closedPeriodInfo.Date,
                        Month = closedPeriodInfo.Date.Month,
                        Year = closedPeriodInfo.Date.Year,
                        SolutionId = closedPeriodInfo.SolutionId,
                        SolutionName = GetSolutionName(solutions, closedPeriodInfo.SolutionId),
                        Status = closedPeriodInfo.Status,
                        Active = closedPeriodInfo.Active
                    });
            }

            return closedPeriods.ToArray();
        }

        public async Task<IEnumerable<ClosedPeriodData>> GetClosedPeriodByLogin(string loginID)
        {
            IUserRoleRepository userRoleRepository = _DataRepositoryFactory.GetRepository<IUserRoleRepository>();
            IClosedPeriodRepository closedPeriodRepository = _DataRepositoryFactory.GetRepository<IClosedPeriodRepository>();


            var solutionIds = (await userRoleRepository.GetUserRoleInfo()).Where(c => c.UserSetup.UserName == loginID).Select(c => c.Solution.SolutionId).Distinct();

            var solutions = await GetSolutions();
            var solutionId2s = solutions.Where(c => c.Active).Select(c => c.EntityId).Distinct();

            List<ClosedPeriodData> closedPeriods = new List<ClosedPeriodData>();

            IEnumerable<ClosedPeriod> closedPeriodInfos = (await closedPeriodRepository.GetAllAsync()).Where(c => solutionIds.Contains(c.SolutionId) && solutionId2s.Contains(c.SolutionId)).OrderByDescending(c => c.Status).ThenByDescending(c => c.Date).ToArray();
            //IEnumerable<IFRSRegistryInfo> ifrsRegistryInfos = ifrsRegistryRepository.GetIFRSRegistrys(flag).OrderBy(c => c.IFRSRegistry.FinType).ThenBy(c => c.IFRSRegistry.Position).ToArray();

            foreach (var closedPeriodInfo in closedPeriodInfos)
            {
                closedPeriods.Add(
                    new ClosedPeriodData
                    {
                        ClosedPeriodId = closedPeriodInfo.EntityId,
                        Date = closedPeriodInfo.Date,
                        Month = closedPeriodInfo.Date.Month,
                        Year = closedPeriodInfo.Date.Year,
                        SolutionId = closedPeriodInfo.SolutionId,
                        SolutionName = GetSolutionName(solutions, closedPeriodInfo.SolutionId),
                        Status = closedPeriodInfo.Status,
                        Active = closedPeriodInfo.Active,
                        Deleted = closedPeriodInfo.Deleted
                    });
            }

            return closedPeriods.ToArray();
        }

        public async Task<ClosedPeriod> ClosePeriod(ClosedPeriod closedPeriod)
        {
            using (var scope = new TransactionScope(TransactionScopeOption.Required, TransactionScopeAsyncFlowOption.Enabled))
            {
                IClosedPeriodRepository closedPeriodRepository = _DataRepositoryFactory.GetRepository<IClosedPeriodRepository>();
                IClosedPeriodTemplateRepository closedPeriodTemplateRepository = _DataRepositoryFactory.GetRepository<IClosedPeriodTemplateRepository>();

                var template = (await closedPeriodTemplateRepository.GetAllAsync()).Where(c => c.SolutionId == closedPeriod.SolutionId).FirstOrDefault();

                ClosedPeriod updatedEntity = null;

                using (TransactionScope ts = new TransactionScope())
                {
                    List<SqlParameter> parameters = new List<SqlParameter>
                    {
                        new SqlParameter
                        {
                            ParameterName = "RunDate",
                            Value = closedPeriod.Date
                        }
                    };

                    await SqlDataManager.RunProcedure(_connectionString, template.Action, parameters.ToArray());

                    ts.Complete();
                }
                scope.Complete();

                return updatedEntity;
            }
        }

        #endregion

        #region ClosedPeriodTemplate operations

        public async Task UpdateClosedPeriodTemplate(ClosedPeriodTemplate closedPeriodTemplate)
        {
            using (var scope = new TransactionScope(TransactionScopeOption.Required, TransactionScopeAsyncFlowOption.Enabled))
            {
                IClosedPeriodTemplateRepository closedPeriodTemplateRepository = _DataRepositoryFactory.GetRepository<IClosedPeriodTemplateRepository>();

                ClosedPeriodTemplate updatedEntity = null;

                if (closedPeriodTemplate.ClosedPeriodTemplateId == 0)
                    await closedPeriodTemplateRepository.AddAsync(closedPeriodTemplate);
                else
                    await closedPeriodTemplateRepository.UpdateAsync(closedPeriodTemplate);

                scope.Complete();
            }
        }

        public async Task DeleteClosedPeriodTemplate(int closedPeriodTemplateId)
        {
            using (var scope = new TransactionScope(TransactionScopeOption.Required, TransactionScopeAsyncFlowOption.Enabled))
            {
                IClosedPeriodTemplateRepository closedPeriodTemplateRepository = _DataRepositoryFactory.GetRepository<IClosedPeriodTemplateRepository>();

                await closedPeriodTemplateRepository.DeleteAsync(closedPeriodTemplateId);

                scope.Complete();
            }
        }

        public async Task<ClosedPeriodTemplate> GetClosedPeriodTemplate(int closedPeriodTemplateId)
        {
            IClosedPeriodTemplateRepository closedPeriodTemplateRepository = _DataRepositoryFactory.GetRepository<IClosedPeriodTemplateRepository>();

            ClosedPeriodTemplate closedPeriodTemplateEntity = await closedPeriodTemplateRepository.GetByIdAsync(closedPeriodTemplateId);
            if (closedPeriodTemplateEntity == null)
            {
                NotFoundException ex = new NotFoundException(string.Format("ClosedPeriodTemplate with ID of {0} is not in database", closedPeriodTemplateId));
                throw new FaultException<NotFoundException>(ex);
            }

            return closedPeriodTemplateEntity;
        }

        public async Task<IEnumerable<ClosedPeriodTemplate>> GetAllClosedPeriodTemplates()
        {
            IClosedPeriodTemplateRepository closedPeriodTemplateRepository = _DataRepositoryFactory.GetRepository<IClosedPeriodTemplateRepository>();

            IEnumerable<ClosedPeriodTemplate> closedPeriodTemplates = await closedPeriodTemplateRepository.GetAllAsync();

            return closedPeriodTemplates.ToArray();
        }

        public async Task<IEnumerable<ClosedPeriodTemplateData>> GetClosedPeriodTemplates()
        {
            IClosedPeriodTemplateRepository closedPeriodTemplateRepository = _DataRepositoryFactory.GetRepository<IClosedPeriodTemplateRepository>();

            var solutions = await GetSolutions();
            var solutionIds = solutions.Where(c => c.Active).Select(c => c.EntityId).Distinct();

            List<ClosedPeriodTemplateData> closedPeriodTemplates = new List<ClosedPeriodTemplateData>();
            IEnumerable<ClosedPeriodTemplate> closedPeriodTemplateInfos = (await closedPeriodTemplateRepository.GetAllAsync()).Where(c => solutionIds.Contains(c.SolutionId)).ToArray();

            foreach (var closedPeriodTemplateInfo in closedPeriodTemplateInfos)
            {
                closedPeriodTemplates.Add(
                    new ClosedPeriodTemplateData
                    {
                        ClosedPeriodTemplateId = closedPeriodTemplateInfo.EntityId,
                        Action = closedPeriodTemplateInfo.Action,
                        SolutionId = closedPeriodTemplateInfo.SolutionId,
                        SolutionName = GetSolutionName(solutions, closedPeriodTemplateInfo.SolutionId),
                        Active = closedPeriodTemplateInfo.Active
                    });
            }

            return closedPeriodTemplates.ToArray();
        }

        public async Task<IEnumerable<ClosedPeriodTemplateData>> GetClosedPeriodTemplateByLogin(string loginID)
        {
            IUserRoleRepository userRoleRepository = _DataRepositoryFactory.GetRepository<IUserRoleRepository>();
            IClosedPeriodTemplateRepository closedPeriodTemplateRepository = _DataRepositoryFactory.GetRepository<IClosedPeriodTemplateRepository>();

            var solutionIds = (await userRoleRepository.GetUserRoleInfo()).Where(c => c.UserSetup.UserName == loginID).Select(c => c.Solution.SolutionId).Distinct();


            List<ClosedPeriodTemplateData> closedPeriodTemplates = new List<ClosedPeriodTemplateData>();

            var solutions = await GetSolutions();
            var solutionId2s = solutions.Where(c => c.Active).Select(c => c.EntityId).Distinct();

            IEnumerable<ClosedPeriodTemplate> closedPeriodTemplateInfos = (await closedPeriodTemplateRepository.GetAllAsync()).Where(c => solutionIds.Contains(c.SolutionId) && solutionId2s.Contains(c.SolutionId)).ToArray();

            foreach (var closedPeriodTemplateInfo in closedPeriodTemplateInfos)
            {
                closedPeriodTemplates.Add(
                    new ClosedPeriodTemplateData
                    {
                        ClosedPeriodTemplateId = closedPeriodTemplateInfo.EntityId,
                        Action = closedPeriodTemplateInfo.Action,
                        SolutionId = closedPeriodTemplateInfo.SolutionId,
                        SolutionName = GetSolutionName(solutions, closedPeriodTemplateInfo.SolutionId),
                        Active = closedPeriodTemplateInfo.Active
                    });
            }

            return closedPeriodTemplates.ToArray();
        }

        #endregion


        #region Helper
        protected async Task<Solution[]> GetSolutions()
        {
            ISolutionRepository solutionRepository = _DataRepositoryFactory.GetRepository<ISolutionRepository>();

            var solutions = await solutionRepository.GetAllAsync();

            return solutions.ToArray();
        }

        protected string GetSolutionName(Solution[] solutions, int solutionId)
        {
            foreach (var solution in solutions)
            {
                if (solution.SolutionId == solutionId)
                    return solution.Alias;
            }

            return string.Empty;
        }

        protected async Task<IEnumerable<Roles>> GetRoles()
        {
            IRoleRepository roleRepository = _DataRepositoryFactory.GetRepository<IRoleRepository>();

            var roles = await roleRepository.GetAllAsync();

            return roles.ToArray();
        }

        protected string GetRoleName(IEnumerable<Roles> roles, int roleId)
        {
            foreach (var role in roles)
            {
                if (role.Id == roleId)
                    return role.Name;
            }

            return string.Empty;
        }

        public string GetServiceServerName()
        {
            string serviceServerName = "";

            if (!string.IsNullOrEmpty(_configuration["ServiceServerName"]))
            {
                serviceServerName = _configuration["ServiceServerName"];
            }

            return serviceServerName;
        }
        #endregion

    }
}
