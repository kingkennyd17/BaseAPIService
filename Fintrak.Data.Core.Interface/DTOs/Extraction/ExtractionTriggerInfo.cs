using Fintrak.Model.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fintrak.Data.Core.Interface
{
    public class ExtractionTriggerInfo
    {
        public ExtractionTrigger ExtractionTrigger { get; set; }
        public ExtractionJob ExtractionJob { get; set; }
        public Extraction Extraction { get; set; }
    }
}
