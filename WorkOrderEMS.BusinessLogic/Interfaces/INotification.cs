using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WorkOrderEMS.Helper;
using WorkOrderEMS.Models;

namespace WorkOrderEMS.BusinessLogic
{
    public interface INotification
    {
        List<EmailHelper> GetEmaintanaceUnseenList(NotificationDetailModel objDetails);
        List<NotificationDetailModel> GetNotification(long userId, string Username);
        bool ReadNotificationById(long NotificationId);
        ApplicantDetails GetApplicantDetails(long ApplicantId);
        bool SaveNotification(NotificationDetailModel obj);
        NotificationDetailModel NotificationDetailsforMeetingDateTime(NotificationDetailModel obj);
        bool ScheduleAppriasalMeetingNotification(string EmployeeId, string ManagerId, string Assessmnt);
        bool UnlockEmployee(string EmployeeId, string ManagerId);
        NotificationDetailModel GetNotificationData(string EmployeeId, string AssignedId, bool AssignStatus, string Message, string SubModule, string Module, string SubModuleId, string Priority, string UserName);
    }
}
