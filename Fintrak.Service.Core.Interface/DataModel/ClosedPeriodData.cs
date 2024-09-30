using Fintrak.Model.Core.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Fintrak.Service.Core.Interface
{
    public class ClosedPeriodData
    {
        [DataMember]
        public int ClosedPeriodId { get; set; }

        [DataMember]
        public int SolutionId { get; set; }

        [DataMember]
        public string SolutionName { get; set; }

        [DataMember]
        public DateTime Date { get; set; }

        [DataMember]
        public int Month { get; set; }

        [DataMember]
        public int Year { get; set; }

        [DataMember]
        public bool Status { get; set; }

        [DataMember]
        public bool Active { get; set; }

        [DataMember]
        public bool Deleted { get; set; }
    }
}
