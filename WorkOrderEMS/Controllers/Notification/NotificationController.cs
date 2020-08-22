using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WorkOrderEMS.BusinessLogic;
using WorkOrderEMS.BusinessLogic.Managers;
using WorkOrderEMS.Helper;
using WorkOrderEMS.Models;

namespace WorkOrderEMS.Controllers
{
    public class NotificationController : Controller
    {
        // GET: Notification
        private readonly INotification _INotification;
        public NotificationController(INotification _INotification)
        {
            this._INotification = _INotification;
        }
        public ActionResult Index()
        {
            return View();
        }
        /// <summary>
        /// Created By  :Ashwajit Bansod
        /// Created Date : 02-04-2020
        /// Created For :  To get Unseen list of user
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult GetUnseenList()
        {
            eTracLoginModel ObjLoginModel = null;
            if (Session["eTrac"] != null)
            {
                ObjLoginModel = (eTracLoginModel)(Session["eTrac"]);
            }
            var getList = _INotification.GetNotification(ObjLoginModel.UserId, ObjLoginModel.UserName);
            return View("_NotificationList", getList);
        }
        /// <summary>
        /// Created By : Ashwajit Bansod
        /// Created Date : 03-04-2020
        /// Created For : To make notification readable
        /// </summary>
        /// <param name="NotificationId"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult ReadNotification(long NotificationId)
        {
            try
            {
                if (NotificationId > 0)
                {
                    var isRead = _INotification.ReadNotificationById(NotificationId);
                    return RedirectToAction("GetUnseenList");
                }
                else
                    return RedirectToAction("GetUnseenList");
            }
            catch (Exception ex)
            {
                return RedirectToAction("GetUnseenList");
            }
        }
        /// <summary>
        /// Created By : Ashwajit Bansod
        /// Created For : To get applicant details by Applicant id
        /// Created Date : 03-04-2020
        /// </summary>
        /// <param name="ApplicantId"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult GetViewForApplicantDetails(long ApplicantId, string ApplicantStatus)
        {
            var getData = new ApplicantDetails();
            try
            {
                if (ApplicantId > 0 && ApplicantStatus != null)
                {
                    getData = _INotification.GetApplicantDetails(ApplicantId);
                }
                else
                {
                    return View("_ApplicantDetailsWhenAcceptRejectCounterOffer", getData);
                }
            }
            catch (Exception ex)
            {

            }
            return View("_ApplicantDetailsWhenAcceptRejectCounterOffer", getData);
        }
        [HttpPost]
        public ActionResult ViewForMeeting(NotificationDetailModel obj)
        {
            try
            {
                if (obj != null)
                {
                    var getnotificationdetails = _INotification.NotificationDetailsforMeetingDateTime(obj);

                    return View("~/Views/CorrectiveAction/MeetingScheduler.cshtml", getnotificationdetails);
                }
                else
                {
                    return View("~/Views/CorrectiveAction/MeetingScheduler.cshtml");
                }
            }
            catch (Exception ex)
            {

            }
            return View("~/Views/CorrectiveAction/MeetingScheduler.cshtml");
        }

        /// <summary>
        /// Created by : Ashwajit Bansod
        /// Created For : To unlock employeee by manager
        /// Created Date : 28-07-2020
        /// </summary>
        /// <param name="EmployeeId"></param>
        /// <param name="ManagerId"></param>
        /// <returns></returns>
        public JsonResult UnlockEmployee(string Emp_Id)
        {
            eTracLoginModel ObjLoginModel = null;
            string message = "";
            if (Session["eTrac"] != null)
            {
                ObjLoginModel = (eTracLoginModel)(Session["eTrac"]);
            }
            try
            {
                if(Emp_Id != null)
                {
                    var data = _INotification.UnlockEmployee(Emp_Id, ObjLoginModel.UserName);
                    message = "Employee has been successfully unlock";
                }
                else
                    message = "Incorrect Employee Id";
            }
            catch(Exception ex)
            {
                Json(ex.Message, JsonRequestBehavior.AllowGet);
            }
            return Json(message, JsonRequestBehavior.AllowGet);
        }
        /// <summary>
        /// Created By : Ashwajit Bansod
        /// Created For : To send notification to employee.
        /// </summary>
        /// <param name="EmployeeId"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult SendMeetingNotificationToEmployee(string EmployeeId,string Assessment)
        {
            eTracLoginModel ObjLoginModel = null;
            string message = "";
            if (Session["eTrac"] != null)
            {
                ObjLoginModel = (eTracLoginModel)(Session["eTrac"]);
            }
            try
            {
                if (EmployeeId != null)
                {
                    EmployeeId = Cryptography.GetDecryptedData(EmployeeId, true);

                    var data = _INotification.ScheduleAppriasalMeetingNotification(EmployeeId, ObjLoginModel.UserName, Assessment);
                    message = "Notification Send to employee";
                }
                else
                    message = "Incorrect Employee Id";
            }
            catch (Exception ex)
            {
                Json(ex.Message, JsonRequestBehavior.AllowGet);
            }
            return Json(message, JsonRequestBehavior.AllowGet);
        }
        /// <summary>
        /// Created By : Ashwajit Bansod
        /// Created Date : 08-07-2020
        /// Created For : To set session
        /// </summary>
        /// <param name="SessionStatus"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult SetSession(string SessionStatus)
        {
            try
            {
                if (SessionStatus == "AppriasalMeeting")
                {
                    Session["AppriasalMeeting"] = true;
                }
                if(SessionStatus == "AcceptDisputeByHR")
                    Session["AcceptDisputeByHR"] = true;
            }
            catch (Exception ex)
            {
                Json(ex.Message, JsonRequestBehavior.AllowGet);
            }
            return Json(true, JsonRequestBehavior.AllowGet);
        }
        /// <summary>
        /// Created By : Ashwajit Bansod
        /// Created Date : 08-08-202
        /// Created For : TO set Notification
        /// </summary>
        /// <param name="EmployeeId"></param>
        /// <param name="SubModule"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult SetNotification(string EmployeeId, string SubModule)
        {
            try
            {
                var getEMPDetails = new GlobalAdminManager();
                eTracLoginModel ObjLoginModel = null;
                string message = "";
                if (Session["eTrac"] != null)
                {
                    ObjLoginModel = (eTracLoginModel)(Session["eTrac"]);
                }
                if (EmployeeId != null)
                {
                  
                    switch (SubModule)
                    {
                        case "HRDenyAppriasal" :
                            var dataEMP = getEMPDetails.GetEmployeeDetails(EmployeeId);
                            string Message = DarMessage.HRDenyAppriasal(dataEMP.EMP_FirstName + " " + dataEMP.EMP_LastName);
                            var data = _INotification.GetNotificationData(EmployeeId, dataEMP.EMP_ManagerId, true, Message,ModuleSubModule.HRDenyToManager,ModuleSubModule.Performance, dataEMP.EMP_ManagerId, "M", ObjLoginModel.UserName);
                            break;
                    }
                }
            }
            catch (Exception ex)
            {
                Json(ex.Message, JsonRequestBehavior.AllowGet);
            }
            return Json(true, JsonRequestBehavior.AllowGet);
        }
        
        public Action userAssessmentView()
        {
            return null;
        }
        
    }
}