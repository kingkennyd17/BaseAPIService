using Fintrak.Model.Core;
using Fintrak.Service.Core.Interface;

namespace Fintrak.Host.ServicePortal2._0.Models
{
    public class ExtractionModel
    {
        public Extraction Extraction { get; set; }
        public IEnumerable<ExtractionRoleData> ExtractionRoles { get; set; }
    }
}
