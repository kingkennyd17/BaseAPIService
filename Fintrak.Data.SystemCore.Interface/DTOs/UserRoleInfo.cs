using Fintrak.Model.SystemCore.Common;
using Fintrak.Model.SystemCore;

namespace Fintrak.Data.SystemCore.Interface
{
    public class UserRoleInfo
    {
        public UserRole UserRole { get; set; }
        public UserSetup UserSetup { get; set; }
        public Roles Role { get; set; }
        public Solution Solution { get; set; }
    }
}
