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
    
    public partial class SubModule
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public SubModule()
        {
            this.UserOverrideUsertypes = new HashSet<UserOverrideUsertype>();
            this.VehicleSeatingSubModuleMappings = new HashSet<VehicleSeatingSubModuleMapping>();
        }
    
        public long SMD_Id { get; set; }
        public long SMD_MDL_Id { get; set; }
        public string SMD_SubModuleName { get; set; }
        public Nullable<System.DateTime> SMD_Date { get; set; }
        public string SMD_IsActive { get; set; }
    
        public virtual Module Module { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<UserOverrideUsertype> UserOverrideUsertypes { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<VehicleSeatingSubModuleMapping> VehicleSeatingSubModuleMappings { get; set; }
    }
}
