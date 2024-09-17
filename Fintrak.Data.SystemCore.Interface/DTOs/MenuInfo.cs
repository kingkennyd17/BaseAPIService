using Fintrak.Model.SystemCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fintrak.Data.SystemCore.Interface
{
    public class MenuInfo
    {
        public Menu Menu { get; set; }
        public Module Module { get; set; }
        public Menu Parent { get; set; }
    }
}
