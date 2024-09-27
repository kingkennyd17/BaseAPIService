using Fintrak.Model.Core.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Fintrak.Service.Core.Interface
{
    public class ExtractionData
    {
        [DataMember]
        public int ExtractionId { get; set; }

        [DataMember]
        public string Title { get; set; }

        [DataMember]
        public PackageRunType RunType { get; set; }

        [DataMember]
        public string RunTypeName { get; set; }

        [DataMember]
        public string PackageName { get; set; }

        [DataMember]
        public string PackagePath { get; set; }

        [DataMember]
        public string ProcedureName { get; set; }

        [DataMember]
        public string ScriptText { get; set; }

        [DataMember]
        public string NeedArchiveAction { get; set; }

        [DataMember]
        public int SolutionId { get; set; }

        [DataMember]
        public string SolutionName { get; set; }

        [DataMember]
        public int Position { get; set; }

        [DataMember]
        public bool Active { get; set; }
    }
}
