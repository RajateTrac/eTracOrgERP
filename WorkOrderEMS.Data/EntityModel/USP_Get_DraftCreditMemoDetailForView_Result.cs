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
    
    public partial class USP_Get_DraftCreditMemoDetailForView_Result
    {
        public long Id { get; set; }
        public string DraftCreditMemoNumber { get; set; }
        public string InvoiceNumber { get; set; }
        public Nullable<System.DateTime> CreditIssuedDate { get; set; }
        public string CreditIssuedDateDisplay { get; set; }
        public Nullable<System.DateTime> InvoiceDate { get; set; }
        public string InvoiceDateDisplay { get; set; }
        public string CreditMemoStatus { get; set; }
        public string CreditMemoStatusDesc { get; set; }
        public string InvoiceStatus { get; set; }
        public Nullable<int> CreditMemoType { get; set; }
        public string CreditMemoTypeDesc { get; set; }
        public Nullable<long> CMP_Id { get; set; }
        public Nullable<long> ClientLocationCode { get; set; }
        public string ClientLocationName { get; set; }
        public string ClientCompanyName { get; set; }
        public string LocationCode { get; set; }
        public string LocationAddress { get; set; }
        public string ClientPointOfContactName { get; set; }
        public string PositionTitle { get; set; }
        public string InvoicePaymentTermsDesc { get; set; }
        public Nullable<decimal> SubTotal { get; set; }
        public Nullable<decimal> TaxPercentage { get; set; }
        public Nullable<decimal> TaxAmount { get; set; }
        public Nullable<decimal> GrandTotal { get; set; }
        public Nullable<decimal> PendingAmount { get; set; }
        public string Comment { get; set; }
        public string IsActive { get; set; }
        public Nullable<long> EntryBy { get; set; }
        public Nullable<System.DateTime> EntryOn { get; set; }
        public Nullable<long> ModifiedBy { get; set; }
        public Nullable<System.DateTime> ModifiedOn { get; set; }
        public Nullable<long> ApprovedBy { get; set; }
        public Nullable<System.DateTime> ApprovedOn { get; set; }
        public string InvoiceDocument { get; set; }
        public string EntryOnDisplay { get; set; }
        public string ModifiedOnDisplay { get; set; }
        public string EntryByDisplay { get; set; }
        public string ModifiedByDisplay { get; set; }
        public Nullable<decimal> GrandTotal1 { get; set; }
        public string InvoiceDocument1 { get; set; }
        public string InvoiceLastSenttoclientDate { get; set; }
        public string ApprovedByDisplay { get; set; }
        public string ApprovedOnDisplay { get; set; }
    }
}
