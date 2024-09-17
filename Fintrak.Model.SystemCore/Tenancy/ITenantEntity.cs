using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Fintrak.Model.SystemCore.Tenancy
{
    public interface ITenantEntity
    {
        string TenantId { get; set; }

        public bool Active { get; set; }

        public bool Deleted { get; set; }

        public string? CreatedBy { get; set; }

        public DateTime CreatedOn { get; set; }

        public string? UpdatedBy { get; set; }

        public DateTime UpdatedOn { get; set; }
    }

}
