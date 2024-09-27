using System;
using Fintrak.Model.Core;
using Fintrak.Shared.Common.Interface;

namespace Fintrak.Data.Core.Interface
{
    public interface IExtractionRoleRepository : IDataRepository<ExtractionRole>
    {
        Task<IEnumerable<ExtractionRoleInfo>> GetExtractionRoles();
        Task<IEnumerable<ExtractionRoleInfo>> GetExtractionRoleByExtraction(int extractionId);
    }
}
