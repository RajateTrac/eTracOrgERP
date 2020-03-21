using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WorkOrderEMS.BusinessLogic;
using WorkOrderEMS.Models;

namespace WorkOrderEMS.Controllers.Notification
{
    public class NotificationController : Controller
    {
        // GET: /SuperAdmin/
        private readonly ICommonMethod _ICommonMethod;
        private readonly IGlobalAdmin _IGlobalAdmin;
        private readonly ILogin _ILogin;
        private readonly IUser _IUser;


        public NotificationController(ICommonMethod _ICommonMethod, IGlobalAdmin _IGlobalAdmin, IUser _IUser, ILogin _ILogin)
        {
            this._ICommonMethod = _ICommonMethod;
            this._IGlobalAdmin = _IGlobalAdmin;
            this._IUser = _IUser;
            this._ILogin = _ILogin;
        }
        public NotificationController()
        {
        }
        // GET: Notification
        public ActionResult GetNotifications()
        {
            eTracLoginModel ObjLoginModel = null;
            long UserId = 0;
            if (Session["eTrac"] != null)
            {
                ObjLoginModel = (eTracLoginModel)(Session["eTrac"]);
                UserId = ObjLoginModel.UserId;
            }
            return PartialView("GetNotifications", _ICommonMethod.GetNotifications(ObjLoginModel.UserName));
        }
    }
}