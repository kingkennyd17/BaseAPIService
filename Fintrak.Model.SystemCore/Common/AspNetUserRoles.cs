using Fintrak.Model.SystemCore.Common;
using Fintrak.Model.SystemCore.Tenancy;
using Fintrak.Shared.Common;
using Fintrak.Shared.Common.Interface;
using Microsoft.AspNet.Identity;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Fintrak.Model.SystemCore
{
    public class UserRole : IdentityUserRole<int>
    {
        public UserSetup User { get; set; }  // Navigation property
        public Roles Role { get; set; }  // Navigation property
    }

}
