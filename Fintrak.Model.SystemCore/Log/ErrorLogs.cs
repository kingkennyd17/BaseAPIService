using Fintrak.Model.SystemCore.Tenancy;
using Fintrak.Shared.Common;
using Fintrak.Shared.Common.Interface;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Fintrak.Model.SystemCore
{
    public class ErrorLogs : EntityBase, IIdentifiableEntity, ITenantEntity
    {
        [DataMember]
        [Browsable(false)]
        public int Id { get; set; }

        [DataMember]
        public string? ExceptionMessage { get; set; }

        [DataMember]
        public string? StackTrace { get; set; }

        [DataMember]
        public DateTime Timestamp { get; set; }


        public int EntityId
        {
            get
            {
                return Id;
            }
        }
    }

}
