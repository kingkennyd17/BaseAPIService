using Fintrak.Model.Core.Enum;
using Fintrak.Shared.Common;
using Fintrak.Shared.Common.Interface;
using Fintrak.Shared.Common.Tenancy;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Runtime.Serialization;

namespace Fintrak.Model.Core
{
    public class Processes : EntityBase, IIdentifiableEntity, ITenantEntity
    {
        [DataMember]
        [Browsable (false)]
        public int ProcessId { get; set; }

        [DataMember]
        [Required]
        public string Title { get; set; }

        [DataMember]
        [Required]
        public PackageRunType RunType { get; set; }

        [DataMember]
        [Required]
        public string PackageName { get; set; }

        [DataMember]
        [Required]
        public string PackagePath { get; set; }  

        [DataMember]
        [Required]
        public int ModuleId { get; set; }

        [DataMember]
        [Required]
        public int Position { get; set; }

      

        public int EntityId
        {
            get
            {
                return ProcessId;
            }
        }
    }
}
