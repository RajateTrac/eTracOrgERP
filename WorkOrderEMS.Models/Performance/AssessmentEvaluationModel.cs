using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WorkOrderEMS.Models.NewAdminModel;

namespace WorkOrderEMS.Models.Performance
{
    public class AssessmentEvaluationModel
    {
        public List<GWCQUestionModel> GWCQUestionModelAssessment { get; set; }
        public List<GWCQUestionModel> GWCQUestionModelEvaluation { get; set; }
    }
}
