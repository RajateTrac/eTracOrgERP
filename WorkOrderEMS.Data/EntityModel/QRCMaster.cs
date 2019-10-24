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
    
    public partial class QRCMaster
    {
        public QRCMaster()
        {
            this.QRCMasterLogs = new HashSet<QRCMasterLog>();
            this.QRCScanLogs = new HashSet<QRCScanLog>();
            this.UserRegistrations = new HashSet<UserRegistration>();
        }
    
        public long QRCID { get; set; }
        public string QRCName { get; set; }
        public string SpecialNotes { get; set; }
        public long QRCDefaultSize { get; set; }
        public long QRCTYPE { get; set; }
        public long ModuleNameId { get; set; }
        public long CreatedBy { get; set; }
        public System.DateTime CreatedDate { get; set; }
        public Nullable<long> ModifiedBy { get; set; }
        public Nullable<System.DateTime> ModifiedDate { get; set; }
        public bool IsDeleted { get; set; }
        public Nullable<long> DeletedBy { get; set; }
        public Nullable<System.DateTime> DeletedDate { get; set; }
        public Nullable<long> MotorType { get; set; }
        public Nullable<long> VendorID { get; set; }
        public Nullable<long> ClientTypeID { get; set; }
        public Nullable<long> VehicleType { get; set; }
        public string QRCodeID { get; set; }
        public string ModelNo { get; set; }
        public string SerialNo { get; set; }
        public string Make { get; set; }
        public string Model { get; set; }
        public string AssetPicture { get; set; }
        public string LocationPicture { get; set; }
        public string VendorName { get; set; }
        public string PointOfContact { get; set; }
        public string TelephoneNo { get; set; }
        public string EmialAdd { get; set; }
        public Nullable<System.DateTime> WarrantyEndDate { get; set; }
        public string Website { get; set; }
        public Nullable<long> PurchaseType { get; set; }
        public string WarrantyDoc { get; set; }
        public Nullable<System.DateTime> InsuranceExpDate { get; set; }
        public string PurchaseTypeRemark { get; set; }
        public string QRCTypeDetails { get; set; }
        public Nullable<long> AllotedTo { get; set; }
        public Nullable<long> LocationId { get; set; }
        public Nullable<bool> CheckOutStatus { get; set; }
        public string UserName { get; set; }
        public Nullable<bool> ExpirationStatus { get; set; }
        public Nullable<bool> IsDamage { get; set; }
        public Nullable<bool> IsDamageVerified { get; set; }
        public Nullable<long> DamageVerifiedBy { get; set; }
        public string QRCImage { get; set; }
    
        public virtual GlobalCode GlobalCode { get; set; }
        public virtual GlobalCode GlobalCode1 { get; set; }
        public virtual GlobalCode GlobalCode2 { get; set; }
        public virtual GlobalCode GlobalCode3 { get; set; }
        public virtual GlobalCode GlobalCode4 { get; set; }
        public virtual GlobalCode GlobalCode5 { get; set; }
        public virtual LocationMaster LocationMaster { get; set; }
        public virtual ICollection<QRCMasterLog> QRCMasterLogs { get; set; }
        public virtual ICollection<QRCScanLog> QRCScanLogs { get; set; }
        public virtual ICollection<UserRegistration> UserRegistrations { get; set; }
    }
}
