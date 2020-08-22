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
    
    public partial class USP_GetInvoiceDataForCreditMemo_Result
    {
        public long InvoiceId { get; set; }
        public string InvoiceNumber { get; set; }
        public Nullable<System.DateTime> InvoiceDate { get; set; }
        public Nullable<System.DateTime> InvoiceDueDate { get; set; }
        public Nullable<int> InvoiceType { get; set; }
        public string InvoiceStatus { get; set; }
        public Nullable<long> CMP_Id { get; set; }
        public Nullable<long> CompanyId { get; set; }
        public string ClientCompanyName { get; set; }
        public Nullable<long> ClientLocationCode { get; set; }
        public string LocationCode { get; set; }
        public string LocationAddress { get; set; }
        public Nullable<long> ContractType { get; set; }
        public string ContractTypeDesc { get; set; }
        public string ClientPointOfContactName { get; set; }
        public string PositionTitle { get; set; }
        public Nullable<long> InvoicePaymentTerms { get; set; }
        public string InvoicePaymentTermsDesc { get; set; }
        public Nullable<long> ReasonForOffScheduleInvoice { get; set; }
        public Nullable<decimal> SubTotal { get; set; }
        public Nullable<decimal> TaxPercentage { get; set; }
        public Nullable<decimal> TaxAmount { get; set; }
        public Nullable<decimal> GrandTotal { get; set; }
        public string Comment { get; set; }
        public string IsActive { get; set; }
        public Nullable<long> EntryBy { get; set; }
        public Nullable<System.DateTime> EntryOn { get; set; }
        public Nullable<long> ModifiedBy { get; set; }
        public Nullable<System.DateTime> ModifiedOn { get; set; }
        public Nullable<long> ApprovedBy { get; set; }
        public Nullable<System.DateTime> ApprovedOn { get; set; }
        public string InvoiceDocument { get; set; }
        public string InvoiceStatusDesc { get; set; }
        public string InvoiceStatusDescForCredit { get; set; }
    }
}
