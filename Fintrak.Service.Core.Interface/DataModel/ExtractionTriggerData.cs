using Fintrak.Model.Core.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Fintrak.Service.Core.Interface
{
    public class ExtractionTriggerData
    {
        [DataMember]
        public int ExtractionTriggerId { get; set; }

        [DataMember]
        public int ExtractionJobId { get; set; }

        [DataMember]
        public string ExtractionJobCode { get; set; }

        [DataMember]
        public int ExtractionId { get; set; }

        [DataMember]
        public string ExtractionTitle { get; set; }

        [DataMember]
        public string Code { get; set; }

        [DataMember]
        public PackageStatus Status { get; set; }

        [DataMember]
        public string StatusName { get; set; }

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

        [DataMember]
        public bool Active { get; set; }
    }
}
