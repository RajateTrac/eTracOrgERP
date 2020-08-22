using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.UI.WebControls;
using WorkOrderEMS.Models;

namespace WorkOrderEMS.BusinessLogic
{ 
    public interface IFillableFormManager
    {
       CommonFormModel GetFormDetails(CommonFormModel Obj);
        List<FormTypeListModel> GetFileList(eTracLoginModel obj);
        bool SaveFile(UploadedFiles Obj, string EmployeeId);
        bool SaveFileList(CommonFormModel obj);
        WorkOrderEMS.Data.EntityModel.FileUpload GetFileDataByEmployeeId(string EmployeeId, string FileName);
    }
}
