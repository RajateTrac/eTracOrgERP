using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.Entity.Core.Objects;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using WorkOrderEMS.BusinessLogic;
using WorkOrderEMS.BusinessLogic.Interfaces;
using WorkOrderEMS.BusinessLogic.Managers;
using WorkOrderEMS.Data;
using WorkOrderEMS.Data.Classes;
using WorkOrderEMS.Data.DataRepository;
using WorkOrderEMS.Data.EntityModel;
using WorkOrderEMS.Helper;
using WorkOrderEMS.Helpers;
using WorkOrderEMS.Models;
using WorkOrderEMS.Models.CommonModels;
using WorkOrderEMS.Models.Employee;
using WorkOrderEMS.Models.ManagerModels;
using WorkOrderEMS.Models.NewAdminModel;
using WorkOrderEMS.Models.Performance;

namespace WorkOrderEMS.Controllers.NewAdmin
{
    [Authorize]
    public class NewAdminController : Controller
    {
        // GET: NewAdmin
        UserRepository ObjUserRepository;
        private readonly IDepartment _IDepartment;
        private readonly IGlobalAdmin _GlobalAdminManager;
        private readonly ICommonMethod _ICommonMethod;
        private readonly IQRCSetup _IQRCSetup;
        private readonly IePeopleManager _IePeopleManager;
        private readonly IApplicantManager _IApplicantManager;
        private readonly IGuestUser _IGuestUserRepository;
        private readonly IFillableFormManager _IFillableFormManager;
        private readonly INotification _INotification;
        private readonly string HostingPrefix = Convert.ToString(System.Configuration.ConfigurationManager.AppSettings["hostingPrefix"], CultureInfo.InvariantCulture);
        private readonly string WorkRequestImagepath = ConfigurationManager.AppSettings["WorkRequestImage"];
        private readonly string ProfilePicPath = ConfigurationManager.AppSettings["ProfilePicPath"];
        private readonly string ConstantImages = ConfigurationManager.AppSettings["ConstantImages"];
        private readonly string NoImage = ConfigurationManager.AppSettings["DefaultImage"];
        private readonly string SinglePDFDocPath  = ConfigurationManager.AppSettings["FilesUploadRedYellowGreen"];


        DBUtilities DB = new DBUtilities();
        public NewAdminController(IDepartment _IDepartment, IGlobalAdmin _GlobalAdminManager, ICommonMethod _ICommonMethod, IQRCSetup _IQRCSetup, IePeopleManager _IePeopleManager, IApplicantManager _IApplicantManager, IGuestUser _IGuestUserRepository, IFillableFormManager _IFillableFormManager, INotification _INotification)
        {
            this._IDepartment = _IDepartment;
            this._GlobalAdminManager = _GlobalAdminManager;
            this._ICommonMethod = _ICommonMethod;
            this._IQRCSetup = _IQRCSetup;
            this._IePeopleManager = _IePeopleManager;
            this._IApplicantManager = _IApplicantManager;
            this._IGuestUserRepository = _IGuestUserRepository;
            this._IFillableFormManager = _IFillableFormManager;
            this._INotification = _INotification;
        }
        public ActionResult Index()
        {
            //if(Session["eTrac"]==null)
            //{
            //  string usedd=  HttpContext.User.Identity.Name;
            //    ObjUserRepository = new UserRepository();
            //    UserRegistration loginUser = ObjUserRepository.GetAll(u => u.AlternateEmail == usedd).FirstOrDefault();

            //    Session["eTrac"] = loginUser;
            // }
            return View("~/Views/Shared/_NewDashboard.cshtml");
        }

        public ActionResult ListLocation()
        {
            return View();
        }
        /// <summary>
        /// Created By : Ashwajit Bansod
        /// Created Date : 17-August-2019
        /// Created For : To get location List
        /// </summary>
        /// <param name="_search"></param>
        /// <param name="UserId"></param>
        /// <param name="locationId"></param>
        /// <param name="rows"></param>
        /// <param name="page"></param>
        /// <param name="TotalRecords"></param>
        /// <param name="sord"></param>
        /// <param name="txtSearch"></param>
        /// <param name="sidx"></param>
        /// <param name="UserType"></param>
        /// <returns></returns>
        [HttpGet]
        public JsonResult GetListLocation()
        {
            eTracLoginModel ObjLoginModel = null;
            GlobalAdminManager _GlobalAdminManager = new GlobalAdminManager();
            string _search = string.Empty; long? UserId; int? locationId = null; int? rows = 20; int? page = 1; int? TotalRecords = 10; string sord = null; string txtSearch = null; string sidx = null; string UserType = null;
            if (Session["eTrac"] != null)
            {
                ObjLoginModel = (eTracLoginModel)(Session["eTrac"]);
                //if (locationId == null)
                //{
                //    locationId = Convert.ToInt32(ObjLoginModel.LocationID);
                //}
                UserId = ObjLoginModel.UserId;
            }

            JQGridResults result = new JQGridResults();
            var result1 = new List<ListLocationModel>();
            List<JQGridRow> rowss = new List<JQGridRow>();
            sord = string.IsNullOrEmpty(sord) ? "desc" : sord;
            sidx = string.IsNullOrEmpty(sidx) ? "LocationId" : sidx;
            txtSearch = string.IsNullOrEmpty(_search) ? "" : _search; //UserType = Convert.ToInt64(Helper.UserType.ITAdministrator);   
            try
            {
                ObjectParameter paramTotalRecords = new ObjectParameter("TotalRecords", typeof(int));
                var data = _GlobalAdminManager.ListAllLocation(locationId, page, rows, sidx, sord, txtSearch, paramTotalRecords);

                foreach (var locList in data)
                {
                    //Convert Id to Encrypted data
                    var id = Cryptography.GetEncryptedData(locList.LocationId.ToString(), true);
                    locList.Id = id;
                    result1.Add(locList);
                }
                //This is for JSGrid
                var tt = result1.ToArray();
                //foreach (var locList in result1)
                //{
                //    JQGridRow row = new JQGridRow();
                //    row.id = Cryptography.GetEncryptedData(locList.LocationId.ToString(), true);
                //    row.cell = new string[11];
                //    row.cell[0] = locList.LocationName;
                //    row.cell[1] = locList.Address + "," + locList.City + ", " + locList.State + ", " + locList.ZipCode + "," + locList.Country;
                //    row.cell[2] = locList.LocationAdministrator;
                //    row.cell[3] = locList.LocationManager;
                //    row.cell[4] = locList.LocationEmployee;
                //    row.cell[5] = locList.City;
                //    row.cell[6] = locList.State;
                //    row.cell[7] = locList.Country;
                //    row.cell[8] = locList.PhoneNo + " / " + locList.Mobile;
                //    row.cell[9] = locList.Description;
                //    row.cell[10] = Convert.ToString(locList.QRCID);
                //    rowss.Add(row);
                //}
                result.rows = rowss.ToArray();
                result.page = Convert.ToInt32(page);
                result.total = (int)Math.Ceiling((decimal)Convert.ToInt32(TotalRecords.Value) / rows.Value);
                result.records = Convert.ToInt32(TotalRecords.Value);
                return Json(tt, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            { return Json(ex.Message, JsonRequestBehavior.AllowGet); }
        }
        /// <summary>
        /// Created By : Ashwajit Bansod
        /// Created Date : 18-August-2019
        /// Created For : To get Location details by Location Id
        /// </summary>
        /// <param name="LocationId"></param>
        /// <param name="rows"></param>
        /// <param name="page"></param>
        /// <param name="TotalRecords"></param>
        /// <param name="sord"></param>
        /// <param name="sidx"></param>
        /// <param name="txtSearch"></param>
        /// <returns></returns>
        [HttpGet]
        public ActionResult DisplayLocationData(int LocationId, int? rows = 20, int? page = 1, int? TotalRecords = 10, string sord = null, String sidx = null, string txtSearch = null)
        {
            eTracLoginModel ObjLoginModel = null;
            GlobalAdminManager _GlobalAdminManager = new GlobalAdminManager();
            var details = new LocationDetailsModel();
            if (Session["eTrac"] != null)
            {
                ObjLoginModel = (eTracLoginModel)(Session["eTrac"]);
                if (LocationId == null)
                {
                    LocationId = Convert.ToInt32(ObjLoginModel.LocationID);
                }
            }
            try
            {
                sord = string.IsNullOrEmpty(sord) ? "desc" : sord;
                sidx = string.IsNullOrEmpty(sidx) ? "LocationId" : sidx;
                var obj_Common_B = new Common_B();
                ObjectParameter paramTotalRecords = new ObjectParameter("TotalRecords", typeof(int));
                var data = _GlobalAdminManager.LocationDetailByLocationID(LocationId);
                if (data.Count() > 0)
                {
                    var ListLocationModel = new ListLocationModel();
                    var serivces = obj_Common_B.GetLocationServicesByLocationID(LocationId, 0);
                    foreach (var locList in data)
                    {
                        //var id = Cryptography.GetEncryptedData(locList.LocationId.ToString(), true);
                        ListLocationModel.LocationName = locList.LocationName;
                        ListLocationModel.Address = locList.Address1 + "," + locList.Address2;
                        ListLocationModel.City = locList.City;
                        ListLocationModel.State = locList.LocationState;
                        ListLocationModel.Country = locList.LocationCountry;
                        ListLocationModel.Mobile = locList.PhoneNo + " / " + locList.Mobile;
                        ListLocationModel.Description = locList.Description;
                        ListLocationModel.ZipCode = locList.ZipCode;
                        ListLocationModel.LocationSubTypeDesc = locList.LocationSubTypeDesc;
                        //ListLocationModel.LocationCode = locList.Loc;
                        ListLocationModel.LocationServices = serivces;
                    }
                    details.ListLocationModel = ListLocationModel;
                }
                var ContractDetailsModel = new ContractDetailsModel();
                var LocationRuleMappingModel = new LocationRuleMappingModel();
                details.ContractDetailsModel = GetContractInfo(LocationId).First();
                details.LocationRuleMappingModel = GetLocationRules(LocationId).First();
            }
            catch (Exception ex)
            {

            }
            return View(details);
        }
        [HttpGet]
        public ActionResult AddNewLocation()
        {
            return View();
        }
        #region Operation
        /// <summary>
        /// Created By : Ashwajit Bansod
        /// Created Date : 23-August-2019
        /// Created For : To Show Operation Dashboard
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult OperationDashboard()
        {
            eTracLoginModel ObjLoginModel = null;
            long Totalrecords = 0;
            GlobalAdminManager _GlobalAdminManager = new GlobalAdminManager();
            var details = new LocationDetailsModel();
            if (Session["eTrac"] != null)
            {
                ObjLoginModel = (eTracLoginModel)(Session["eTrac"]);
            }
            //#region WO
            //UserType _UserType = (WorkOrderEMS.Helper.UserType)ObjLoginModel.UserRoleId;
            //if (_UserType == UserType.Administrator)
            //    ViewBag.Location = _ICommonMethod.GetLocationByAdminId(ObjLoginModel.UserId);
            //else if (_UserType == UserType.Manager)
            //    ViewBag.Location = _ICommonMethod.GetLocationByManagerId(ObjLoginModel.UserId);
            //else
            //ViewBag.Location = _ICommonMethod.GetAllLocation();
            //ViewBag.AssignToUserWO = _GlobalAdminManager.GetLocationEmployeeWO(ObjLoginModel.LocationID);
            //ViewBag.AssignToUser = _GlobalAdminManager.GetLocationEmployee(ObjLoginModel.LocationID);
            //ViewBag.Asset = _ICommonMethod.GetAssetList(ObjLoginModel.LocationID);
            //ViewBag.GetAssetListWO = _ICommonMethod.GetAssetListWO(ObjLoginModel.LocationID);
            //ViewBag.UpdateMode = false;
            //ViewBag.PriorityLevel = _ICommonMethod.GetGlobalCodeData("WORKPRIORITY");
            //ViewBag.WorkRequestType = _ICommonMethod.GetGlobalCodeData("WORKREQUESTTYPE");
            //ViewBag.WorkRequestProjectTypeID = _ICommonMethod.GetGlobalCodeData("WORKREQUESTPROJECTTYPE");
            //ViewBag.FacilityRequest = _ICommonMethod.GetGlobalCodeData("FACILITYREQUESTTYPE");
            //ViewBag.StateId = _ICommonMethod.GetStateByCountryId(1);
            //#endregion WO        
            return PartialView("_OperationDashboard");
        }
        /// <summary>
        /// Created By : Ashwajit Bansod
        /// Created Date : 24-Sept-2019
        /// Created For : To View eMaiantanace
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult EMaintananceDashboard()
        {
            eTracLoginModel ObjLoginModel = null;
            GlobalAdminManager _GlobalAdminManager = new GlobalAdminManager();
            var details = new LocationDetailsModel();
            if (Session["eTrac"] != null)
            {
                ObjLoginModel = (eTracLoginModel)(Session["eTrac"]);
            }
            return PartialView("~/Views/NewAdmin/WorkOrderView/_WorkOrderDashboard.cshtml");
        }
        /// <summary>
        /// Created By : Ashwajit Bansod
        /// Created Date : 24-Sept-2019
        /// Created For : To View eScan
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult EScanDashboard()
        {
            eTracLoginModel ObjLoginModel = null;
            GlobalAdminManager _GlobalAdminManager = new GlobalAdminManager();
            var details = new LocationDetailsModel();
            if (Session["eTrac"] != null)
            {
                ObjLoginModel = (eTracLoginModel)(Session["eTrac"]);
            }
            return PartialView("~/Views/NewAdmin/QRCView/_eScanDashboard.cshtml");
        }
        /// <summary>
        /// Created By : Ashwajit Bansod
        /// Created Date : 24-Sept-2019
        /// Created For : To View DAR
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult DARDashboard()
        {
            eTracLoginModel ObjLoginModel = null;
            GlobalAdminManager _GlobalAdminManager = new GlobalAdminManager();
            var details = new LocationDetailsModel();
            if (Session["eTrac"] != null)
            {
                ObjLoginModel = (eTracLoginModel)(Session["eTrac"]);
            }
            return PartialView("~/Views/NewAdmin/DAR/_DARDashboard.cshtml");
            //return View("~/Views/NewAdmin/DAR/DARDashboard.cshtml");
        }
        /// <summary>
        /// Created By  :Ashwajit Bansod
        /// Created Date : 26-Sept-2019
        /// Created For : To View Report of WO, DAR, QRC
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult OperationReports()
        {
            eTracLoginModel ObjLoginModel = null;
            GlobalAdminManager _GlobalAdminManager = new GlobalAdminManager();
            var details = new LocationDetailsModel();
            if (Session["eTrac"] != null)
            {
                ObjLoginModel = (eTracLoginModel)(Session["eTrac"]);
            }
            return PartialView("~/Views/NewAdmin/_Reports.cshtml");

        }
        /// <summary>
        /// Created BY : Ashwajit Bansod
        /// Created Date : 01-Sept-2019
        /// Created For : TO get Work order List
        /// </summary>
        /// <param name="LocationId"></param>
        /// <param name="workRequestAssignmentId"></param>
        /// <param name="OperationName"></param>
        /// <param name="UserId"></param>
        /// <param name="RequestedBy"></param>
        /// <param name="StartDate"></param>
        /// <param name="EndDate"></param>
        /// <param name="filter"></param>
        /// <param name="filterqrc"></param>
        /// <param name="filterwrtype"></param>
        /// <param name="rows"></param>
        /// <param name="page"></param>
        /// <param name="TotalRecords"></param>
        /// <param name="sord"></param>
        /// <param name="sidx"></param>
        /// <param name="txtSearch"></param>
        /// <returns></returns>
        //public JsonResult GetWorkOrderList(int LocationId,long? workRequestProjectId, long UserId, long?RequestedBy ,  string filter, string filterqrc, string filterwrtype,int? rows = 20, int? page = 1, int? TotalRecords = 10, string sord = null, String sidx = null, string txtSearch = null)        
        [HttpGet]
        public JsonResult GetWorkOrderList(long LocationId, long workRequestProjectId, string filterwrtype, string filterqrc, string filter)
        {
            eTracLoginModel ObjLoginModel = null;
            var details = new List<WorkRequestAssignmentModelList>();
            long UserId = 0, RequestedBy = 0;
            int? rows = 20; int? page = 1;
            int? TotalRecords = 10; string sord = null; String sidx = null; string txtSearch = "";
            GlobalAdminManager _GlobalAdminManager = new GlobalAdminManager();
            if (Session["eTrac"] != null)
            {
                ObjLoginModel = (eTracLoginModel)(Session["eTrac"]);
                UserId = ObjLoginModel.UserId;
                if (LocationId == 0)
                {
                    LocationId = Convert.ToInt32(ObjLoginModel.LocationID);
                }
            }
            sord = string.IsNullOrEmpty(sord) ? "desc" : sord;
            sidx = string.IsNullOrEmpty(sidx) ? "CreatedDate" : sidx;
            DateTime StartDate = DateTime.UtcNow;
            DateTime EndDate = DateTime.UtcNow;
            var obj_Common_B = new Common_B();
            ObjectParameter paramTotalRecords = new ObjectParameter("TotalRecords", typeof(int));
            var data = _GlobalAdminManager.GetAllWorkRequestAssignmentList(workRequestProjectId, RequestedBy, "GetAllWorkRequestAssignment", page, rows, sidx, sord, txtSearch, LocationId, UserId, StartDate, EndDate, (filter == "All" ? "" : filter), (filterqrc == "All" ? "" : filterqrc), (filterwrtype == "All" ? "" : filterwrtype), paramTotalRecords);
            if (data.Count() > 0)
            {
                foreach (var item in data)
                {
                    item.id = Cryptography.GetEncryptedData(item.WorkRequestAssignmentID.ToString(), true);
                    item.QRCType = String.IsNullOrEmpty(item.QRCType) ? ((item.eFleetVehicleID != null && item.eFleetVehicleID != "" ? "Shuttle Bus" : "N/A")) : item.QRCType + " (" + item.QRCodeID + ")";
                    item.FacilityRequestType = (item.FacilityRequestType == null || item.FacilityRequestType.TrimWhiteSpace() == "" || item.FacilityRequestType.Trim() == "") ? "N/A" : item.FacilityRequestType;
                    item.ProfileImage = item.ProfileImage == null ? HostingPrefix + ConstantImages.Replace("~", "") + "no-profile-pic.jpg" : HostingPrefix + ProfilePicPath.Replace("~", "") + item.ProfileImage;
                    item.AssignedWorkOrderImage = item.AssignedWorkOrderImage == null ? HostingPrefix + ConstantImages.Replace("~", "") + "no-asset-pic.png" : HostingPrefix + WorkRequestImagepath.Replace("~", "") + item.AssignedWorkOrderImage;
                    details.Add(item);
                }
                return Json(details, JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json(details, JsonRequestBehavior.AllowGet);
            }
        }
        #endregion Operation

        #region ePeople
        public ActionResult ePeopleDashboard()
        {
            eTracLoginModel ObjLoginModel = null;
            GlobalAdminManager _GlobalAdminManager = new GlobalAdminManager();
            var details = new LocationDetailsModel();
            if (Session["eTrac"] != null)
            {
                ObjLoginModel = (eTracLoginModel)(Session["eTrac"]);
            }
            return PartialView("~/Views/NewAdmin/ePeople/_EmployeeManagement.cshtml");
        }

        /// <summary>
        /// Created By : Ashwajit Bansod
        /// Created Date : 23-Sept-2019
        /// Created For : To get All user List by location
        /// </summary>
        /// <param name="LocationId"></param>
        /// <returns></returns>
        [HttpGet]
        public JsonResult GetUserList(long? LocationId)
        {
            eTracLoginModel ObjLoginModel = null;
            var details = new List<UserModelList>();
            if (Session["eTrac"] != null)
            {
                ObjLoginModel = (eTracLoginModel)(Session["eTrac"]);
                if (LocationId == 0)
                {
                    LocationId = Convert.ToInt32(ObjLoginModel.LocationID);
                }
            }
            var data = _IePeopleManager.GetUserList(LocationId);
            if (data.Count() > 0)
            {
                foreach (var item in data)
                {
                    item.ProfileImage = item.ProfileImage == null ? HostingPrefix + ConstantImages.Replace("~", "") + "no-profile-pic.jpg" : HostingPrefix + ProfilePicPath.Replace("~", "") + item.ProfileImage;
                    details.Add(item);
                }
                return Json(details, JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json(details, JsonRequestBehavior.AllowGet);
            }
        }
        #endregion ePeople
        [HttpGet]
        public JsonResult GeDARList(long? LocationId, int? TastType, long? EmployeeId, string FromDate, string ToDate, string FromTime, string ToTime)
        {
            eTracLoginModel ObjLoginModel = null;
            DARManager objDARDetailsList = new DARManager();
            var details = new List<WorkRequestAssignmentModelList>();
            long UserId = 0;
            string sord = null; String sidx = null; string txtSearch = "";
            GlobalAdminManager _GlobalAdminManager = new GlobalAdminManager();
            if (Session["eTrac"] != null)
            {
                ObjLoginModel = (eTracLoginModel)(Session["eTrac"]);
                UserId = ObjLoginModel.UserId;
                if (LocationId == 0)
                {
                    LocationId = Convert.ToInt32(ObjLoginModel.LocationID);
                    UserId = ObjLoginModel.UserId;
                }
            }
            //Getting client date time. 
            var clientdt = DateTime.UtcNow.GetClientDateTimeNow();
            //flag status for if user filter record in time span so to date is till midnight. 
            bool isUTCDay = true;
            //Fetching record like 2017-06-11T00:00:00-04:00 to 2017-06-12T00:0000-04:00
            string fromDate = (FromDate == null || FromDate == " " || FromDate == "") ? clientdt.Date.ToString() : FromDate;
            string toDate = (ToDate == null || ToDate == " " || ToDate == "") ? clientdt.AddDays(1).Date.ToString() : ToDate;

            //maintaining flag  if interval date come then need to fetch record till midnight of todate day
            if (ToDate != null && ToDate != "" && FromDate != "null")
            {
                DateTime tt = Convert.ToDateTime(toDate);
                if (tt.ToLongTimeString() == "12:00:00 AM")
                {
                    isUTCDay = false;
                }
            }

            if (fromDate != null && toDate != null)
            {
                DateTime frmd = Convert.ToDateTime(fromDate);
                DateTime tod = Convert.ToDateTime(toDate);
                ////if interval date come then need to fetch record till midnight of todate day
                if ((frmd.Date != tod.Date) && (tod.ToLongTimeString() == "12:00:00 AM") && isUTCDay == false)
                {
                    tod = tod.AddDays(1).Date;
                    toDate = tod.ToString();
                }
                if ((frmd.Date == tod.Date) && (tod.ToLongTimeString() == "12:00:00 AM"))
                {
                    tod = tod.AddDays(1).Date;
                    toDate = tod.ToString();
                }
            }
            //Converting datetime from userTZ to UTC
            fromDate = Convert.ToDateTime(fromDate).ConvertClientTZtoUTC().ToString();
            toDate = Convert.ToDateTime(toDate).ConvertClientTZtoUTC().ToString();
            int? rows = 20; int? page = 1;
            sord = string.IsNullOrEmpty(sord) ? "desc" : sord;
            sidx = string.IsNullOrEmpty(sidx) ? "CreatedDate" : sidx;
            var obj_Common_B = new Common_B();
            ObjectParameter paramTotalRecords = new ObjectParameter("TotalRecords", typeof(int));
            var data = objDARDetailsList.GetDARDetails(UserId, LocationId, EmployeeId, TastType, page, rows, sord, sord, txtSearch, paramTotalRecords, fromDate, toDate);
            if (data.Count() > 0)
            {
                return Json(data, JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json(data, JsonRequestBehavior.AllowGet);
            }
        }

        public ActionResult PerformanceManagement()
        {
            eTracLoginModel ObjLoginModel = null;
            GlobalAdminManager _GlobalAdminManager = new GlobalAdminManager();
            var details = new LocationDetailsModel();
            if (Session["eTrac"] != null)
            {
                ObjLoginModel = (eTracLoginModel)(Session["eTrac"]);
            }
            return PartialView("_PerformanceManagement");
        }
        [HttpPost]
        public ActionResult userAssessmentView(string Id, string Assesment, string Name, string Image, string JobTitle, string Department, string LocationName)
        {
            eTracLoginModel ObjLoginModel = null;
            string Employee_Id = string.Empty;
            GlobalAdminManager _GlobalAdminManager = new GlobalAdminManager();
            var details = new LocationDetailsModel();
            if (Session["eTrac"] != null)
            {
                ObjLoginModel = (eTracLoginModel)(Session["eTrac"]);
            }
            List<GWCQUestionModel> ListQuestions = new List<GWCQUestionModel>();
            try
            {
                Employee_Id = Cryptography.GetDecryptedData(Id, true);
            }
            catch (Exception e)
            {
                Employee_Id = Id;
            }

            ListQuestions = _GlobalAdminManager.GetGWCQuestions(Employee_Id, Assesment, null);
            ViewData["employeeInfo"] = new GWCQUestionModel() { EmployeeName = Name, AssessmentType = Assesment, Image = Image, JobTitle = JobTitle, Department = Department, LocationName = LocationName };
            return PartialView("userAssessmentView", ListQuestions);
        }
        public ActionResult PerformanceManagementGrid()
        {
            eTracLoginModel ObjLoginModel = null;
            GlobalAdminManager _GlobalAdminManager = new GlobalAdminManager();
            var details = new LocationDetailsModel();
            if (Session["eTrac"] != null)
            {
                ObjLoginModel = (eTracLoginModel)(Session["eTrac"]);
            }
            return PartialView("_306090Grid");
        }
        /// <summary>
        /// Created By : Mayur Sahu
        /// Created Date : 13-Oct-2019
        /// Created For : To Get Performance 306090 list
        /// </summary>
        /// <param name="_search"></param>
        /// <param name="UserId"></param>
        /// <param name="locationId"></param>
        /// <param name="rows"></param>
        /// <param name="page"></param>
        /// <param name="TotalRecords"></param>
        /// <param name="sord"></param>
        /// <param name="txtSearch"></param>
        /// <param name="sidx"></param>
        /// <param name="UserType"></param>
        /// <returns></returns>
        [HttpGet]
        public JsonResult GetListOf306090ForJSGrid(string _search, long? UserId, long? locationId, int? rows = 20, int? page = 1, int? TotalRecords = 10, string sord = null, string txtSearch = null, string sidx = null, string UserType = null)
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
            if (UserType == null)
            {
                UserType = "All Users";
            }
            sord = string.IsNullOrEmpty(sord) ? "desc" : sord;
            sidx = string.IsNullOrEmpty(sidx) ? "Name" : sidx;
            txtSearch = string.IsNullOrEmpty(txtSearch) ? "" : txtSearch;
            long TotalRows = 0;
            try
            {
                long paramTotalRecords = 0;
                List<PerformanceModel> ITAdministratorList = _GlobalAdminManager.GetListOf306090ForJSGrid(ObjLoginModel.UserName, Convert.ToInt64(locationId), page, rows, sidx, sord, txtSearch, UserType, out paramTotalRecords);
                foreach (var ITAdmin in ITAdministratorList)
                {
                    var empid = ITAdmin.EMP_EmployeeID;
                   
                    ITAdmin.IsCurrentEmployee = (ITAdmin.EMP_EmployeeID == ObjLoginModel.UserName) ? true : false;
                    ITAdmin.EMP_Photo = (ITAdmin.EMP_Photo == "" || ITAdmin.EMP_Photo == "null") ? HostingPrefix + ConstantImages.Replace("~", "") + "no-profile-pic.jpg" : HostingPrefix + ProfilePicPath.Replace("~/", "") + ITAdmin.EMP_Photo;
                    ITAdmin.EMP_EmployeeID = Cryptography.GetEncryptedData(ITAdmin.EMP_EmployeeID.ToString(), true);
                    ITAdmin.Status = ITAdmin.Status == "S" ? "Assessment Submitted" : ITAdmin.Status == "Y" ? "Assessment Drafted" 
                        : (ITAdmin.Status == "G" && ITAdmin.Days > 3) 
                        ? "Assessment Lock" 
                        : (ITAdmin.Status == "E" && ITAdmin.Days > 3) 
                        ? "Evaluation Lock" 
                        : ITAdmin.Status == "C"?"Evaluation Done"
                        : (ITAdmin.Status == "E" && ITAdmin.Days <= 3 && ObjLoginModel.UserName != empid)
                        ?"Assessment Submitted"
                        : (ITAdmin.Status == PerformanceEmployeeStatus.FinalSubmitByManager && ObjLoginModel.UserName == empid)
                        ?"Final Submit"
                        : (ObjLoginModel.UserName == empid && ITAdmin.Status == "E" && ITAdmin.Days <= 3) 
                        ? "Evaluation Pending"                
                        : (ITAdmin.Status == PerformanceEmployeeStatus.MeetingSchedule && ObjLoginModel.UserName != empid)
                        ? "Meeting Scheduled"
                        : (ITAdmin.Status == PerformanceEmployeeStatus.MeetingSchedule && ObjLoginModel.UserName == empid)
                        ? "Meeting Schedule"
                        : (ITAdmin.Status == PerformanceEmployeeStatus.MeetingDone && ObjLoginModel.UserName == empid)
                        ? "Meeting Done"
                        : (ITAdmin.Status == PerformanceEmployeeStatus.MeetingStart && ObjLoginModel.UserName != empid)
                        ? "Meeting Start"
                        : (ITAdmin.Status == PerformanceEmployeeStatus.EMPDispute && ObjLoginModel.UserName == empid)
                        ? "Dispute"
                        : (ITAdmin.Status == PerformanceEmployeeStatus.EMPDispute && ObjLoginModel.UserName != empid && ObjLoginModel.JobTitleName == WorkOrderEMS.Helper.JobTItle.HR)
                        ? "Dispute Appriasal"
                        : "Assessment Pending"
                        ;
                    detailsList.Add(ITAdmin);
                }
            }
            catch (Exception ex)
            { return Json(ex.Message, JsonRequestBehavior.AllowGet); }
            return Json(detailsList, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult draftSelfAssessment(List<GWCQUestionModel> data)
        {
            eTracLoginModel ObjLoginModel = null;
            bool result = false;
            if (Session["eTrac"] != null)
            {
                ObjLoginModel = (eTracLoginModel)(Session["eTrac"]);
            }
            try
            {
                result = _GlobalAdminManager.saveSelfAssessment(data, "D", ObjLoginModel.UserName);
            }
            catch (Exception ex)
            { return Json(ex.Message, JsonRequestBehavior.AllowGet); }

            return Json(result, JsonRequestBehavior.AllowGet);

        }
        [HttpPost]
        public JsonResult saveSelfAssessment(List<GWCQUestionModel> data)
        {
            eTracLoginModel ObjLoginModel = null;
            bool result = false;
            if (Session["eTrac"] != null)
            {
                ObjLoginModel = (eTracLoginModel)(Session["eTrac"]);
            }
            try
            {
                result = _GlobalAdminManager.saveSelfAssessment(data, "S", ObjLoginModel.UserName);
            }
            catch (Exception ex)
            { return Json(ex.Message, JsonRequestBehavior.AllowGet); }

            return Json(result, JsonRequestBehavior.AllowGet);

        }

        [HttpPost]
        public ActionResult userEvaluationView(string Id, string Assesment, string Name, string Image, string JobTitle, string Department, string LocationName, string Status)
        {
            eTracLoginModel ObjLoginModel = null;
            string Employee_Id = string.Empty;
            GlobalAdminManager _GlobalAdminManager = new GlobalAdminManager();
            var details = new LocationDetailsModel();
            if (Session["eTrac"] != null)
            {
                ObjLoginModel = (eTracLoginModel)(Session["eTrac"]);
            }
            List<GWCQUestionModel> ListQuestions = new List<GWCQUestionModel>();
            try
            {
                Employee_Id =  Cryptography.GetDecryptedData(Id, true);
            }
            catch (Exception e)
            {
                Employee_Id = Id;
            }
            ListQuestions = _GlobalAdminManager.GetGWCQuestions(Employee_Id, Assesment == "30" ? "31" : Assesment == "60" ? "61" : "91", null);
            ViewData["employeeInfo"] = new GWCQUestionModel() {EmployeeId= Employee_Id, EmployeeName = Name, AssessmentType = Assesment, Image = Image, JobTitle = JobTitle, Department = Department, LocationName = LocationName, Status = Status };
            return PartialView("userEvaluationView", ListQuestions);
        }

        [HttpPost]
        public JsonResult draftEvaluation(List<GWCQUestionModel> data)
        {
            eTracLoginModel ObjLoginModel = null;
            bool result = false;
            if (Session["eTrac"] != null)
            {
                ObjLoginModel = (eTracLoginModel)(Session["eTrac"]);
            }
            try
            {
                result = _GlobalAdminManager.saveEvaluation(data, "D", ObjLoginModel.UserName);
            }
            catch (Exception ex)
            { return Json(ex.Message, JsonRequestBehavior.AllowGet); }

            return Json(result, JsonRequestBehavior.AllowGet);

        }
        [HttpPost]
        public JsonResult saveEvaluation(List<GWCQUestionModel> data)
        {
            eTracLoginModel ObjLoginModel = null;
            bool result = false;
            if (Session["eTrac"] != null)
            {
                ObjLoginModel = (eTracLoginModel)(Session["eTrac"]);
            }
            try
            {
                result = _GlobalAdminManager.saveEvaluation(data, "S", ObjLoginModel.UserName);
                //if (result)
                //   AssessmentEvaluationPDF(data, ObjLoginModel.UserName);

            }
            catch (Exception ex)
            { return Json(ex.Message, JsonRequestBehavior.AllowGet); }

            return Json(result, JsonRequestBehavior.AllowGet);

        }

        /// <summary>
        /// Created By : Mayur Sahu
        /// Created Date : 13-Oct-2019
        /// Created For : To Get Performance 306090 list
        /// </summary>
        /// <param name="_search"></param>
        /// <param name="UserId"></param>
        /// <param name="locationId"></param>
        /// <param name="rows"></param>
        /// <param name="page"></param>
        /// <param name="TotalRecords"></param>
        /// <param name="sord"></param>
        /// <param name="txtSearch"></param>
        /// <param name="sidx"></param>
        /// <param name="UserType"></param>
        /// <returns></returns>
        [HttpGet]
        public JsonResult GetListOfQExpectationsForJSGrid(string _search, long? UserId, long? locationId, int? rows = 20, int? page = 1, int? TotalRecords = 10, string sord = null, string txtSearch = null, string sidx = null, string UserType = null)
        
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
            if (UserType == null)
            {
                UserType = "All Users";
            }
            sord = string.IsNullOrEmpty(sord) ? "desc" : sord;
            sidx = string.IsNullOrEmpty(sidx) ? "Name" : sidx;
            txtSearch = string.IsNullOrEmpty(txtSearch) ? "" : txtSearch;
            long TotalRows = 0;
            try
            {
                long paramTotalRecords = 0;
                List<PerformanceModel> ITAdministratorList = _GlobalAdminManager.GetListOfExpectationsForJSGrid(ObjLoginModel.UserName, Convert.ToInt64(locationId), page, rows, sidx, sord, txtSearch, UserType, out paramTotalRecords);
                foreach (var ITAdmin in ITAdministratorList)
                {
                    ITAdmin.EMP_Photo = (ITAdmin.EMP_Photo == "" || ITAdmin.EMP_Photo == "null") ? HostingPrefix + ConstantImages.Replace("~", "") + "no-profile-pic.jpg" : HostingPrefix + ProfilePicPath.Replace("~/", "") + ITAdmin.EMP_Photo;

                    ITAdmin.EMP_EmployeeID = Cryptography.GetEncryptedData(ITAdmin.EMP_EmployeeID.ToString(), true);
                    ITAdmin.Status = ITAdmin.Status == "C" ? "Expectations Submitted" : ITAdmin.Status == "S" ? "Expectations Submitted" : ITAdmin.Status == "Y" ? "Expectations Drafted" : "Expectations Pending";
                    detailsList.Add(ITAdmin);
                }
            }
            catch (Exception ex)
            { return Json(ex.Message, JsonRequestBehavior.AllowGet); }
            return Json(detailsList, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult userExpectationsView(string Id, string Assesment, string Name, string Image, string JobTitle, string FinYear, string FinQuarter, string Department, string LocationName)
        {
            eTracLoginModel ObjLoginModel = null;
            string Employee_Id = string.Empty;
            GlobalAdminManager _GlobalAdminManager = new GlobalAdminManager();
            var details = new LocationDetailsModel();
            if (Session["eTrac"] != null)
            {
                ObjLoginModel = (eTracLoginModel)(Session["eTrac"]);
            }
            List<GWCQUestionModel> ListQuestions = new List<GWCQUestionModel>();
            try
            {
                Employee_Id = Cryptography.GetDecryptedData(Id, true);
            }
            catch (Exception e)
            {
                Employee_Id = Id;
            }

            ListQuestions = _GlobalAdminManager.GetGWCQuestions(Employee_Id, Assesment, "Expectation");
            foreach (var item in ListQuestions)
            {
                item.EEL_FinencialYear = FinYear;
                item.EEL_FinQuarter = FinQuarter;
                item.AssessmentType = Assesment;
                item.EmployeeId = Employee_Id;


            }
            ViewData["employeeInfo"] = new GWCQUestionModel() { EmployeeId = Employee_Id, EmployeeName = Name, AssessmentType = Assesment, Image = Image, JobTitle = JobTitle, Department = Department, LocationName = LocationName };
            return PartialView("userExpectationsView", ListQuestions);
        }

        [HttpPost]
        public JsonResult draftExpectations(List<GWCQUestionModel> data)
        {
            eTracLoginModel ObjLoginModel = null;
            bool result = false;
            if (Session["eTrac"] != null)
            {
                ObjLoginModel = (eTracLoginModel)(Session["eTrac"]);
            }
            try
            {
                result = _GlobalAdminManager.saveExpectations(data, "D");
            }
            catch (Exception ex)
            { return Json(ex.Message, JsonRequestBehavior.AllowGet); }

            return Json(result, JsonRequestBehavior.AllowGet);

        }
        [HttpPost]
        public JsonResult saveExpectations(List<GWCQUestionModel> data)
        {
            eTracLoginModel ObjLoginModel = null;
            bool result = false;
            if (Session["eTrac"] != null)
            {
                ObjLoginModel = (eTracLoginModel)(Session["eTrac"]);
            }
            try
            {
                result = _GlobalAdminManager.saveExpectations(data, "S");
            }
            catch (Exception ex)
            { return Json(ex.Message, JsonRequestBehavior.AllowGet); }
            return Json(result, JsonRequestBehavior.AllowGet);

        }

        #region Hiring ob boarding
        [HttpGet]
        public ActionResult HiringOnBoardingDashboard()
        {
            eTracLoginModel ObjLoginModel = null;
            GlobalAdminManager _GlobalAdminManager = new GlobalAdminManager();
            var details = new LocationDetailsModel();
            if (Session["eTrac"] != null)
            {
                ObjLoginModel = (eTracLoginModel)(Session["eTrac"]);
            }
            return PartialView("~/Views/NewAdmin/ePeople/_HiringOnBoardingDashboardNew.cshtml");
        }
        [HttpGet]
        public ActionResult MyOpenings(long PostingId)
        {
            //var cont = new RecruiteeAPIController();
            //string url = "";
            //var tt = cont.Index();
            var myOpenings = _GlobalAdminManager.GetMyOpenings(PostingId);
            return Json(myOpenings, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// Created by: Rajat Toppo
        /// Date: 15/06/2020
        /// Get status of applicantcount status in Donut chart 
        /// </summary>
        /// <param name="JobPostingId"></param>
        /// <returns></returns>
        [HttpGet]
        public ActionResult GetApplicantJobSummary(long JobPostingId)
        {
            eTracLoginModel ObjLoginModel = null;
            try
            {
                if (Session["eTrac"] != null)
                {
                    ObjLoginModel = (eTracLoginModel)(Session["eTrac"]);
                }
                var getCount = _GlobalAdminManager.GetEMP_ApplicantCount(Convert.ToInt64(JobPostingId));
                return Json(getCount, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(null, JsonRequestBehavior.AllowGet);
            }

        }
        /// <summary>
        /// Created by : Ashwajit Bansod
        /// Created Date : 07-07-2020
        /// Created For : To Set applicant status
        /// </summary>
        /// <param name="ApplicantId"></param>
        /// <param name="Status"></param>
        /// <param name="IsActive"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult SaveApplicantStatus(long ApplicantId, string Status, string IsActive)
        {
            eTracLoginModel ObjLoginModel = null;
            try
            {
                if (Session["eTrac"] != null)
                {
                    ObjLoginModel = (eTracLoginModel)(Session["eTrac"]);
                }
                var getCount = _IApplicantManager.SetApplicantStatus(ApplicantId, Status, IsActive);
                return Json(getCount, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(null, JsonRequestBehavior.AllowGet);
            }

        }

        [HttpGet]
        public ActionResult MyInterviews()
        {
            var ObjLoginModel = (eTracLoginModel)(Session["eTrac"]);
            var myOpenings = _GlobalAdminManager.GetMyInterviews(ObjLoginModel.UserId);
            return Json(myOpenings, JsonRequestBehavior.AllowGet);
        }
        /// <summary>
        /// Created By : Ashwajit Bansod
        /// Created Date : 24-07-2020
        /// Created For : To get onboarding list
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult OnboardingList()
        {
            var ObjLoginModel = (eTracLoginModel)(Session["eTrac"]);
            var myOpenings = _GlobalAdminManager.GetOnbaordingList(ObjLoginModel.UserId);
            return Json(myOpenings, JsonRequestBehavior.AllowGet);
        }
        
        [HttpGet]
        public ActionResult GetJobPostong()
        {
            var ObjLoginModel = (eTracLoginModel)(Session["eTrac"]);
            var jobPostings = _GlobalAdminManager.GetJobPostong(ObjLoginModel.UserId);
            return Json(jobPostings, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public PartialViewResult InfoFactSheet(MyOpeningModel model)
        {
            InfoFactSheet sheet = new InfoFactSheet { ResumePath = "" };
            sheet.model = model;

            return PartialView("ePeople/_infoFactSheet", sheet);
        }
        [HttpGet]
        public PartialViewResult GetInterviewers(long applicantId)
        {
            Session["eTrac_questions_number"] = null;
            Session["eTrac_questions"] = null;
            var ObjLoginModel = (eTracLoginModel)(Session["eTrac"]);
            var interviewersList = _GlobalAdminManager.GetInterviewersList(applicantId, ObjLoginModel.UserId);
            return PartialView("ePeople/_interviewers", interviewersList);
        }
        //[HttpGet]
        //public PartialViewResult GetInterviewQuestionView()
        //{
        //    //var questions = _GlobalAdminManager.GetInterviewQuestions().Where(x => x.INQ_Id == 1).ToList();
        //    var questions = _GlobalAdminManager.GetInterviewQuestions().ToList();//.Where(x => x.INQ_Id == 1).ToList();
        //    Session["eTrac_questions"] = questions;
        //    return PartialView("ePeople/_questionsview");
        //}
        [HttpGet]
        public PartialViewResult GetInterviewQuestionView(string isExempt)
        {
            //var questions = _GlobalAdminManager.GetInterviewQuestions().Where(x => x.INQ_Id == 1).ToList();
            var questions = _GlobalAdminManager.GetInterviewQuestions(isExempt).ToList();//.Where(x => x.INQ_Id == 1).ToList();
            Session["eTrac_questions"] = questions;
            return PartialView("ePeople/_questionsview");
        }
        [HttpGet]
        public PartialViewResult CheckForTypeInterview(long id)
        {
            if (id > 0)
            {
                var getDataForIsExempt = _IApplicantManager.GetRateOfPayInfo(id, null);
                return PartialView("ePeople/OnBoarding/_CheckForDOT", getDataForIsExempt);
            }
            else
            {
                return PartialView("ePeople/OnBoarding/_CheckForDOT");
            }
            //var questions = _GlobalAdminManager.GetInterviewQuestions().Where(x => x.INQ_Id == 1).ToList();
            // var questions = _GlobalAdminManager.GetInterviewQuestions("Y").ToList();//.Where(x => x.INQ_Id == 1).ToList();
            //Session["eTrac_questions"] = questions;

        }
        /// <summary>
        /// Created By  :Ashwajit Bansod
        /// Created Date : 12-06-2020
        /// Created For : To cancel interview by login interviewer.
        /// </summary>
        /// <param name="InterviewerID"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult CancelInterview(string InterviewerID, long ApplicantId, string Comment)
        {
            bool isCanceled = false;
            try
            {
                if(InterviewerID != null && ApplicantId > 0)
                {
                    //var  checkMail = 
                    isCanceled = _IApplicantManager.CancelInterviewManager(InterviewerID, ApplicantId, Comment);
                }
            }
            catch(Exception ex)
            {
                ViewBag.Message = ex.Message;
                ViewBag.ClassName = "danger";
            }
            return Json(isCanceled, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public PartialViewResult GetInterviewQuestions(int? id, int Applicant)
        {
            IEnumerable<InterviewQuestionMaster> Masterquestions = (List<InterviewQuestionMaster>)Session["eTrac_questions"];
            var questions = new InterviewQuestionAnswerModel();
            var ObjLoginModel = (eTracLoginModel)(Session["eTrac"]);
            int num = 0;
            if (Applicant > 0 && id != 6)
            {
                if (Masterquestions != null)
                {
                    if (Session["eTrac_questions_number"] != null)
                    {
                        num = (int)Session["eTrac_questions_number"];
                        num += 1;
                        if (num <= Masterquestions.Count() - 1)
                        {

                            Session["eTrac_questions_number"] = num;
                            var currentQus1 = Masterquestions.Skip(num).Take(1).FirstOrDefault();
                            if (id > 0)
                            {
                                num = Convert.ToInt32(id) + 1;
                            }
                            var getLst = _GlobalAdminManager.GetInterviewChildQuestions(num).ToList();
                            if (getLst.Count() > 0)
                            {
                                questions.ChildrenQuestionModel = getLst;
                                questions.MasterId = currentQus1.IQM_Id;
                                questions.MasterQuestion = currentQus1.IQM_Question;
                            }

                            //    MasterId = currentQus1.IQM_Id,
                            //    MasterQuestion = currentQus1.IQM_Question,
                            //     ChildrenQuestionModel = 
                            //}).ToList();
                            return PartialView("~/Views/NewAdmin/ePeople/_questions.cshtml", questions);
                        }
                        //return PartialView("~/Views/NewAdmin/ePeople/_questions.cshtml", Masterquestions.Where(x => x.INQ_QuestionType == "LastQuestion").FirstOrDefault());
                    }
                    else
                    {
                        var getLst = _GlobalAdminManager.GetInterviewChildQuestions(1).ToList();
                        if (getLst.Count() > 0)
                        {
                            questions.ChildrenQuestionModel = getLst;
                        }
                        var currentQus = Masterquestions.Skip(0).Take(1).Select(x => new InterviewQuestionAnswerModel()
                        {
                            MasterId = x.IQM_Id,
                            MasterQuestion = x.IQM_Question,
                            ChildrenQuestionModel = getLst
                        }).FirstOrDefault();
                        Session["eTrac_questions_number"] = 0;
                        return PartialView("~/Views/NewAdmin/ePeople/_questions.cshtml", currentQus);
                    }
                }
            }
            else
            {
                var getLst = _GlobalAdminManager.GetInterviewAnswerByApplicantId(Applicant, ObjLoginModel.UserName);
                getLst.IsShortlisted = false;
                Session["IsInterviewDone"] = true;
                return PartialView("~/Views/NewAdmin/ePeople/OnBoarding/_ViewAnswerQuestion.cshtml", getLst);
            }
            return PartialView("~/Views/NewAdmin/ePeople/_questions.cshtml", null);
        }
        /// <summary>
        /// Created By : Ashwajit Bansod
        /// Created Date : 17-March-2020
        /// Created For : To screened or reject applicant as per detail
        /// </summary>
        /// <param name="ApplicantId"></param>
        /// <param name="IsScreened"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult ScreenedRejectApplicant(long ApplicantId, bool IsScreened)
        {
            string Message = string.Empty;
            try
            {
                if (ApplicantId > 0)
                {
                    var isSaved = _IApplicantManager.SaveScreenRejectStatusApplicant(ApplicantId, IsScreened);
                    if (isSaved)
                        Message = CommonMessage.ApplicantScreened();
                    else
                        Message = CommonMessage.ApplicantReject();
                }
            }
            catch (Exception ex)
            {
                return Json(ex, JsonRequestBehavior.AllowGet);
            }
            return Json(Message, JsonRequestBehavior.AllowGet);
        }

        //[HttpPost]
        //public PartialViewResult GetInterviewQuestions(int? id)
        //{
        //    IEnumerable<spGetInterviewQuestion_Result1> questions = (List<spGetInterviewQuestion_Result1>)Session["eTrac_questions"];
        //    int num = 0;
        //    if (questions != null)
        //    {
        //        if (Session["eTrac_questions_number"] != null)
        //        {
        //            num = (int)Session["eTrac_questions_number"];
        //            num += 1;
        //            if (num <= questions.Count() - 1)
        //            {
        //                Session["eTrac_questions_number"] = num;
        //                var currentQus = questions.Skip(num).Take(1).FirstOrDefault();

        //                return PartialView("~/Views/NewAdmin/ePeople/_questions.cshtml", currentQus);
        //            }
        //            return PartialView("~/Views/NewAdmin/ePeople/_questions.cshtml", questions.Where(x => x.INQ_QuestionType == "LastQuestion").FirstOrDefault());
        //        }
        //        else
        //        {
        //            var currentQus = questions.Skip(0).Take(1).FirstOrDefault();
        //            Session["eTrac_questions_number"] = 0;
        //            return PartialView("~/Views/NewAdmin/ePeople/_questions.cshtml", currentQus);
        //        }
        //    }
        //    return PartialView("~/Views/NewAdmin/ePeople/_questions.cshtml", null);
        //}
        [HttpPost]
        public JsonResult SaveAnswers(InterviewQuestionAnswerModel model, List<AnswerArr> AnswerArr)
        {
            var ObjLoginModel = (eTracLoginModel)(Session["eTrac"]);
            var isAnsSaveSuccess = _GlobalAdminManager.SaveInterviewAnswers(model, AnswerArr, ObjLoginModel.UserId);
            return Json(true ? true : false, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// Created By : Ashwajit bansod
        /// Created For : To change the status interview schedule to Interview Completed
        /// Created Date : 29-06-2020
        /// </summary>
        /// <param name="ApplicantId"></param>
        /// <returns></returns>
        public JsonResult InterviewCompleted(long ApplicantId)
        {
            string Message = string.Empty;
            try
            {
                if (ApplicantId > 0)
                {
                    var isSaved = _IApplicantManager.InterviewCompleted(ApplicantId);
                    if (isSaved)
                        Message = CommonMessage.ApplicantScreened();
                    else
                        Message = CommonMessage.ApplicantReject();
                }
            }
            catch (Exception ex)
            {
                return Json(ex, JsonRequestBehavior.AllowGet);
            }
            return Json(Message, JsonRequestBehavior.AllowGet);
        }

        public JsonResult OriantationDone(long ApplicantId, string Status, string IsActive)
        {
            try
            {
                var ObjLoginModel = (eTracLoginModel)(Session["eTrac"]);
                if (ApplicantId > 0 && Status != null && IsActive != null)
                {
                    var setStatus = _IApplicantManager.SetApplicantStatus(ApplicantId, Status, IsActive);
                    var sentMail = _IApplicantManager.SendMailToEmployeeOriantationDone(ApplicantId, ObjLoginModel.UserName, IsActive);
                    return Json(true, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    return Json(false, JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception ex)
            {
                return Json(ex, JsonRequestBehavior.AllowGet);
            }
        }


        public FileStreamResult GetPDF()
        {
            FileStream fs = new FileStream(Server.MapPath("~/App_Data/resume.pdf"), FileMode.Open, FileAccess.Read);
            return File(fs, "application/pdf");
        }

        /// <summary>
        /// Created By : Ashwajit Bansod
        /// Created Date : To download single PDF.
        /// </summary>
        /// <param name="ApplicantId"></param>
        /// <returns></returns>
        [HttpPost]
        public FileResult SinglePDFDownload(string stFile)
        {
            byte[] fileBytes = System.IO.File.ReadAllBytes(stFile);

            return File(fileBytes, System.Net.Mime.MediaTypeNames.Application.Octet, Path.GetFileName(stFile));
            //try
            //{
            //    if (ApplicantId > 0)
            //    {
            //        var getEmployee = _IApplicantManager.GetEmployeeData(ApplicantId);
            //        var getDetails = _IApplicantManager.GetFilesData("Onboarding Files", getEmployee.EMP_EmployeeID);
            //        string RootDirectory = System.Web.Hosting.HostingEnvironment.ApplicationPhysicalPath;
            //        string IsFileExist = RootDirectory + SinglePDFDocPath.Replace("~\\","");
            //        RootDirectory = IsFileExist + getDetails;
            //        //RootDirectory = RootDirectory.Substring(0, RootDirectory.Length - 2).Substring(0, RootDirectory.Substring(0, RootDirectory.Length - 2).LastIndexOf("\\")) + DisclaimerFormPath + ObjWorkRequestAssignmentModel.DisclaimerForm;

            //        string path = Server.MapPath("~/Content/FilesRGY");

            //        byte[] fileBytes = System.IO.File.ReadAllBytes(path + @"\" + getDetails);
            //        string contentType = "application/pdf";
            //        return File(RootDirectory, contentType, "Report.pdf");
            //       // return File(fileBytes, System.Net.Mime.MediaTypeNames.Application.Octet, getDetails);
            //        //if (Directory.GetFiles(IsFileExist, getDetails).Length > 0)
            //        //{
            //        //    byte[] fileBytes = System.IO.File.ReadAllBytes(RootDirectory);
            //        //    return File(fileBytes, System.Net.Mime.MediaTypeNames.Application.Octet, getDetails);
            //        //}
            //        //else
            //        //{
            //        //    RootDirectory = System.Web.Hosting.HostingEnvironment.ApplicationPhysicalPath + SinglePDFDocPath + "FileNotFound.png";
            //        //    byte[] fileBytes = System.IO.File.ReadAllBytes(RootDirectory);
            //        //    return File(fileBytes, System.Net.Mime.MediaTypeNames.Application.Octet, "FileNotFound.png");
            //        //}
            //    }
            //    else { return File(System.Web.Hosting.HostingEnvironment.ApplicationPhysicalPath, "application/pdf", "FileNotFound.png"); }
            //}
            //catch (Exception ex)
            //{
            //    ViewBag.Message = ex.Message;
            //    ViewBag.AlertMessageClass = "Danger";
            //    //return Json(ex.Message);
            //}
            //return File(System.Web.Hosting.HostingEnvironment.ApplicationPhysicalPath, "application/pdf", "FileNotFound.png");
        }
        #region Hiring On Boarding
        [HttpGet]
        public JsonResult GetApplicantInfo()
        {
            var ObjLoginModel = (eTracLoginModel)(Session["eTrac"]);
            var data = _GlobalAdminManager.GetApplicantInfo(ObjLoginModel.UserId);
            return Json(data, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult SaveApplicantInfo(OnboardingDetailRequestModel onboardingDetailRequestModel)
        {
            var ObjLoginModel = (eTracLoginModel)(Session["eTrac"]);
            onboardingDetailRequestModel.CreatedBy = ObjLoginModel.UserId;
            var data = _GlobalAdminManager.SaveApplicantInfo(onboardingDetailRequestModel);
            return Json(data, JsonRequestBehavior.AllowGet);
        }
        /// <summary>
        /// Created By : Ashwajit bansod
        /// Created Date : 05-Nov-2019
        /// Created for : to verify user
        /// </summary>
        /// <param name="onboardingDetailRequestModel"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult VerifyEmployeeAfterGenerate(OnboardingDetailRequestModel onboardingDetailRequestModel)
        {
            var ObjLoginModel = (eTracLoginModel)(Session["eTrac"]);
            onboardingDetailRequestModel.CreatedBy = ObjLoginModel.UserId;
            var data = _GlobalAdminManager.VerifyEmployee(onboardingDetailRequestModel);
            return Json(data, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult SaveGuestEmployeeBasicInfo(GuestEmployeeBasicInfoRequestModel guestEmployeeBasicInfoRequestModel)
        {

            var data = _GlobalAdminManager.SaveGuestEmployeeBasicInfo(guestEmployeeBasicInfoRequestModel);
            return Json(data, JsonRequestBehavior.AllowGet);
        }
        public JsonResult GetStateList()
        {
            return Json(_ICommonMethod.GetStateByCountryId(1), JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public JsonResult CanInterviewerIsOnline(long ApplicantId, string IsAvailable, string Comment)
        {
            var ObjLoginModel = (eTracLoginModel)(Session["eTrac"]);
            var res = _GlobalAdminManager.IsInterviewerOnline(ApplicantId, ObjLoginModel.UserId, IsAvailable, Comment);
            return Json(res, JsonRequestBehavior.AllowGet);
        }
        [HttpGet]
        public JsonResult GetScore(long ApplicantId)
        {
            var res = _GlobalAdminManager.GetScore(ApplicantId);
            return Json(res, JsonRequestBehavior.AllowGet);
        }
        [HttpGet]
        public JsonResult CheckNextQuestion(long ApplicantId, long QusId)
        {
            var res = _GlobalAdminManager.CheckIfAllRespondedForQuestion(ApplicantId, QusId);
            return Json(res, JsonRequestBehavior.AllowGet);
        }
        /// <summary>
        /// Created By : Ashwajit Bansod
        /// Created Date : 26-MArch-2020
        /// Created For : To get the answer details of applicant
        /// </summary>
        /// <param name="ApplicantId"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult GetApplicantInterviewAnswerDetails(int ApplicantId)
        {
            eTracLoginModel ObjLoginModel = null;
            if (Session["eTrac"] != null)
            {
                ObjLoginModel = (eTracLoginModel)(Session["eTrac"]);
            }
            var getLst = _GlobalAdminManager.GetInterviewAnswerByApplicantId(ApplicantId, ObjLoginModel.UserName);
            Session["IsInterviewDone"] = true;
            getLst.IsShortlisted = true;
            return PartialView("~/Views/NewAdmin/ePeople/OnBoarding/_ViewAnswerQuestion.cshtml", getLst);
        }

        /// <summary>
        /// Method to  validate the duplicate employee id ie alternativeemail in db col.
        /// </summary>
        /// <param name="empId"></param>
        /// <returns></returns>
        [HttpGet]
        public ActionResult ValidateEmployeeID(string empId)
        {
            var response = _ICommonMethod.CheckEmployeeIdExist(empId);
            return Json(response, JsonRequestBehavior.AllowGet);
        }

        public ActionResult GuestEmployee()
        {
            return View("~/Views/NewAdmin/GuestEmployee/GuestEmployee.cshtml");
        }
        [HttpPost]
        public JsonResult AssessmentStatusChange(string Status, string IsActive, long ApplicantId)
        {
            string message = string.Empty;
            long UserId = 0;
            eTracLoginModel ObjLoginModel = null;
            if (Session["eTrac"] != null)
            {
                ObjLoginModel = (eTracLoginModel)(Session["eTrac"]);
                UserId = ObjLoginModel.UserId;
            }
            try
            {
                if (Status != null && IsActive != null && ApplicantId > 0)
                {
                    var sendForAssessment = _IePeopleManager.SendForAssessment(Status, IsActive, ApplicantId, UserId);
                    message = CommonMessage.SendAssessment();
                    return Json(message, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    message = CommonMessage.WrongParameterMessage();
                    return Json(message, JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception ex)
            {
                return Json(ex, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        public JsonResult BackgroundStatusChange(string Status, string IsActive, long ApplicantId)
        {
            string message = string.Empty;
            long UserId = 0;
            eTracLoginModel ObjLoginModel = null;
            if (Session["eTrac"] != null)
            {
                ObjLoginModel = (eTracLoginModel)(Session["eTrac"]);
                UserId = ObjLoginModel.UserId;
            }
            try
            {
                if (Status != null && IsActive != null && ApplicantId > 0)
                {
                    var sendForAssessment = _IePeopleManager.SendForBackgroundCheck(Status, IsActive, ApplicantId, UserId);
                    message = CommonMessage.SendAssessment();
                    return Json(message, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    message = CommonMessage.WrongParameterMessage();
                    return Json(message, JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception ex)
            {
                return Json(ex, JsonRequestBehavior.AllowGet);
            }
        }
        [HttpPost]
        public PartialViewResult InterviewAcceptCancel(string status, long ApplicantId)
        {
            eTracLoginModel ObjLoginModel = null;
            try
            {
                if (status != null && ApplicantId > 0)
                {
                    string IsActive = "Y";
                    var isAccept = _GlobalAdminManager.SetInterviewAcceptCancel(status, ApplicantId, IsActive);
                }
            }
            catch (Exception ex)
            {

            }
            //return Json(status, JsonRequestBehavior.AllowGet);
            return PartialView("~/Views/NewAdmin/ePeople/_HiringOnBoardingDashboardNew.cshtml");
        }

        [HttpPost]
        public PartialViewResult ShowAssetsForApplicant(long ApplicantId)
        {
            var model = new AssetsAllocationModel();
            model.ApplicantId = ApplicantId;
            return PartialView("~/Views/NewAdmin/ePeople/OnBoarding/_AssetsAllocation.cshtml", model);
        }
        /// <summary>
        /// Created By : Ashwajit Bansod
        /// Created Date : 20-Dec-2019
        /// Created For : To save Assets
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult SaveShowAssetsForApplicant(AssetsAllocationModel model)
        {
            eTracLoginModel ObjLoginModel = null;
            bool isSaved = false;
            if (Session["eTrac"] != null)
            {
                ObjLoginModel = (eTracLoginModel)(Session["eTrac"]);
            }
            try
            {
                if (model != null)
                {
                    if (model.AssetsId == 0)
                    {
                        model.Action = "I";
                        isSaved = _IApplicantManager.SaveAssets(model);
                    }
                    else
                    {
                        model.Action = "U";
                        isSaved = _IApplicantManager.SaveAssets(model);
                    }
                }
            }
            catch (Exception ex)
            {
                return Json(isSaved, JsonRequestBehavior.AllowGet);
            }
            return Json(isSaved, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public PartialViewResult ShowOfferLetter(long ApplicantId)
        {
            var model = new OfferModel();
            model.ApplicantId = ApplicantId;
            model = _IApplicantManager.GetOfferDetailsOfApplicant(ApplicantId);
            return PartialView("~/Views/NewAdmin/ePeople/OnBoarding/_OfferLetter.cshtml", model);
        }
        /// <summary>
        /// Created By : Ashwajit Bansod
        /// Created For : To send offer letter to applicant
        /// created Date : 24-Dec-2019
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult SendOfferToApplicant(OfferModel model)
        {
            eTracLoginModel ObjLoginModel = null;
            bool isSaved = false;
            if (Session["eTrac"] != null)
            {
                ObjLoginModel = (eTracLoginModel)(Session["eTrac"]);
            }
            try
            {
                if (model != null)
                {
                    model.Action = "I";
                    model.UserId = ObjLoginModel.UserId;
                    isSaved = _IApplicantManager.SendOffer(model);
                }
            }
            catch (Exception ex)
            {
                return Json(isSaved, JsonRequestBehavior.AllowGet);
            }
            return Json(isSaved, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// Created By : Ashwajit Bansod
        /// Created Date : 14--5-2020
        /// Created For : TO open close and hold job by job id
        /// </summary>
        /// <param name="JobId"></param>
        /// <param name="JobStatus"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult CloseHoldOpenJob(long JobId, string JobStatus)
        {
            try
            {
                if (JobId > 0 && JobStatus != null)
                {
                    var update = _IePeopleManager.UpdateCloseHoldOpenJob(JobId, JobStatus);
                    if (update)
                        return Json(true, JsonRequestBehavior.AllowGet);
                    else
                        return Json(false, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    return Json(false, JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception ex)
            {
                return Json(false, JsonRequestBehavior.AllowGet);
            }
        }
        /// <summary>
        /// Created By : Ashwajit Bansod
        /// Created Date : 24-05-2020
        /// Created For : TO view document for applicant id also bind applicant id through viewbag
        /// </summary>
        /// <param name="ApplicantId"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult OpenDocuments(long ApplicantId)
        {
            ViewBag.ApplicantId = ApplicantId;
            return PartialView("~/Views/NewAdmin/ePeople/OnBoarding/_ViewDocument.cshtml");
        }

        public FileStreamResult GetCertificate(long id)
        {
            FileStream fs = new FileStream(Server.MapPath("~/App_Data/resume.pdf"), FileMode.Open, FileAccess.Read);
            return File(fs, "application/pdf");
        }

        [HttpPost]
        public JsonResult GetCountOfHiredAndTotalApplicant()
        {
            var getCount = new ApplicantCountDetails();
            try
            {
                getCount = _IApplicantManager.GetApplicantCount();
            }
            catch (Exception ex)
            {
                ViewBag.message = ex.Message;
                ViewBag.className = "danger";
            }
            return Json(getCount, JsonRequestBehavior.AllowGet);
        }
        /// <summary>
        /// Created By  :Ashwajit Bansod
        /// Created For : To applicant hird after accepting offer letter
        /// Created Date : 08-06-2020
        /// </summary>
        /// <param name="ApplicantId"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult HireApplicant(long ApplicantId)
        {
            var _repo = new ePeopleRepository();
            var isSaved = false;
            try
            {
                if (ApplicantId > 0)
                {
                    var sendMailToApplicant = _IApplicantManager.SendUserNameAndPassword(ApplicantId);
                    var hiresaved = _repo.SendForAssessment(ApplicantStatus.Onboarding, ApplicantIsActiveStatus.Onboarding, ApplicantId);
                    isSaved = true;
                }
                else
                    isSaved = false;
            }
            catch(Exception ex)
            {
                ViewBag.message = ex.Message;
                ViewBag.className = "danger";
            }
            return Json(isSaved, JsonRequestBehavior.AllowGet);
        }
        #endregion Hiring onboarding
        [HttpPost]
        public ActionResult SaveApplicant(CommonApplicantModel model)
        {
            eTracLoginModel ObjLoginModel = null;
            bool isSaved = false;
            W4FormModel returnModel = new W4FormModel();
            if (Session["eTrac"] != null)
            {
                ObjLoginModel = (eTracLoginModel)(Session["eTrac"]);
            }
            try
            {
                if (model != null)
                {
                    //if (model.ApplicantId == 0)
                    //{
                    ////    model.Action = "I";
                    ////    isSaved = _IApplicantManager.SaveAssets(model);
                    //}
                    //else
                    //{
                    //Comment For Testing
                    var obj = _IGuestUserRepository.GetW4Form(ObjLoginModel.UserId);
                    if (obj != null)
                    {
                        obj.IsSignature = false;
                        return PartialView("~/Views/Guest/_W4Form.cshtml", obj);
                    }
                    else
                    {
                        returnModel.IsSignature = false;
                        return PartialView("~/Views/Guest/_W4Form.cshtml", new W4FormModel());
                    }
                    //End 

                    //    model.Action = "U";
                    //    isSaved = _IApplicantManager.SaveAssets(model);

                    //}
                }
            }
            catch (Exception ex)
            {
                return PartialView("~/Views/Guest/Index1.cshtml", returnModel);
            }
            //return RedirectToAction("BenifitSection", "Guest");
            return PartialView("~/Views/Guest/_W4Form.cshtml", returnModel);
        }
        #endregion Applicant
        [HttpPost]
        public ActionResult QEvaluationView(string Id, string Assesment, string Name, string Image, string JobTitle, string FinYear, string FinQuarter, string Department, string LocationName)
        {
            eTracLoginModel ObjLoginModel = null;
            string Employee_Id = string.Empty;
            GlobalAdminManager _GlobalAdminManager = new GlobalAdminManager();
            var details = new LocationDetailsModel();
            if (Session["eTrac"] != null)
            {
                ObjLoginModel = (eTracLoginModel)(Session["eTrac"]);
            }
            List<GWCQUestionModel> ListQuestions = new List<GWCQUestionModel>();
            try
            {
                Employee_Id = Cryptography.GetDecryptedData(Id, true);
            }
            catch (Exception e)
            {
                Employee_Id = Id;
            }

            ListQuestions = _GlobalAdminManager.GetGWCQuestions(Employee_Id, Assesment,"Evaluation");
            foreach (var item in ListQuestions)
            {
                item.EEL_FinencialYear = FinYear;
                item.EEL_FinQuarter = FinQuarter;
                item.AssessmentType = Assesment;


            }
            ViewData["employeeInfo"] = new GWCQUestionModel() { EmployeeName = Name, AssessmentType = Assesment, Image = Image, JobTitle = JobTitle, Department = Department, LocationName = LocationName };
            return PartialView("QEvaluationView", ListQuestions);
        }

        /// <summary>
        /// Created By : Mayur Sahu
        /// Created Date : 13-Oct-2019
        /// Created For : To Get Performance 306090 list
        /// </summary>
        /// <param name="_search"></param>
        /// <param name="UserId"></param>
        /// <param name="locationId"></param>
        /// <param name="rows"></param>
        /// <param name="page"></param>
        /// <param name="TotalRecords"></param>
        /// <param name="sord"></param>
        /// <param name="txtSearch"></param>
        /// <param name="sidx"></param>
        /// <param name="UserType"></param>
        /// <returns></returns>
        [HttpGet]
        public JsonResult GetListOfQEvaluationsForJSGrid(string _search, long? UserId, long? locationId, int? rows = 20, int? page = 1, int? TotalRecords = 10, string sord = null, string txtSearch = null, string sidx = null, string UserType = null)
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
            if (UserType == null)
            {
                UserType = "All Users";
            }
            sord = string.IsNullOrEmpty(sord) ? "desc" : sord;
            sidx = string.IsNullOrEmpty(sidx) ? "Name" : sidx;
            txtSearch = string.IsNullOrEmpty(txtSearch) ? "" : txtSearch;
            long TotalRows = 0;
            try
            {
                long paramTotalRecords = 0;
                List<PerformanceModel> ITAdministratorList = _GlobalAdminManager.GetListOfQEvaluationsForJSGrid(ObjLoginModel.UserName, Convert.ToInt64(locationId), page, rows, sidx, sord, txtSearch, UserType, out paramTotalRecords);
                foreach (var ITAdmin in ITAdministratorList)
                {
                    ITAdmin.EMP_Photo = (ITAdmin.EMP_Photo == "" || ITAdmin.EMP_Photo == "null") ? HostingPrefix + ConstantImages.Replace("~", "") + "no-profile-pic.jpg" : HostingPrefix + ProfilePicPath.Replace("~/", "") + ITAdmin.EMP_Photo;

                    ITAdmin.EMP_EmployeeID = Cryptography.GetEncryptedData(ITAdmin.EMP_EmployeeID.ToString(), true);
                    ITAdmin.Status = ITAdmin.Status == "C"? "Evaluation Submitted" : ITAdmin.Status == "S" ? "Expectations Submitted" : ITAdmin.Status == "Y" ? "Expectations Drafted" : "Expectations Pending";
                    detailsList.Add(ITAdmin);
                }
            }
            catch (Exception ex)
            { return Json(ex.Message, JsonRequestBehavior.AllowGet); }
            return Json(detailsList, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult draftQEvaluations(List<GWCQUestionModel> data)
        {
            eTracLoginModel ObjLoginModel = null;
            bool result = false;
            if (Session["eTrac"] != null)
            {
                ObjLoginModel = (eTracLoginModel)(Session["eTrac"]);
            }
            try
            {
                result = _GlobalAdminManager.saveQEvaluations(data, "D");
            }
            catch (Exception ex)
            { return Json(ex.Message, JsonRequestBehavior.AllowGet); }
            return Json(result, JsonRequestBehavior.AllowGet);

        }
        [HttpPost]
        public JsonResult saveQEvaluations(List<GWCQUestionModel> data)
        {
            eTracLoginModel ObjLoginModel = null;
            bool result = false;
            if (Session["eTrac"] != null)
            {
                ObjLoginModel = (eTracLoginModel)(Session["eTrac"]);
            }
            try
            {
                foreach (var item in data)
                {
                    item.EEL_EMP_EmployeeIdManager = ObjLoginModel.UserName;


                }
                result = _GlobalAdminManager.saveQEvaluations(data, "S");
            }
            catch (Exception ex)
            { return Json(ex.Message, JsonRequestBehavior.AllowGet); }
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public JsonResult saveFinalEvaluations(List<GWCQUestionModel> data)
        {
            eTracLoginModel ObjLoginModel = null;
            bool result = false;
            if (Session["eTrac"] != null)
            {
                ObjLoginModel = (eTracLoginModel)(Session["eTrac"]);
            }
            try
            {
                foreach (var item in data)
                {
                    item.EEL_EMP_EmployeeIdManager = ObjLoginModel.UserName;


                }
                result = _GlobalAdminManager.saveQEvaluations(data, "C");
            }
            catch (Exception ex)
            { return Json(ex.Message, JsonRequestBehavior.AllowGet); }
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        #region Location wise Deal Master
        /// <summary>
        /// Created By : Jemin Vasoya
        /// Created Date : 19-Oct-2019
        /// Created For : To Manage Location wise Deal
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public JsonResult GetListDealsSpecific(int id)
        {
            List<ListDealsSpecificToLocation> LDSTL = new List<ListDealsSpecificToLocation>();
            try
            {
                LDSTL = new List<ListDealsSpecificToLocation>();
                LDSTL = GetDealsSpecificList(id);
                //This is for JSGrid
                var tt = LDSTL.ToArray();
                return Json(tt, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            { return Json(ex.Message, JsonRequestBehavior.AllowGet); }
        }

        public List<ContractDetailsModel> GetContractInfo(int LocationId)
        {
            string QueryString = "exec Usp_GetContractInfo '" + LocationId + "'";
            DataTable dataTable = DB.GetDTResponse(QueryString);
            List<ContractDetailsModel> ItemList = DataRowToObject.CreateListFromTable<ContractDetailsModel>(dataTable);
            return ItemList;
        }

        public List<ListDealsSpecificToLocation> GetDealsSpecificList(int id)
        {
            string QueryString = "exec Usp_GetDealsSpecificList '" + id + "'";
            DataTable dataTable = DB.GetDTResponse(QueryString);
            List<ListDealsSpecificToLocation> ItemList = DataRowToObject.CreateListFromTable<ListDealsSpecificToLocation>(dataTable);
            return ItemList;
        }

        public List<LocationRuleMappingModel> GetLocationRules(int LocationId)
        {
            string QueryString = "exec Usp_GetContractInfo '" + LocationId + "'";
            DataTable dataTable = DB.GetDTResponse(QueryString);
            List<LocationRuleMappingModel> ItemList = DataRowToObject.CreateListFromTable<LocationRuleMappingModel>(dataTable);
            return ItemList;
        }
        #endregion
        public ActionResult GetCompanyOpening()
        {
            eTracLoginModel ObjLoginModel = null;
            var _workorderems = new workorderEMSEntities();
            var data = new List<spGetJobPosting_ForCompanyOpening_Result>();
            if (Session["eTrac"] != null)
            {
                ObjLoginModel = (eTracLoginModel)(Session["eTrac"]);
            }
            var getEMPID = _workorderems.UserRegistrations.Where(x => x.UserId == ObjLoginModel.UserId).FirstOrDefault().EmployeeID;
            data = _GlobalAdminManager.GetJobPostingForCompanyOpening(getEMPID);
            return Json(data, JsonRequestBehavior.AllowGet);
        }
        /// <summary>
        /// Created by  : Ashwajit Bansod
        /// Created Date : 20-Nov-2019
        /// Created For : To get hiring manager count
        /// </summary>
        /// <param name="postingId"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult GetHringChartData(long postingId)
        {
            eTracLoginModel ObjLoginModel = null;
            if (Session["eTrac"] != null)
            {
                ObjLoginModel = (eTracLoginModel)(Session["eTrac"]);
            }
            try
            {
                var getChartData = _GlobalAdminManager.HiringGraphCount(postingId);
                return Json(getChartData, JsonRequestBehavior.AllowGet);
            }
            catch
            {
                return Json(null, JsonRequestBehavior.AllowGet);
            }
        }
        //[HttpPost]
        //public ActionResult QEvaluationView(string Id, string Assesment, string Name, string Image, string JobTitle, string FinYear, string FinQuarter)
        //{
        //    eTracLoginModel ObjLoginModel = null;
        //    string Employee_Id = string.Empty;
        //    GlobalAdminManager _GlobalAdminManager = new GlobalAdminManager();
        //    var details = new LocationDetailsModel();
        //    if (Session["eTrac"] != null)
        //    {
        //        ObjLoginModel = (eTracLoginModel)(Session["eTrac"]);
        //    }
        //    List<GWCQUestionModel> ListQuestions = new List<GWCQUestionModel>();
        //    try
        //    {
        //        Employee_Id = Cryptography.GetDecryptedData(Id, true);
        //    }
        //    catch (Exception e)
        //    {
        //        Employee_Id = Id;
        //    }

        //    ListQuestions = _GlobalAdminManager.GetGWCQuestions(Employee_Id, Assesment);
        //    foreach (var item in ListQuestions)
        //    {
        //        item.EEL_FinencialYear = FinYear;
        //        item.EEL_FinQuarter = FinQuarter;
        //        item.AssessmentType = Assesment;


        //    }
        //    ViewData["employeeInfo"] = new GWCQUestionModel() { EmployeeName = Name, AssessmentType = Assesment, Image = Image, JobTitle = JobTitle };
        //    return PartialView("QEvaluationView", ListQuestions);
        //}
        [HttpPost]
        public JsonResult SetupMeeting(SetupMeeting SetupMeeting)
        {
            eTracLoginModel ObjLoginModel = null;
            bool result = false;
            if (Session["eTrac"] != null)
            {
                ObjLoginModel = (eTracLoginModel)(Session["eTrac"]);
            }
            try
            {
                SetupMeeting.ReceipientEmailId = Cryptography.GetDecryptedData(SetupMeeting.ReceipientEmailId, true);
                result = _GlobalAdminManager.SetupMeetingEmail(SetupMeeting);
            }
            catch (Exception ex)
            {
                result = _GlobalAdminManager.SetupMeetingEmail(SetupMeeting);
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public JsonResult GetMeetingDetail(string Id, string FinYear, string FinQuarter)
        {
            eTracLoginModel ObjLoginModel = null;
            string result =string.Empty;
            if (Session["eTrac"] != null)
            {
                ObjLoginModel = (eTracLoginModel)(Session["eTrac"]);
            }
            try
            {
                Id = Cryptography.GetDecryptedData(Id, true);
                result = _GlobalAdminManager.GetMeetingDetail(Id, FinYear, FinQuarter);
            }
            catch (Exception ex)
            {
                result = _GlobalAdminManager.GetMeetingDetail(Id, FinYear, FinQuarter);
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        [HttpGet]
        ///Get Scheduled Meeting List
        ///17/11/2019
        public JsonResult GetMeetingList()
        {
            List<ReviewMeeting> MeetingList;
            try
            {
                MeetingList = _GlobalAdminManager.GetMeetingList();
                foreach (var ITAdmin in MeetingList)
                {
                    ITAdmin.ManagerPhoto = (ITAdmin.ManagerPhoto == "" || ITAdmin.ManagerPhoto == "null") ? HostingPrefix + ConstantImages.Replace("~", "") + "no-profile-pic.jpg" : HostingPrefix + ProfilePicPath.Replace("~/", "") + ITAdmin.ManagerPhoto;
                    ITAdmin.EmployeePhoto = (ITAdmin.EmployeePhoto == "" || ITAdmin.EmployeePhoto == "null") ? HostingPrefix + ConstantImages.Replace("~", "") + "no-profile-pic.jpg" : HostingPrefix + ProfilePicPath.Replace("~/", "") + ITAdmin.EmployeePhoto;
                    ITAdmin.MeetingDate = ITAdmin.PRMeetingDateTime.HasValue ? ITAdmin.PRMeetingDateTime.Value.ToShortDateString() : "";
                    ITAdmin.MeetingTime = ITAdmin.PRMeetingDateTime.HasValue ? ITAdmin.PRMeetingDateTime.Value.ToShortTimeString() : "";
                    MeetingList.Add(ITAdmin);
                }
                
            }
            catch (Exception)
            {

                throw;
            }
            return Json(MeetingList, JsonRequestBehavior.AllowGet);
        }
        #region Scheduler
        public ActionResult GetMyEvents(string start, string end, string _)
        {
            eTracLoginModel ObjLoginModel = null;
            List<EventModel> result = new List<EventModel>();
            if (Session["eTrac"] != null)
            {
                ObjLoginModel = (eTracLoginModel)(Session["eTrac"]);
            }
            try
            {
                result = _GlobalAdminManager.GetMyEvents(ObjLoginModel.UserName, start, end);
                var eventList = from e in result
                                select new
                                {
                                    id = e.id,
                                    title = e.title,
                                    start = e.start,
                                    end = e.end,
                                    color = e.StatusColor,
                                    //className = e.ClassName,
                                    //someKey = e.SomeImportantKeyID,
                                    allDay = false
                                };

                var rows = eventList.ToArray();
                return Json(rows, JsonRequestBehavior.AllowGet);
            }
            catch (Exception e)
            {
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        private long ToUnixTimespan(DateTime date)
        {
            TimeSpan tspan = date.ToUniversalTime().Subtract(
     new DateTime(1970, 1, 1, 0, 0, 0));
            return (long)Math.Truncate(tspan.TotalSeconds);
        }
        public string SaveEvent(string Title, string NewEventDate, string  NewEventTime, string NewEventDuration,string JobId, string ApplicantName,string ApplicantEmail,string selectedManagers)
        {
            eTracLoginModel ObjLoginModel = null;
            if (Session["eTrac"] != null)
            {
                ObjLoginModel = (eTracLoginModel)(Session["eTrac"]);
            }
            try
            {
                return _GlobalAdminManager.CreateNewEvent(Title, NewEventDate, NewEventTime, NewEventDuration, JobId, ApplicantName, ApplicantEmail, ObjLoginModel.UserName, selectedManagers);
            }
            catch (Exception)
            {
                throw;
            }
        }
        public void UpdateEvent(int id, string NewEventStart, string NewEventEnd)
        {
            _GlobalAdminManager.UpdateEvent(id, NewEventStart, NewEventEnd);
        }

        public ActionResult GetBookedSlots(string ApplicantId)
        {
            eTracLoginModel ObjLoginModel = null;
            List<EventModel> result = new List<EventModel>();
            if (Session["eTrac"] != null)
            {
                ObjLoginModel = (eTracLoginModel)(Session["eTrac"]);
            }
            try
            {
                result = _GlobalAdminManager.GetBookedSlots(ObjLoginModel.UserName).OrderByDescending(x=>x.start).ToList();
                var eventList = from e in result
                                select new 
                                {
                                    id = e.id,
                                    title = e.title,
                                    startDate = Convert.ToDateTime(e.start).ToShortDateString(),
                                    startTime = Convert.ToDateTime(e.start).ToShortTimeString(),
                                    end = Convert.ToDateTime(e.end).ToShortTimeString(),
                                    color = e.StatusColor,
                                    allDay = false
                                } ;
                var rows = eventList.ToArray();
                return Json(rows, JsonRequestBehavior.AllowGet);
            }
            catch (Exception e)
            {
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        //public ActionResult GetManagerList()
        //{
        //    eTracLoginModel ObjLoginModel = null;
        //    List<ManagerModel> result = new List<ManagerModel>();
        //    if (Session["eTrac"] != null)
        //    {
        //        ObjLoginModel = (eTracLoginModel)(Session["eTrac"]);
        //    }
        //    try
        //    {
        //        result = _ICommonMethod.GetManagerList();
        //        var eventList = from e in result
        //                        select new
        //                        {
        //                            label=e.UserName,
        //                            value=e.UserID.ToString()
        //                        };
        //        var rows = eventList.ToArray();
        //        return Json(rows, JsonRequestBehavior.AllowGet);
        //    }
        //    catch (Exception)
        //    {
        //        return Json(null, JsonRequestBehavior.AllowGet);
        //    }
        //}
        [HttpGet]
        public ActionResult GetManagerList()
        {
            eTracLoginModel ObjLoginModel = null;
            List<ManagerModel> result = new List<ManagerModel>();
            if (Session["eTrac"] != null)
            {
                ObjLoginModel = (eTracLoginModel)(Session["eTrac"]);
            }
            try
            {
                result = _ICommonMethod.GetManagerList();
                var eventList = from e in result
                                select new
                                {
                                    label = e.UserName,
                                    value = e.UserEmail//.Split('@')[0]
                                };
                var rows = eventList.Where(x => x.value.ToLower() != ObjLoginModel.UserName.ToLower()).ToArray();
                return Json(rows, JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {
                return Json(null, JsonRequestBehavior.AllowGet);
            }
        }
        [HttpPost]
        public ActionResult GetManagerListForApplicant(string IsExempt)
        {
            eTracLoginModel ObjLoginModel = null;
            List<ManagerModel> result = new List<ManagerModel>();
            if (Session["eTrac"] != null)
            {
                ObjLoginModel = (eTracLoginModel)(Session["eTrac"]);
            }
            try
            {
                result = _ICommonMethod.GetManagerList();
                if (IsExempt == "Y")
                {
                    var eventList = from e in result
                                    select new
                                    {
                                        label = e.UserName,
                                        value = e.UserEmail//.Split('@')[0]
                                    };
                    var rows = eventList.Where(x => x.value != ObjLoginModel.UserName.ToLower()).ToArray();
                    return Json(rows.ToArray(), JsonRequestBehavior.AllowGet);
                }
                else
                {
                    var eventList = from e in result
                                    select new
                                    {
                                        label = e.UserName,
                                        value = e.UserEmail//.Split('@')[0]
                                    };
                    var rows = eventList.Where(x => x.value == ObjLoginModel.UserName.ToLower()).ToArray();
                    return Json(rows.ToArray(), JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception)
            {
                return Json(null, JsonRequestBehavior.AllowGet);
            }
        }
        public ActionResult UpdateInterviewPanel(string selectedManagers, string JobId,string JobTitle)
        {
            eTracLoginModel ObjLoginModel = null;
            bool result = false;
            if (Session["eTrac"] != null)
            {
                ObjLoginModel = (eTracLoginModel)(Session["eTrac"]);
            }
            try
            {
                if (selectedManagers != ObjLoginModel.UserName)
                {
                    result = _GlobalAdminManager.UpdateInterviewPanel(selectedManagers, ObjLoginModel.UserName, JobId, JobTitle);
                }
                else
                {
                    result = _GlobalAdminManager.UpdateInterviewPanel("", ObjLoginModel.UserName, JobId, JobTitle);
                }
                    return Json(result, JsonRequestBehavior.AllowGet);

            }
            catch (Exception)
            {
                return Json(result, JsonRequestBehavior.AllowGet);
            }
        }

        /// <summary>
        /// Created By : Ashwajit Bansod
        /// Created Date : 16-06-2020
        /// Created For : To update interview panel for my opening grid for applicant 
        /// </summary>
        /// <param name="selectedManagers"></param>
        /// <param name="JobId"></param>
        /// <param name="JobTitle"></param>
        /// <param name="ApplicantId"></param>
        /// <returns></returns>
        
        public ActionResult UpdateInterviewPanelForMyOpening(string selectedManagers, long ApplicantId)
        {
            eTracLoginModel ObjLoginModel = null;
            bool result = false;
            if (Session["eTrac"] != null)
            {
                ObjLoginModel = (eTracLoginModel)(Session["eTrac"]);
            }
            try
            {
                result = _GlobalAdminManager.UpdateInterviewPanelForMyOpening(selectedManagers, ObjLoginModel.UserName, null, null, ApplicantId);               
                return Json(result, JsonRequestBehavior.AllowGet);

            }
            catch (Exception)
            {
                return Json(result, JsonRequestBehavior.AllowGet);
            }
        }


        public JsonResult GetSlotTimings(string date) {
            eTracLoginModel ObjLoginModel = null;
            if (Session["eTrac"] != null)
            {
                ObjLoginModel = (eTracLoginModel)(Session["eTrac"]);
            }
            List<CustomSlotTime> list = new List<CustomSlotTime>();
            try
            {
                list= _ICommonMethod.GetSlotTimings(ObjLoginModel.UserName,date);
            }
            catch (Exception)
            {
                throw;
            }
            return Json(list,JsonRequestBehavior.AllowGet);
        }
        [HttpGet]
        public void GetOutlookMeeting()
        {
            _GlobalAdminManager.GetOutlookMeetingDetails("","");
        }

        /// <summary>
        /// Created By : Ashwajit Bansod
        /// Created Date : 11-06-2020
        /// Created For : TO make interviewer absent
        /// </summary>
        /// <param name="InterviewrID"></param>
        /// <param name="ApplicantId"></param>
        /// <param name="Comment"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult MarkAbsentToInterviewer(string InterviewerID,long ApplicantId, string Comment)
        {
            bool AbsentSaved = false;
            try
            {
                var manager = new GlobalAdminManager();
                if(InterviewerID != null)
                {
                     AbsentSaved = manager.MakeAbsent(InterviewerID, ApplicantId, Comment);              
                }
            }
            catch(Exception ex)
            {
                ViewBag.Message = ex.Message;
                ViewBag.ClassName = "danger";
            }
            return Json(AbsentSaved,JsonRequestBehavior.AllowGet);
        }
        #endregion
        //public ActionResult MySchedules() {
        //          try
        //          {

        //          }
        //          catch (Exception)
        //          {

        //              throw;
        //          }
        //          return View();

        //      }
        [HttpGet]
        public ActionResult GetManagerAssessmentDetails()
        {
            eTracLoginModel ObjLoginModel = null;
            PerformanceModel model = new PerformanceModel();
            if (Session["eTrac"] != null)
            {
                ObjLoginModel = (eTracLoginModel)(Session["eTrac"]);
            }
            try
            {
                model = _GlobalAdminManager.GetManagerAssessmentDetails(ObjLoginModel.UserName);
            }
            catch (Exception ex)
            {
            }
            return Json(model, JsonRequestBehavior.AllowGet);

        }

        [HttpPost]
        public JsonResult insertChangedExpectations(List<GWCQUestionModel> data)
        {
            eTracLoginModel ObjLoginModel = null;
            bool result = false;
            if (Session["eTrac"] != null)
            {
                ObjLoginModel = (eTracLoginModel)(Session["eTrac"]);
            }
            try
            {
                result = _GlobalAdminManager.saveChangedExpectations(data, null, ObjLoginModel.UserName);
            }
            catch (Exception ex)
            { return Json(ex.Message, JsonRequestBehavior.AllowGet); }
            return Json(result, JsonRequestBehavior.AllowGet);

        }

        [HttpPost]
        public JsonResult updateChangedExpectations(List<GWCQUestionModel> data)
        {
            eTracLoginModel ObjLoginModel = null;
            bool result = false;
            if (Session["eTrac"] != null)
            {
                ObjLoginModel = (eTracLoginModel)(Session["eTrac"]);
            }
            try
            {
                result = _GlobalAdminManager.saveChangedExpectations(data, "S", ObjLoginModel.UserName);
            }
            catch (Exception ex)
            { return Json(ex.Message, JsonRequestBehavior.AllowGet); }
            return Json(result, JsonRequestBehavior.AllowGet);

        }

        [HttpPost]
        public ActionResult SelfAssessmentView(string Id, string Assesment, string Name, string Image, string JobTitle, string FinYear, string FinQuarter, string Department, string LocationName)
        {
            eTracLoginModel ObjLoginModel = null;
            string Employee_Id = string.Empty;
            GlobalAdminManager _GlobalAdminManager = new GlobalAdminManager();
            var details = new LocationDetailsModel();
            if (Session["eTrac"] != null)
            {
                ObjLoginModel = (eTracLoginModel)(Session["eTrac"]);
            }
            List<GWCQUestionModel> ListQuestions = new List<GWCQUestionModel>();
            try
            {
                Employee_Id = Cryptography.GetDecryptedData(Id, true);
            }
            catch (Exception e)
            {
                Employee_Id = Id;
            }

            ListQuestions = _GlobalAdminManager.GetSelfAssessmentView(Employee_Id, Assesment);
            foreach (var item in ListQuestions)
            {
                item.EEL_FinencialYear = FinYear;
                item.EEL_FinQuarter = FinQuarter;
                item.AssessmentType = Assesment;
                item.EmployeeId = Employee_Id;


            }
            ViewData["employeeInfo"] = new GWCQUestionModel() { EmployeeName = Name, AssessmentType = Assesment, Image = Image, JobTitle = JobTitle, Department = Department, LocationName = LocationName };
            return PartialView("SelfAssessmentView", ListQuestions);
        }

        /// <summary>
        /// Created by : Ashwajit Bansod
        /// Created For : To maintain a status of employee.
        /// </summary>
        /// <param name="EmployeeId"></param>
        /// <param name="Status"></param>
        /// <returns></returns>
       [HttpPost]
        public JsonResult PerformanceStatusChange(string EmployeeId, string Status)
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
                if (EmployeeId != null && Status != null)
                {
                    if(Status == PerformanceEmployeeStatus.FinalSubmitByManager)
                    {
                        var dataEMP = getEMPDetails.GetEmployeeDetails(EmployeeId);
                        string Message = DarMessage.HRDenyAppriasal(dataEMP.EMP_FirstName + " " + dataEMP.EMP_LastName);
                        var data = _INotification.GetNotificationData(EmployeeId, EmployeeId, true, Message, ModuleSubModule.FinalSubmitEvaluation, ModuleSubModule.Performance, EmployeeId, "M", ObjLoginModel.UserName);
                    }
                    return Json(true, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    return Json(false, JsonRequestBehavior.AllowGet);
                }
            }
            catch(Exception ex)
            {
                return Json(ex, JsonRequestBehavior.AllowGet);
            }
            return Json(false, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// Created By : Ashwajit Bansod
        /// Created For : TO update the performance status
        /// </summary>
        /// <param name="EmployeeId"></param>
        /// <param name="Status"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult UpdatePerformanceStatus(string EmployeeId, string Status, long? RMS_Id, string Assessment)
        {
            try
            {
                eTracLoginModel ObjLoginModel = null;
                if (Session["eTrac"] != null)
                {
                    ObjLoginModel = (eTracLoginModel)(Session["eTrac"]);
                }
                if (EmployeeId != null && Status != null)
                {
                    var save = _IePeopleManager.Status(EmployeeId, Status, ObjLoginModel.UserName, RMS_Id, Assessment);
                    return Json(true, JsonRequestBehavior.AllowGet);
                }
                else
                {
                   return Json(false, JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception ex)
            {
                ViewBag.Message = ex.Message;
                ViewBag.Class = "danger";
            }
           return Json(true, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// Created By : Ashwajit Bansod
        /// Created Date : 11-08-2020
        /// Created For : To save dispute appriasal file
        /// </summary>
        /// <param name="EmployeeId"></param>
        /// <param name="Status"></param>
        /// <param name="Comment"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult UploadFilesAppriasalDispute(string EmployeeId, string Status,string Comment)
        {
            eTracLoginModel ObjLoginModel = null;
            var _manager = new GlobalAdminManager();
            try
            {
                if (EmployeeId != null && Status == PerformanceEmployeeStatus.EMPDispute)
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
                                var getUser = _manager.GetEmployeeDetails(EmployeeId);
                                if (getUser != null)
                                {
                                    if (fname != null)
                                    {

                                        string FName = ObjLoginModel.UserId + "_" + DateTime.Now.Ticks.ToString() + "_" + fname;
                                        CommonHelper.StaticUploadImage(file, Server.MapPath(ConfigurationManager.AppSettings["ApplicantSignature"]), FName);;
                                        var getDetails = _IFillableFormManager.GetFileList(ObjLoginModel).Where(x => x.FileType == "Green" && x.FileTypeId == Convert.ToInt64(FileTypeId.PerformanceAppraisal)).FirstOrDefault();
                                        var Obj = new UploadedFiles();
                                        Obj.FileName = "Dispute Appriasal";
                                        Obj.FileId = Convert.ToInt64(FileTypeId.PerformanceAppraisal);
                                        Obj.FileEmployeeId = EmployeeId;
                                        string LoginEmployeeId = EmployeeId;
                                        Obj.AttachedFileName = fname;
                                        var IsSaved = _IFillableFormManager.SaveFile(Obj, LoginEmployeeId);
                                        var getHRList = _IDepartment.GetDepartmentEmployeeList(WorkOrderEMS.Helper.Department.HR);
                                        foreach (var item in getHRList)
                                        {
                                            string Message = DarMessage.EMPAcceptDispute(getUser.EMP_FirstName + " " + getUser.EMP_LastName,"Dispute");
                                            var data = _INotification.GetNotificationData(item.EmployeeId, item.EmployeeId, true, Message, ModuleSubModule.HRDisputeAppriasal, ModuleSubModule.Performance, item.EmployeeId, "H", ObjLoginModel.UserName);
                                        }
                                        var save = _IePeopleManager.Status(EmployeeId, Status, ObjLoginModel.UserName,null, null);
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
                    var getUser = _manager.GetEmployeeDetails(EmployeeId);
                    var getHRList = _IDepartment.GetDepartmentEmployeeList(WorkOrderEMS.Helper.Department.HR);
                    foreach (var item in getHRList)
                    {
                        string Message = DarMessage.EMPAcceptDispute(getUser.EMP_FirstName + " " + getUser.EMP_LastName,"Accept");
                        var data = _INotification.GetNotificationData(item.EmployeeId, item.EmployeeId, true, Message, ModuleSubModule.HRDisputeAppriasal, ModuleSubModule.Performance, item.EmployeeId, "H", ObjLoginModel.UserName);
                    }
                }
            }
            catch(Exception ex)
            {
                return Json(ex.Message, JsonRequestBehavior.AllowGet);
            }
            return Json(true, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// Created By : Ashwajit Bansod
        /// Created Date : 12-08-2020
        /// Created For : To get employee file data for view file.
        /// </summary>
        /// <param name="EmployeeId"></param>
        /// <returns></returns>
        public ActionResult GetEmployeeDisputeComment(string EmployeeId)
        {
            var _manager = new FillableFormRepository();
            try
            {
                if(EmployeeId != null)
                {
                    var getDetails = _manager.GetFileDataByEmployeeId(EmployeeId, FileName.EmpoyeeDisputeAppriasal);
                    ViewBag.AttachedFileName = getDetails.FLU_FileAttached;
                    ViewBag.FileId = getDetails.FLU_FileId;
                    ViewBag.EmployeeId = EmployeeId;
                    ViewBag.FileName = getDetails.FLU_FileName;
                }
            }
            catch(Exception ex)
            {
                ViewBag.Message = ex.Message;
                ViewBag.Class ="danger";
            }
            return View("~/Views/NewAdmin/_AcceptDisputeByHRComment.cshtml");
        }

        /// <summary>
        /// Created By : Ashwajit Bansod
        /// Created Date : 12-08-2020
        /// Created For : To save Accept deny by HR
        /// </summary>
        /// <param name="EmployeeId"></param>
        /// <param name="Status"></param>
        /// <returns></returns>
        public JsonResult AcceptDisputeByHR(string EmployeeId, string Status, string Remedy)
        {
            eTracLoginModel ObjLoginModel = null;
            var _manager = new GlobalAdminManager();
            try
            {
                if (Session["eTrac"] != null)
                {
                    ObjLoginModel = (eTracLoginModel)(Session["eTrac"]);
                }
                if (EmployeeId != null && Status != null)
                {
                    var save = _IePeopleManager.Status(EmployeeId, Status, ObjLoginModel.UserName,null, null);
                    var data = _manager.GetEmployeeDetails(EmployeeId);
                    if (data.EMP_EmployeeID != null)
                    {
                        string Message = string.Empty;
                        var dataEMP = _manager.GetEmployeeDetails(EmployeeId);
                        if (Status == PerformanceEmployeeStatus.DisputeDeny)
                        {
                             Message = DarMessage.HRAcceptDispute(dataEMP.EMP_FirstName + " " + dataEMP.EMP_LastName, Remedy);
                        }
                        else {
                             Message = DarMessage.HRAcceptDispute(dataEMP.EMP_FirstName + " " + dataEMP.EMP_LastName, Remedy);
                        }
                            var notificationEMP = _INotification.GetNotificationData(dataEMP.EMP_EmployeeID, dataEMP.EMP_EmployeeID, true, Message, ModuleSubModule.HRDisputeAppriasal, ModuleSubModule.Performance, data.EMP_EmployeeID, "H", ObjLoginModel.UserName);
                        
                    }
                    else
                    {
                        string Message = string.Empty;
                        var dataMAN = _manager.GetEmployeeDetails(data.EMP_ManagerId);
                        if (Status == PerformanceEmployeeStatus.DisputeDeny)
                        {
                             Message = DarMessage.HRAcceptDisputeDeny(dataMAN.EMP_FirstName + " " + dataMAN.EMP_LastName);
                        }
                        else
                        {
                             Message = DarMessage.HRAcceptDisputeDeny(dataMAN.EMP_FirstName + " " + dataMAN.EMP_LastName);
                        }
                            var notificationEMP = _INotification.GetNotificationData(dataMAN.EMP_EmployeeID, dataMAN.EMP_EmployeeID, true, Message, ModuleSubModule.HRDisputeAppriasal, ModuleSubModule.Performance, dataMAN.EMP_EmployeeID, "H", ObjLoginModel.UserName);
                    }
                }
                return Json(true, JsonRequestBehavior.AllowGet);
            }
            catch(Exception ex)
            {
                return Json(false, JsonRequestBehavior.AllowGet);
            }
            //return Json(true, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// Created By : Ashwajit Bansod
        /// Created Date : 09-08-2020
        /// Created For : To generate PDF of evaluationa nd Assessment of employee
        /// </summary>
        /// <param name="data"></param>
        /// <param name="UserName"></param>
        /// <returns></returns>
        [HttpGet]
        public bool AssessmentEvaluationPDF(List<GWCQUestionModel> data, string UserName)
        {
            bool isPDFGenerate = false;
            var AssessmentEvaluationData = new AssessmentEvaluationModel();
            try
            {
                if(data.Count() > 0)
                {
                    var employeeId = data[0].SAR_EMP_EmployeeId;
                    AssessmentEvaluationData.GWCQUestionModelEvaluation = data;
                    AssessmentEvaluationData.GWCQUestionModelAssessment = _GlobalAdminManager.GetGWCQuestions(employeeId, data[0].AssessmentType, null);
                    var pdfpath = employeeId + "_Assessment.pdf";
                    var pdf = HtmlConvertToPdf("AssessmentEvaluationPDF", AssessmentEvaluationData, pdfpath, Convert.ToInt64(FileTypeId.PerformanceAppraisal), UserName, "Assessment File");
                    isPDFGenerate = true;
                }
            }
            catch(Exception ex)
            {
                ViewBag.Message = ex.Message;
                ViewBag.Class = "danger";
                isPDFGenerate = false;
            }
            return isPDFGenerate;
        }
        /// <summary>
        /// Created By : Ashwajit Bansod
        /// Created Date : 01-08-2020
        /// </summary>
        /// <param name="viewName"></param>
        /// <param name="model"></param>
        /// <param name="path"></param>
        /// <param name="FileId"></param>
        /// <param name="EmployeeId"></param>
        /// <param name="FileName"></param>
        /// <returns></returns>
        public async Task<bool> HtmlConvertToPdf(string viewName, object model, string path, long FileId, string EmployeeId, string FileName)
        {
            bool status = false;
            try
            {
                var pdf = new Rotativa.ViewAsPdf(viewName, model)
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