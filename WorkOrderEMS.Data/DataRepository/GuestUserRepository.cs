using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WorkOrderEMS.Data.EntityModel;
using WorkOrderEMS.Data.Interfaces;
using WorkOrderEMS.Models;
using WorkOrderEMS.Models.Employee;
using WorkOrderEMS.Models.NewAdminModel.FormsModel;

namespace WorkOrderEMS.Data.DataRepository
{
    public class GuestUserRepositoryData : IGuestUserRepository
    {
        private readonly string HostingPrefix = Convert.ToString(System.Configuration.ConfigurationManager.AppSettings["hostingPrefix"], CultureInfo.InvariantCulture);
        private readonly string ProfilePicPath = System.Configuration.ConfigurationManager.AppSettings["ProfilePicPath"];
        private readonly workorderEMSEntities objworkorderEMSEntities;
        public GuestUserRepositoryData()
        {
            objworkorderEMSEntities = new workorderEMSEntities();
        }
        public EmployeeVIewModel GetEmployee(long userId)
        {

            var employeeId = objworkorderEMSEntities.UserRegistrations.Where(x => x.UserId == userId).FirstOrDefault()?.EmployeeID;
             
                var getDetailsOfApplicant = objworkorderEMSEntities.spGetApplicantAllDetails(userId).//spGetEmployeePersonalInfo(employeeId).
                Select(x => new EmployeeVIewModel
                {
                    //Address = x.ad,
                    //City = x.ci,
                   // State = x.AAD_State,
                    //Cityzenship = x.c,
                    //DlNumber = x.dl,
                    //Dob = x.Da,
                    //Email = x.E,
                    //EmpId = x.ABH_StillEmployed,
                    FirstName = x.API_FirstName,
                    LastName = x.API_LastName,
                    MiddleName = x.API_MidName,
                    //Image = x.Ph,
                    Phone = x.ACI_PhoneNo,
                    //SocialSecurityNumber = x.ss,
                    //Zip = x.AAD_ziZip,
                    //LicenseNumber = x.ALH_LicenceNumber,
                    ApplicantId = Convert.ToInt64(userId)
                }).FirstOrDefault();
            return getDetailsOfApplicant;
        }
        /// <summary>
        /// Created BY : Ashwajit Bansod
        /// Created Date : 25-05-2020
        /// Created For : To get all employee details for update.
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public EmployeeVIewModel GetEmployeeDetails(long userId)
        {

            var employeeId = objworkorderEMSEntities.UserRegistrations.Where(x => x.UserId == userId).FirstOrDefault()?.EmployeeID;

            var getDetailsOfApplicant = objworkorderEMSEntities.spGetEmployeePersonalInfo(employeeId).//spGetEmployeePersonalInfo(employeeId).
            Select(x => new EmployeeVIewModel
            {
                Address = x.EMA_Address,
                City = x.EMA_City,
                State = x.EMA_State,
                Cityzenship = x.CTZ_Citizenship,
                DlNumber = x.EMP_DrivingLicenseNumber,
                Dob = x.EMP_DateOfBirth,
                Email = x.EMP_Email,
                EmpId = x.EMP_EmployeeID,
                FirstName = x.EMP_FirstName,
                LastName = x.EMP_LastName,
                MiddleName = x.EMP_MiddleName,
                Image = x.EMP_Photo,
                Phone = x.EMP_Phone,
                SocialSecurityNumber = x.EMP_SSN,
                Zip = x.EMA_Zip,
                //LicenseNumber = x.ALH_LicenceNumber,
                ApplicantId = Convert.ToInt64(x.EMP_API_ApplicantId)
            }).FirstOrDefault();
            return getDetailsOfApplicant;
        }
        public bool UpdateApplicantInfo(EmployeeVIewModel onboardingDetailRequestModel)
        {
            try
            {
                using (workorderEMSEntities Context = new workorderEMSEntities())
                {
                    var isEmployeeExists = Context.Employees.Where(x => x.EMP_Email == onboardingDetailRequestModel.Email).Any();
                    var getDetails = Context.Employees.Where(x => x.EMP_Email == onboardingDetailRequestModel.Email).FirstOrDefault();
                    var Image = getDetails == null ? null : getDetails.EMP_Photo;
                    var EMPAction = isEmployeeExists ? "U" : "I";
                    var result = Context.spSetEmployee(EMPAction, null, onboardingDetailRequestModel.EmpId, null,
                                                onboardingDetailRequestModel.FirstName, onboardingDetailRequestModel.MiddleName, onboardingDetailRequestModel.LastName,
                                                onboardingDetailRequestModel.Email, onboardingDetailRequestModel.Phone
                                                , onboardingDetailRequestModel.DlNumber, onboardingDetailRequestModel.Dob, onboardingDetailRequestModel.SocialSecurityNumber,
                                                Image, null, null, null, null,
                                                null, null, null, DateTime.Now, "1", null, onboardingDetailRequestModel.Address,
                                                onboardingDetailRequestModel.City, onboardingDetailRequestModel.State, null, onboardingDetailRequestModel.Cityzenship).ToList();

                    if (result.Any())
                        return true;
                    return false;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public bool UpdateApplicantInfoEMPMangemnt(EmployeeVIewModel onboardingDetailRequestModel)
        {
            bool isUpdate = false;
            try
            {
                using (workorderEMSEntities Context = new workorderEMSEntities())
                {
                    //var isEmployeeExists = Context.Employees.Where(x => x.EMP_Email == onboardingDetailRequestModel.Email).Any();
                    //var getDetails = Context.Employees.Where(x => x.EMP_Email == onboardingDetailRequestModel.Email).FirstOrDefault();
                    var getDetails = Context.spGetEmployeePersonalInfo(onboardingDetailRequestModel.EmpId).FirstOrDefault();
                    var UserDetails = Context.UserRegistrations.Where(x => x.EmployeeID == onboardingDetailRequestModel.EmpId).FirstOrDefault();
                    //var Image = getDetails == null ? null : getDetails.EMP_Photo;
                    var EMPAction = "U";
                    if(onboardingDetailRequestModel.Image != null && onboardingDetailRequestModel.ImagePath != null)
                    {                       
                        System.IO.File.Delete(onboardingDetailRequestModel.ImagePath);
                    }
                    if (UserDetails != null && getDetails != null)
                    {
                        var result = Context.spSetEmployee(EMPAction, null, onboardingDetailRequestModel.EmpId, getDetails.EMP_API_ApplicantId,
                                                    onboardingDetailRequestModel.FirstName, onboardingDetailRequestModel.MiddleName, onboardingDetailRequestModel.LastName,
                                                    onboardingDetailRequestModel.Email, onboardingDetailRequestModel.Phone
                                                    , onboardingDetailRequestModel.DlNumber, onboardingDetailRequestModel.Dob, onboardingDetailRequestModel.SocialSecurityNumber,
                                                    onboardingDetailRequestModel.Image, null, Convert.ToInt64(getDetails.EMP_Gender), getDetails.EMP_JobTitleId, getDetails.EMP_ManagerId,
                                                    getDetails.EMP_DateOfJoining, getDetails.EMP_LocationId, onboardingDetailRequestModel.CreatedBy, DateTime.Now, "1", UserDetails.UserType, onboardingDetailRequestModel.Address,
                                                    onboardingDetailRequestModel.City, onboardingDetailRequestModel.State, onboardingDetailRequestModel.Zip, onboardingDetailRequestModel.Cityzenship).ToList();
                        if (result.Any())
                            isUpdate =  true;
                        isUpdate = false;
                    }

                    return isUpdate;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public EmployeeVIewModel GetEmployeeDetails(string employeeId)
        {            
            return objworkorderEMSEntities.spGetEmployeePersonalInfo(employeeId).
                Select(x => new EmployeeVIewModel
                {
                    Address = x.EMA_Address,
                    City = x.EMA_City,
                    State = x.EMA_State,
                    Cityzenship = x.CTZ_Citizenship,
                    DlNumber = x.EMP_DrivingLicenseNumber,
                    Dob = x.EMP_DateOfBirth,
                    Email = x.EMP_Email,
                    EmpId = x.EMP_EmployeeID,
                    FirstName = x.EMP_FirstName,
                    LastName = x.EMP_LastName,
                    MiddleName = x.EMP_MiddleName,
                    Image = x.EMP_Photo == null ? HostingPrefix + ProfilePicPath.Replace("~", "") + "no-profile-pic.jpg" : HostingPrefix + ProfilePicPath.Replace("~", "") + x.EMP_Photo,
                    Phone = x.EMP_Phone,
                    SocialSecurityNumber = x.EMP_SSN,
                    Zip = x.EMA_Zip,
                    LicenseNumber = x.EMP_DrivingLicenseNumber
                }).FirstOrDefault(); ;
        }


        public List<EducationVarificationModel> GetEducationVarificationForm(long userId, long ApplicantId, DirectDepositeFormModel model)
        {
            try
            {
                using (workorderEMSEntities Context = new workorderEMSEntities())
                {
                    var empid = Context.UserRegistrations.Where(x => x.UserId == userId)?.FirstOrDefault().EmployeeID;
                    var name = Context.spGetEducationVerificationForm(empid).Select(x => new EducationVarificationModel
                    {
                        EmpId = empid,
                        FormStatusw4 = model.FormStatusw4,
                        FormStatusbcf = model.FormStatusbcf,
                        FormStatusdd = model.FormStatusdd,
                        FormStatusEvf = model.FormStatusEvf,
                        FormStatusI9 = model.FormStatusI9,
                        FormStatusprfcaf = model.FormStatusprfcaf,
                        FormStatusprfecf = model.FormStatusprfecf,
                        FormStatusrop = model.FormStatusrop,
                        FormStatussif = model.FormStatussif,
                        FormStatusprf = model.FormStatusprf,
                        FormStatusff = model.FormStatusff,

                        formName = "educationverificationform",
                        HighSchool = new Education
                        {
                            AttendFrom = x.EVF_AttendedFrom,
                            AttendTo = x.EVF_AttendedTo,
                            City = x.EVF_City,
                            SchoolName = x.EVF_OrgnizationName,
                            State = x.EVF_State,
                            Cretificate = x.EVF_SchoolDegreeDiplomaCirtificate,
                        },
                        Name = x.EmployeeName,
                        EvfId = x.EVF_Id
                    }).ToList();
                    return name;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        #region Form Status
        public FormNameStatus GetFormStatusModel(W4FormModel obj)
        {
            var FormObj = new FormNameStatus
            {
                FormStatusw4 = obj.FormStatusw4,
                FormStatusI9 = obj.FormStatusI9,
                FormStatusdd = obj.FormStatusdd,
                FormStatusEvf = obj.FormStatusEvf,
                FormStatussif = obj.FormStatussif,
                FormStatusbcf = obj.FormStatusbcf,
                FormStatusrop = obj.FormStatusrop,
                FormStatusprfecf = obj.FormStatusprfecf,
                FormStatusprfcaf = obj.FormStatusprfcaf,
                FormStatusprf = obj.FormStatusprf,
                FormStatusff = obj.FormStatusff

            };
            return FormObj;
        }
        public FormNameStatus GetFormStatusModelI9(I9FormModel obj)
        {
            var FormObj = new FormNameStatus
            {
                FormStatusw4 = obj.FormStatusw4,
                FormStatusI9 = obj.FormStatusI9,
                FormStatusdd = obj.FormStatusdd,
                FormStatusEvf = obj.FormStatusEvf,
                FormStatussif = obj.FormStatussif,
                FormStatusbcf = obj.FormStatusbcf,
                FormStatusrop = obj.FormStatusrop,
                FormStatusprfecf = obj.FormStatusprfecf,
                FormStatusprfcaf = obj.FormStatusprfcaf,
                FormStatusprf = obj.FormStatusprf,
                FormStatusff = obj.FormStatusff

            };
            return FormObj;
        }
        #endregion Form Status
    }

}
