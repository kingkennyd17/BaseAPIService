using Fintrak.Model.SystemCore.Enum;
using Fintrak.Shared.Common.Interface;
using Fintrak.Shared.Common;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;
using System.ComponentModel.DataAnnotations.Schema;

namespace Fintrak.Model.SystemCore.Common
{
    public class Roles : IdentityRole<int>, IIdentifiableEntity
    {
        [DataMember]
        public string? Description { get; set; }

        [DataMember]
        public RoleType Type { get; set; }

        [DataMember]
        [Required]
        public int SolutionId { get; set; }

        [XmlIgnore]
        [DataMember]
        [Timestamp]
        public byte[]? RowVersion { get; set; }

        public int EntityId
        {
            get
            {
                return Id;
            }
        }

        public virtual ICollection<UserRole> UserRoles { get; set; }
    }

}
