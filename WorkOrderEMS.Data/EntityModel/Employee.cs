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
    
    public partial class Employee
    {
        public long EMP_Id { get; set; }
        public string EMP_EmployeeID { get; set; }
        public Nullable<long> EMP_API_ApplicantId { get; set; }
        public string EMP_FirstName { get; set; }
        public string EMP_MiddleName { get; set; }
        public string EMP_LastName { get; set; }
        public string EMP_Email { get; set; }
        public Nullable<long> EMP_Phone { get; set; }
        public string EMP_DrivingLicenseNumber { get; set; }
        public Nullable<System.DateTime> EMP_DateOfBirth { get; set; }
        public string EMP_SSN { get; set; }
        public string EMP_Photo { get; set; }
        public string EMP_MilitaryService { get; set; }
        public string EMP_Gender { get; set; }
        public Nullable<long> EMP_JobTitleId { get; set; }
        public string EMP_ManagerId { get; set; }
        public Nullable<System.DateTime> EMP_DateOfJoining { get; set; }
        public Nullable<long> EMP_LocationId { get; set; }
        public Nullable<long> EMP_IsCreatedBy { get; set; }
        public Nullable<System.DateTime> EMP_IsCreatedOn { get; set; }
        public string EMP_IsActive { get; set; }
    }
}
