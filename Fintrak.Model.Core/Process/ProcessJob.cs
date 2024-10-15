using Fintrak.Model.Core.Enum;
using Fintrak.Shared.Common;
using Fintrak.Shared.Common.Core;
using Fintrak.Shared.Common.Interface;
using Fintrak.Shared.Common.Tenancy;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Runtime.Serialization;

namespace Fintrak.Model.Core
{
    public class ProcessJob : EntityBase, IIdentifiableEntity, ITenantEntity
    {
        [DataMember]
        [Browsable(false)]
        public int ProcessJobId { get; set; }

        [DataMember]
        public string Code { get; set; }

        [DataMember]
        public PackageStatus Status  { get; set; }

        [DataMember]
        public string Remark { get; set; }

        [DataMember]
        [Required]
        public string UserName { get; set; }

        [DataMember]
        public DateTime StartDate { get; set; }

        [DataMember]
        public DateTime EndDate { get; set; }

        [DataMember]
        public DateTime? RunTime { get; set; }

        public int EntityId
        {
            get
            {
                return ProcessJobId;
            }
        }
    }
}




















