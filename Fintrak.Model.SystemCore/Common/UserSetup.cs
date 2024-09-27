using Fintrak.Model.SystemCore.Enum;
using Fintrak.Shared.Common.Tenancy;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace Fintrak.Model.SystemCore.Common
{
    public class UserSetup : IdentityUser<int>, ITenantEntity
    {
        [DataMember]
        public ApprovalStatus ApprovalStatus { get; set; } = ApprovalStatus.Pending;

        [DataMember]
        public string? CurrentSessionId { get; set; }

        [DataMember]
        public string? Name { get; set; }

        [DataMember]
        public string? StaffID { get; set; }

        [DataMember]
        public bool MultiCompanyAccess { get; set; }

        [DataMember]
        public DateTime LatestConnection { get; set; }

        [DataMember]
        public string? PhotoUrl { get; set; }

        [XmlIgnore]
        [DataMember]
        public byte[]? Photo { get; set; }

        [DataMember]
        public bool IsApplicationUser { get; set; }

        [DataMember]
        public bool IsReportUser { get; set; }

        [DataMember]
        public string? CompanyCode { get; set; }

        [DataMember]
        public bool Active { get; set; }

        [DataMember]
        public bool Deleted { get; set; }

        [DataMember]
        public string? CreatedBy { get; set; }

        [DataMember]
        public DateTime CreatedOn { get; set; }

        [DataMember]
        public string? UpdatedBy { get; set; }

        [DataMember]
        public DateTime UpdatedOn { get; set; }

        [XmlIgnore]
        [DataMember]
        [Timestamp]
        public byte[]? RowVersion { get; set; }

        [DataMember]
        public string TenantId { get; set; }

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
