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
    
    public partial class spGetApplicantInfo_Result2
    {
        public long API_ApplicantId { get; set; }
        public string API_FirstName { get; set; }
        public string API_MiddleName { get; set; }
        public string API_LastName { get; set; }
        public string API_HomeAddress { get; set; }
        public string API_City { get; set; }
        public string API_State { get; set; }
        public Nullable<int> API_Zip { get; set; }
        public string API_Resume { get; set; }
        public string API_CoverLetter { get; set; }
        public long API_PhoneNumber { get; set; }
        public string API_Email { get; set; }
        public string API_DLNumber { get; set; }
        public string API_Photo { get; set; }
        public string API_Citizenship { get; set; }
        public string API_MilitaryService { get; set; }
        public string API_Gender { get; set; }
        public string API_HighestEducation { get; set; }
        public string API_AnyRefOrEmployeeInELITE { get; set; }
        public Nullable<System.DateTime> API_EverELITEWorkFrom { get; set; }
        public Nullable<System.DateTime> API_EverELITEWorkTo { get; set; }
        public string API_WorkEligibleInUS { get; set; }
        public Nullable<long> JPS_JobPostingId { get; set; }
        public Nullable<System.DateTime> JobPostingDate { get; set; }
        public Nullable<long> VST_Id { get; set; }
        public Nullable<long> API_JobTitleID { get; set; }
        public string API_HiringManagerId { get; set; }
        public Nullable<System.DateTime> API_DateOfJoining { get; set; }
        public Nullable<decimal> API_DesireSalary { get; set; }
        public string API_ApplicantStatus { get; set; }
        public string API_IsActive { get; set; }
        public string VST_Title { get; set; }
        public string JBT_JobTitle { get; set; }
        public string HiringManager { get; set; }
        public string INS_EMP_InterviewerEmployeeId { get; set; }
        public string INS_IsHiringManager { get; set; }
        public Nullable<System.DateTime> INS_InterviewDateTime { get; set; }
    }
}
