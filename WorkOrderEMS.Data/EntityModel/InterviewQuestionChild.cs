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
    
    public partial class InterviewQuestionChild
    {
        public long IQC_Id { get; set; }
        public long IQC_IQM_Id { get; set; }
        public string IQC_Question { get; set; }
        public Nullable<int> IQC_ScoreYes { get; set; }
        public Nullable<int> IQC_ScoreNo { get; set; }
        public Nullable<System.DateTime> IQC_Date { get; set; }
        public string IQC_IsActive { get; set; }
    
        public virtual InterviewQuestionMaster InterviewQuestionMaster { get; set; }
    }
}
