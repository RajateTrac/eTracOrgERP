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
    
    public partial class Tbl_Customer_Vehicle_Details
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Tbl_Customer_Vehicle_Details()
        {
            this.Tbl_Log_Customer_Vehicle_Details = new HashSet<Tbl_Log_Customer_Vehicle_Details>();
            this.Tbl_Log_Customer_Vehicle_Details1 = new HashSet<Tbl_Log_Customer_Vehicle_Details>();
            this.Tbl_Log_Customer_Vehicle_Details2 = new HashSet<Tbl_Log_Customer_Vehicle_Details>();
        }
    
        public long Id { get; set; }
        public Nullable<long> CMP_Id { get; set; }
        public Nullable<int> SrNo { get; set; }
        public string LicensePlateNo { get; set; }
        public Nullable<int> State { get; set; }
        public string Year { get; set; }
        public string Make { get; set; }
        public string Model { get; set; }
        public string Color { get; set; }
        public string IsActive { get; set; }
    
        public virtual Company Company { get; set; }
        public virtual Company Company1 { get; set; }
        public virtual Company Company2 { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Tbl_Log_Customer_Vehicle_Details> Tbl_Log_Customer_Vehicle_Details { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Tbl_Log_Customer_Vehicle_Details> Tbl_Log_Customer_Vehicle_Details1 { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Tbl_Log_Customer_Vehicle_Details> Tbl_Log_Customer_Vehicle_Details2 { get; set; }
    }
}
