using Fintrak.Model.Core;
using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;

namespace Fintrak.Data.Core
{
    [Export(typeof(IProcessTriggerRepository))]
    [PartCreationPolicy(CreationPolicy.NonShared)]
    public class ProcessTriggerRepository : DataRepositoryBase<ProcessTrigger>, IProcessTriggerRepository
    {
        public ProcessHistoryRepository(CoreDbContext context) : base(context)
        {
        }

        public IEnumerable<ProcessTriggerInfo> GetProcessTriggers()
        {
            using (CoreContext entityContext = new CoreContext())
            {
                var query = from a in entityContext.ProcessTriggerSet
                            join b in entityContext.ProcessSet on a.ProcessId equals b.ProcessId
                            
                            select new ProcessTriggerInfo()
                            {
                                ProcessTrigger = a,
                                Processes = b
                            };

                return query.ToFullyLoaded();
            }
        }

        public IEnumerable<ProcessTriggerInfo> GetProcessTriggerByProcess(int processId)
        {
            using (CoreContext entityContext = new CoreContext())
            {
                var query = from a in entityContext.ProcessTriggerSet
                            join b in entityContext.ProcessSet on a.ProcessId equals b.ProcessId
                            join c in entityContext.ProcessJobSet on a.ProcessJobId equals c.ProcessJobId
                            where a.ProcessId == processId 
                            select new ProcessTriggerInfo()
                            {
                                ProcessTrigger = a,
                                Processes = b,
                                ProcessJob = c
                            };

                return query.ToFullyLoaded();
            }
        }

        public IEnumerable<ProcessTriggerInfo> GetProcessTriggerByJob(string jobCode)
        {
            using (CoreContext entityContext = new CoreContext())
            {
                var query = from a in entityContext.ProcessTriggerSet
                            join b in entityContext.ProcessSet on a.ProcessId equals b.ProcessId
                            join c in entityContext.ProcessJobSet on a.ProcessJobId equals c.ProcessJobId
                            where c.Code == jobCode
                            select new ProcessTriggerInfo()
                            {
                                ProcessTrigger = a,
                                Processes = b,
                                ProcessJob = c
                            };

                return query.ToFullyLoaded();
            }
        }

        public IEnumerable<ProcessTriggerInfo> GetProcessTriggerByRunDate()
        {
            using (CoreContext entityContext = new CoreContext())
            {
                var query = from a in entityContext.ProcessTriggerSet
                            join b in entityContext.ProcessSet on a.ProcessId equals b.ProcessId
                          
                            select new ProcessTriggerInfo()
                            {
                                ProcessTrigger = a,
                                Processes = b
                            };

                return query.ToFullyLoaded();
            }
        }

        public IEnumerable<ProcessTriggerInfo> GetProcessTriggerByRunTime(DateTime runTime)
        {
            using (CoreContext entityContext = new CoreContext())
            {
                var query = from a in entityContext.ProcessTriggerSet
                            join b in entityContext.ProcessSet on a.ProcessId equals b.ProcessId
                           
                            select new ProcessTriggerInfo()
                            {
                                ProcessTrigger = a,
                                Processes = b
                            };

                return query.ToFullyLoaded();
            }
        }
    }
}
