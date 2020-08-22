using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WorkOrderEMS.Models.NewAdminModel.OnBoarding
{
    public class JobSummaryModel
    {
        
        public int? StatusApplied { get; set; }
        public int? StatusScreened { get; set; }
        public int? StatusInterviewSchedule { get; set; }
        public int? StatusInterviewCanceled { get; set; }
        public int? StatusShortListed { get; set; }
        public int? StatusAssessmentSend { get; set; }
        public int? StatusAssessmentPass { get; set; }
        public int? StatusAssessmentFailed { get; set; }
        public int? StatusOnhold { get; set; }
        public int? StatusHired { get; set; }
        public int? StatusOfferSent { get; set; }
        public int? StatusOfferAccepted { get; set; }
        public int? StatusOfferCountered { get; set; }
        public int? StatusOfferDeclined { get; set; }
        public int? StatusOfferCanceled { get; set; }
        public int? StatusOnboarding { get; set; }
        public int? StatusOnboarded { get; set; }
        public int? StatusOnboardingDrop { get; set; }
        public int? StatusBackgroundCheckSend { get; set; }
        public int? StatusBackgroundCheckPass { get; set; }
        public int? StatusBackgroundCheckFail { get; set; }
        public int? StatusOrientationSchedule { get; set; }
        public int? StatusOrientationDone { get; set; }
        public int? StatusOrientationNotDone { get; set; }
        public int? StatusReject { get; set; }
        

    }
}
