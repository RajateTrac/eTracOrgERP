using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WorkOrderEMS.BusinessLogic.Interfaces;
using WorkOrderEMS.Data.EntityModel;
using WorkOrderEMS.Models.NewAdminModel;

namespace WorkOrderEMS.BusinessLogic.Managers
{
    public class HRManager : IHRManager
    {
        workorderEMSEntities _db = new workorderEMSEntities();
        public List<PerformanceModel> GetPerformanceListForHR(long? locationId, string UserId, string UserType)
        {
            var lstPerformanceHR = new List<PerformanceModel>();
            try
            {
                lstPerformanceHR = _db.spGetAssessmentListForHR(UserId).Select(t =>
                                new PerformanceModel()
                                {
                                    EMP_EmployeeID = t.EMP_EmployeeID,
                                    EmployeeName = t.EmployeeName,
                                    EMP_Photo = t.EMP_Photo,
                                    DepartmentName = t.DepartmentName,
                                    JBT_JobTitle = t.JBT_JobTitle,
                                    LocationName = t.LocationName,
                                    EMP_DateOfJoining = t.EMP_DateOfJoining,
                                    AssessmentType  = t.Assesment
                                   //l = t.LocationName
                                }).ToList();
                return lstPerformanceHR;
            }
            catch (Exception ex)
            {
                Exception_B.Exception_B.exceptionHandel_Runtime(ex, "public List<PerformanceModel> GetPerformanceListForHR(long? locationId, string UserId, string UserType)", "Exception while getting the list of performance For HR", UserId);
                throw;
            }

        }
    }
}
