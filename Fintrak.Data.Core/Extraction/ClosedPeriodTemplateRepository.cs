using Fintrak.Data.Core.Interface;
using Fintrak.Model.Core;
using Fintrak.Model.SystemCore;
using Fintrak.Model.SystemCore.Common;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.Composition;

namespace Fintrak.Data.Core
{
    [Export(typeof(IClosedPeriodTemplateRepository))]
    [PartCreationPolicy(CreationPolicy.NonShared)]
    public class ClosedPeriodTemplateRepository : DataRepositoryBase<ClosedPeriodTemplate>, IClosedPeriodTemplateRepository
    {
        public ClosedPeriodTemplateRepository(CoreDbContext context) : base(context)
        {
        }
    }
}
