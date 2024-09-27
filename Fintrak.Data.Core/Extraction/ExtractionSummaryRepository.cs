using Fintrak.Data.Core.Interface;
using Fintrak.Model.Core;
using Fintrak.Model.SystemCore;
using Fintrak.Model.SystemCore.Common;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.Composition;

namespace Fintrak.Data.Core
{
    [Export(typeof(IExtractionSummaryRepository))]
    [PartCreationPolicy(CreationPolicy.NonShared)]
    public class ExtractionSummaryRepository : DataRepositoryBase<ExtractionSummary>, IExtractionSummaryRepository
    {
        public ExtractionSummaryRepository(CoreDbContext context) : base(context)
        {
        }
    }
}
