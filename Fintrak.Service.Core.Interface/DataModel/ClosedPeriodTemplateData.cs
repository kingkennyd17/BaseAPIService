using Fintrak.Model.Core.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Fintrak.Service.Core.Interface
{
    public class ClosedPeriodTemplateData
    {
        [DataMember]
        public int ClosedPeriodTemplateId { get; set; }

        [DataMember]
        public int SolutionId { get; set; }

        [DataMember]
        public string SolutionName { get; set; }

        [DataMember]
        public string Action { get; set; }

        [DataMember]
        public bool Active { get; set; }
    }
}
