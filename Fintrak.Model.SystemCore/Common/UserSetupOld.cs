using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;
using Fintrak.Shared.Common;

namespace Fintrak.Model.SystemCore
{
    public class UserSetupOld : EntityBase
    {
        [DataMember]
        [Browsable(false)]
        public int UserSetupId { get; set; }

        [DataMember]
        [Required]
        public string LoginID { get; set; }

        [DataMember]
        [Required]
        public string Name { get; set; }

        [DataMember]
        [Required]
        public string Email { get; set; }

        [DataMember]
        public string StaffID { get; set; }

        [DataMember]
        public bool MultiCompanyAccess { get; set; }

        [DataMember]
        public DateTime LatestConnection { get; set; }

        [DataMember]
        public string PhotoUrl { get; set; }

        [XmlIgnore]
        [DataMember]
        public byte[] Photo { get; set; }

        [DataMember]
        public bool IsApplicationUser { get; set; }

        [DataMember]
        public bool IsReportUser { get; set; }

        [DataMember]
        public string CompanyCode { get; set; }

        [DataMember]
        public bool? LogOut { get; set; }

        [DataMember]
        public DateTime? LastIdleTime { get; set; }

    }
}
