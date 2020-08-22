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
    
    public partial class GlobalCode
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public GlobalCode()
        {
            this.DashboardWidgetSettings = new HashSet<DashboardWidgetSetting>();
            this.eFleetFuelings = new HashSet<eFleetFueling>();
            this.eFleetMaintenances = new HashSet<eFleetMaintenance>();
            this.eFleetMeters = new HashSet<eFleetMeter>();
            this.eFleetPassengerTrackingRoutes = new HashSet<eFleetPassengerTrackingRoute>();
            this.eFleetPreventativeMaintenances = new HashSet<eFleetPreventativeMaintenance>();
            this.eFleetPreventativeMaintenances1 = new HashSet<eFleetPreventativeMaintenance>();
            this.eFleetVehicles = new HashSet<eFleetVehicle>();
            this.eFleetVehicleScanLogs = new HashSet<eFleetVehicleScanLog>();
            this.EMaintenanceSurveys = new HashSet<EMaintenanceSurvey>();
            this.LocationMasters = new HashSet<LocationMaster>();
            this.QRCMasters = new HashSet<QRCMaster>();
            this.QRCMasters1 = new HashSet<QRCMaster>();
            this.QRCMasters2 = new HashSet<QRCMaster>();
            this.QRCMasters3 = new HashSet<QRCMaster>();
            this.QRCMasters4 = new HashSet<QRCMaster>();
            this.QRCMasters5 = new HashSet<QRCMaster>();
            this.UserOverrideUsertypes = new HashSet<UserOverrideUsertype>();
            this.QRCScanLogs = new HashSet<QRCScanLog>();
            this.QRCScanLogs1 = new HashSet<QRCScanLog>();
            this.UserRegistrations = new HashSet<UserRegistration>();
            this.UserRegistrations1 = new HashSet<UserRegistration>();
            this.UserRegistrations2 = new HashSet<UserRegistration>();
            this.UserRegistrations3 = new HashSet<UserRegistration>();
            this.WorkRequestAssignments = new HashSet<WorkRequestAssignment>();
            this.WorkRequestAssignments1 = new HashSet<WorkRequestAssignment>();
            this.WorkRequestAssignments2 = new HashSet<WorkRequestAssignment>();
        }
    
        public long GlobalCodeId { get; set; }
        public string Category { get; set; }
        public string CodeName { get; set; }
        public string Description { get; set; }
        public bool IsActive { get; set; }
        public Nullable<int> SortOrder { get; set; }
        public long CreatedBy { get; set; }
        public System.DateTime CreatedDate { get; set; }
        public Nullable<long> ModifiedBy { get; set; }
        public Nullable<System.DateTime> ModifiedDate { get; set; }
        public bool IsDeleted { get; set; }
        public Nullable<long> DeletedBy { get; set; }
        public Nullable<System.DateTime> DeletedDate { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<DashboardWidgetSetting> DashboardWidgetSettings { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<eFleetFueling> eFleetFuelings { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<eFleetMaintenance> eFleetMaintenances { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<eFleetMeter> eFleetMeters { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<eFleetPassengerTrackingRoute> eFleetPassengerTrackingRoutes { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<eFleetPreventativeMaintenance> eFleetPreventativeMaintenances { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<eFleetPreventativeMaintenance> eFleetPreventativeMaintenances1 { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<eFleetVehicle> eFleetVehicles { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<eFleetVehicleScanLog> eFleetVehicleScanLogs { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<EMaintenanceSurvey> EMaintenanceSurveys { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<LocationMaster> LocationMasters { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<QRCMaster> QRCMasters { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<QRCMaster> QRCMasters1 { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<QRCMaster> QRCMasters2 { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<QRCMaster> QRCMasters3 { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<QRCMaster> QRCMasters4 { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<QRCMaster> QRCMasters5 { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<UserOverrideUsertype> UserOverrideUsertypes { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<QRCScanLog> QRCScanLogs { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<QRCScanLog> QRCScanLogs1 { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<UserRegistration> UserRegistrations { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<UserRegistration> UserRegistrations1 { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<UserRegistration> UserRegistrations2 { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<UserRegistration> UserRegistrations3 { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<WorkRequestAssignment> WorkRequestAssignments { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<WorkRequestAssignment> WorkRequestAssignments1 { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<WorkRequestAssignment> WorkRequestAssignments2 { get; set; }
    }
}
