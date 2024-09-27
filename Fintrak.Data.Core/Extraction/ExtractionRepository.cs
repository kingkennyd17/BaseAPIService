using Fintrak.Data.Core.Interface;
using Fintrak.Model.Core;
using Fintrak.Model.SystemCore;
using Fintrak.Model.SystemCore.Common;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.Composition;

namespace Fintrak.Data.Core
{
    [Export(typeof(IExtractionRepository))]
    [PartCreationPolicy(CreationPolicy.NonShared)]
    public class ExtractionRepository : DataRepositoryBase<Extraction>, IExtractionRepository
    {
        public ExtractionRepository(CoreDbContext context) : base(context)
        {
        }
    }
}
