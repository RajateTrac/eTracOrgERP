using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WorkOrderEMS.Models;
using WorkOrderEMS.Models.Employee;
using WorkOrderEMS.Models.NewAdminModel.FormsModel;

namespace WorkOrderEMS.Data.Interfaces
{
    public interface IGuestUserRepository
    {
        EmployeeVIewModel GetEmployee(long UserId);
        bool UpdateApplicantInfo(EmployeeVIewModel onboardingDetailRequestModel);
        bool UpdateApplicantInfoEMPMangemnt(EmployeeVIewModel onboardingDetailRequestModel);
        EmployeeVIewModel GetEmployeeDetails(string employeeId);
        //EmployeeVIewModel GetEmployeeDetails(string employeeId);
        EmployeeVIewModel GetEmployeeDetails(long userId);
        FormNameStatus GetFormStatusModel(W4FormModel obj);
        FormNameStatus GetFormStatusModelI9(I9FormModel obj);

    }
}
