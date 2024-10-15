using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Runtime.Serialization;
using Fintrak.Shared.Common;
using Fintrak.Shared.Common.Interface;
using Fintrak.Shared.Common.Tenancy;

namespace Fintrak.Model.Core
{
    public class ProcessHistoryRun : EntityBase, IIdentifiableEntity, ITenantEntity
    {
        [DataMember]
        [Browsable(false)]
        [Key]
        public int ProcessHistoryRunId { get; set; }

        [DataMember]
        [Required]
        public string Alias { get; set; }

        [DataMember]
        [Required]
        public string Action { get; set; }

        [DataMember]
        [Required]
        public string Type { get; set; }


        public int EntityId
        {
            get
            {
                return ProcessHistoryRunId;
            }
        }
    }
}
