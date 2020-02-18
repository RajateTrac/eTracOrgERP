﻿using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WorkOrderEMS.BusinessLogic;
using WorkOrderEMS.BusinessLogic.Interfaces;
using WorkOrderEMS.Data.EntityModel;
using WorkOrderEMS.Data.Interfaces;
using WorkOrderEMS.Helper;
using WorkOrderEMS.Models;
using WorkOrderEMS.Models.Employee;
using WorkOrderEMS.Models.NewAdminModel;

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
        public GuestController(ICommonMethod _ICommonMethod, IGlobalAdmin _IGlobalAdmin, ICompanyAdmin _ICompanyAdmin, IGuestUser _GuestUserRepository, IApplicantManager _IApplicantManager, IFillableFormManager _IFillableFormManager)
        {
            this._IGlobalAdmin = _IGlobalAdmin;
            this._ICommonMethod = _ICommonMethod;
            this._ICompanyAdmin = _ICompanyAdmin;
            this._IGuestUserRepository = _GuestUserRepository;
            this._IApplicantManager = _IApplicantManager;
            this._IFillableFormManager = _IFillableFormManager;
        }
        //
        // GET: /Guest/
        public ActionResult Index()
        {
            var ObjLoginModel = (eTracLoginModel)(Session["eTrac"]);
            EmployeeVIewModel model = new EmployeeVIewModel();
            ViewBag.StateList = _ICommonMethod.GetStateByCountryId(1);
            model = _IGuestUserRepository.GetEmployee(ObjLoginModel.UserId);
            //Only for test
            Session["ApplicantId"] = 2;model.ApplicantId = 2;
            return View("~/Views/Guest/Index1.cshtml", model);
        }
        [HttpPost]
        public ActionResult Index(EmployeeVIewModel model)
        {
            ViewBag.StateList = _ICommonMethod.GetStateByCountryId(1);
            if (ModelState.IsValid)
            {
                var isSaveSuccess = _IGuestUserRepository.UpdateApplicantInfo(model);
                if (isSaveSuccess)
                    return RedirectToAction("PersonalFile");
                else
                {
                    ViewBag.message = "Something went wrong!!!";
                    return View(model);
                }
            }
            return View(model);
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
            return View();
        }
        [HttpGet]
        public PartialViewResult _DirectDepositeForm()
        {
            DirectDepositeFormModel model = new DirectDepositeFormModel();
            var ObjLoginModel = (eTracLoginModel)(Session["eTrac"]);
            model = _IGuestUserRepository.GetDirectDepositeDataByUserId(ObjLoginModel.UserId);
            return PartialView("_directDepositeForm", model);
        }
        [HttpPost]
        public ActionResult _DirectDepositeForm(DirectDepositeFormModel model)
        {
            if (model != null)
            {
                var ObjLoginModel = (eTracLoginModel)(Session["eTrac"]);
                _IGuestUserRepository.SetDirectDepositeFormData(model, ObjLoginModel.UserId);
                //if(ObjLoginModel != null)
                //{
                //    var details = 
                //}
                return Json(true, JsonRequestBehavior.AllowGet);
                //return PartialView("PartialView/_CommonModals", new ContactListModel());
            }
            ViewBag.NotSaved = true;
            return Json(true, JsonRequestBehavior.AllowGet);
            //return PartialView("PartialView/_CommonModals", new ContactListModel());

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
        public ActionResult _EmployeeHandbook(EmployeeHandbookModel model)
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
            if(applicantId > 0)
            {
                 getI9Info = _IApplicantManager.GetI9FormData(applicantId, ObjLoginModel.UserId);

                return PartialView("_I9Form", getI9Info); ;
            }           
            return PartialView("_I9Form", getI9Info);
        }
        /// <summary>
        /// Created By : Ashwajit Bansod
        /// Created Date : 17-Feb-2020
        /// Created For : To save I9 form Details of applicant
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult _I9Form(I9FormModel model)
        {
            var _model = new EmergencyContectForm();
            var applicantId = Convert.ToInt64(Session["ApplicantId"]);
            if (ModelState.IsValid)
            {
                var objloginmodel = (eTracLoginModel)(Session["etrac"]);
                var saved = _IApplicantManager.SetI9Form(objloginmodel.UserId, applicantId, model);
                if (saved)
                    return RedirectToAction("_emergencyContactForm");
                else
                    return RedirectToAction("_I9Form");
                //return Json(true, JsonRequestBehavior.AllowGet);
            }
            return RedirectToAction("_emergencyContactForm");
            //return PartialView("_emergencyContactForm", _model);
            //return PartialView("_I9Form");
        }
        [HttpGet]
        public PartialViewResult _W4Form()
        {
            W4FormModel model = new W4FormModel();
            var objloginmodel = (eTracLoginModel)(Session["etrac"]);
            model = _IGuestUserRepository.GetW4Form(objloginmodel.UserId);

            return PartialView("_W4Form", model);
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

        [HttpPost]
        public ActionResult _W4Form(W4FormModel model)
        {
            if (model != null)
            {
                var objloginmodel = (eTracLoginModel)(Session["etrac"]);
                _IGuestUserRepository.SetW4Form(objloginmodel.UserId, model);
                return RedirectToAction("_I9Form");
                //return Json(true, JsonRequestBehavior.AllowGet);
            }
            return RedirectToAction("_I9Form");
            //return PartialView("_I9Form", model);
        }
        [HttpGet]
        public PartialViewResult _PhotoReleaseForm()
        {
            PhotoRelease model = new PhotoRelease();
            var objloginmodel = (eTracLoginModel)(Session["etrac"]);
            var d = _IGuestUserRepository.GetPhotoRelease(objloginmodel.UserId);
            model.Name = d;
            return PartialView("_PhotoReleaseForm", model);
        }
        [HttpPost]
        public ActionResult _photoreleaseform(PhotoRelease model)
        {
            if (ModelState.IsValid)
            {
                var objloginmodel = (eTracLoginModel)(Session["etrac"]);


                _IGuestUserRepository.SetPhotoRelease(objloginmodel.UserId, model);
                return Json(true, JsonRequestBehavior.AllowGet);
            }
            ViewBag.notsaved = true;
            return PartialView("_photoreleaseform", model);
        }

		[HttpGet]
		public PartialViewResult _EducationVarificationForm()
		{
			EducationVarificationModel model = new EducationVarificationModel();
			var objloginmodel = (eTracLoginModel)(Session["etrac"]);
			model = _IGuestUserRepository.GetEducationVerificationForm(objloginmodel.UserId);
			return PartialView("_EducationVarificationForm",model);
		}
		[HttpPost]
		public ActionResult _EducationVarificationForm(EducationVarificationModel model)
		{
			if (ModelState.IsValid)
			{
				var objloginmodel = (eTracLoginModel)(Session["etrac"]);
				_IGuestUserRepository.SetEducationVerificationForm(objloginmodel.UserId, model);
				return Json(true, JsonRequestBehavior.AllowGet);
			}
			model.IsSave = false;
			return PartialView("_EducationVarificationForm",model);
		}
		[HttpGet]
		public PartialViewResult _ConfidentialityAgreementForm()
		{
			return PartialView("_ConfidentialityAgreementForm");
		}
		[HttpPost]
		public ActionResult _ConfidentialityAgreementForm(ConfidenialityAgreementModel model)
		{
			if (ModelState.IsValid)
			{
				var objloginmodel = (eTracLoginModel)(Session["etrac"]);
				_IGuestUserRepository.SetConfidenialityAgreementForm(objloginmodel.UserId, model);
				return Json(true, JsonRequestBehavior.AllowGet);
			}
			return PartialView("_ConfidentialityAgreementForm", model);
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
			model = _IGuestUserRepository.GetEmergencyForm(objloginmodel.UserId);
			return PartialView("_emergencyContactForm", model);
		}
		//[HttpPost]
		//public ActionResult _emergencyContactForm(EmergencyContectForm model)
		//{
		//	if (ModelState.IsValid)
		//	{
		//		var objloginmodel = (eTracLoginModel)(Session["etrac"]);
		//		_IGuestUserRepository.SetEmergencyForm(objloginmodel.UserId, model);
		//		return Json(true, JsonRequestBehavior.AllowGet);
		//	}
		//	return PartialView("_emergencyContactForm", model);
		//}
        [HttpPost]
        public ActionResult _emergencyContactForm(EmergencyContectForm model)
        {
            var _model = new DirectDepositeFormModel();
            if (model != null)
            {
                var objloginmodel = (eTracLoginModel)(Session["etrac"]);
                _IGuestUserRepository.SetEmergencyForm(objloginmodel.UserId, model);
                //return Json(true, JsonRequestBehavior.AllowGet);
                return RedirectToAction("_directDepositeForm");
            }
            return RedirectToAction("_directDepositeForm");
            //return PartialView("_directDepositeForm", _model);
        }
        [HttpGet]
		public ActionResult GetFormsStatus()
		{			
				var objloginmodel = (eTracLoginModel)(Session["etrac"]);
				var data=_IGuestUserRepository.GetFormsStatus(objloginmodel.UserId);
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
        public ActionResult _ContactSavedForm(ContactListModel model,List<ContactModel> lstModel)
        {
            try
            {
                var objloginmodel = (eTracLoginModel)(Session["etrac"]);
                if(lstModel.Count() > 0)
                {
                   var updateContact =  _IApplicantManager.UpdateContactDetailsApplicant(model, lstModel);
                    if (updateContact)
                    {
                        return RedirectToAction("_BackGroundCheckForm");
                    }
                    else
                    {
                        return Json(false, JsonRequestBehavior.AllowGet);
                    }
                }
            }
            catch(Exception ex)
            {
                return Json(false, JsonRequestBehavior.AllowGet);
            }
            return  RedirectToAction("_BackGroundCheckForm");
        }
        /// <summary>
        /// Created By : Ashwajit Bansod
        /// Created Date : 12-Feb-2020
        /// Created For : To Get Applicant details by applicant Id
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public PartialViewResult _BackGroundCheckForm()
        {
            var getApplicantId = Convert.ToInt64(Session["ApplicantId"]);
            var getApplicantContact = _IApplicantManager.GetApplicantByApplicantId(getApplicantId);
            return PartialView("PartialView/_BackGroundCheckForm", getApplicantContact);
        }

        /// <summary>
        /// Created By : Ashwajit Bansod
        /// Created Date : 12-Feb-2020
        /// Created For : To send Applicant Details For Backgroud check and Create for same
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        public PartialViewResult _BackGroundCheckForm(EmployeeVIewModel model)
        {
            var _model = new ContactListModel();
            try
            {
                var getApplicantId = Convert.ToInt64(Session["ApplicantId"]);
                var sendForBackgroundCheck = _IApplicantManager.SendApplicantInfoForBackgrounddCheck(model);
                var getApplicantContact = _IApplicantManager.GetApplicantByApplicantId(getApplicantId);
                if (sendForBackgroundCheck == true)
                {
                    return PartialView("PartialView/_UploadDocuments", _model);
                    //return PartialView("PartialView/_UploadDocuments");
                }
            }
            catch (Exception ex)
            {

            }
            return PartialView("PartialView/_CommonModals");
        }
        /// <summary>
        /// Created By  :Ashwajit Bansod
        /// Created Date : 17-Feb-2020
        /// Created For : To upload applicant file .
        /// </summary>
        /// <param name="filesLicense"></param>
        /// <param name="filesSSN"></param>
        /// <returns></returns>
        public ActionResult UploadFilesApplicant()
        {
            var Obj = new UploadedFiles() ;
            var _db = new workorderEMSEntities();
            eTracLoginModel ObjLoginModel = null;
            HttpFileCollectionBase files = Request.Files;
            if (Session["eTrac"] != null)
            {
                ObjLoginModel = (eTracLoginModel)(Session["eTrac"]);
            }
            //if(isLicense == true)
            //{

            //}
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
                                CommonHelper.StaticUploadImage(file, Server.MapPath(ConfigurationManager.AppSettings["ApplicantFiles"]), FName);
                                Obj.AttachedFileName = FName;
                                Obj.FileName = fname;
                                Obj.FileEmployeeId = getUser.EmployeeID;
                                Obj.FileId = Convert.ToInt64(Helper.FileType.FileType);
                                var IsSaved = _IFillableFormManager.SaveFile(Obj, Obj.FileEmployeeId);
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
    }
}