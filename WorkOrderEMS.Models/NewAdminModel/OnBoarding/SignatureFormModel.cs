using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WorkOrderEMS.Models.NewAdminModel
{
    public class SignatureFormModel
    {
        public string W4 { get; set; }
        public string I9 { get; set; }
        public string RateOfPay { get; set; }
        public string PhotoReleaseForm { get; set; }
        public string EducationVerification { get; set; }
        public string ConfidentialityAgreementForm { get; set; }
        public string EmergencyContactForm { get; set; }
    }
}
