using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;
using Fintrak.Shared.Common;
using Fintrak.Shared.Common.Interface;
using Fintrak.Shared.Common.Tenancy;

namespace Fintrak.Model.Core
{
    public class ProcessHistory : EntityBase, IIdentifiableEntity, ITenantEntity
    {
        [DataMember]
        [Browsable(false)]
        [Key]
        public int ProcessHistoryId { get; set; }

        [DataMember]
        [Required]
        public string Process { get; set; }

        [DataMember]
        [Required]
        public string ProcessMessage { get; set; }

        [DataMember]
        [Required]
        public string ProcessErrorMessage { get; set; }

        [DataMember]
        [Required]
        public string ProcessStatus { get; set; }


        public int EntityId
        {
            get
            {
                return ProcessHistoryId;
            }
        }
    }
}
