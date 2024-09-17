using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Fintrak.Service.SystemCore.Interface.DataModel
{
    [DataContract]
    public class MenuData
    {
        [DataMember]
        public int MenuId { get; set; }

        [DataMember]
        public string Name { get; set; }

        [DataMember]
        public string Alias { get; set; }

        [DataMember]
        public string Action { get; set; }

        [DataMember]
        public string ActionUrl { get; set; }

        [DataMember]
        public byte[]? Image { get; set; }

        [DataMember]
        public string? ImageUrl { get; set; }

        [DataMember]
        public int? ParentId { get; set; }

        [DataMember]
        public string? ParentName { get; set; }

        [DataMember]
        public int? ModuleId { get; set; }

        [DataMember]
        public string? ModuleName { get; set; }

        [DataMember]
        public int? Position { get; set; }

        [DataMember]
        public bool Active { get; set; }
    }
}
