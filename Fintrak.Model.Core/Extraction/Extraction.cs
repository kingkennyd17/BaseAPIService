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
    public class Extraction : EntityBase, IIdentifiableEntity, ITenantEntity
    {
        [DataMember]
        [Browsable(false)]
        public int ExtractionId { get; set; }

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
        public string ProcedureName { get; set; }

        [DataMember]
        [Required]
        public string ScriptText { get; set; }

        [DataMember]
        public string NeedArchiveAction { get; set; }

        [DataMember]
        [Required]
        public int SolutionId { get; set; }

        [DataMember]
        [Required]
        public int Position { get; set; }


        public int EntityId
        {
            get
            {
                return ExtractionId;
            }
        }
    }

}
