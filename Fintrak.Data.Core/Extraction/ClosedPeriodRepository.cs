using Fintrak.Data.Core.Interface;
using Fintrak.Model.Core;
using Fintrak.Model.SystemCore;
using Fintrak.Model.SystemCore.Common;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.Composition;

namespace Fintrak.Data.Core
{
    [Export(typeof(IClosedPeriodRepository))]
    [PartCreationPolicy(CreationPolicy.NonShared)]
    public class ClosedPeriodRepository : DataRepositoryBase<ClosedPeriod>, IClosedPeriodRepository
    {
        public ClosedPeriodRepository(CoreDbContext context) : base(context)
        {
        }
    }
}
