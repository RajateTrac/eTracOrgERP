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
    
    public partial class SP_GetUser_Result
    {
        public long UserId { get; set; }
        public string Password { get; set; }
        public string UserEmail { get; set; }
        public string AlternateEmail { get; set; }
        public string SubscriptionEmail { get; set; }
        public long UserType { get; set; }
        public Nullable<long> ProjectID { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public long Gender { get; set; }
        public string GenderName { get; set; }
        public Nullable<System.DateTime> DOB { get; set; }
        public string ProfileImage { get; set; }
        public bool IsLoginActive { get; set; }
        public bool IsEmailVerify { get; set; }
        public string BloodGroup { get; set; }
        public Nullable<System.DateTime> HiringDate { get; set; }
        public Nullable<long> AddressMasterId { get; set; }
        public string Address1 { get; set; }
        public string Address2 { get; set; }
        public string City { get; set; }
        public Nullable<int> StateId { get; set; }
        public Nullable<int> CountryId { get; set; }
        public string Mobile { get; set; }
        public string Phone { get; set; }
        public string ZipCode { get; set; }
        public string EmployeeID { get; set; }
        public string JobTitle { get; set; }
        public string JobTitleOther { get; set; }
    }
}
