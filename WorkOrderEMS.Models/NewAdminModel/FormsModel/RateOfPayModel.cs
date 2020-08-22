using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WorkOrderEMS.Models
{
    public class RateOfPayModel
    {
        public int RateOfPayId { get; set; }
        public string EmployeeName { get; set; }
        public string EmployeeNumber { get; set; }
        public string ManagerName { get; set; }
        public string Operations { get; set; }
        public string Location { get; set; }
        public string JobTitle { get; set; }
        public decimal? RateOfPay { get; set; }
        public string TypeOfPayChange { get; set; }
        public string JobStatus { get; set; }
        public string Emp_Signature { get; set; }
        public Nullable<DateTime> Emp_Date { get; set; }
        public string Man_Signature { get; set; }
        public Nullable<DateTime> Man_Date { get; set; }
        public string ApprovedBy { get; set; }
        public Nullable<DateTime> ApprovedDate { get; set; }
        public bool IsSave { get; set; }
        public string IsExempt { get; set; }
        public string EmployeeSignatureName { get; set; }
        public string SignatureBase { get; set; }
        public string formName { get; set; }
        public string FormStatusw4 { get; set; }
        public string FormStatusI9 { get; set; }
        public string FormStatusdd { get; set; }
        public string FormStatusEvf { get; set; }
        public string FormStatussif { get; set; }
        public string FormStatusbcf { get; set; }
        public string FormStatusrop { get; set; }
        public string FormStatusprfecf { get; set; }
        public string FormStatusprfcaf { get; set; }
        public string FormStatusprf { get; set; }
        public string FormStatusff { get; set; }
    }
}
