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
    
    public partial class AssetAllocation
    {
        public long ATA_Id { get; set; }
        public string ATA_EMP_EmployeeId { get; set; }
        public string ATA_Type { get; set; }
        public string ATA_AssetName { get; set; }
        public string ATA_AssetDescription { get; set; }
        public Nullable<System.DateTime> ATA_Make { get; set; }
        public string ATA_Model { get; set; }
        public string ATA_SerialNumber { get; set; }
        public string ATA_Login { get; set; }
        public string ATA_Password { get; set; }
        public Nullable<System.DateTime> ATA_AssignDate { get; set; }
        public string ATA_ReturnAcceptBy { get; set; }
        public Nullable<System.DateTime> ATA_ReturnDate { get; set; }
        public string ATA_ReturnStatus { get; set; }
        public Nullable<System.DateTime> ATA_Date { get; set; }
        public string ATA_IsActive { get; set; }
    }
}
