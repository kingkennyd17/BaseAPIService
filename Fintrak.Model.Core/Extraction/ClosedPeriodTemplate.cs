using Fintrak.Model.Core.Enum;
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
    public class ClosedPeriodTemplate : EntityBase, IIdentifiableEntity, ITenantEntity
    {
        [DataMember]
        [Browsable(false)]
        public int ClosedPeriodTemplateId { get; set; }

        [DataMember]
        [Required]
        public int SolutionId { get; set; }

        [DataMember]
        [Required]
        public string Action { get; set; }


        public int EntityId
        {
            get
            {
                return ClosedPeriodTemplateId;
            }
        }
    }

}
