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
    
    public partial class spGetEmployeeStatusChangeList_Result
    {
        public long ESC_Id { get; set; }
        public string ESC_EMP_EmployeeId { get; set; }
        public string ESC_ChangeType { get; set; }
        public string ESC_ApprovedBy { get; set; }
        public string ESC_ApprovalStatus { get; set; }
        public Nullable<System.DateTime> ESC_Date { get; set; }
    }
}
