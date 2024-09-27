using Fintrak.Model.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fintrak.Data.Core.Interface
{
    public class ExtractionRoleInfo
    {
        public ExtractionRole ExtractionRole { get; set; }
        public Extraction Extraction { get; set; }
    }
}
