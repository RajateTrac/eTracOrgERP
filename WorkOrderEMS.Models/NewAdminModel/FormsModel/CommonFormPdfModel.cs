using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WorkOrderEMS.Models.Employee;

namespace WorkOrderEMS.Models.NewAdminModel.FormsModel
{
    public class CommonFormPdfModel
    {   
        public CommonFormPdfModel()
        {
            this.PhotoRelease = new PhotoRelease();
            this.ConfidenialityAgreementModel = new ConfidenialityAgreementModel();
        }
        public BackgroundCheckForm BackgroundCheckForm { get; set; }
        public ConfidenialityAgreementModel ConfidenialityAgreementModel { get; set; }
        public DirectDepositeFormModel DirectDepositeFormModel { get; set; }
        public List<EducationVarificationModel> EducationVarificationModel { get; set; }
        public EmergencyContectForm EmergencyContectForm { get; set; }
        public I9FormModel I9FormModel { get; set; }
        public PhotoRelease PhotoRelease { get; set; }
        public RateOfPayModel RateOfPayModel { get; set; }
        public SelfIdentificationModel SelfIdentificationModel { get; set; }
        public W4FormModel W4FormModel { get; set; }
        public string Isexempt { get; set; }
        //public bool FormStatusw4 { get; set; }
        //public bool FormStatusI9 { get; set; }
        //public bool FormStatusdd { get; set; }
        //public bool FormStatusEvf { get; set; }
        //public bool FormStatussif { get; set; }
        //public bool FormStatusbcf { get; set; }
        //public bool FormStatusrop { get; set; }
        //public bool FormStatusprfecf { get; set; }
        //public bool FormStatusprfcaf { get; set; }

    }
}
