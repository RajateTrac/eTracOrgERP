using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WorkOrderEMS.Data.EntityModel;
using WorkOrderEMS.Models;
using WorkOrderEMS.Models.Employee;
using WorkOrderEMS.Models.NewAdminModel;
using WorkOrderEMS.Models.NewAdminModel.FormsModel;

namespace WorkOrderEMS.BusinessLogic.Interfaces
{
    public interface IApplicantManager
    {
        ServiceResponseModel<eTracLoginModel> ValidateApplicant(eTracLoginModel obj);
        ServiceResponseModel<eTracLoginModel> ForgotPassword(eTracLoginModel obj);
        ServiceResponseModel<eTracLoginModel> SetForgotPassword(eTracLoginModel obj);
        ServiceResponseModel<string> SignUpApplicant(eTracLoginModel obj);
        ServiceResponseModel<string> ChangePassword(eTracLoginModel obj);
        ServiceResponseModel<string> CheckLoginId(eTracLoginModel obj);
        bool SaveAssets(AssetsAllocationModel model);
        bool SendOffer(OfferModel model);
        bool SaveApplicantData(CommonApplicantModel Obj);
        bool UpdateContactDetailsApplicant(ContactListModel model, List<ContactModel> lstModel);
        ContactListModel GetContactListByApplicantId(long ApplicantId);
        BackgroundCheckForm GetApplicantByApplicantId(long ApplicantId);
        bool SendApplicantInfoForBackgrounddCheck(BackgroundCheckForm model);
        I9FormModel GetI9FormData(long ApplicantId, long UserId, FormNameStatus obj);
        bool SetI9Form(long UserId, long ApplicantId, I9FormModel model);
        Desclaimer GetSignature(long ApplicantId);
        bool SaveDesclaimerData(Desclaimer obj);
        bool SaveSelfIdentification(SelfIdentificationModel obj);
        SelfIdentificationModel GetSelfIdentification(string EmployeeId);
        bool SaveApplicantFunFacts(ApplicantFunFactModel obj);
        RateOfPayModel GetRateOfPayInfo(long ApplicantId, string employeeId);
        RateOfPayModel GetIsExempt(string EmployeeId);
        List<JobPostingDropDownModel> GetListJobPosting(eTracLoginModel obj);
        bool SaveOriantation(OriantationModel model);
        OfferModel GetOfferDetailsOfApplicant(long ApplicantId);
        bool SaveScreenRejectStatusApplicant(long ApplicantId, bool IsScreened);
        bool SaveAcadmicertificate(AcadmicCertification obj);
        EmployeeVIewModel GetApplicantAllDetails(long ApplicantId);
        bool SaveResume(string FileName, long Id);
        ApplicantCountDetails GetApplicantCount();
        bool SendUserNameAndPassword(long ApplicantId);
        bool CancelInterviewManager(string InterviewerID, long ApplicantId, string Comment);
        bool InterviewCompleted(long ApplicantId);
        bool SetApplicantStatus(long ApplicantId, string Status, string IsActive);
        bool SendMailToEmployeeOriantationDone(long ApplicantId, string EmployeeId, string IsActive);
        string GetFilesData(string fileName, string EmployeeID);
        Employee GetEmployeeData(long applicantId);
        ApplicantFunFactModel GetFunFacts(string EmpId);
        bool UpdateFunFacts(ApplicantFunFactModel obj);

    }
}
