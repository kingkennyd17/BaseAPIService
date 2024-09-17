using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fintrak.Model.SystemCore
{
    public class NewTenant
    {
        //public string UserId { get; set; }
        public string Username { get; set; }
        public string Email { get; set; }
        public string Name { get; set; }
        //public bool IsApplicationUser { get; set; }
        public string StaffID { get; set; }
        public string Password { get; set; }
        public string TenantId { get; set; }
        //public List<string> Roles { get; set; }
    }
}
