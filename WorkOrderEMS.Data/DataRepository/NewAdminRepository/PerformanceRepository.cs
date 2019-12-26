using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using WorkOrderEMS.Data.EntityModel;
using WorkOrderEMS.Models.NewAdminModel;
using System.Web;
using System.Data.Entity.Core.Objects;

namespace WorkOrderEMS.Data.DataRepository.NewAdminRepository
{
    public class PerformanceRepository
    {
        workorderEMSEntities objworkorderEMSEntities = new workorderEMSEntities();
        /// <summary>
        /// SaveMeetingDetails
        /// </summary>
        /// <param name="objSetupMeeting"></param>
        /// <returns></returns>
        public bool SaveMeetingDetails(SetupMeeting objSetupMeeting)
        {
            try
            {
                DateTime dt = DateTimeOffset.Parse(objSetupMeeting.StartDate + " " + objSetupMeeting.StartTime).UtcDateTime;
                string _datetime = dt.ToString();
                //objworkorderEMSEntities.spSetReviewMeetingDateTime("I", null, null, objSetupMeeting.ReceipientEmailId, objSetupMeeting.FinYear, objSetupMeeting.FinQrtr, _datetime);

            }
            catch (Exception)
            { throw; }
            return true;

        }
        public SetupMeeting GetMeetingDetail(string Id, string FinYear, string FinQuarter)
        {
            SetupMeeting result;
            try
            {
                result = objworkorderEMSEntities.spGetReviewMeetingDateTime(Id, FinYear, FinQuarter).Select(x => new SetupMeeting()
                {
                    RMS_Id = x.RMS_Id,
                    ReceipientEmailId = x.RMS_EMP_EmployeeId,
                    FinYear = x.RMS_FinencialYear,
                    FinQrtr = x.RMS_FinQuarter,
                    RMS_InterviewDateTime = x.RMS_InterviewDateTime,
                    RMS_Date = x.RMS_Date,
                    RMS_IsActive = x.RMS_IsActive
                }).SingleOrDefault();

            }
            catch (Exception)
            {
                result = null;
            }
            return result;
        }
        /// <summary>
        /// Get Meeting List For HR
        /// </summary>
        /// <returns></returns>
        public List<ReviewMeeting> GetMeetingList()
        {
            try
            {
                return objworkorderEMSEntities.spGetReviewMeetingList().Select(x => new ReviewMeeting()
                {
                    EmployeeName = x.EmployeeName,
                    EMP_EmployeeID = x.EMP_EmployeeID,
                    EMP_ManagerId = x.EMP_ManagerId,
                    ManagerPhoto = x.ManagerPhoto,
                    ManagerName = x.ManagerName,
                    EmployeePhoto = x.EmployeePhoto,
                    PRMeetingDateTime = x.PRMeetingDateTime.HasValue ? new DateTimeOffset(x.PRMeetingDateTime.Value, TimeSpan.FromHours(0)).ToLocalTime().DateTime : (DateTime?)null
                }).ToList();
            }
            catch (Exception)
            {
                throw;
            }
        }

        public List<EventModel> GetMyEvents(string UserName,string start,string end)
        {
            List<EventModel> result = new List<EventModel>();
            try
            {
                var fromDate = Convert.ToDateTime(start);
                var toDate = Convert.ToDateTime(end);
                using (objworkorderEMSEntities=new workorderEMSEntities())
                {
                    var rslt = objworkorderEMSEntities.Appointments.Where(s => s.DateTimeScheduled >= fromDate && EntityFunctions.AddMinutes(s.DateTimeScheduled, s.AppointmentLength) <= toDate);
                    foreach (var item in rslt)
                    {
                        EventModel rec = new EventModel();
                        rec.id = item.ID;
                       // rec.SomeImportantKey = item.SomeImportantKey;
                        rec.start = item.DateTimeScheduled.ToString("s"); // "s" is a preset format that outputs as: "2009-02-27T12:12:22"
                        rec.end = item.DateTimeScheduled.AddMinutes(item.AppointmentLength).ToString("s"); // field AppointmentLength is in minutes
                        rec.title = item.Title + " - " + item.AppointmentLength.ToString() + " mins";
                        //rec.StatusString = Enums.GetName<AppointmentStatus>((AppointmentStatus)item.StatusENUM);
                        //rec.StatusColor = Enums.GetEnumDescription<AppointmentStatus>(rec.StatusString);
                        rec.StatusString = "#FF8000:BOOKED";
                        rec.StatusColor = "#FF8000:BOOKED";
                        string ColorCode = rec.StatusColor.Substring(0, rec.StatusColor.IndexOf(":"));
                        rec.ClassName = rec.StatusColor.Substring(rec.StatusColor.IndexOf(":") + 1, rec.StatusColor.Length - ColorCode.Length - 1);
                        rec.StatusColor = ColorCode;
                        result.Add(rec);
                    }
                }
            }
            catch (Exception)
            {
                throw;
            }
            return result;
        }
        private static DateTime ConvertFromUnixTimestamp(double timestamp)
        {
            var origin = new DateTime(1970, 1, 1, 0, 0, 0, 0);
            return origin.AddSeconds(timestamp);
        }
        public bool CreateNewEvent(string Title, string NewEventDate, string NewEventTime, string NewEventDuration)
        {
            bool result = false;
            Appointment obj = new Appointment();
            try
            {
                obj.Title = Title;
                obj.DateTimeScheduled = DateTime.ParseExact(NewEventDate + " " + NewEventTime, "dd/MM/yyyy HH:mm", CultureInfo.InvariantCulture);
                obj.AppointmentLength = Int32.Parse(NewEventDuration);
                objworkorderEMSEntities.Appointments.Add(obj);
                objworkorderEMSEntities.SaveChanges();
                result = true;

            }
            catch (Exception)
            {

                throw;
            }
            return result;
        }

    }
}
