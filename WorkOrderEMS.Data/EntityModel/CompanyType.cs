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
    
    public partial class CompanyType
    {
        public CompanyType()
        {
            this.Companies = new HashSet<Company>();
            this.LogCompanies = new HashSet<LogCompany>();
        }
    
        public long COT_Id { get; set; }
        public string COT_CompanyType { get; set; }
        public string COT_IsActive { get; set; }
    
        public virtual ICollection<Company> Companies { get; set; }
        public virtual ICollection<LogCompany> LogCompanies { get; set; }
    }
}
