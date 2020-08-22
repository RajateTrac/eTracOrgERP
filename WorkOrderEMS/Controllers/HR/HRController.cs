using System;
using System.Collections.Generic;
using System.Configuration;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WorkOrderEMS.BusinessLogic.Interfaces;
using WorkOrderEMS.Helper;
using WorkOrderEMS.Models;
using WorkOrderEMS.Models.NewAdminModel;

namespace WorkOrderEMS.Controllers.HR
{
    public class HRController : Controller
    {
        // GET: HR
        private readonly IHRManager _IHRManager;
        private readonly string HostingPrefix = Convert.ToString(System.Configuration.ConfigurationManager.AppSettings["hostingPrefix"], CultureInfo.InvariantCulture);
        private readonly string WorkRequestImagepath = ConfigurationManager.AppSettings["WorkRequestImage"];
        private readonly string ProfilePicPath = ConfigurationManager.AppSettings["ProfilePicPath"];
        private readonly string ConstantImages = ConfigurationManager.AppSettings["ConstantImages"];
        private readonly string NoImage = ConfigurationManager.AppSettings["DefaultImage"];
        private readonly string SinglePDFDocPath = ConfigurationManager.AppSettings["FilesUploadRedYellowGreen"];

        public HRController(IHRManager _IHRManager)
        {
            this._IHRManager = _IHRManager;
        }
        public JsonResult GetListOfPerformanceForHR(long? locationId)
        {
            eTracLoginModel ObjLoginModel = null;
            var detailsList = new List<PerformanceModel>();
            if (Session["eTrac"] != null)
            {
                ObjLoginModel = (eTracLoginModel)(Session["eTrac"]);
                if (locationId == null)
                {
                    locationId = ObjLoginModel.LocationID;
                }

            }
            try
            {
                long paramTotalRecords = 0;
                List<PerformanceModel> ITAdministratorList = _IHRManager.GetPerformanceListForHR(locationId, ObjLoginModel.UserName,"");
                foreach (var ITAdmin in ITAdministratorList)
                {
                    ITAdmin.EMP_Photo = (ITAdmin.EMP_Photo == "" || ITAdmin.EMP_Photo == "null") ? HostingPrefix + ConstantImages.Replace("~", "") + "no-profile-pic.jpg" : HostingPrefix + ProfilePicPath.Replace("~/", "") + ITAdmin.EMP_Photo;
                    ITAdmin.EMP_EmployeeID = Cryptography.GetEncryptedData(ITAdmin.EMP_EmployeeID.ToString(), true);
                    ITAdmin.Status = ITAdmin.Status == "S" ? "Assessment Submitted" : ITAdmin.Status == "Y" ? "Assessment Drafted" : (ITAdmin.Status == "G" && ITAdmin.Days > 3) ? "Assessment Lock" : (ITAdmin.Status == "E" && ITAdmin.Days > 3) ? "Evaluation Lock" : ITAdmin.Status == "C" ? "Evaluation Done" : (ITAdmin.Status == "E" && ITAdmin.Days <= 3) ? "Evaluation Pending" : "Assessment Pending";
                    detailsList.Add(ITAdmin);
                }
            }
            catch (Exception ex)
            { return Json(ex.Message, JsonRequestBehavior.AllowGet); }
            return Json(detailsList, JsonRequestBehavior.AllowGet);
        }
    }
}