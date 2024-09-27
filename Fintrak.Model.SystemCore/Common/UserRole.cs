using Fintrak.Model.SystemCore.Common;
using Microsoft.AspNetCore.Identity;

namespace Fintrak.Model.SystemCore
{
    public class UserRole : IdentityUserRole<int>
    {
        public UserSetup User { get; set; }  // Navigation property
        public Roles Role { get; set; }  // Navigation property
    }

}
