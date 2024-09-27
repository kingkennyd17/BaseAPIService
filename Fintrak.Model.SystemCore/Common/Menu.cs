using Fintrak.Shared.Common;
using Fintrak.Shared.Common.Interface;
using Fintrak.Shared.Common.Tenancy;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace Fintrak.Model.SystemCore
{
    public class Menu : EntityBase, IIdentifiableEntity, ITenantEntity
    {
        [DataMember]
        [Browsable(false)]
        public int MenuId { get; set; }

        [DataMember]
        [Required]
        public string Name { get; set; }

        [DataMember]
        [Required]
        public string Alias { get; set; }

        [DataMember]
        public string Action { get; set; }

        [DataMember]
        public string ActionUrl { get; set; }

        [DataMember]
        [XmlIgnore]
        public byte[]? Image { get; set; }

        [DataMember]
        public string? ImageUrl { get; set; }

        [DataMember]
        public int? ParentId { get; set; }

        [DataMember]
        [Required]
        public int? ModuleId { get; set; }

        [DataMember]
        public int? Position { get; set; }

        public int EntityId
        {
            get
            {
                return MenuId;
            }
        }

        public Menu ParentMenu { get; set; }  // Self-referencing navigation property for parent menu
        public ICollection<Menu> SubMenus { get; set; }  // Collection navigation for child menus
    }

}
