//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace WorkOrderEMS.Data.EntityModel
{
    using System;
    using System.Collections.Generic;
    
    public partial class AplicantCertificationDetail
    {
        public long ACD_Id { get; set; }
        public Nullable<long> ACD_APT_ApplicantId { get; set; }
        public string ACD_CertificationName { get; set; }
        public Nullable<System.DateTime> ACD_CertificationEarnedYear { get; set; }
        public string ACD_CertifyingAgency { get; set; }
        public string ACD_CertificateAttached { get; set; }
        public string ACD_Address { get; set; }
        public string ACD_City { get; set; }
        public string ACD_State { get; set; }
        public Nullable<int> ACD_Zip { get; set; }
        public Nullable<System.DateTime> ACD_Date { get; set; }
        public string ACD_IsActive { get; set; }
    }
}
