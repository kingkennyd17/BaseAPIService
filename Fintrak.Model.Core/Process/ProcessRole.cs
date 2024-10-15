using Fintrak.Shared.Common;
using Fintrak.Shared.Common.Interface;
using Fintrak.Shared.Common.Tenancy;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Runtime.Serialization;

namespace Fintrak.Model.Core
{
    public partial class ProcessRole : EntityBase, IIdentifiableEntity, ITenantEntity
    {
        [DataMember]
        [Browsable (false)]
        public int ProcessRoleId { get; set; }

        [DataMember]
        [Required]
        public int RoleId { get; set; }

       
        [DataMember]
        [Required]
        public int ProcessId { get; set; }

        public int EntityId
        {
            get
            {
                return ProcessRoleId;
            }
        }
    }
}
