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
    
    public partial class OfferLetter
    {
        public long OFL_Id { get; set; }
        public long OFL_APT_ApplicantId { get; set; }
        public Nullable<System.DateTime> OFL_OfferLetterDueDate { get; set; }
        public Nullable<decimal> OFL_OfferAmount { get; set; }
        public Nullable<System.DateTime> OFL_Date { get; set; }
        public string OFL_IsActive { get; set; }
    }
}
