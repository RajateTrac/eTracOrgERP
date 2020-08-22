using System;
using System.Collections.Generic;
using System.Configuration;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using WorkOrderEMS.BusinessLogic;
using WorkOrderEMS.BusinessLogic.Interfaces;
using WorkOrderEMS.Data.DataRepository;
using WorkOrderEMS.Data.EntityModel;
using WorkOrderEMS.Data.Interfaces;
using WorkOrderEMS.Helper;
using WorkOrderEMS.Models;
using WorkOrderEMS.Models.Employee;
using WorkOrderEMS.Models.NewAdminModel;
using WorkOrderEMS.Models.NewAdminModel.FormsModel;
using WorkOrderEMS.Models.NewAdminModel.OnBoarding;
using WorkOrderEMS.Models.Performance;

namespace WorkOrderEMS.Controllers.Guest
{

    public class GuestController : Controller
    {
        private readonly ICommonMethod _ICommonMethod;
        private readonly IGlobalAdmin _IGlobalAdmin;
        private readonly ICompanyAdmin _ICompanyAdmin;
        private readonly IGuestUser _IGuestUserRepository;
        private readonly IApplicantManager _IApplicantManager;
        private readonly IFillableFormManager _IFillableFormManager;
        private readonly IDepartment _IDepartment;
        private readonly string RefreshI9Token;
        private string HostingPrefix = Convert.ToString(System.Configuration.ConfigurationManager.AppSettings["hostingPrefix"], CultureInfo.InvariantCulture);
        private string ApplicantSignature = ConfigurationManager.AppSettings["ApplicantSignature"];
        private string PDFUrl = ConfigurationManager.AppSettings["PDFSignature"];
        public GuestController(ICommonMethod _ICommonMethod, IGlobalAdmin _IGlobalAdmin, ICompanyAdmin _ICompanyAdmin, IGuestUser _GuestUserRepository, IApplicantManager _IApplicantManager, IFillableFormManager _IFillableFormManager, IDepartment _IDepartment)
        {
            this._IGlobalAdmin = _IGlobalAdmin;
            this._ICommonMethod = _ICommonMethod;
            this._ICompanyAdmin = _ICompanyAdmin;
            this._IGuestUserRepository = _GuestUserRepository;
            this._IApplicantManager = _IApplicantManager;
            this._IFillableFormManager = _IFillableFormManager;
            this._IDepartment = _IDepartment;
        }
        //
        // GET: /Guest/

        public ActionResult Index()
        {
            var ObjLoginModel = (eTracLoginModel)(Session["eTrac"]);
            CommonApplicantModel model = new CommonApplicantModel();
            ViewBag.StateList = _ICommonMethod.GetStateByCountryId(1);
            var employee = _IGuestUserRepository.GetEmployeeDetails(ObjLoginModel.UserId);
            var commonModel = _IGuestUserRepository.GetApplicantAllDetailsToView(employee.ApplicantId);
            //var commonModel = _IGuestUserRepository.GetApplicantAllDetailsToView(ApplicantId);
            //commonModel.ApplicantId = ApplicantId;
            //var _model = _IApplicantManager.GetRateOfPayInfo(employee.ApplicantId, employee.EmpId);
            var _model = _IApplicantManager.GetIsExempt(employee.EmpId);
            var IsExempt = _model.IsExempt;
            Session["IsExempt"] = IsExempt;
            commonModel.ApplicantId = employee.ApplicantId;

            Session["ApplicantId"] = employee.ApplicantId; commonModel.ApplicantId = employee.ApplicantId;
            return View("~/Views/Guest/Index1.cshtml", commonModel);
        }
        [HttpPost]
        public ActionResult Index(CommonApplicantModel CommonApplicantModel)
        {
            ApplicantManager _IApplicantManager = new ApplicantManager();
            ViewBag.StateList = _ICommonMethod.GetStateByCountryId(1);
            var getDetails = _IApplicantManager.SaveApplicantData(CommonApplicantModel);
            if (ModelState.IsValid)
            {
                var isSaveSuccess = true;
                if (isSaveSuccess)
                    return RedirectToAction("_W4Form");
                else
                {
                    ViewBag.message = "Something went wrong!!!";
                    return View();
                }
            }
            return View();
        }
        [HttpGet]
        public ActionResult PersonalFile(bool? isSaved)
        {
            return View();
        }
        [Route("welcome")]
        [HttpGet]
        public ActionResult LandingPage()
        {
            return View();
        }
        public ActionResult ThankYou()
        {

            var NotificationModel = new NotificationDetailModel();
            var manager = new NotificationManager();
            var ObjLoginModel = (eTracLoginModel)(Session["eTrac"]);
            var applicantId = Convert.ToInt64(Session["ApplicantId"]);
            if (applicantId > 0)
            {
                OnboardingFormPdf();
                var getDetails = _IApplicantManager.GetApplicantAllDetails(applicantId);
                var applicantStatus = _IApplicantManager.SetApplicantStatus(applicantId,ApplicantStatus.Onboarding, ApplicantIsActiveStatus.Pass);
                string message = DarMessage.OnboardingComplete(ObjLoginModel.FName + " " + ObjLoginModel.LName);
                NotificationModel.Message = message;
                NotificationModel.CreatedByUser = ObjLoginModel.UserName;
                NotificationModel.CreatedByIsWorkable = false;
                NotificationModel.AssignToUser = getDetails.HiringManager;
                NotificationModel.AssignToIsWorkable = true;
                NotificationModel.Priority = Priority.Medium;
                NotificationModel.Module = ModuleSubModule.ePeople;
                NotificationModel.SubModule = ModuleSubModule.OnBoardingComplete;
                NotificationModel.SubModuleId1 = applicantId.ToString();
                var save = manager.SaveNotification(NotificationModel);
            }
            return View();
        }
        [HttpGet]
        public PartialViewResult _DirectDepositeForm()
        {
            DirectDepositeFormModel model = new DirectDepositeFormModel();
            var ObjLoginModel = (eTracLoginModel)(Session["eTrac"]);
            model = _IGuestUserRepository.GetDirectDepositeDataByUserId(ObjLoginModel.UserId);
            if (model == null)
            {
                var _model = new DirectDepositeFormModel();
                var apt_Id = Convert.ToInt64(Session["ApplicantId"]);
                var getApplicantDetails = _IApplicantManager.GetApplicantAllDetails(apt_Id);
                _model.EmployeeId = ObjLoginModel.UserName;
                //_model.EmployeeSignatureName = getApplicantDetails.FirstName + " " + getApplicantDetails.LastName;
                _model.PrintedName = getApplicantDetails.FirstName + " " + getApplicantDetails.LastName;


                return PartialView("_directDepositeForm", _model);
            }
            return PartialView("_directDepositeForm", model);
        }

        /// <summary>
        /// Click next in direct deposit form and getting data for education varification form 
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult _DirectDepositeForm(DirectDepositeFormModel _model)
        {
            var model = new List<EducationVarificationModel>();
            var objloginmodel = (eTracLoginModel)(Session["etrac"]);
            var applicantId = Convert.ToInt64(Session["ApplicantId"]);
            model = _IGuestUserRepository.GetEducationVerificationForm(objloginmodel.UserId, applicantId);
            if (model != null)
            {

                Session["EducationForm"] = model;
                model[0].FormStatusw4 = _model.FormStatusw4;
                model[0].FormStatusbcf = _model.FormStatusbcf;
                model[0].FormStatusdd = _model.FormStatusdd;
                model[0].FormStatusEvf = _model.FormStatusEvf;
                model[0].FormStatusI9 = _model.FormStatusI9;
                model[0].FormStatusprfcaf = _model.FormStatusprfcaf;
                model[0].FormStatusprfecf = _model.FormStatusprfecf;
                model[0].FormStatusrop = _model.FormStatusrop;
                model[0].FormStatussif = _model.FormStatussif;
                model[0].FormStatusprf = _model.FormStatusprf;
                model[0].FormStatusff = _model.FormStatusff;
                model[0].formName = "educationverificationform";
                return PartialView("_EducationVarificationForm", model);
            }
            else
            {   
                return PartialView("_EducationVarificationForm", new List<EducationVarificationModel>());
            }

        }

        /// <summary>
        /// created by: Rajat Toppo
        /// Date: 20-07-2020
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        public PartialViewResult DirectDepositeFormFromCheclist(FormNameStatus obj)
        {
            DirectDepositeFormModel model = new DirectDepositeFormModel();
            var ObjLoginModel = (eTracLoginModel)(Session["eTrac"]);
            model = _IGuestUserRepository.GetDirectDepositeDataByUserId(ObjLoginModel.UserId);
            var Signdd = Convert.ToString(Session["Signaturedd"]);
            var newSigndd = Signdd.Replace("-", "/");
            
            if (model == null)
            {
                var _model = new DirectDepositeFormModel();
                var apt_Id = Convert.ToInt64(Session["ApplicantId"]);
                var getApplicantDetails = _IApplicantManager.GetApplicantAllDetails(apt_Id);
                _model.EmployeeId = ObjLoginModel.UserName;
                //_model.EmployeeSignatureName = getApplicantDetails.FirstName + " " + getApplicantDetails.LastName;
                _model.PrintedName = getApplicantDetails.FirstName + " " + getApplicantDetails.LastName;
                _model.FormStatusw4 = obj.FormStatusw4;
                _model.FormStatusbcf = obj.FormStatusbcf;
                _model.FormStatusdd = obj.FormStatusdd;
                _model.FormStatusEvf = obj.FormStatusEvf;
                _model.FormStatusI9 = obj.FormStatusI9;
                _model.FormStatusprfcaf = obj.FormStatusprfcaf;
                _model.FormStatusprfecf = obj.FormStatusprfecf;
                _model.FormStatusrop = obj.FormStatusrop;
                _model.FormStatussif = obj.FormStatussif;
                _model.FormStatusprf = obj.FormStatusprf;
                _model.FormStatusff = obj.FormStatusff;
                _model.EmployeeSignatureName = newSigndd;
                _model.formName = "depositeForm";

                return PartialView("_directDepositeForm", _model);
            }
            model.FormStatusw4 = obj.FormStatusw4;
            model.FormStatusbcf = obj.FormStatusbcf;
            model.FormStatusdd = obj.FormStatusdd;
            model.FormStatusEvf = obj.FormStatusEvf;
            model.FormStatusI9 = obj.FormStatusI9;
            model.FormStatusprfcaf = obj.FormStatusprfcaf;
            model.FormStatusprfecf = obj.FormStatusprfecf;
            model.FormStatusrop = obj.FormStatusrop;
            model.FormStatussif = obj.FormStatussif;
            model.FormStatusprf = obj.FormStatusprf;
            model.FormStatusff = obj.FormStatusff;
            model.formName = "depositeForm";
            model.EmployeeSignatureName = newSigndd;
            return PartialView("_directDepositeForm", model);
        }

        [HttpGet]
        public PartialViewResult _EmployeeHandbook()
        {
            EmployeeHandbookModel model = new EmployeeHandbookModel();
            var ObjLoginModel = (eTracLoginModel)(Session["eTrac"]);
            model = _IGuestUserRepository.GetEmployeeHandBookByUserId(ObjLoginModel.UserId);
            return PartialView("_employeeHandbook", model);
        }
        [HttpPost]
        public ActionResult _EmployeeHandbook(EmployeeHandbookModel model, bool formStatus)
        {
            if (ModelState.IsValid)
            {
                if (formStatus == false)
                {
                    var ObjLoginModel = (eTracLoginModel)(Session["eTrac"]);
                    _IGuestUserRepository.SetEmployeeHandbookData(model, ObjLoginModel.UserId);
                    return Json(true, JsonRequestBehavior.AllowGet);
                }
            }
            //ViewBag.NotSaved = true;
            return PartialView("_employeeHandbook", model);
        }

        /// <summary>
        /// Created by: Rajat Toppo
        /// Date:20-07-2020
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult _SaveEmployeeHandbook(EmployeeHandbookModel model)
        {
            if (ModelState.IsValid)
            {
                var ObjLoginModel = (eTracLoginModel)(Session["eTrac"]);
                _IGuestUserRepository.SetEmployeeHandbookData(model, ObjLoginModel.UserId);
                return Json(true, JsonRequestBehavior.AllowGet);
            }
            ViewBag.NotSaved = true;
            return PartialView("_employeeHandbook", model);
        }
        [HttpGet]
        public PartialViewResult _I9Form()
        {
            var getI9Info = new I9FormModel();
            eTracLoginModel ObjLoginModel = null;
            if (Session != null)
            {
                if (Session["eTrac"] != null)
                {
                    ObjLoginModel = (eTracLoginModel)(Session["eTrac"]);
                }
            }
            var applicantId = Convert.ToInt64(Session["ApplicantId"]);
            if (applicantId > 0)
            {
                FormNameStatus obj = new FormNameStatus();
                getI9Info = _IApplicantManager.GetI9FormData(applicantId, ObjLoginModel.UserId, obj);
                getI9Info.IsSignature = false;
                getI9Info.I9F_EMP_EmployeeId = ObjLoginModel.UserName;
                //if(Convert.ToBoolean(Session["IsSignature"]) == )
                return PartialView("_I9Form", getI9Info); ;
            }
            return PartialView("_I9Form", getI9Info);
        }
        /// <summary>
        /// Click next in I9 form and getting data and going to background check form
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult _I9Form(I9FormModel model)
        {
            var _model = new EmergencyContectForm();
            var applicantId = Convert.ToInt64(Session["ApplicantId"]);

            if (model != null)
            {

                var objloginmodel = (eTracLoginModel)(Session["etrac"]);
                string ImagePath = string.Empty;
                string ImageUniqueName = string.Empty;
                string url = string.Empty;
                string ImageURL = string.Empty;
                var signDataTranslator = string.Empty;
                var getBApplicant = new BackgroundCheckForm();

                var getApplicantId = Convert.ToInt64(Session["ApplicantId"]);
                getBApplicant = _IApplicantManager.GetApplicantByApplicantId(getApplicantId);
                Session["ApplicantAddress"] = getBApplicant.ApplicantAddress;
                getBApplicant.IsSignature = false;
                getBApplicant.FormStatusw4 = model.FormStatusw4;
                getBApplicant.FormStatusbcf = model.FormStatusbcf;
                getBApplicant.FormStatusdd = model.FormStatusdd;
                getBApplicant.FormStatusEvf = model.FormStatusEvf;
                getBApplicant.FormStatusI9 = model.FormStatusI9;
                getBApplicant.FormStatusprfcaf = model.FormStatusprfcaf;
                getBApplicant.FormStatusprfecf = model.FormStatusprfecf;
                getBApplicant.FormStatusrop = model.FormStatusrop;
                getBApplicant.FormStatussif = model.FormStatussif;
                getBApplicant.formName = "BackgroundCheckForm";

                return PartialView("PartialView/_BackGroundCheckForm", getBApplicant);

            }
            return RedirectToAction("_I9Form");
        }

        /// <summary>
        /// Created  by: Rajat Toppo
        /// Date:20-07-2020
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        public PartialViewResult I9FormfromCheckList(FormNameStatus obj)
        {
            var getI9Info = new I9FormModel();
            eTracLoginModel ObjLoginModel = null;
            if (Session != null)
            {
                if (Session["eTrac"] != null)
                {
                    ObjLoginModel = (eTracLoginModel)(Session["eTrac"]);
                }
            }
            var applicantId = Convert.ToInt64(Session["ApplicantId"]);
            if (applicantId > 0)
            {

                getI9Info = _IApplicantManager.GetI9FormData(applicantId, ObjLoginModel.UserId, obj);
                getI9Info.IsSignature = false;
                getI9Info.I9F_EMP_EmployeeId = ObjLoginModel.UserName;
                var SignI9 = Convert.ToString(Session["SignatureI9"]);
                var newSignI9 = SignI9.Replace("-", "/");
                getI9Info.EmployeeSignatureName = newSignI9;
                //if(Convert.ToBoolean(Session["IsSignature"]) == )
                return PartialView("_I9Form", getI9Info); ;
            }
            return PartialView("_I9Form", getI9Info);
        }

        [HttpGet]
        public PartialViewResult _W4Form()
        {
            W4FormModel model = new W4FormModel();
            var objloginmodel = (eTracLoginModel)(Session["etrac"]);
            var objmodel = _IGuestUserRepository.GetW4Form(objloginmodel.UserId);
            
            Session["IsSignature"] = false;
            if (objmodel != null)
            {
                var apt_Id = Convert.ToInt64(Session["ApplicantId"]);
                objmodel.ApplicantId = apt_Id;
                objmodel.IsSignature = false;
                //ViewBag.IsSignature = false;//To filup form no need to display signature button so we make it hide
                return PartialView("_W4Form", objmodel);
            }
            else
            {
                var apt_Id = Convert.ToInt64(Session["ApplicantId"]);
                var getApplicantDetails = _IApplicantManager.GetApplicantAllDetails(apt_Id);
                model.FirstName = getApplicantDetails.FirstName;
                model.LastName = getApplicantDetails.LastName;
                model.MiddleName = getApplicantDetails.MiddleName;
                model.EIN = objloginmodel.UserName;
                model.FirstEmployeementDate = getApplicantDetails.AvailableDate;
                model.EmployeerNameAndAddress = getApplicantDetails.Address;
                model.IsSignature = false;
                model.SSN = getApplicantDetails.SocialSecurityNumber;

                return PartialView("_W4Form", model);
            }
        }
        //[HttpPost]
        //public ActionResult _W4Form(W4FormModel model)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        var objloginmodel = (eTracLoginModel)(Session["etrac"]);
        //        _IGuestUserRepository.SetW4Form(objloginmodel.UserId, model);
        //        return Json(true, JsonRequestBehavior.AllowGet);
        //    }
        //    return PartialView("_W4Form", model);
        //}
        /// <summary>
        /// get w4 from checklist
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public PartialViewResult _W4FormfromCheckList(FormNameStatus obj)
        {
            W4FormModel model = new W4FormModel();
            var objloginmodel = (eTracLoginModel)(Session["etrac"]);
            var objmodel = _IGuestUserRepository.GetW4Form(objloginmodel.UserId);
            Session["IsSignature"] = false;
           
                var apt_Id = Convert.ToInt64(Session["ApplicantId"]);
                var getApplicantDetails = _IApplicantManager.GetApplicantAllDetails(apt_Id);
                model.FirstName = getApplicantDetails.FirstName;
                model.LastName = getApplicantDetails.LastName;
                model.MiddleName = getApplicantDetails.MiddleName;
                model.EIN = objloginmodel.UserName;
                model.FirstEmployeementDate = getApplicantDetails.AvailableDate;
                model.EmployeerNameAndAddress = getApplicantDetails.Address;
                model.IsSignature = false;
                var SignW4 = Convert.ToString(Session["SignatureW4"]);
                var newSignW4 =  SignW4.Replace("-", "/");
                model.EmployeeSignature = newSignW4;
                model.SSN = getApplicantDetails.SocialSecurityNumber;
                model.FormStatusw4 = obj.FormStatusw4;
                model.FormStatusbcf = obj.FormStatusbcf;
                model.FormStatusdd = obj.FormStatusdd;
                model.FormStatusEvf = obj.FormStatusEvf;
                model.FormStatusI9 = obj.FormStatusI9;
                model.FormStatusprfcaf = obj.FormStatusprfcaf;
                model.FormStatusprfecf = obj.FormStatusprfecf;
                model.FormStatusrop = obj.FormStatusrop;
                model.FormStatussif = obj.FormStatussif;
                model.FormStatusprf = obj.FormStatusprf;
                model.FormStatusff = obj.FormStatusff;
                model.formName = "w4form";
                return PartialView("_W4Form", model);
           
        }

        /// <summary>
        /// Click Next in W4 form, getting data for I9 form and going to I9 form
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult _W4Form(W4FormModel model)
        {

            var _FillableFormRepository = new FillableFormRepository();
            var getI9Info = new I9FormModel();
            var _manager = new GuestUserRepositoryData();
            var objloginmodel = (eTracLoginModel)(Session["etrac"]);

            if (model != null)
            {
                var statusObj = _manager.GetFormStatusModel(model);
                var applicantId = Convert.ToInt64(Session["ApplicantId"]);
                var ObjgetI9Info = _IApplicantManager.GetI9FormData(applicantId, objloginmodel.UserId, statusObj);
                if (ObjgetI9Info != null)
                {

                    return PartialView("_I9Form", ObjgetI9Info);

                }

            }
            return RedirectToAction("_I9Form", model);

        }

        /// <summary>
        /// Click on save button to save diffrent forms
        /// </summary>
        /// <param name="onboardingformdata"></param>
        /// <param name="FormName"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult SaveOnboardingForms(string onboardingformdata, string FormName)
        {
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            var _FillableFormRepository = new FillableFormRepository();
            var getI9Info = new I9FormModel();
            var objloginmodel = (eTracLoginModel)(Session["etrac"]);
            string viewName = "";
            string path = "";
            string pdfName = "";


            switch (FormName)
            {
                case "w4form":
                    pdfName = "W4";
                    if (onboardingformdata != null)
                    {
                        var model = !string.IsNullOrEmpty(onboardingformdata) ? serializer.Deserialize<W4FormModel>(onboardingformdata.Replace("/", "-")) : null;

                        if (model != null)
                        {
                            var SignatureW4 = model.EmployeeSignature;
                            Session["SignatureW4"] = SignatureW4;
                            var RSignatureW4 = SignatureW4.Replace("-", "/");
                            model.EmployeeSignature = RSignatureW4;
                            viewName = "_W4FormPdf";
                            path = Session["ApplicantId"] + model.FirstName + viewName + ".pdf";
                            var employeeId = objloginmodel.UserName;

                            _IGuestUserRepository.SetW4Form(objloginmodel.UserId, model);//need help

                            var applicantId = Convert.ToInt64(Session["ApplicantId"]);
                            var sign = Signature(model.EmployeeSignature, applicantId, OnboardingForms.W4);
                            model.EmployeeSignature = sign;
                            #region PDF
                            var getDetails = _FillableFormRepository.GetFileList().Where(x => x.FLT_FileType == "Yellow" && x.FLT_Id == Convert.ToInt64(FileTypeId.W4)).FirstOrDefault();
                            var getpdf = HtmlConvertToPdf(viewName, model, path, getDetails.FLT_Id, employeeId, pdfName);

                            //var ObjgetI9Info = _IApplicantManager.GetI9FormData(applicantId, objloginmodel.UserId);


                            model.formName = "w4form";
                            model.FormStatusw4 = "true";
                            return Json(model);

                            #endregion PDF
                        }
                    }
                    break;
                case "depositeForm":
                    if (onboardingformdata != null)
                    {
                        var model = !string.IsNullOrEmpty(onboardingformdata) ? serializer.Deserialize<DirectDepositeFormModel>(onboardingformdata.Replace("/","-")) : null;
                        if (model != null)
                        {

                            var ObjLoginModel = (eTracLoginModel)(Session["eTrac"]);

                            var save = _IGuestUserRepository.SetDirectDepositeFormData(model, ObjLoginModel.UserId);//need help

                            var applicantId = Convert.ToInt64(Session["ApplicantId"]);
                            var Signaturedd = model.EmployeeSignatureName;
                            Session["Signaturedd"] = Signaturedd;
                            var sign = Signature(model.Signature, applicantId, OnboardingForms.DirectDeposite);
                            model.Signature = sign;
                            #region PDF
                            viewName = "_directDepositeFormPdf";
                            var employeeId = ObjLoginModel.UserName;
                            path = applicantId + model.PrintedName.Trim() + viewName + ".pdf";
                            //var _FillableFormRepository = new FillableFormRepository();
                            var getDetails = _FillableFormRepository.GetFileList().Where(x => x.FLT_FileType == "Yellow" && x.FLT_Id == Convert.ToInt64(FileTypeId.W4)).FirstOrDefault();
                            var getpdf = HtmlConvertToPdf(viewName, model, path, getDetails.FLT_Id, employeeId, "Direct Deposite");
                            //return Json(true, JsonRequestBehavior.AllowGet);
                            #endregion PDF
                            model.FormStatusdd = "true";
                            model.formName = "depositeForm";
                            return Json(model);

                        }

                    }

                    break;
                case "employeeHandbook":
                    if (onboardingformdata != null)
                    {
                        var model = !string.IsNullOrEmpty(onboardingformdata) ? serializer.Deserialize<W4FormModel>(onboardingformdata.Replace("/", "-")) : null;
                    }
                    break;
                case "photoreleaseform":
                    if (onboardingformdata != null)
                    {
                        var model = !string.IsNullOrEmpty(onboardingformdata) ? serializer.Deserialize<PhotoRelease>(onboardingformdata.Replace("/", "-")) : null;
                        var Signatureprf = model.Signature;
                        Session["Signatureprf"] = Signatureprf;
                        if (model != null)
                        {
                            if (ModelState.IsValid)
                            {
                                var apt_Id = Convert.ToInt64(Session["ApplicantId"]);
                                var sign = Signature(model.Signature, apt_Id, OnboardingForms.PhotoRelease);
                                model.Signature = sign;
                                #region PDF
                                var applicantId = Convert.ToInt64(Session["ApplicantId"]);
                                viewName = "_PhotoReleaseFormPdf";
                                var employeeId = objloginmodel.UserName;
                                path = applicantId + model.Name + viewName + ".pdf";
                                //var _FillableFormRepository = new FillableFormRepository();
                                var getDetails = _FillableFormRepository.GetFileList().Where(x => x.FLT_FileType == "Yellow" && x.FLT_Id == Convert.ToInt64(FileTypeId.W4)).FirstOrDefault();
                                var getpdf = HtmlConvertToPdf(viewName, model, path, getDetails.FLT_Id, employeeId, "Photo Release");
                                #endregion PDF
                                _IGuestUserRepository.SetPhotoRelease(objloginmodel.UserId, model);

                                var saveStatus = _IApplicantManager.SetApplicantStatus(apt_Id, ApplicantStatus.BackgroundCheck, ApplicantIsActiveStatus.Sent);
                                model.FormStatusprf = "true";
                                model.formName = "photoreleaseform";
                                return Json(model);

                            }
                        }

                    }
                    break;
                case "emergencycontactform":
                    if (onboardingformdata != null)
                    {
                        var model = !string.IsNullOrEmpty(onboardingformdata) ? serializer.Deserialize<EmergencyContectForm>(onboardingformdata.Replace("/", "-")) : null;

                        if (model != null)
                        {
                            eTracLoginModel ObjLoginModel = null;
                            if (Session != null)
                            {
                                if (Session["eTrac"] != null)
                                {
                                    ObjLoginModel = (eTracLoginModel)(Session["eTrac"]);
                                }
                            }
                            _IGuestUserRepository.SetEmergencyForm(ObjLoginModel.UserId, model);
                            #region PDF
                            var applicantId = Convert.ToInt64(Session["ApplicantId"]);
                            viewName = "_emergencyContactFormPdf";
                            var employeeId = ObjLoginModel.UserName;
                            path = applicantId + model.FirstName + viewName + ".pdf";
                            //var _FillableFormRepository = new FillableFormRepository();
                            var getDetails = _FillableFormRepository.GetFileList().Where(x => x.FLT_FileType == "Yellow" && x.FLT_Id == Convert.ToInt64(FileTypeId.W4)).FirstOrDefault();
                            var getpdf = HtmlConvertToPdf(viewName, model, path, getDetails.FLT_Id, employeeId, "Emergency Contact Form");
                            //return Json(true, JsonRequestBehavior.AllowGet);
                            #endregion PDF
                            model.FormStatusprfecf = "true";
                            model.formName = "emergencycontactform";
                            return Json(model);
                        }

                    }
                    break;
                case "confidentialityagreementform":
                    if (onboardingformdata != null)
                    {
                        var model = !string.IsNullOrEmpty(onboardingformdata) ? serializer.Deserialize<ConfidenialityAgreementModel>(onboardingformdata.Replace("/", "-")) : null;
                        if (model != null)
                        {
                            var applicantId = Convert.ToInt64(Session["ApplicantId"]);
                            var Signaturecaf = model.Signature;
                            Session["Signaturecaf"] = Signaturecaf;
                            var sign = Signature(model.Signature, applicantId, OnboardingForms.ConfidentialityAgreement);
                            model.Signature = sign;
                            //var objloginmodel = (eTracLoginModel)(Session["etrac"]);
                            #region PDF

                            viewName = "_ConfidentialityAgreementFormPdf";
                            var employeeId = objloginmodel.UserName;
                            path = applicantId + model.Name + viewName + ".pdf";
                            //var _FillableFormRepository = new FillableFormRepository();
                            var getDetails = _FillableFormRepository.GetFileList().Where(x => x.FLT_FileType == "Yellow" && x.FLT_Id == Convert.ToInt64(FileTypeId.W4)).FirstOrDefault();
                            var getpdf = HtmlConvertToPdf(viewName, model, path, getDetails.FLT_Id, employeeId, "Confidentiality Agreement");
                            #endregion PDF
                            _IGuestUserRepository.SetConfidenialityAgreementForm(objloginmodel.UserId, model);
                            model.FormStatusprfcaf = "true";
                            model.formName = "confidentialityagreementform";
                            return Json(model);
                        }
                    }
                    break;
                case "educationverificationform"://need to change code form saving education varificationform
                    if (onboardingformdata != null)
                    {
                        var model = !string.IsNullOrEmpty(onboardingformdata) ? serializer.Deserialize<EducationVarificationModel>(onboardingformdata.Replace("/", "-")) : null;

                        if (model != null)
                        {
                            var SignatureEvf = model.Signature;
                            Session["SignatureEvf"] = SignatureEvf;
                            // model = (List<EducationVarificationModel>)Session["EducationForm"];
                            objloginmodel = (eTracLoginModel)(Session["etrac"]);
                            var applicantId = Convert.ToInt64(Session["ApplicantId"]);
                            var obj = new List<EducationVarificationModel>();
                            var _manager = new GuestUserRepositoryData();
                            var education = _IGuestUserRepository.GetEducationVerificationForm(objloginmodel.UserId, applicantId);

                            var sign = Signature(model.Signature, applicantId, OnboardingForms.EducationVerifcation);
                            ViewBag.Signature = sign;
                            #region PDF

                            viewName = "_EducationVarificationFormPdf";
                            var employeeId = objloginmodel.UserName;
                            path = applicantId + model.Name + viewName + ".pdf";

                            var getDetails = _FillableFormRepository.GetFileList().Where(x => x.FLT_FileType == "Yellow" && x.FLT_Id == Convert.ToInt64(FileTypeId.W4)).FirstOrDefault();
                            var getpdf = HtmlConvertToPdf(viewName, education, path, getDetails.FLT_Id, employeeId, "Education Varification");
                            #endregion PDF
                            _IGuestUserRepository.SetEducationVerificationForm(objloginmodel.UserId, education); //need to change

                            model.FormStatusEvf = "true";
                            model.formName = "educationverificationform";
                            return Json(model);
                        }
                        else
                        {
                            model = (EducationVarificationModel)Session["EducationForm"];
                            objloginmodel = (eTracLoginModel)(Session["etrac"]);
                            #region PDF
                            var applicantId = Convert.ToInt64(Session["ApplicantId"]);
                            viewName = "_EducationVarificationFormPdf";
                            var employeeId = objloginmodel.UserName;
                            path = applicantId + model.Name + viewName + ".pdf";

                            var getDetails = _FillableFormRepository.GetFileList().Where(x => x.FLT_FileType == "Yellow" && x.FLT_Id == Convert.ToInt64(FileTypeId.W4)).FirstOrDefault();
                            var getpdf = HtmlConvertToPdf(viewName, model, path, getDetails.FLT_Id, employeeId, "Education Varification");
                            #endregion PDF
                            //_IGuestUserRepository.SetEducationVerificationForm(objloginmodel.UserId, model);
                            model.FormStatusEvf = "true";
                            model.formName = "educationverificationform";
                            return Json(model);
                        }

                    }
                    break;
                case "I9Form":
                    if (onboardingformdata != null)
                    {
                        var model = !string.IsNullOrEmpty(onboardingformdata) ? serializer.Deserialize<I9FormModel>(onboardingformdata.Replace("/", "-")) : null;
                        var _model = new EmergencyContectForm();
                        var applicantId = Convert.ToInt64(Session["ApplicantId"]);
                        var SignatureI9 = model.EmployeeSignatureName;
                        Session["SignatureI9"] = SignatureI9;
                        if (ModelState.IsValid)
                        {
                            //var objloginmodel = (eTracLoginModel)(Session["etrac"]);
                            string ImagePath = string.Empty;
                            string ImageUniqueName = string.Empty;
                            string url = string.Empty;
                            string ImageURL = string.Empty;
                            var signDataTranslator = string.Empty;
                            if (model != null)
                            {
                                if (model.SignatureImageBase != null)
                                {
                                    ImagePath = Server.MapPath(ConfigurationManager.AppSettings["ApplicantSignature"].ToString());
                                    ImageUniqueName = DateTime.Now.ToString("yyyyMMddHHmm") + model.I9F_Sec1_PreparerAndTranslator + "_" + applicantId;
                                    url = HostingPrefix + ApplicantSignature.Replace("~", "") + ImageUniqueName + ".jpg";
                                    ImageURL = ImageUniqueName + ".jpg";
                                    if (!Directory.Exists(ImagePath))
                                    {
                                        Directory.CreateDirectory(ImagePath);
                                    }
                                    var ImageLocation = ImagePath + ImageURL;
                                    //bcz memory stream cannot read this string so replace the unwanted data from string
                                    signDataTranslator = model.SignatureImageBase.Replace("data:image/jpeg;base64,", "");
                                    signDataTranslator = model.SignatureImageBase.Replace("data:image/jpg;base64,", "");
                                    signDataTranslator = model.SignatureImageBase.Replace("data:image/png;base64,", "");
                                    //Save the image to directory
                                    using (MemoryStream ms = new MemoryStream(Convert.FromBase64String(model.SignatureImageBase)))
                                    {
                                        using (Bitmap bm2 = new Bitmap(ms))
                                        {
                                            //bm2.Save("SavingPath" + "ImageName.jpg");
                                            bm2.Save(ImageLocation);
                                            model.I9F_Sec1_SignatureOfPreparerOrTranslator = ImageURL;
                                            //imgupload.ImageUrl = ImageLocation;
                                        }
                                    }
                                }
                                model.RefreshTokenI9 = objloginmodel.RefreshI9Token;
                                model.I9CompanyId = objloginmodel.I9CompanyId;
                                model.I9F_EMP_EmployeeId = objloginmodel.UserName;
                                var saved = _IApplicantManager.SetI9Form(objloginmodel.UserId, applicantId, model);
                                if (saved)
                                {
                                    var getApplicant = new BackgroundCheckForm();
                                    //var _FillableFormRepository = new FillableFormRepository();
                                    //getApplicant = _IApplicantManager.GetApplicantByApplicantId(applicantId);
                                    var employeeId = objloginmodel.UserName;
                                    var sign = Signature(model.EmployeeSignatureName, applicantId, OnboardingForms.I9);
                                    model.EmployeeSignatureName = sign;
                                    #region PDF
                                    viewName = "_I9FormPdf";
                                    path = applicantId + model.I9F_Sec1_FirstName + viewName + ".pdf";
                                    var getDetails = _FillableFormRepository.GetFileList().Where(x => x.FLT_FileType == "Yellow" && x.FLT_Id == Convert.ToInt64(FileTypeId.I9)).FirstOrDefault();
                                    var getpdf = HtmlConvertToPdf(viewName, model, path, getDetails.FLT_Id, employeeId, "I9");
                                    #endregion PDF
                                    
                                    model.formName = "I9Form";
                                    model.FormStatusI9 = "true";
                                    return Json(model);
                                }
                                else
                                    return Json("false");

                            }
                        }
                    }
                    break;
                case "ContactSavedForm":
                    if (onboardingformdata != null)
                    {
                        var model = !string.IsNullOrEmpty(onboardingformdata) ? serializer.Deserialize<W4FormModel>(onboardingformdata.Replace("/", "-")) : null;
                    }
                    break;
                case "BackGroundCheckForm":
                    if (onboardingformdata != null)
                    {
                        var model = !string.IsNullOrEmpty(onboardingformdata) ? serializer.Deserialize<BackgroundCheckForm>(onboardingformdata.Replace("/", "-")) : null;
                        var _model = new ContactListModel();
                        var Signaturebcf = model.Singnature;
                        Session["Signaturebcf"] = Signaturebcf;
                        eTracLoginModel ObjLoginModel = null;
                        if (Session != null)
                        {
                            if (Session["eTrac"] != null)
                            {
                                ObjLoginModel = (eTracLoginModel)(Session["eTrac"]);
                            }
                        }
                        var getApplicantId = Convert.ToInt64(Session["ApplicantId"]);
                        if (model != null)
                        {


                            var applicantId = Convert.ToInt64(Session["ApplicantId"]);
                            var sign = Signature(model.Singnature, applicantId, OnboardingForms.BackgroudCheck);
                            model.Singnature = sign;
                            #region PDF

                            var address = (List<WorkOrderEMS.Models.ApplicantAddress>)(Session["ApplicantAddress"]);
                            model.ApplicantAddress = address;
                            viewName = "_BackGroundCheckFormPdf";
                            var employeeId = ObjLoginModel.UserName;
                            path = applicantId + model.API_FirstName + viewName + ".pdf";
                            //var _FillableFormRepository = new FillableFormRepository();
                            var getDetails = _FillableFormRepository.GetFileList().Where(x => x.FLT_FileType == "Yellow" && x.FLT_Id == Convert.ToInt64(FileTypeId.W4)).FirstOrDefault();
                            var getpdf = HtmlConvertToPdf(viewName, model, path, getDetails.FLT_Id, employeeId, "Background Check");
                            #endregion PDF
                            model.API_APT_ApplicantId = getApplicantId;
                            model.UserId = ObjLoginModel.UserId;

                            //var getApplicantContact = _IApplicantManager.GetApplicantByApplicantId(getApplicantId);
                            model.FormStatusbcf = "true";
                            model.formName = "BackGroundCheckForm";
                            return Json(model);
                        }
                    }
                    break;
                case "SaveBenifit":
                    if (onboardingformdata != null)
                    {
                        var model = !string.IsNullOrEmpty(onboardingformdata) ? serializer.Deserialize<W4FormModel>(onboardingformdata.Replace("/", "-")) : null;
                    }
                    break;
                case "selfIdentificationForm":
                    if (onboardingformdata != null)
                    {
                        var model = !string.IsNullOrEmpty(onboardingformdata) ? serializer.Deserialize<SelfIdentificationModel>(onboardingformdata.Replace("/", "-")) : null;
                        if (model != null)
                        {
                            Session["EEOstatus"] = model.EEOstatus;
                            eTracLoginModel ObjLoginModel = null;
                            if (Session != null)
                            {
                                if (Session["eTrac"] != null)
                                {
                                    ObjLoginModel = (eTracLoginModel)(Session["eTrac"]);
                                    model.EmployeeId = ObjLoginModel.UserName;
                                    var getApplicantId = Convert.ToInt64(Session["ApplicantId"]);
                                }
                            }
                            var isSaved = _IApplicantManager.SaveSelfIdentification(model);
                            if (isSaved)
                            {
                                #region PDF
                                var applicantId = Convert.ToInt64(Session["ApplicantId"]);
                                viewName = "_SelfIdentificationFormPdf";
                                var employeeId = ObjLoginModel.UserName;
                                path = applicantId + model.FirstName + viewName + ".pdf";
                                //var _FillableFormRepository = new FillableFormRepository();
                                var getDetails = _FillableFormRepository.GetFileList().Where(x => x.FLT_FileType == "Yellow" && x.FLT_Id == Convert.ToInt64(FileTypeId.W4)).FirstOrDefault();
                                var getpdf = HtmlConvertToPdf(viewName, model, path, getDetails.FLT_Id, employeeId, "Self-Identification Form");
                                //return Json(true, JsonRequestBehavior.AllowGet);
                                #endregion PDF
                                model.FormStatussif = "true";
                                model.formName = "selfIdentificationForm";
                                return Json(model);

                            }
                        }
                    }
                    break;
                case "ApplicantFunFactForm":
                    if (onboardingformdata != null)
                    {
                        var obj = !string.IsNullOrEmpty(onboardingformdata) ? serializer.Deserialize<ApplicantFunFactModel>(onboardingformdata.Replace("/", "-")) : null;
                        try
                        {
                            W4FormModel model = new W4FormModel();
                            eTracLoginModel ObjLoginModel = null;
                            if (Session != null)
                            {
                                if (Session["eTrac"] != null)
                                {
                                    ObjLoginModel = (eTracLoginModel)(Session["eTrac"]);
                                    obj.Employee_Id = ObjLoginModel.UserName;
                                    obj.Applicant_Id = Convert.ToInt64(Session["ApplicantId"]);
                                }
                            }
                            if (obj != null)
                            {
                                var isSaved = _IApplicantManager.SaveApplicantFunFacts(obj);
                                if (isSaved)
                                {
                                    objloginmodel = (eTracLoginModel)(Session["etrac"]);
                                    model = _IGuestUserRepository.GetW4Form(objloginmodel.UserId);
                                    var _model = new SignatureFormModel();
                                    Session["IsSignature"] = true;//To filup form no need to display signature button so we make it hide
                                    model.IsSignature = true;

                                }
                                obj.FormStatusff = "true";
                                obj.formName = "ApplicantFunFactForm";
                                return Json(obj);
                            }
                        }
                        catch (Exception ex)
                        {
                            return Json(false, JsonRequestBehavior.AllowGet); //RedirectToAction("ApplicantFunFacts");
                        }
                        return Json(false);
                    }
                    break;
                case "RateOfPay":
                    if (onboardingformdata != null)
                    {
                        var model = !string.IsNullOrEmpty(onboardingformdata) ? serializer.Deserialize<RateOfPayModel>(onboardingformdata.Replace("/", "-")) : null;
                        //var objloginmodel = (eTracLoginModel)(Session["etrac"]);
                        var Signaturerop = model.SignatureBase;
                        Session["Signaturerop"] = Signaturerop;

                        string ImagePath = string.Empty;
                        string ImageUniqueName = string.Empty;
                        string url = string.Empty;
                        string ImageURL = string.Empty;
                        if (model != null)
                        {
                            if (model.SignatureBase != null)
                            {
                                ImagePath = Server.MapPath(ConfigurationManager.AppSettings["ApplicantSignature"].ToString());
                                ImageUniqueName = DateTime.Now.ToString("yyyyMMddHHmm") + model.ManagerName + "_" + Convert.ToInt64(Session["ApplicantId"]);
                                url = HostingPrefix + ApplicantSignature.Replace("~", "") + ImageUniqueName + ".jpg";
                                ImageURL = ImageUniqueName + ".jpg";
                                if (!Directory.Exists(ImagePath))
                                {
                                    Directory.CreateDirectory(ImagePath);
                                }
                                var ImageLocation = ImagePath + ImageURL;
                                //Save the image to directory
                                //using (MemoryStream ms = new MemoryStream(Convert.FromBase64String(model.SignatureBase)))
                                //{
                                //    using (Bitmap bm2 = new Bitmap(ms))
                                //    {

                                //        bm2.Save(ImageLocation);
                                //        model.SignatureBase = ImageURL;

                                //    }
                                //}
                            }

                            var applicantId = Convert.ToInt64(Session["ApplicantId"]);
                            var sign = Signature(model.SignatureBase, applicantId, OnboardingForms.RateOfPay);
                            model.SignatureBase = sign;
                            #region PDF

                            viewName = "_RateOfPayPdf";
                            var employeeId = objloginmodel.UserName;
                            path = applicantId + model.EmployeeName.Trim() + viewName + ".pdf";
                            //var _FillableFormRepository = new FillableFormRepository();
                            var getDetails = _FillableFormRepository.GetFileList().Where(x => x.FLT_FileType == "Yellow" && x.FLT_Id == Convert.ToInt64(FileTypeId.W4)).FirstOrDefault();
                            var getpdf = HtmlConvertToPdf(viewName, model, path, getDetails.FLT_Id, employeeId, "Rate Of Pay");
                            //return Json(true, JsonRequestBehavior.AllowGet);
                            #endregion PDF

                            model.FormStatusrop = "true";
                            model.formName = "RateOfPay";
                            return Json(model);
                        }
                    }
                    break;


            }

            return Json(false);

        }
        [HttpGet]
        public PartialViewResult _PhotoReleaseForm()
        {
            PhotoRelease model = new PhotoRelease();
            var objloginmodel = (eTracLoginModel)(Session["etrac"]);
            var d = _IGuestUserRepository.GetPhotoRelease(objloginmodel.UserId);
            if (d == null)
            {
                var apt_Id = Convert.ToInt64(Session["ApplicantId"]);
                var getApplicantDetails = _IApplicantManager.GetApplicantAllDetails(apt_Id);
                model.Name = getApplicantDetails.FirstName + " " + getApplicantDetails.LastName;
                model.EmpId = objloginmodel.UserName;
                model.FirstName = getApplicantDetails.FirstName;
                model.LastName = getApplicantDetails.LastName;

                return PartialView("_PhotoReleaseForm", model);
            }
            else
                model.Name = d;
            return PartialView("_PhotoReleaseForm", model);
        }

        /// <summary>
        /// click next in photo release form and go to self identification form
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult _photoreleaseform(PhotoRelease model)
        {
            if (model != null)
            {
                var getApplicantId = Convert.ToInt64(Session["ApplicantId"]);
                var _model = new SelfIdentificationModel();
                var getApplicantDetails = _IApplicantManager.GetApplicantAllDetails(getApplicantId);
                _model.FirstName = getApplicantDetails.FirstName;
                _model.MidleName = getApplicantDetails.MiddleName;
                _model.LastName = getApplicantDetails.LastName;
                _model.Gender = getApplicantDetails.EMP_Gender;

                _model.FormStatusw4 = model.FormStatusw4;
                _model.FormStatusbcf = model.FormStatusbcf;
                _model.FormStatusdd = model.FormStatusdd;
                _model.FormStatusEvf = model.FormStatusEvf;
                _model.FormStatusI9 = model.FormStatusI9;
                _model.FormStatusprfcaf = model.FormStatusprfcaf;
                _model.FormStatusprfecf = model.FormStatusprfecf;
                _model.FormStatusrop = model.FormStatusrop;
                _model.FormStatussif = model.FormStatussif;
                _model.FormStatusprf = model.FormStatusprf;
                _model.FormStatusff = model.FormStatusff;
                _model.formName = "selfIdentificationForm";
                return PartialView("PartialView/_SelfIdentificationForm", _model);

            }

            return RedirectToAction("_PhotoReleaseForm");
        }

        /// <summary>
        /// Added by: Rajat Toppo
        /// Date:20-07-2020
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        public PartialViewResult photoreleaseformfromCheckList(FormNameStatus obj)
        {
            PhotoRelease model = new PhotoRelease();
            var objloginmodel = (eTracLoginModel)(Session["etrac"]);
            var d = _IGuestUserRepository.GetPhotoRelease(objloginmodel.UserId);
            var Signprf = Convert.ToString(Session["Signatureprf"]);
            var newSignprf = Signprf.Replace("-", "/");
            
            if (d == null)
            {
                var apt_Id = Convert.ToInt64(Session["ApplicantId"]);
                var getApplicantDetails = _IApplicantManager.GetApplicantAllDetails(apt_Id);
                model.Name = getApplicantDetails.FirstName + " " + getApplicantDetails.LastName;
                model.EmpId = objloginmodel.UserName;
                model.FirstName = getApplicantDetails.FirstName;
                model.LastName = getApplicantDetails.LastName;
                model.FormStatusw4 = obj.FormStatusw4;
                model.FormStatusbcf = obj.FormStatusbcf;
                model.FormStatusdd = obj.FormStatusdd;
                model.FormStatusEvf = obj.FormStatusEvf;
                model.FormStatusI9 = obj.FormStatusI9;
                model.FormStatusprfcaf = obj.FormStatusprfcaf;
                model.FormStatusprfecf = obj.FormStatusprfecf;
                model.FormStatusrop = obj.FormStatusrop;
                model.FormStatussif = obj.FormStatussif;
                model.FormStatusprf = obj.FormStatusprf;
                model.FormStatusff = obj.FormStatusff;
                model.formName = "photoreleaseform";
                model.Signature = newSignprf;
                return PartialView("_PhotoReleaseForm", model);
            }
            else
                model.Name = d;
            model.FormStatusw4 = obj.FormStatusw4;
            model.FormStatusbcf = obj.FormStatusbcf;
            model.FormStatusdd = obj.FormStatusdd;
            model.FormStatusEvf = obj.FormStatusEvf;
            model.FormStatusI9 = obj.FormStatusI9;
            model.FormStatusprfcaf = obj.FormStatusprfcaf;
            model.FormStatusprfecf = obj.FormStatusprfecf;
            model.FormStatusrop = obj.FormStatusrop;
            model.FormStatussif = obj.FormStatussif;
            model.FormStatusprf = obj.FormStatusprf;
            model.FormStatusff = obj.FormStatusff;
            model.formName = "photoreleaseform";
            model.Signature = newSignprf;
            return PartialView("_PhotoReleaseForm", model);
        }
        [HttpGet]
        public PartialViewResult _EducationVarificationForm()
        {
            var model = new List<EducationVarificationModel>();
            var objloginmodel = (eTracLoginModel)(Session["etrac"]);
            var applicantId = Convert.ToInt64(Session["ApplicantId"]);
            model = _IGuestUserRepository.GetEducationVerificationForm(objloginmodel.UserId, applicantId);
            if (model != null)
            {
                Session["EducationForm"] = model;
                return PartialView("_EducationVarificationForm", model);
            }
            else
                return PartialView("_EducationVarificationForm", new List<EducationVarificationModel>());
        }
        [HttpPost]
        public PartialViewResult _EducationVarificationFormForChecklist(FormNameStatus _model)
        {
            var model = new List<EducationVarificationModel>();
            var objloginmodel = (eTracLoginModel)(Session["etrac"]);
            var applicantId = Convert.ToInt64(Session["ApplicantId"]);
            model = _IGuestUserRepository.GetEducationVerificationForm(objloginmodel.UserId, applicantId);
            if (model != null)
            {
                Session["EducationForm"] = model;
                model[0].FormStatusw4 = _model.FormStatusw4;
                model[0].FormStatusbcf = _model.FormStatusbcf;
                model[0].FormStatusdd = _model.FormStatusdd;
                model[0].FormStatusEvf = _model.FormStatusEvf;
                model[0].FormStatusI9 = _model.FormStatusI9;
                model[0].FormStatusprfcaf = _model.FormStatusprfcaf;
                model[0].FormStatusprfecf = _model.FormStatusprfecf;
                model[0].FormStatusrop = _model.FormStatusrop;
                model[0].FormStatussif = _model.FormStatussif;
                model[0].FormStatusprf = _model.FormStatusprf;
                model[0].FormStatusff = _model.FormStatusff;
                model[0].formName = "educationverificationform";
                var SignI9 = Convert.ToString(Session["SignatureI9"]);
                var newSignI9 = SignI9.Replace("-", "/");
                model[0].Signature = newSignI9;
                return PartialView("_EducationVarificationForm", model);
            }
            else
            {
                return PartialView("_EducationVarificationForm", new List<EducationVarificationModel>());
            }
        }
        [HttpPost]
        //public ActionResult _EducationVarificationForm(List<EducationVarificationModel> model)
        public ActionResult _EducationVarificationForm(EducationVarificationModel model)//need to change
        {
            if (model != null)
            {
                var obj = new RateOfPayModel();
                var objloginmodel = (eTracLoginModel)(Session["etrac"]);
                var Applicant_Id = Convert.ToInt64(Session["ApplicantId"]);
                var employeeId = objloginmodel.UserName;
                obj = _IApplicantManager.GetRateOfPayInfo(Applicant_Id, employeeId);

                obj.FormStatusw4 = model.FormStatusw4;
                obj.FormStatusbcf = model.FormStatusbcf;
                obj.FormStatusdd = model.FormStatusdd;
                obj.FormStatusEvf = model.FormStatusEvf;
                obj.FormStatusI9 = model.FormStatusI9;
                obj.FormStatusprfcaf = model.FormStatusprfcaf;
                obj.FormStatusprfecf = model.FormStatusprfecf;
                obj.FormStatusrop = model.FormStatusrop;
                obj.FormStatussif = model.FormStatussif;
                obj.FormStatusprf = model.FormStatusprf;
                obj.FormStatusff = model.FormStatusff;
                obj.formName = "RateOfPay";

                return PartialView("_RateOfPay", obj);
            }
            else
            {
                return RedirectToAction("_RateOfPay");
            }

        }

        /// <summary>
        /// Added by Rajat Toppo
        /// Date: 20-07-2020
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        public PartialViewResult _SaveEducationVarificationForm(EducationVarificationModel obj)
        // public JsonResult _SaveEducationVarificationForm(List<EducationVarificationModel> model)
        {
            var model = new List<EducationVarificationModel>();
            var objloginmodel = (eTracLoginModel)(Session["etrac"]);
            var applicantId = Convert.ToInt64(Session["ApplicantId"]);
            model = _IGuestUserRepository.GetEducationVerificationForm(objloginmodel.UserId, applicantId);

            if (model != null)
            {
                //model.FormStatusw4 = obj.FormStatusw4;
                //model.FormStatusbcf = obj.FormStatusbcf;
                //model.FormStatusdd = obj.FormStatusdd;
                //model.FormStatusEvf = obj.FormStatusEvf;
                //model.FormStatusI9 = obj.FormStatusI9;
                //model.FormStatusprfcaf = obj.FormStatusprfcaf;
                //model.FormStatusprfecf = obj.FormStatusprfecf;
                //model.FormStatusrop = obj.FormStatusrop;
                //model.FormStatussif = obj.FormStatussif;
                //model.FormStatusprf = obj.FormStatusprf;
                //model.FormStatusff = obj.FormStatusff;
                //model.formName = "emergencycontactform";
                Session["EducationForm"] = model;
                return PartialView("_EducationVarificationForm", model);
            }
            else
                return PartialView("_EducationVarificationForm", new List<EducationVarificationModel>());

        }

        [HttpGet]
        public PartialViewResult _ConfidentialityAgreementForm()
        {
            var objloginmodel = (eTracLoginModel)(Session["etrac"]);
            var APT_Id = Convert.ToInt64(Session["ApplicantId"]);
            var getDetails = _IApplicantManager.GetApplicantAllDetails(APT_Id);
            var _model = new ConfidenialityAgreementModel();
            _model.EmpAddress = getDetails.Address;
            _model.EmployeeName = getDetails.FirstName + " " + getDetails.LastName;
            _model.Between = getDetails.FirstName + " " + getDetails.LastName;
            _model.Title = getDetails.Title;

            _model.Date = DateTime.Now.ToString("dd-MM-yyyy");

            return PartialView("_ConfidentialityAgreementForm", _model);
        }
        /// <summary>
        /// click on confidentiality agreement form and go to photo releaseform
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult _ConfidentialityAgreementForm(ConfidenialityAgreementModel model)
        {
            var objloginmodel = (eTracLoginModel)(Session["etrac"]);
            if (model != null)
            {
                PhotoRelease _model = new PhotoRelease();

                var d = _IGuestUserRepository.GetPhotoRelease(objloginmodel.UserId);
                //if (d == null)
                //{
                var apt_Id = Convert.ToInt64(Session["ApplicantId"]);
                var getApplicantDetails = _IApplicantManager.GetApplicantAllDetails(apt_Id);
                _model.Name = getApplicantDetails.FirstName + " " + getApplicantDetails.LastName;
                _model.EmpId = objloginmodel.UserName;
                _model.FirstName = getApplicantDetails.FirstName;
                _model.LastName = getApplicantDetails.LastName;
                //}
                _model.FormStatusw4 = model.FormStatusw4;
                _model.FormStatusbcf = model.FormStatusbcf;
                _model.FormStatusdd = model.FormStatusdd;
                _model.FormStatusEvf = model.FormStatusEvf;
                _model.FormStatusI9 = model.FormStatusI9;
                _model.FormStatusprfcaf = model.FormStatusprfcaf;
                _model.FormStatusprfecf = model.FormStatusprfecf;
                _model.FormStatusrop = model.FormStatusrop;
                _model.FormStatussif = model.FormStatussif;
                _model.FormStatusprf = model.FormStatusprf;
                _model.FormStatusff = model.FormStatusff;
                _model.formName = "photoreleaseform";

                return PartialView("_PhotoReleaseForm", _model);


            }
            return RedirectToAction("_ConfidentialityAgreementForm");
        }

        /// <summary>
        /// Added by: Rajat Toppo
        /// Date: 20-07-2020
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        public PartialViewResult ConfidentialityAgreementFormFromCheckList(FormNameStatus model)
        {
            var objloginmodel = (eTracLoginModel)(Session["etrac"]);
            var APT_Id = Convert.ToInt64(Session["ApplicantId"]);
            var getDetails = _IApplicantManager.GetApplicantAllDetails(APT_Id);
            var Signcaf = Convert.ToString(Session["Signaturecaf"]);
            var newSigncaf = Signcaf.Replace("-", "/");

            var _model = new ConfidenialityAgreementModel();
            _model.EmpAddress = getDetails.Address;
            _model.EmployeeName = getDetails.FirstName + " " + getDetails.LastName;
            _model.Between = getDetails.FirstName + " " + getDetails.LastName;
            _model.Title = getDetails.Title;

            _model.Date = DateTime.Now.ToString("dd-MM-yyyy");
            _model.Signature = newSigncaf;
            _model.FormStatusw4 = model.FormStatusw4;
            _model.FormStatusbcf = model.FormStatusbcf;
            _model.FormStatusdd = model.FormStatusdd;
            _model.FormStatusEvf = model.FormStatusEvf;
            _model.FormStatusI9 = model.FormStatusI9;
            _model.FormStatusprfcaf = model.FormStatusprfcaf;
            _model.FormStatusprfecf = model.FormStatusprfecf;
            _model.FormStatusrop = model.FormStatusrop;
            _model.FormStatussif = model.FormStatussif;
            _model.FormStatusprf = model.FormStatusprf;
            _model.FormStatusff = model.FormStatusff;
            _model.formName = "confidentialityagreementform";

            return PartialView("_ConfidentialityAgreementForm", _model);
        }
        [HttpGet]
        public PartialViewResult _CreditCardAuthorizationForm()
        {
            return PartialView("_CreditCardAuthorizationForm");
        }
        [HttpGet]
        public PartialViewResult _PreviousEmployeement()
        {
            return PartialView("_PreviousEmployeement");
        }
        [HttpGet]
        public PartialViewResult _emergencyContactForm()
        {
            var model = new EmergencyContectForm();
            var objloginmodel = (eTracLoginModel)(Session["etrac"]);
            var apt_Id = Convert.ToInt64(Session["ApplicantId"]);
            model = _IGuestUserRepository.GetEmergencyForm(objloginmodel.UserId);
            if (model == null)
            {
                var _model = new EmergencyContectForm();
                var getApplicantDetails = _IApplicantManager.GetApplicantAllDetails(apt_Id);
                _model.MiddleName = getApplicantDetails.MiddleName;
                _model.FirstName = getApplicantDetails.FirstName;
                _model.LastName = getApplicantDetails.LastName;
                _model.HomePhone = getApplicantDetails.Phone;
                _model.HomeAddress = getApplicantDetails.Address;
                _model.Citizenship = getApplicantDetails.Cityzenship;
                _model.EmpId = objloginmodel.UserName;
                _model.HomeEmail = getApplicantDetails.Email;
                _model.SSN = getApplicantDetails.SocialSecurityNumber;
                _model.License = getApplicantDetails.DlNumber;
                return PartialView("_emergencyContactForm", _model);
            }
            return PartialView("_emergencyContactForm", model);
        }
        /// <summary>
        /// click on next in emergency contact form and go to direct deposit form
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult _emergencyContactForm(EmergencyContectForm model)
        {
            if (model != null)
            {
                var _model = new DirectDepositeFormModel();
                var ObjLoginModel = (eTracLoginModel)(Session["eTrac"]);
                _model = _IGuestUserRepository.GetDirectDepositeDataByUserId(ObjLoginModel.UserId);
                _model.FormStatusw4 = model.FormStatusw4;
                _model.FormStatusbcf = model.FormStatusbcf;
                _model.FormStatusdd = model.FormStatusdd;
                _model.FormStatusEvf = model.FormStatusEvf;
                _model.FormStatusI9 = model.FormStatusI9;
                _model.FormStatusprfcaf = model.FormStatusprfcaf;
                _model.FormStatusprfecf = model.FormStatusprfecf;
                _model.FormStatusrop = model.FormStatusrop;
                _model.FormStatussif = model.FormStatussif;
                _model.FormStatusprf = model.FormStatusprf;
                _model.FormStatusff = model.FormStatusff;
                _model.formName = "depositeForm";
                return PartialView("_directDepositeForm", _model);
            }

            return RedirectToAction("_directDepositeForm");

        }

        /// <summary>
        /// Added by: Rajat Toppo
        /// Date:20-07-2020
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        public PartialViewResult emergencyContactFormCheckList(EmergencyContectForm obj)
        {
            var model = new EmergencyContectForm();
            var objloginmodel = (eTracLoginModel)(Session["etrac"]);
            var apt_Id = Convert.ToInt64(Session["ApplicantId"]);
            model = _IGuestUserRepository.GetEmergencyForm(objloginmodel.UserId);
            if (model == null)
            {
                var _model = new EmergencyContectForm();
                var getApplicantDetails = _IApplicantManager.GetApplicantAllDetails(apt_Id);
                _model.MiddleName = getApplicantDetails.MiddleName;
                _model.FirstName = getApplicantDetails.FirstName;
                _model.LastName = getApplicantDetails.LastName;
                _model.HomePhone = getApplicantDetails.Phone;
                _model.HomeAddress = getApplicantDetails.Address;
                _model.Citizenship = getApplicantDetails.Cityzenship;
                _model.EmpId = objloginmodel.UserName;
                _model.HomeEmail = getApplicantDetails.Email;
                _model.SSN = getApplicantDetails.SocialSecurityNumber;
                _model.License = getApplicantDetails.DlNumber;

                _model.FormStatusw4 = obj.FormStatusw4;
                _model.FormStatusbcf = obj.FormStatusbcf;
                _model.FormStatusdd = obj.FormStatusdd;
                _model.FormStatusEvf = obj.FormStatusEvf;
                _model.FormStatusI9 = obj.FormStatusI9;
                _model.FormStatusprfcaf = obj.FormStatusprfcaf;
                _model.FormStatusprfecf = obj.FormStatusprfecf;
                _model.FormStatusrop = obj.FormStatusrop;
                _model.FormStatussif = obj.FormStatussif;
                _model.FormStatusprf = obj.FormStatusprf;
                _model.FormStatusff = obj.FormStatusff;
                _model.formName = "emergencycontactform";
                return PartialView("_emergencyContactForm", _model);
            }
            model.FormStatusw4 = obj.FormStatusw4;
            model.FormStatusbcf = obj.FormStatusbcf;
            model.FormStatusdd = obj.FormStatusdd;
            model.FormStatusEvf = obj.FormStatusEvf;
            model.FormStatusI9 = obj.FormStatusI9;
            model.FormStatusprfcaf = obj.FormStatusprfcaf;
            model.FormStatusprfecf = obj.FormStatusprfecf;
            model.FormStatusrop = obj.FormStatusrop;
            model.FormStatussif = obj.FormStatussif;
            model.FormStatusprf = obj.FormStatusprf;
            model.FormStatusff = obj.FormStatusff;
            model.formName = "emergencycontactform";
            return PartialView("_emergencyContactForm", model);
        }
        [HttpGet]
        public ActionResult GetFormsStatus()
        {
            var objloginmodel = (eTracLoginModel)(Session["etrac"]);
            var data = _IGuestUserRepository.GetFormsStatus(objloginmodel.UserId);
            return Json(data, JsonRequestBehavior.AllowGet);
        }
        /// <summary>
        /// Created By : Ashwajit Bansod
        /// Created Date : 12-Feb-2020
        /// Create For : TO open Contact Modal
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public PartialViewResult _ContactInfo()
        {
            var getApplicantId = Convert.ToInt64(Session["ApplicantId"]);
            var getApplicantContact = _IApplicantManager.GetContactListByApplicantId(getApplicantId);
            return PartialView("PartialView/_CommonModals", getApplicantContact);
        }
        /// <summary>
        /// Created By  :Ashwajit Bansod
        /// Created Date : 11-Feb-2020
        /// Created For : To update Contact Details
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult _ContactSavedForm(ContactListModel model, List<ContactModel> lstModel)
        {
            try
            {
                eTracLoginModel ObjLoginModel = null;
                if (Session != null)
                {
                    if (Session["eTrac"] != null)
                    {
                        ObjLoginModel = (eTracLoginModel)(Session["eTrac"]);
                    }
                }

                if (lstModel.Count() > 0)
                {
                    var updateContact = _IApplicantManager.UpdateContactDetailsApplicant(model, lstModel);
                    if (updateContact)
                    {
                        return Json(true, JsonRequestBehavior.AllowGet);
                    }
                    else
                    {
                        return Json(false, JsonRequestBehavior.AllowGet);
                    }
                }
            }
            catch (Exception ex)
            {
                return Json(false, JsonRequestBehavior.AllowGet);
            }
            return RedirectToAction("_emergencyContactForm");
        }

        /// <summary>
        /// Added by: Rajat Toppo
        /// Date: 20-07-2020
        /// </summary>
        /// <param name="model"></param>
        /// <param name="lstModel"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult _SaveContactSavedForm(ContactListModel model, List<ContactModel> lstModel)
        {
            try
            {
                eTracLoginModel ObjLoginModel = null;
                if (Session != null)
                {
                    if (Session["eTrac"] != null)
                    {
                        ObjLoginModel = (eTracLoginModel)(Session["eTrac"]);
                    }
                }
                if (lstModel.Count() > 0)
                {
                    var updateContact = _IApplicantManager.UpdateContactDetailsApplicant(model, lstModel);
                    if (updateContact)
                    {
                        return Json(true, JsonRequestBehavior.AllowGet);
                    }
                    else
                    {
                        return Json(false, JsonRequestBehavior.AllowGet);
                    }
                }
            }
            catch (Exception ex)
            {
                return Json(false, JsonRequestBehavior.AllowGet);
            }
            return RedirectToAction("_emergencyContactForm");
        }
        /// <summary>
        /// Created By : Ashwajit Bansod
        /// Created Date : 12-Feb-2020
        /// Created For : To Get Applicant details by applicant Id
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult _BackGroundCheckForm()
        {
            var getApplicant = new BackgroundCheckForm();
            try
            {
                var getApplicantId = Convert.ToInt64(Session["ApplicantId"]);
                getApplicant = _IApplicantManager.GetApplicantByApplicantId(getApplicantId);
                Session["ApplicantAddress"] = getApplicant.ApplicantAddress;
                getApplicant.IsSignature = false;

                return PartialView("PartialView/_BackGroundCheckForm", getApplicant);
            }

            catch (Exception ex)
            {
                return PartialView("PartialView/_BackGroundCheckForm", getApplicant);
            }
        }

        /// <summary>
        /// Created By : Ashwajit Bansod
        /// Created Date : 12-Feb-2020
        /// Created For : To send Applicant Details For Backgroud check and Create for same
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult _BackGroundCheckForm(BackgroundCheckForm Obj)
        {
            var model = new EmergencyContectForm();
            var objloginmodel = (eTracLoginModel)(Session["etrac"]);
            var apt_Id = Convert.ToInt64(Session["ApplicantId"]);
            model = _IGuestUserRepository.GetEmergencyForm(objloginmodel.UserId);
            if (model == null)
            {
                var _model = new EmergencyContectForm();
                var getApplicantDetails = _IApplicantManager.GetApplicantAllDetails(apt_Id);
                _model.MiddleName = getApplicantDetails.MiddleName;
                _model.FirstName = getApplicantDetails.FirstName;
                _model.LastName = getApplicantDetails.LastName;
                _model.HomePhone = getApplicantDetails.Phone;
                _model.HomeAddress = getApplicantDetails.Address;
                _model.Citizenship = getApplicantDetails.Cityzenship;
                _model.EmpId = objloginmodel.UserName;
                _model.HomeEmail = getApplicantDetails.Email;
                _model.SSN = getApplicantDetails.SocialSecurityNumber;
                _model.License = getApplicantDetails.DlNumber;

                _model.FormStatusw4 = Obj.FormStatusw4;
                _model.FormStatusbcf = Obj.FormStatusbcf;
                _model.FormStatusdd = Obj.FormStatusdd;
                _model.FormStatusEvf = Obj.FormStatusEvf;
                _model.FormStatusI9 = Obj.FormStatusI9;
                _model.FormStatusprfcaf = Obj.FormStatusprfcaf;
                _model.FormStatusprfecf = Obj.FormStatusprfecf;
                _model.FormStatusrop = Obj.FormStatusrop;
                _model.FormStatussif = Obj.FormStatussif;
                _model.FormStatusprf = Obj.FormStatusprf;
                _model.FormStatusff = Obj.FormStatusff;
                _model.formName = "emergencycontactform";

                return PartialView("_emergencyContactForm", _model);

            }
            else
            {
                model.FormStatusw4 = Obj.FormStatusw4;
                model.FormStatusbcf = Obj.FormStatusbcf;
                model.FormStatusdd = Obj.FormStatusdd;
                model.FormStatusEvf = Obj.FormStatusEvf;
                model.FormStatusI9 = Obj.FormStatusI9;
                model.FormStatusprfcaf = Obj.FormStatusprfcaf;
                model.FormStatusprfecf = Obj.FormStatusprfecf;
                model.FormStatusrop = Obj.FormStatusrop;
                model.FormStatussif = Obj.FormStatussif;
                model.FormStatusprf = Obj.FormStatusprf;
                model.FormStatusff = Obj.FormStatusff;
                model.formName = "emergencycontactform";

                return PartialView("_emergencyContactForm", model);

                //return PartialView("_emergencyContactForm", model);
            }

        }

        /// <summary>
        /// Added by: Rajat Toppo
        /// Date: 20-07-2020
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        public PartialViewResult BackGroundCheckFormCheckList(FormNameStatus model)
        {
            var getApplicant = new BackgroundCheckForm();
            try
            {
                var getApplicantId = Convert.ToInt64(Session["ApplicantId"]);
                getApplicant = _IApplicantManager.GetApplicantByApplicantId(getApplicantId);
                Session["ApplicantAddress"] = getApplicant.ApplicantAddress;
                var Signbcf = Convert.ToString(Session["Signaturebcf"]);
                var newSignbcf = Signbcf.Replace("-", "/");
                getApplicant.Singnature = newSignbcf;
                getApplicant.IsSignature = false;
                getApplicant.FormStatusw4 = model.FormStatusw4;
                getApplicant.FormStatusbcf = model.FormStatusbcf;
                getApplicant.FormStatusdd = model.FormStatusdd;
                getApplicant.FormStatusEvf = model.FormStatusEvf;
                getApplicant.FormStatusI9 = model.FormStatusI9;
                getApplicant.FormStatusprfcaf = model.FormStatusprfcaf;
                getApplicant.FormStatusprfecf = model.FormStatusprfecf;
                getApplicant.FormStatusrop = model.FormStatusrop;
                getApplicant.FormStatussif = model.FormStatussif;
                getApplicant.FormStatusprf = model.FormStatusprf;
                getApplicant.FormStatusff = model.FormStatusff;
                getApplicant.formName = "BackgroundCheckForm";



                return PartialView("PartialView/_BackGroundCheckForm", getApplicant);
            }

            catch (Exception ex)
            {
                return PartialView("PartialView/_BackGroundCheckForm", getApplicant);
            }
        }
        /// <summary>
        /// Created By  :Ashwajit Bansod
        /// Created Date : 17-Feb-2020
        /// Created For : To upload applicant file .
        /// </summary>
        /// <param name="filesLicense"></param>
        /// <param name="filesSSN"></param>
        /// <returns></returns>
        public ActionResult UploadFilesApplicant(bool isLicense)
        {
            var Obj = new UploadedFiles();
            var _db = new workorderEMSEntities();

            eTracLoginModel ObjLoginModel = null;
            //Serves as the base class for classes that provide access to files that were uploaded by a client.
            HttpFileCollectionBase files = Request.Files;
            if (Session["eTrac"] != null)
            {
                ObjLoginModel = (eTracLoginModel)(Session["eTrac"]);
            }
            if (files.Count > 0)
            {
                try
                {
                    //  Get all files from Request object  
                    for (int i = 0; i < files.Count; i++)
                    {
                        HttpPostedFileBase file = files[i];
                        string fname;
                        // Checking for Internet Explorer  
                        if (Request.Browser.Browser.ToUpper() == "IE" || Request.Browser.Browser.ToUpper() == "INTERNETEXPLORER")
                        {
                            string[] testfiles = file.FileName.Split(new char[] { '\\' });
                            fname = testfiles[testfiles.Length - 1];
                        }
                        else
                        {
                            fname = file.FileName;
                        }

                        var getUser = _db.UserRegistrations.Where(x => x.UserId == ObjLoginModel.UserId && x.IsDeleted == false && x.IsEmailVerify == true).FirstOrDefault();
                        if (getUser != null)
                        {
                            if (fname != null)
                            {
                                string FName = ObjLoginModel.UserId + "_" + DateTime.Now.Ticks.ToString() + "_" + fname;
                                CommonHelper.StaticUploadImage(file, Server.MapPath(ConfigurationManager.AppSettings["FilesUploadRedYellowGreen"]), FName);
                                Obj.AttachedFileName = FName;
                                Obj.FileName = fname;
                                Obj.FileEmployeeId = getUser.EmployeeID;
                                Obj.FileId = Convert.ToInt64(Helper.FileType.FileType);
                                var IsSaved = _IFillableFormManager.SaveFile(Obj, Obj.FileEmployeeId);
                            }
                        }
                    }
                    // Returns message that successfully uploaded  
                    if (isLicense == false)
                    {
                        //return Json(true, JsonRequestBehavior.AllowGet);
                        return Json(true);
                    }

                    else
                    {
                        //return Json(true, JsonRequestBehavior.AllowGet);
                        return Json(true);
                    }
                }
                catch (Exception ex)
                {
                    //return Json(true, JsonRequestBehavior.AllowGet);
                    return RedirectToAction("_emergencyContactForm");
                }
            }
            else
            {
                return Json("No files selected.");
            }
        }
        /// <summary>
        /// Created By : Ashwajit Bansod
        /// Created Date : 18-Feb-2020
        /// Created For : To get signature by applicant Id
        /// </summary>
        /// <returns></returns>
        public JsonResult GetSignature()
        {
            var getSignature = new Desclaimer();
            var url = string.Empty;
            try
            {
                var signature = string.Empty;
                var getApplicantId = Convert.ToInt64(Session["ApplicantId"]);
                if (getApplicantId > 0)
                {
                    getSignature = _IApplicantManager.GetSignature(getApplicantId);
                    if (getSignature != null)
                    {
                        url = HostingPrefix + ApplicantSignature.Replace("~", "") + getSignature.Signature + ".jpg";
                        return Json(new { name = getSignature.Signature, imagePath = url }, JsonRequestBehavior.AllowGet);
                    }
                    else
                    {
                        return Json(false, JsonRequestBehavior.AllowGet);
                    }
                }
                return Json(new { name = getSignature.Signature, imagePath = url }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(false, JsonRequestBehavior.AllowGet);
            }
        }
        /// <summary>
        /// Created By : Ashwajit Bansod
        /// Created Date : 19-Feb-2020
        /// Created For :  To save and update signature by using update condition.
        /// </summary>
        /// <param name="isUpdate"></param>
        /// <returns></returns>
        public JsonResult SaveSignature(bool isUpdate)
        {
            var Obj = new Desclaimer();
            var _db = new workorderEMSEntities();
            var getApplicantId = Convert.ToInt64(Session["ApplicantId"]);
            eTracLoginModel ObjLoginModel = null;
            if (isUpdate == true)
            {
                HttpFileCollectionBase files = Request.Files;
                if (Session["eTrac"] != null)
                {
                    ObjLoginModel = (eTracLoginModel)(Session["eTrac"]);
                }
                if (files.Count > 0)
                {
                    try
                    {
                        //  Get all files from Request object  
                        for (int i = 0; i < files.Count; i++)
                        {
                            HttpPostedFileBase file = files[i];
                            string fname;
                            // Checking for Internet Explorer  
                            if (Request.Browser.Browser.ToUpper() == "IE" || Request.Browser.Browser.ToUpper() == "INTERNETEXPLORER")
                            {
                                string[] testfiles = file.FileName.Split(new char[] { '\\' });
                                fname = testfiles[testfiles.Length - 1];
                            }
                            else
                            {
                                fname = file.FileName;
                            }

                            var getUser = _db.UserRegistrations.Where(x => x.UserId == ObjLoginModel.UserId && x.IsDeleted == false && x.IsEmailVerify == true).FirstOrDefault();
                            if (getUser != null)
                            {
                                if (fname != null)
                                {
                                    string FName = ObjLoginModel.UserId + "_" + DateTime.Now.Ticks.ToString() + "_" + fname;
                                    CommonHelper.StaticUploadImage(file, Server.MapPath(ConfigurationManager.AppSettings["ApplicantSignature"]), FName);
                                    Obj.Signature = FName;
                                    Obj.Signature = fname;
                                    Obj.EmployeeId = getUser.EmployeeID;
                                    var IsSaved = _IApplicantManager.SaveDesclaimerData(Obj);
                                }
                            }
                        }
                        // Returns message that successfully uploaded  
                        return Json("File Uploaded Successfully!");
                    }
                    catch (Exception ex)
                    {
                        return Json("Error occurred. Error details: " + ex.Message);
                    }
                }
                else
                {
                    return Json("No files selected.");
                }
            }
            else
            {

                var getDetails = _IApplicantManager.GetSignature(getApplicantId);
                if (getDetails != null)
                {
                    Obj.ApplicantId = getDetails.ApplicantId;
                    Obj.ASG_Date = getDetails.ASG_Date;
                    Obj.EmployeeId = getDetails.EmployeeId;
                    Obj.IsActive = getDetails.IsActive;
                    Obj.Sing_Id = getDetails.Sing_Id;
                    Obj.Signature = getDetails.Signature;
                    var set = _IApplicantManager.SaveDesclaimerData(Obj);
                }
                return Json("Signature is added successfully.");
            }
        }
        /// <summary>
        /// Created By : Ashwajit Bansod
        /// Created Date : 19-Feb-2020
        /// Created For  : To return partial view of Benifi section
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public PartialViewResult BenifitSection()
        {
            try
            {
                var lst = new ePeopleManager();
                var getApplicantId = Convert.ToInt64(Session["ApplicantId"]);
                var getApplicantContact = lst.GetBenifitList(getApplicantId);
                if (getApplicantContact != null && getApplicantContact.configurations.Count() > 0)
                    return PartialView("PartialView/_BenifitSectionFloridaBlue", getApplicantContact);
                else
                    return PartialView("PartialView/_BenifitSectionFloridaBlue", new BenefitList());
            }
            catch (Exception ex)
            {
                return PartialView("PartialView/_BenifitSectionFloridaBlue", new BenefitList());
            }
        }
        /// <summary>
        /// Created by  :Ashwajit Bansod
        /// Created Date : 20-Feb-2020
        /// Created For : To save Benifits from florida blue
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult BenifitSection(BenifitSectionModel obj, bool formStatus)
        {
            eTracLoginModel ObjLoginModel = null;
            if (Session != null)
            {
                if (Session["eTrac"] != null)
                {
                    ObjLoginModel = (eTracLoginModel)(Session["eTrac"]);
                }
            }
            return Json(true, JsonRequestBehavior.AllowGet);
            //return RedirectToAction("SelfIdentificationForm");
        }
        /// <summary>
        /// Added by Rajat Toppo
        /// Date:20-072020
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult _SaveBenifitSection(BenifitSectionModel obj)
        {
            eTracLoginModel ObjLoginModel = null;
            if (Session != null)
            {
                if (Session["eTrac"] != null)
                {
                    ObjLoginModel = (eTracLoginModel)(Session["eTrac"]);
                }
            }
            return Json(true, JsonRequestBehavior.AllowGet);
            //return RedirectToAction("SelfIdentificationForm");
        }
        /// <summary>
        /// Created By : Ashwajit Bansod
        /// Created Date : 20-Feb-2020
        /// Created For : To open self identification form if applciant want to make there data confedential
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult SelfIdentificationForm()
        {
            var getApplicantId = Convert.ToInt64(Session["ApplicantId"]);
            var _model = new SelfIdentificationModel();
            var getApplicantDetails = _IApplicantManager.GetApplicantAllDetails(getApplicantId);
            _model.FirstName = getApplicantDetails.FirstName;
            _model.MidleName = getApplicantDetails.MiddleName;
            _model.LastName = getApplicantDetails.LastName;
            _model.Gender = getApplicantDetails.EMP_Gender;

            return PartialView("PartialView/_SelfIdentificationForm", _model);
        }
        /// <summary>
        /// Created by : Ashwajit Bansod
        /// Created Date : 21-Feb-2020
        /// Created For : TO save self identification form of applicant
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult SaveSelfIdentificationForm(SelfIdentificationModel model)
        {
            try
            {
                if (model.EEOstatus == "false" && model.FormStatussif == null)
                {
                    Session["EEOstatus"] = "false";
                }
                eTracLoginModel ObjLoginModel = null;
                if (Session != null)
                {
                    if (Session["eTrac"] != null)
                    {
                        ObjLoginModel = (eTracLoginModel)(Session["eTrac"]);
                        model.EmployeeId = ObjLoginModel.UserName;
                        var getApplicantId = Convert.ToInt64(Session["ApplicantId"]);
                    }
                }
                if (model != null)
                {

                    var obj = new ApplicantFunFactModel();
                    obj.FormStatusw4 = model.FormStatusw4;
                    obj.FormStatusbcf = model.FormStatusbcf;
                    obj.FormStatusdd = model.FormStatusdd;
                    obj.FormStatusEvf = model.FormStatusEvf;
                    obj.FormStatusI9 = model.FormStatusI9;
                    obj.FormStatusprfcaf = model.FormStatusprfcaf;
                    obj.FormStatusprfecf = model.FormStatusprfecf;
                    obj.FormStatusrop = model.FormStatusrop;
                    obj.FormStatussif = model.FormStatussif;
                    obj.FormStatusprf = model.FormStatusprf;
                    obj.FormStatusff = model.FormStatusff;
                    obj.formName = "ApplicantFunFactForm";

                    return PartialView("PartialView/_ApplicantFunFact", obj);

                }

            }

            catch (Exception ex)
            {
                return Json(false, JsonRequestBehavior.AllowGet); //RedirectToAction("ApplicantFunFacts");
            }
            return RedirectToAction("ApplicantFunFacts");
        }

        /// <summary>
        /// Added by: Rajat Toppo
        /// Date:20-07-2020
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        [HttpPost]
        public PartialViewResult SelfIdentificationFormFromCheckList(FormNameStatus obj)
        {
            var getApplicantId = Convert.ToInt64(Session["ApplicantId"]);
            var _model = new SelfIdentificationModel();
            var getApplicantDetails = _IApplicantManager.GetApplicantAllDetails(getApplicantId);
            _model.FirstName = getApplicantDetails.FirstName;
            _model.MidleName = getApplicantDetails.MiddleName;
            _model.LastName = getApplicantDetails.LastName;
            _model.Gender = getApplicantDetails.EMP_Gender;
            _model.FormStatusw4 = obj.FormStatusw4;
            _model.FormStatusbcf = obj.FormStatusbcf;
            _model.FormStatusdd = obj.FormStatusdd;
            _model.FormStatusEvf = obj.FormStatusEvf;
            _model.FormStatusI9 = obj.FormStatusI9;
            _model.FormStatusprfcaf = obj.FormStatusprfcaf;
            _model.FormStatusprfecf = obj.FormStatusprfecf;
            _model.FormStatusrop = obj.FormStatusrop;
            _model.FormStatussif = obj.FormStatussif;
            _model.FormStatusprf = obj.FormStatusprf;
            _model.FormStatusff = obj.FormStatusff;
            _model.formName = "selfIdentificationForm";
            return PartialView("PartialView/_SelfIdentificationForm", _model);
        }
        [HttpGet]
        public ActionResult ApplicantFunFacts()
        {
            var getApplicantId = Convert.ToInt64(Session["ApplicantId"]);
            //var getApplicantContact = _IApplicantManager.GetApplicantByApplicantId(getApplicantId);
            return PartialView("PartialView/_ApplicantFunFact");
        }
        /// <summary>
        /// Created By : Ashwajit Bansod
        /// Created Date : 21-Feb-2020
        /// Created For : to save fun fact questions and anwers
        /// </summary>
        /// <param name="Obj"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult ApplicantFunFact(ApplicantFunFactModel Obj)
        {
            try
            {
                
                W4FormModel model = new W4FormModel();
                eTracLoginModel ObjLoginModel = null;
                if (Session != null)
                {
                    if (Session["eTrac"] != null)
                    {
                        ObjLoginModel = (eTracLoginModel)(Session["eTrac"]);
                        Obj.Employee_Id = ObjLoginModel.UserName;
                        Obj.Applicant_Id = Convert.ToInt64(Session["ApplicantId"]);
                    }
                }
                //Obj.FormStatusff == "true" && Obj.FormStatusbcf == "true" && Obj.FormStatusdd == "true" && Obj.FormStatusEvf == "true" && Obj.FormStatusI9 == "true" && Obj.FormStatusprf == "true" && Obj.FormStatusprfecf == "true" && Obj.FormStatusrop == "true"  && Obj.FormStatusw4 == "true"
                //var isSaved = _IApplicantManager.SaveApplicantFunFacts(Obj);
                if (Obj != null)
                {
                    var objloginmodel = (eTracLoginModel)(Session["etrac"]);
                    model = _IGuestUserRepository.GetW4Form(objloginmodel.UserId);
                    var _model = new SignatureFormModel();
                    Session["IsSignature"] = true;//To filup form no need to display signature button so we make it hide
                    //model.IsSignature = true;
                    var Isexempt = Convert.ToString(Session["IsExempt"]);
                    var EEOstatus = Convert.ToString(Session["EEOstatus"]);
                    if (Isexempt == "Y")
                    {
                        if (EEOstatus == "false")
                        {
                            if (Obj.FormStatusff == "true" && Obj.FormStatusbcf == "true" && Obj.FormStatusdd == "true" && Obj.FormStatusEvf == "true" && Obj.FormStatusI9 == "true" && Obj.FormStatusprf == "true" && Obj.FormStatusprfecf == "true" && Obj.FormStatusrop == "true" && Obj.FormStatusw4 == "true" && Obj.FormStatusprfcaf == "true" && Obj.FormStatussif == "true")
                            {
                                
                                return RedirectToAction ("ThankYou");
                            }
                            else
                            {
                                return Json(false, JsonRequestBehavior.AllowGet);
                            }
                        }
                        else
                        {
                            if (Obj.FormStatusff == "true" && Obj.FormStatusbcf == "true" && Obj.FormStatusdd == "true" && Obj.FormStatusEvf == "true" && Obj.FormStatusI9 == "true" && Obj.FormStatusprf == "true" && Obj.FormStatusprfecf == "true" && Obj.FormStatusrop == "true" && Obj.FormStatusw4 == "true" && Obj.FormStatusprfcaf == "true")
                            {
                                return RedirectToAction("ThankYou");
                            }
                            else
                            {
                                return Json(false, JsonRequestBehavior.AllowGet);
                            }
                        }
                    }
                    else
                    {
                        if (EEOstatus == "false")
                        {
                            if (Obj.FormStatusff == "true" && Obj.FormStatusbcf == "true" && Obj.FormStatusdd == "true" && Obj.FormStatusEvf == "true" && Obj.FormStatusI9 == "true" && Obj.FormStatusprf == "true" && Obj.FormStatusprfecf == "true" && Obj.FormStatusrop == "true" && Obj.FormStatusw4 == "true" &&  Obj.FormStatussif == "true")
                            {
                                return RedirectToAction("ThankYou");
                            }
                            else
                            {
                                return Json(false, JsonRequestBehavior.AllowGet);
                            }
                        }
                        else
                        {
                            if (Obj.FormStatusff == "true" && Obj.FormStatusbcf == "true" && Obj.FormStatusdd == "true" && Obj.FormStatusEvf == "true" && Obj.FormStatusI9 == "true" && Obj.FormStatusprf == "true" && Obj.FormStatusprfecf == "true" && Obj.FormStatusrop == "true" && Obj.FormStatusw4 == "true")
                            {
                                return RedirectToAction("ThankYou");
                            }
                            else
                            {
                                return Json(false, JsonRequestBehavior.AllowGet);
                            }
                        }
                    }

                }

                return Json(false, JsonRequestBehavior.AllowGet);

            }
            catch (Exception ex)
            {
                return Json(false, JsonRequestBehavior.AllowGet); //RedirectToAction("ApplicantFunFacts");
            }
            //return RedirectToAction("ApplicantFunFacts");
        }

        /// <summary>
        /// Added by Rajat Toppo
        /// Date:20-07-2020
        /// </summary>
        /// <param name="Obj"></param>
        /// <returns></returns>
        [HttpPost]
        public PartialViewResult ApplicantFunFactCheckList(FormNameStatus obj)
        {
            var getApplicantId = Convert.ToInt64(Session["ApplicantId"]);
            var _model = new ApplicantFunFactModel();
            _model.FormStatusw4 = obj.FormStatusw4;
            _model.FormStatusbcf = obj.FormStatusbcf;
            _model.FormStatusdd = obj.FormStatusdd;
            _model.FormStatusEvf = obj.FormStatusEvf;
            _model.FormStatusI9 = obj.FormStatusI9;
            _model.FormStatusprfcaf = obj.FormStatusprfcaf;
            _model.FormStatusprfecf = obj.FormStatusprfecf;
            _model.FormStatusrop = obj.FormStatusrop;
            _model.FormStatussif = obj.FormStatussif;
            _model.FormStatusprf = obj.FormStatusprf;
            _model.FormStatusff = obj.FormStatusff;
            _model.formName = "selfIdentificationForm";
            return PartialView("PartialView/_ApplicantFunFact", _model);

        }
        /// <summary>
        /// Created By : Ashwajit Bansod
        /// Created Date : 26-Feb-2020
        /// Created For : To get rate of pay info of applicant by userid
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public PartialViewResult _RateOfPay()
        {
            var model = new RateOfPayModel();
            var objloginmodel = (eTracLoginModel)(Session["etrac"]);
            var Applicant_Id = Convert.ToInt64(Session["ApplicantId"]);
            var employeeId = objloginmodel.UserName;
            model = _IApplicantManager.GetRateOfPayInfo(Applicant_Id, employeeId);
            if (model != null)
                return PartialView("_RateOfPay", model);
            else
                return PartialView("_RateOfPay", new RateOfPayModel());
        }
        [HttpPost]
        public ActionResult _RateOfPay(RateOfPayModel model)
        {

            var IsExempt = Session["IsExempt"].ToString();
            var objloginmodel = (eTracLoginModel)(Session["etrac"]);
            var APT_Id = Convert.ToInt64(Session["ApplicantId"]);
            if (IsExempt == "Y")
            {
                var getDetails = _IApplicantManager.GetApplicantAllDetails(APT_Id);
                var _model = new ConfidenialityAgreementModel();
                _model.EmpAddress = getDetails.Address;
                _model.EmployeeName = getDetails.FirstName + " " + getDetails.LastName;
                _model.Between = getDetails.FirstName + " " + getDetails.LastName;
                _model.Title = getDetails.Title;

                _model.Date = DateTime.Now.ToString("dd-MM-yyyy");
                _model.FormStatusw4 = model.FormStatusw4;
                _model.FormStatusbcf = model.FormStatusbcf;
                _model.FormStatusdd = model.FormStatusdd;
                _model.FormStatusEvf = model.FormStatusEvf;
                _model.FormStatusI9 = model.FormStatusI9;
                _model.FormStatusprfcaf = model.FormStatusprfcaf;
                _model.FormStatusprfecf = model.FormStatusprfecf;
                _model.FormStatusrop = model.FormStatusrop;
                _model.FormStatussif = model.FormStatussif;
                _model.FormStatusprf = model.FormStatusprf;
                _model.FormStatusff = model.FormStatusff;
                _model.formName = "confidentialityagreementform";

                return PartialView("_ConfidentialityAgreementForm", _model);
            }
            else
            {
                PhotoRelease _model = new PhotoRelease();

                var d = _IGuestUserRepository.GetPhotoRelease(objloginmodel.UserId);
                if (d == null)
                {
                    //var apt_Id = Convert.ToInt64(Session["ApplicantId"]);
                    var getApplicantDetails = _IApplicantManager.GetApplicantAllDetails(APT_Id);
                    _model.Name = getApplicantDetails.FirstName + " " + getApplicantDetails.LastName;
                    _model.EmpId = objloginmodel.UserName;
                    _model.FirstName = getApplicantDetails.FirstName;
                    _model.LastName = getApplicantDetails.LastName;
                }
                _model.FormStatusw4 = model.FormStatusw4;
                _model.FormStatusbcf = model.FormStatusbcf;
                _model.FormStatusdd = model.FormStatusdd;
                _model.FormStatusEvf = model.FormStatusEvf;
                _model.FormStatusI9 = model.FormStatusI9;
                _model.FormStatusprfcaf = model.FormStatusprfcaf;
                _model.FormStatusprfecf = model.FormStatusprfecf;
                _model.FormStatusrop = model.FormStatusrop;
                _model.FormStatussif = model.FormStatussif;
                _model.FormStatusprf = model.FormStatusprf;
                _model.FormStatusff = model.FormStatusff;
                _model.formName = "photoreleaseform";

                return PartialView("_PhotoReleaseForm", _model);
            }




        }

        /// <summary>
        /// Added by: Rajat Toppo
        /// Date:20-17-2020
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        public PartialViewResult RateOfPayfromCheckList(FormNameStatus obj)
        {
            var model = new RateOfPayModel();
            var _model = new RateOfPayModel();
            var objloginmodel = (eTracLoginModel)(Session["etrac"]);
            var Signrop = Convert.ToString(Session["Signaturerop"]);
            var newSignrop = Signrop.Replace("-", "/");
            var Applicant_Id = Convert.ToInt64(Session["ApplicantId"]);
            var employeeId = objloginmodel.UserName;
            model = _IApplicantManager.GetRateOfPayInfo(Applicant_Id, employeeId);
            if (model != null)
            {
                model.FormStatusw4 = obj.FormStatusw4;
                model.FormStatusbcf = obj.FormStatusbcf;
                model.FormStatusdd = obj.FormStatusdd;
                model.FormStatusEvf = obj.FormStatusEvf;
                model.FormStatusI9 = obj.FormStatusI9;
                model.FormStatusprfcaf = obj.FormStatusprfcaf;
                model.FormStatusprfecf = obj.FormStatusprfecf;
                model.FormStatusrop = obj.FormStatusrop;
                model.FormStatussif = obj.FormStatussif;
                model.FormStatusprf = obj.FormStatusprf;
                model.FormStatusff = obj.FormStatusff;
                model.formName = "RateOfPay";
                model.SignatureBase = newSignrop;
                return PartialView("_RateOfPay", model);
            }
            else
            {
                _model.FormStatusw4 = obj.FormStatusw4;
                _model.FormStatusbcf = obj.FormStatusbcf;
                _model.FormStatusdd = obj.FormStatusdd;
                _model.FormStatusEvf = obj.FormStatusEvf;
                _model.FormStatusI9 = obj.FormStatusI9;
                _model.FormStatusprfcaf = obj.FormStatusprfcaf;
                _model.FormStatusprfecf = obj.FormStatusprfecf;
                _model.FormStatusrop = obj.FormStatusrop;
                _model.FormStatussif = obj.FormStatussif;
                _model.FormStatusprf = obj.FormStatusprf;
                _model.FormStatusff = obj.FormStatusff;
                _model.formName = "RateOfPay";
                model.SignatureBase = newSignrop;
                return PartialView("_RateOfPay", _model);
            }
        }
        /// <summary>
        /// Created By : Ashwajit bansod
        /// Created Date : 17-03-2020
        /// Created For : To get applicant all details
        /// </summary>
        /// <param name="ApplicantId"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult GetApplicantDetails(long ApplicantId)
        {
            var ObjLoginModel = (eTracLoginModel)(Session["eTrac"]);
            ViewBag.StateList = _ICommonMethod.GetStateByCountryId(1);
            //var employee = _IGuestUserRepository.GetEmployee(ObjLoginModel.UserId);
            var commonModel = _IGuestUserRepository.GetApplicantAllDetailsToView(ApplicantId);
            commonModel.ApplicantId = ApplicantId;
            Session["ApplicantId"] = ApplicantId;
            return PartialView("~/Views/NewAdmin/ePeople/OnBoarding/_ViewApplicantDetails.cshtml", commonModel);
        }

        [HttpPost]
        public PartialViewResult FormsCheckList(FormNameStatus obj)
        {
            
            obj.IsExempt = Session["IsExempt"].ToString();

            try
            {
                if (obj.EEOstatus == "false" && obj.FormStatussif == null)
                {
                    Session["EEOstatus"] = "false";
                }
                if (obj != null)
                {

                    return PartialView("~/Views/Guest/_checkList.cshtml", obj);
                }
            }
            catch (Exception ex)
            {

            }
            return PartialView("~/Views/Guest/_checkList.cshtml");
        }

        /// <summary>
        /// Created By : Ashwajit Bansod
        /// Created Date : 17-07-2020
        /// Created For : To save signature and remove it from folder
        /// </summary>
        /// <param name="baseImage"></param>
        /// <param name="applicantId"></param>
        /// <param name="FormName"></param>
        /// <returns></returns>

        //[HttpGet]
        //public ActionResult OnboardingFormPdf()
        //{
        //    CommonFormPdfModel model = new CommonFormPdfModel();
        //    return View("~/Views/Guest/OnboardingFormsPdf.cshtml", model);
        //}
        [HttpGet]
        public ActionResult OnboardingFormPdf()
        {

            CommonFormPdfModel model = new CommonFormPdfModel();
            FormNameStatus obj = new FormNameStatus();
            var Applicant_Id = Convert.ToInt64(Session["ApplicantId"]);
            var ObjLoginModel = (eTracLoginModel)(Session["eTrac"]);
            var employee = _IGuestUserRepository.GetEmployeeDetails(ObjLoginModel.UserId);

            //********************PhotoRelease*****************************
            var getApplicantDetails = _IApplicantManager.GetApplicantAllDetails(Applicant_Id);
            model.PhotoRelease.Name = getApplicantDetails.FirstName + " " + getApplicantDetails.LastName;
            model.PhotoRelease.EmpId = ObjLoginModel.UserName;
            model.PhotoRelease.FirstName = getApplicantDetails.FirstName;
            model.PhotoRelease.LastName = getApplicantDetails.LastName;

            //*******************ConfidenialityAgreement********************
            var getDetails = _IApplicantManager.GetApplicantAllDetails(Applicant_Id);
            model.ConfidenialityAgreementModel.EmpAddress = getDetails.Address;
            model.ConfidenialityAgreementModel.EmployeeName = getDetails.FirstName + " " + getDetails.LastName;
            model.ConfidenialityAgreementModel.Between = getDetails.FirstName + " " + getDetails.LastName;
            model.ConfidenialityAgreementModel.Title = getDetails.Title;
            model.ConfidenialityAgreementModel.Date = DateTime.Now.ToString("dd-MM-yyyy");


            //*********************SelfIdentification************************
            model.SelfIdentificationModel = _IApplicantManager.GetSelfIdentification(employee.EmpId);
            model.SelfIdentificationModel.FirstName = getApplicantDetails.FirstName;
            model.SelfIdentificationModel.MidleName = getApplicantDetails.MiddleName;
            model.SelfIdentificationModel.LastName = getApplicantDetails.LastName;
            model.SelfIdentificationModel.Gender = getApplicantDetails.EMP_Gender;

            model.BackgroundCheckForm = _IApplicantManager.GetApplicantByApplicantId(Applicant_Id);

            model.RateOfPayModel = _IApplicantManager.GetRateOfPayInfo(Applicant_Id, employee.EmpId);
            model.EmergencyContectForm = _IGuestUserRepository.GetEmergencyForm(ObjLoginModel.UserId);
            model.W4FormModel = _IGuestUserRepository.GetW4Form(ObjLoginModel.UserId);
            model.I9FormModel = _IApplicantManager.GetI9FormData(Applicant_Id, ObjLoginModel.UserId, obj);
            model.DirectDepositeFormModel = _IGuestUserRepository.GetDirectDepositeDataByUserId(ObjLoginModel.UserId);
            model.EducationVarificationModel = _IGuestUserRepository.GetEducationVerificationForm(ObjLoginModel.UserId, Applicant_Id);
            model.Isexempt = Session["IsExempt"].ToString();
            //var applicantId = Convert.ToInt64(Session["ApplicantId"]);
            var viewName = "OnboardingFiles";
            var employeeId = ObjLoginModel.UserName;
            var path = Applicant_Id + getDetails.FirstName + viewName + ".pdf";
            var _FillableFormRepository = new FillableFormRepository();
            var getDetailpdfs = _FillableFormRepository.GetFileList().Where(x => x.FLT_FileType == "Yellow" && x.FLT_Id == Convert.ToInt64(FileTypeId.W4)).FirstOrDefault();
            var getpdf = HtmlConvertToPdf(viewName, model, path, getDetailpdfs.FLT_Id, employeeId, "Onboarding Files");


            return new EmptyResult();
        }
    

                //Created By  :Ashwajit, To send notification to HR
        //[HttpPost]
        //public JsonResult OnboardigFormIssueNotification(string FormName,long ApplicantId)
        //{
        //    var ObjLoginModel = (eTracLoginModel)(Session["eTrac"]);
        //    var _manager = new NotificationManager();
        //    bool isSaved = false;
        //    try
        //    {
        //        if(FormName != null && ApplicantId > 0)
        //        {
        //            var commonModel = _IApplicantManager.GetApplicantAllDetails(ApplicantId);
        //            var getdatailsHR = _IDepartment.GetDepartmentEmployeeList(WorkOrderEMS.Helper.Departments.HR);//changed by rajat
        //            if(getdatailsHR.Count() == 0)
        //            {
        //                foreach (var item in getdatailsHR)
        //                {
        //                    NotificationDetailModel obj = new NotificationDetailModel();
        //                    obj.Message = DarMessage.OnbordingFormHRNotification(commonModel.FirstName + " " + commonModel.LastName, FormName);
        //                    obj.Module = ModuleSubModule.ePeople;
        //                    obj.SubModule = ModuleSubModule.HRHelpOnboardingForm;
        //                    obj.SubModuleId1 = ApplicantId.ToString();
        //                    obj.CreatedByUser = commonModel.EmpId;
        //                    obj.AssignToUser = item.EmployeeId;
        //                    obj.AssignToIsWorkable = true;
        //                    obj.CreatedByIsWorkable = false;
        //                    obj.Priority = Priority.High;
        //                    isSaved = _manager.SaveNotification(obj);
        //                }
        //                return Json("Notification Send to HR",JsonRequestBehavior.AllowGet);
        //            }
        //        }
        //    }
        //    catch(Exception ex)
        //    {
        //        ViewBag.Message = ex.Message;
        //        ViewBag.Class = "danger";
        //        return Json(ex.Message, JsonRequestBehavior.AllowGet);
        //    }
        //    return Json("Notification Send to HR", JsonRequestBehavior.AllowGet);
        //}

        //Created By  :Ashwajit, To send notification to HR
        [HttpPost]
        public JsonResult OnboardigFormIssueNotification(string FormName, long ApplicantId)
        {
            var ObjLoginModel = (eTracLoginModel)(Session["eTrac"]);
            var _manager = new NotificationManager();
            bool isSaved = false;
            try
            {
                if (FormName != null && ApplicantId > 0)
                {
                    var commonModel = _IApplicantManager.GetApplicantAllDetails(ApplicantId);
                    var getdatailsHR = _IDepartment.GetDepartmentEmployeeList(Helper.Department.HR);//changed by rajat
                    if (getdatailsHR.Count() > 0)
                    {
                        foreach (var item in getdatailsHR)
                        {
                            NotificationDetailModel obj = new NotificationDetailModel();
                            obj.Message = DarMessage.OnbordingFormHRNotification(commonModel.FirstName + " " + commonModel.LastName, FormName);
                            obj.Module = ModuleSubModule.ePeople;
                            obj.SubModule = ModuleSubModule.HRHelpOnboardingForm;
                            obj.SubModuleId1 = ApplicantId.ToString();
                            obj.CreatedByUser = ObjLoginModel.UserName;
                            obj.AssignToUser = item.EmployeeId;
                            obj.AssignToIsWorkable = true;
                            obj.CreatedByIsWorkable = false;
                            obj.Priority = Priority.High;
                            isSaved = _manager.SaveNotification(obj);
                        }
                        return Json("Notification Send to HR", JsonRequestBehavior.AllowGet);
                    }
                }
            }
            catch (Exception ex)
            {
                ViewBag.Message = ex.Message;
                ViewBag.Class = "danger";
                return Json(ex.Message, JsonRequestBehavior.AllowGet);
            }
            return Json("Notification Send to HR", JsonRequestBehavior.AllowGet);
        }

        public string Signature(string baseImage, long applicantId, string FormName)
        {
            string url = string.Empty;
            string ImageLocation = string.Empty;
            string ImageUniqueName = string.Empty;
            string ImagePath = string.Empty; string ImageURL = string.Empty;
            string returnPath = string.Empty;
            try
            {
                if (FormName == OnboardingForms.BackgroudCheck)
                {
                    ImagePath = Server.MapPath(ConfigurationManager.AppSettings["ApplicantSignature"]);
                    //ImagePath = HostingPrefix + ApplicantSignature.Replace("~", "");
                    returnPath = ApplicantSignature.Replace("~", "");
                    //ConfigurationManager.AppSettings["ApplicantSignature"].ToString();
                }
                else
                {
                    returnPath = PDFUrl.Replace("~", "");
                    //ImagePath = HostingPrefix + pdfCurrentPath;
                    ImagePath = Server.MapPath(ConfigurationManager.AppSettings["PDFSignature"]);
                    string[] filePaths = Directory.GetFiles(ImagePath);
                    foreach (string filePath in filePaths)
                    {
                        if (filePath != null)
                            System.IO.File.Delete(filePath);
                    }
                }
                if (baseImage != null)
                {
                    ImagePath = Server.MapPath(ConfigurationManager.AppSettings["PDFSignature"].ToString());
                    ImageUniqueName = DateTime.Now.ToString("yyyyMMddHHmm") + FormName;
                    url = HostingPrefix + PDFUrl.Replace("~", "") + ImageUniqueName + ".jpg";
                    ImageURL = ImageUniqueName + ".jpg";
                    if (!Directory.Exists(ImagePath))
                    {
                        Directory.CreateDirectory(ImagePath);
                    }
                     ImageLocation = ImagePath + ImageURL;
                    string data = baseImage.Replace("-", "/");
                    string convertedString = data.Replace("data:image/png;base64,", "");
                    //Save the image to directory
                    using (MemoryStream ms = new MemoryStream(Convert.FromBase64String(convertedString)))
                    {
                        using (Bitmap bm2 = new Bitmap(ms))
                        {
                            bm2.Save(ImageLocation);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                ViewBag.Message = ex.Message;
            }
            return url;
        }

        [HttpGet]
        public ActionResult AssessmentEvaulation()
        {
            var AssessmentEvaluationModel = new AssessmentEvaluationModel();
           return View("AssessmentEvaluationPDF", AssessmentEvaluationModel);
        }


        /// <summary>
        /// Created By : Deepak Panda
        /// Created Date : 26-Feb-2020
        /// Created For : To Convert Html view to Pdf
        /// </summary>
        public async Task<bool> HtmlConvertToPdf(string viewName, object model, string path, long FileId, string EmployeeId, string FileName)
        {
            bool status = false;
            try
            {
                var pdf = new Rotativa.ViewAsPdf(viewName, model)
               // var pdf = new Rotativa.ActionAsPdf(viewName, model)
                {
                    FileName = path,
                    CustomSwitches = "--page-offset 0 --footer-center [page] --footer-font-size 8"
                };
                byte[] pdfData = pdf.BuildFile(ControllerContext);
                var root = Server.MapPath("~/Content/FilesRGY/");
                var fullPath = Path.Combine(root, pdf.FileName);
                fullPath = Path.GetFullPath(fullPath);
                using (var fileStream = new FileStream(fullPath, FileMode.Create, FileAccess.Write))
                {
                    fileStream.Write(pdfData, 0, pdfData.Length);
                }
                if (path != null)
                {
                    var Obj = new UploadedFiles();
                    Obj.FileName = FileName;
                    Obj.FileId = FileId;
                    Obj.FileEmployeeId = EmployeeId;
                    string LoginEmployeeId = EmployeeId;
                    Obj.AttachedFileName = path;
                    var IsSaved = _IFillableFormManager.SaveFile(Obj, LoginEmployeeId);
                }
                return status = true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }
    }
}
