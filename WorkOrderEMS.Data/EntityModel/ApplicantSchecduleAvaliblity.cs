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
    
    public partial class ApplicantSchecduleAvaliblity
    {
        public long ASA_Id { get; set; }
        public long ASA_APT_ApplicantId { get; set; }
        public string ASA_EMP_EmployeeId { get; set; }
        public string ASA_WeekDay { get; set; }
        public Nullable<System.TimeSpan> ASA_AvaliableStartTime { get; set; }
        public Nullable<System.TimeSpan> ASA_AvaliableEndTime { get; set; }
        public Nullable<System.DateTime> ASA_Date { get; set; }
        public string ASA_IsActive { get; set; }
        public Nullable<long> ASA_AvaliableUserLocation { get; set; }
    }
}
