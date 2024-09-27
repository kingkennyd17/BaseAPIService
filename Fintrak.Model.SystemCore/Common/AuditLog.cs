using Fintrak.Shared.Common;
using Fintrak.Shared.Common.Interface;
using Fintrak.Shared.Common.Tenancy;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Fintrak.Model.SystemCore
{
    public class AuditLog : EntityBase, IIdentifiableEntity, ITenantEntity
    {
        [DataMember]
        [Browsable(false)]
        public int Id { get; set; }

        [DataMember]
        public int? UserId { get; set; }

        [DataMember]
        public string UserName { get; set; }

        [DataMember]
        public string Action { get; set; }

        [DataMember]
        public string TableName { get; set; }

        [DataMember]
        public DateTime Timestamp { get; set; }

        [DataMember]
        public string? OldValues { get; set; }

        [DataMember]
        public string? NewValues { get; set; }

        [DataMember]
        public string? AffectedColumns { get; set; }

        [DataMember]
        public string? IpAddress { get; set; }

        public int EntityId
        {
            get
            {
                return Id;
            }
        }
    }

}
