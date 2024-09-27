using Fintrak.Shared.Common;
using Fintrak.Shared.Common.Interface;
using Fintrak.Shared.Common.Tenancy;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Fintrak.Model.Core
{
    public class ExtractionRole : EntityBase, IIdentifiableEntity, ITenantEntity
    {
        [DataMember]
        [Browsable(false)]
        public int ExtractionRoleId { get; set; }

        [DataMember]
        [Required]
        public int RoleId { get; set; }


        [DataMember]
        [Required]
        public int ExtractionId { get; set; }


        public int EntityId
        {
            get
            {
                return ExtractionRoleId;
            }
        }
    }

}
