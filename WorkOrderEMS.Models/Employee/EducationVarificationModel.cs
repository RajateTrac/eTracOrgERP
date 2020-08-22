﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WorkOrderEMS.Models.Employee
{
	public class EducationVarificationModel
	{
		[Required(ErrorMessage = "*")]
		public string Name { get; set; }
		[Required(ErrorMessage = "*")]
		[DataType(DataType.Date)]
		[DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
		public DateTime DOB { get; set; }
		[Required]
		public string SSN { get; set; }
		public Education HighSchool { get; set; }
		public Education HigherSchool { get; set; }
		public string Certificate { get; set; }
		public string Signature { get; set; }
		public string Date { get; set; }
		public bool IsSave { get; set; }
		public string EmpId { get; set; }
		public long? EvfId { get; set; }
		public string IsActive { get; set; }
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

	public class Education
	{
		[Required(ErrorMessage = "*")]
		public string SchoolName { get; set; }
		[Required(ErrorMessage = "*")]
		public string City { get; set; }
		[Required(ErrorMessage = "*")]
		public string State { get; set; }
		[Required(ErrorMessage = "*")]
		[DataType(DataType.Date)]
		[DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
		public DateTime? AttendFrom { get; set; }
		[Required(ErrorMessage = "*")]
		[DataType(DataType.Date)]
		[DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
		public DateTime? AttendTo { get; set; }
		public string Cretificate { get; set; }
	}

}
