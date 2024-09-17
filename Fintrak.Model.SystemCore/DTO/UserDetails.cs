using Fintrak.Model.SystemCore.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fintrak.Model.SystemCore.DTO
{
    public class UserDetail
    {
        public UserSetup User { get; set; }
        public List<string> Roles { get; set; }
    }
}
