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
    public class ExtractionTrigger : EntityBase, IIdentifiableEntity, ITenantEntity
    {
        [DataMember]
        [Browsable(false)]
        public int ExtractionTriggerId { get; set; }

        [DataMember]
        [Required]
        public int ExtractionJobId { get; set; }

        [DataMember]
        [Required]
        public int ExtractionId { get; set; }

        [DataMember]
        public string Code { get; set; }

        [DataMember]
        public PackageStatus Status { get; set; }

        [DataMember]
        public string Remark { get; set; }

        [DataMember]
        public string UserName { get; set; }

        [DataMember]
        public DateTime StartDate { get; set; }

        [DataMember]
        public DateTime EndDate { get; set; }

        [DataMember]
        public DateTime? RunTime { get; set; }

        [DataMember]
        public bool NeedToArchive { get; set; }


        public int EntityId
        {
            get
            {
                return ExtractionTriggerId;
            }
        }
    }

}
