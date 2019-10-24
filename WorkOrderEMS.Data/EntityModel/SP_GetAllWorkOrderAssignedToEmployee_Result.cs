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
    
    public partial class SP_GetAllWorkOrderAssignedToEmployee_Result
    {
        public long WorkRequestAssignmentID { get; set; }
        public Nullable<long> WorkRequestType { get; set; }
        public Nullable<long> AssetID { get; set; }
        public long LocationID { get; set; }
        public string ProblemDesc { get; set; }
        public Nullable<long> PriorityLevel { get; set; }
        public string WorkRequestImage { get; set; }
        public Nullable<bool> SafetyHazard { get; set; }
        public string ProjectDesc { get; set; }
        public Nullable<long> WorkRequestStatus { get; set; }
        public long RequestBy { get; set; }
        public Nullable<long> AssignToUserId { get; set; }
        public Nullable<long> AssignByUserId { get; set; }
        public string Remarks { get; set; }
        public long CreatedBy { get; set; }
        public System.DateTime CreatedDate { get; set; }
        public Nullable<long> ModifiedBy { get; set; }
        public Nullable<System.DateTime> ModifiedDate { get; set; }
        public Nullable<bool> IsDeleted { get; set; }
        public Nullable<long> DeletedBy { get; set; }
        public Nullable<System.DateTime> DeletedDate { get; set; }
        public long WorkRequestProjectType { get; set; }
        public string AssignedWorkOrderImage { get; set; }
        public Nullable<System.DateTime> StartTime { get; set; }
        public Nullable<System.DateTime> EndTime { get; set; }
        public Nullable<System.DateTime> AssignedTime { get; set; }
        public string WorkStatusDesc { get; set; }
        public string WorkOrderCode { get; set; }
        public long WorkOrderCodeID { get; set; }
        public string WorkRequestType1 { get; set; }
        public string LocationName { get; set; }
        public string PriorityLevel1 { get; set; }
        public string AssignedByUserName { get; set; }
        public string RequestStatus { get; set; }
    }
}
