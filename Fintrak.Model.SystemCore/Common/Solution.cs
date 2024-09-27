using Fintrak.Shared.Common.Interface;
using Fintrak.Shared.Common;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using Fintrak.Shared.Common.Tenancy;

namespace Fintrak.Model.SystemCore
{
    public class Solution : EntityBase, IIdentifiableEntity, ITenantEntity
    {
        [DataMember]
        [Browsable(false)]
        public int SolutionId { get; set; }

        [DataMember]
        [Required]
        public string Name { get; set; }

        [DataMember]
        [Required]
        public string Alias { get; set; }

        public int EntityId
        {
            get
            {
                return SolutionId;
            }
        }
    }
}
