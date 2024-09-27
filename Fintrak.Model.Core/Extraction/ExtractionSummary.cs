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
    public class ExtractionSummary : EntityBase, IIdentifiableEntity, ITenantEntity
    {
        [DataMember]
        [Browsable(false)]
        public int Id { get; set; }

        [DataMember]
        [Required]
        public string Activity { get; set; }


        [DataMember]
        public int Count { get; set; }


        [DataMember]
        public string Status { get; set; }


        public int EntityId
        {
            get
            {
                return Id;
            }
        }
    }

}
