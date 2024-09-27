using Fintrak.Shared.Common;
using Fintrak.Shared.Common.Interface;
using Fintrak.Shared.Common.Tenancy;
using System.ComponentModel;
using System.Runtime.Serialization;

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
