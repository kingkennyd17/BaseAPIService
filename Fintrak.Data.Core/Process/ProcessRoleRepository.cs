using Fintrak.Data.Core.Interface;
using Fintrak.Model.Core;
using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Data.Entity;

namespace Fintrak.Data.Core
{
    [Export(typeof(IProcessRoleRepository))]
    [PartCreationPolicy(CreationPolicy.NonShared)]
    public class ProcessRoleRepository : DataRepositoryBase<ProcessRole>, IProcessRoleRepository
    {
        public ProcessRoleRepository(CoreDbContext context) : base(context)
        {
        }

        public async Task<IEnumerable<ProcessRoleInfo>> GetProcessRoles()
        {
            var query = from a in _context.ProcessRoleSet
                        join b in _context.ProcessSet on a.ProcessId equals b.ProcessId

                        select new ProcessRoleInfo()
                        {
                            ProcessRole = a,
                            Processes = b,

                        };

            return await query.ToListAsync();
        }

        public async Task<IEnumerable<ProcessRoleInfo>> GetProcessRoleByProcess(int processId)
        {
            var query = from a in _context.ProcessRoleSet
                        join b in _context.ProcessSet on a.ProcessId equals b.ProcessId

                        where a.ProcessId == processId
                        select new ProcessRoleInfo()
                        {
                            ProcessRole = a,
                            Processes = b,

                        };

            return await query.ToListAsync();
        }
    }
}
