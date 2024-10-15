using Fintrak.Model.Core;
using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;

namespace Fintrak.Data.Core
{
    [Export(typeof(IProcessRepository))]
    [PartCreationPolicy(CreationPolicy.NonShared)]
    public class ProcessRepository : DataRepositoryBase<Processes>, IProcessRepository
    {
        public ProcessRepository(CoreDbContext context) : base(context)
        {
        }
    }
}
