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
    
    public partial class Tbl_Customer_Payment_Details
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Tbl_Customer_Payment_Details()
        {
            this.Tbl_Log_Customer_Payment_Details = new HashSet<Tbl_Log_Customer_Payment_Details>();
            this.Tbl_Log_Customer_Payment_Details1 = new HashSet<Tbl_Log_Customer_Payment_Details>();
            this.Tbl_Log_Customer_Payment_Details2 = new HashSet<Tbl_Log_Customer_Payment_Details>();
        }
    
        public long Id { get; set; }
        public long CMP_Id { get; set; }
        public string IsMonthlyParkingPaidFor { get; set; }
        public string CompanyName { get; set; }
        public Nullable<bool> IsSendDirectInvoiceToCompany { get; set; }
        public string CompanyEmail { get; set; }
        public string PaymentMethod { get; set; }
        public string AccountNumber { get; set; }
        public string BankName { get; set; }
        public string IFSCcode { get; set; }
        public string BankRoutingNo { get; set; }
        public string CardHolderName { get; set; }
        public string Address { get; set; }
        public string CardNumber { get; set; }
        public string CardType { get; set; }
        public Nullable<System.DateTime> CardExpirationDate { get; set; }
        public string SwiftBICcode { get; set; }
        public Nullable<bool> IsSignupForAutomaticPayment { get; set; }
        public string IsActive { get; set; }
    
        public virtual Company Company { get; set; }
        public virtual Company Company1 { get; set; }
        public virtual Company Company2 { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Tbl_Log_Customer_Payment_Details> Tbl_Log_Customer_Payment_Details { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Tbl_Log_Customer_Payment_Details> Tbl_Log_Customer_Payment_Details1 { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Tbl_Log_Customer_Payment_Details> Tbl_Log_Customer_Payment_Details2 { get; set; }
    }
}
