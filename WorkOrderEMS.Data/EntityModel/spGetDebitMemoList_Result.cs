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
    
    public partial class spGetDebitMemoList_Result
    {
        public long DBM_Id { get; set; }
        public long DBM_LocationId { get; set; }
        public string LocationName { get; set; }
        public Nullable<long> LCM_CMP_Id { get; set; }
        public Nullable<long> CMP_Id { get; set; }
        public string CMP_NameLegal { get; set; }
        public Nullable<long> DBM_PurchaseOrder { get; set; }
        public long DBM_DebitAmount { get; set; }
        public string DBM_Note { get; set; }
        public Nullable<int> DBM_Status { get; set; }
        public string DBM_DocumentName { get; set; }
        public Nullable<System.DateTime> DBM_CreatedDate { get; set; }
    }
}
