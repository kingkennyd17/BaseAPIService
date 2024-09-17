using Fintrak.Model.SystemCore.Common;
using Fintrak.Model.SystemCore.Tenancy;
using Fintrak.Shared.Common;
using Fintrak.Shared.Common.Interface;
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
    public class MenuRole : EntityBase, IIdentifiableEntity, ITenantEntity
    {
        [DataMember]
        [Browsable(false)]
        public int MenuRoleId { get; set; }

        [DataMember]
        [Required]
        public int MenuId { get; set; }

        [DataMember]
        [Required]
        public int RoleId { get; set; }

        public int EntityId
        {
            get
            {
                return MenuRoleId;
            }
        }


        public Roles Role { get; set; }  // Navigation property
        public Menu Menu { get; set; }  // Navigation property
    }

}
