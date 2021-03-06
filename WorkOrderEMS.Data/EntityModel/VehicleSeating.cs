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
    
    public partial class VehicleSeating
    {
        public VehicleSeating()
        {
            this.UserVehicleSeatingMaps = new HashSet<UserVehicleSeatingMap>();
            this.VehicleSeatingSubModuleMappings = new HashSet<VehicleSeatingSubModuleMapping>();
        }
    
        public long VST_Id { get; set; }
        public string VST_Title { get; set; }
        public string VST_JobDescription { get; set; }
        public string VST_RolesAndResponsiblities { get; set; }
        public string VST_Level { get; set; }
        public long VST_ParentId { get; set; }
        public long VST_DPT_Id { get; set; }
        public Nullable<System.DateTime> VST_Date { get; set; }
        public string VST_IsActive { get; set; }
        public string VST_EmploymentStatus { get; set; }
        public string VST_IsExempt { get; set; }
        public Nullable<decimal> VST_RateOfPay { get; set; }
    
        public virtual Department Department { get; set; }
        public virtual ICollection<UserVehicleSeatingMap> UserVehicleSeatingMaps { get; set; }
        public virtual ICollection<VehicleSeatingSubModuleMapping> VehicleSeatingSubModuleMappings { get; set; }
    }
}
