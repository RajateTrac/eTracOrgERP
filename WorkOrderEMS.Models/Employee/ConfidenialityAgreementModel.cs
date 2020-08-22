using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WorkOrderEMS.Models.Employee
{
	public class ConfidenialityAgreementModel
	{
		public string EmpId { get; set; }
		public string EmployeeName { get; set; }
		public string EmpAddress { get; set; }
		public string Email { get; set; }
		public long? CafId { get; set; }
		public string IsActive { get; set; }
		public string Name { get; set; }
		public string Title { get; set; }
		public string Signature { get; set; }
		public string Date { get; set; }
		public string By { get; set; }
		public string Between { get; set; }
		public bool IsSaved { get; set; }
		//public string IsExempt { get; set; }
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
