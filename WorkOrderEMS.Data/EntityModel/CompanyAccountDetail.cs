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
    
    public partial class CompanyAccountDetail
    {
        public CompanyAccountDetail()
        {
            this.LogCompanyAccountDetails = new HashSet<LogCompanyAccountDetail>();
        }
    
        public long CAD_Id { get; set; }
        public long CAD_CMP_Id { get; set; }
        public long CAD_PMD_Id { get; set; }
        public string CAD_CardOrBankName { get; set; }
        public string CAD_BankLocation { get; set; }
        public string CAD_AccountNumber { get; set; }
        public string CAD_CreditCardNumber { get; set; }
        public string CAD_IFSCcode { get; set; }
        public string CAD_SwiftBICcode { get; set; }
        public string CAD_AccountDocument { get; set; }
        public string CAD_IsActive { get; set; }
        public Nullable<decimal> CAD_Balance { get; set; }
        public Nullable<long> CAD_QBKId { get; set; }
        public string CAD_CardHolderName { get; set; }
        public Nullable<System.DateTime> CAD_CardExpirationDate { get; set; }
        public string CAD_IsPrimary { get; set; }
    
        public virtual PaymentMode PaymentMode { get; set; }
        public virtual ICollection<LogCompanyAccountDetail> LogCompanyAccountDetails { get; set; }
        public virtual Company Company { get; set; }
    }
}
