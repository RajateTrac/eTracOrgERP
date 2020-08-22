using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WorkOrderEMS.Models.NewAdminModel;

namespace WorkOrderEMS.BusinessLogic.Interfaces
{
    public interface IHRManager
    {
        List<PerformanceModel> GetPerformanceListForHR(long? locationId, string UserId, string UserType);
    }
}
