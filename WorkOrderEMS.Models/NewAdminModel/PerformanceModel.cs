﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WorkOrderEMS.Models.NewAdminModel
{
    public class PerformanceModel
    {
        public string EMP_EmployeeID { get; set; }
        public string EMP_Photo { get; set; }
        public string EmployeeName { get; set; }
        public string DepartmentName { get; set; }
        public string JBT_JobTitle { get; set; }
        public string LocationName { get; set; }
        public DateTime? EMP_DateOfJoining { get; set; }
        public int? Assesment { get; set; }
    }
    public class GWCQUestionModel
    {
        public long QuestionId { get; set; }
        public string AssessmentType{ get; set; }
        public string QuestionType{ get; set; }
        public string Question{ get; set; }

        public bool Answer { get; set; }

        public string EmployeeId { get; set;}
        public long? SelfAssessmentId { get; set;}

    }
}
