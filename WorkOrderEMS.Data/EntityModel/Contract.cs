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
    
    public partial class Contract
    {
        public long CNT_Id { get; set; }
        public long CNT_CMP_IdFirsParty { get; set; }
        public long CNT_CMP_IdSecondParty { get; set; }
        public long CNT_CTT_Id { get; set; }
        public string CNT_ExcutedBy { get; set; }
        public string CNT_IssuedBy { get; set; }
        public long CNT_PTM_Id { get; set; }
        public long CNT_PMD_Id { get; set; }
        public Nullable<int> CNT_GracePeriod { get; set; }
        public string CNT_invoicingFrequency { get; set; }
        public Nullable<int> CNT_CostDuringPeriod { get; set; }
        public Nullable<int> CNT_AllocationNeeded { get; set; }
        public System.DateTime CNT_StartDate { get; set; }
        public System.DateTime CNT_EndDate { get; set; }
        public Nullable<decimal> CNT_AnnualValueOfAggreement { get; set; }
        public Nullable<int> CNT_MinimumBillAmount { get; set; }
        public Nullable<System.DateTime> CNT_BillDueDate { get; set; }
        public Nullable<decimal> CNT_LateFeeFine { get; set; }
        public string CNT_ContractDocument { get; set; }
        public string CNT_IsActive { get; set; }
        public string CNT_IsReoccurring { get; set; }
    }
}
