﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WorkOrderEMS.Data.EntityModel;
using WorkOrderEMS.Data.Interfaces;
using WorkOrderEMS.Models.Employee;

namespace WorkOrderEMS.BusinessLogic
{
	public class GuestUserRepository : IGuestUser
	{
		private readonly workorderEMSEntities objworkorderEMSEntities;

		public GuestUserRepository()
		{
			objworkorderEMSEntities = new workorderEMSEntities();
		}
		public EmployeeVIewModel GetEmployee(long userId)
		{
			var employeeId = objworkorderEMSEntities.UserRegistrations.Where(x => x.UserId == userId).FirstOrDefault()?.EmployeeID;
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
					Image = x.EMP_Photo,
					Phone = x.EMP_Phone,
					SocialSecurityNumber = x.EMP_SSN,
					Zip = x.EMA_Zip
				}).FirstOrDefault(); ;
		}
		public bool UpdateApplicantInfo(EmployeeVIewModel onboardingDetailRequestModel)
		{

			try
			{

				using (workorderEMSEntities Context = new workorderEMSEntities())
				{
					var isEmployeeExists = Context.Employees.Where(x => x.EMP_Email == onboardingDetailRequestModel.Email).Any();

					var EMPAction = isEmployeeExists ? "U" : "I";
					var result = Context.spSetEmployee(EMPAction, null, onboardingDetailRequestModel.EmpId, null,
												onboardingDetailRequestModel.FirstName, onboardingDetailRequestModel.MiddleName, onboardingDetailRequestModel.LastName,
												onboardingDetailRequestModel.Email, onboardingDetailRequestModel.Phone
												, onboardingDetailRequestModel.DlNumber, onboardingDetailRequestModel.Dob, onboardingDetailRequestModel.SocialSecurityNumber,
												null, null, null, null, null,
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
		public DirectDepositeFormModel GetDirectDepositeDataByUserId(long UserId)
		{
			try
			{
				DirectDepositeFormModel result = new DirectDepositeFormModel();
				using (workorderEMSEntities Context = new workorderEMSEntities())
				{
					var EmployeeId = objworkorderEMSEntities.UserRegistrations.Where(x => x.UserId == UserId).FirstOrDefault()?.EmployeeID;
					result = Context.spGetDirectDepositForm(EmployeeId).Select(x => new DirectDepositeFormModel
					{
						Account1 = new AccountModel
						{
							Account = x.DDF_AccountNumber_1,
							AccountType = x.DDF_AccountType_1,
							BankRouting = x.DDF_BankRoutingNumber_1,
							DepositeAmount = x.DDF_PrcentageOrDollarAmount_1,
							EmployeeBankName = x.DDF_BankRoutingNumber_1

						},
						Account2 = new AccountModel
						{
							Account = x.DDF_AccountNumber_2,
							AccountType = x.DDF_AccountType_2,
							BankRouting = x.DDF_BankRoutingNumber_2,
							DepositeAmount = x.DDF_PrcentageOrDollarAmount_1,
							EmployeeBankName = x.DDF_BankRoutingNumber_2
						},
						EmployeeId = EmployeeId,
						PrintedName = x.EmployeeName,


					}).FirstOrDefault();
					result =result==null? new DirectDepositeFormModel():result;
					result.EmployeeId = EmployeeId;
					return result;
				}
			}
			catch (Exception ex)
			{
				throw ex;
			}
		}
		public DirectDepositeFormModel GetDirectDepositeDataByEmployeeId(string EmployeeId)
		{
			try
			{

				using (workorderEMSEntities Context = new workorderEMSEntities())
				{

					var result = Context.spGetDirectDepositForm(EmployeeId).Select(x => new DirectDepositeFormModel
					{
						Account1 = new AccountModel
						{
							Account = x.DDF_AccountNumber_1,
							AccountType = x.DDF_AccountType_1,
							BankRouting = x.DDF_BankRoutingNumber_1,
							DepositeAmount = x.DDF_PrcentageOrDollarAmount_1,
							EmployeeBankName = x.DDF_BankRoutingNumber_1

						},
						Account2 = new AccountModel
						{
							Account = x.DDF_AccountNumber_2,
							AccountType = x.DDF_AccountType_2,
							BankRouting = x.DDF_BankRoutingNumber_2,
							DepositeAmount = x.DDF_PrcentageOrDollarAmount_1,
							EmployeeBankName = x.DDF_BankRoutingNumber_2
						},
						EmployeeId = x.DDF_EMP_EmployeeID,
						PrintedName = x.EmployeeName,


					}).FirstOrDefault();

					return result;
				}
			}
			catch (Exception ex)
			{
				throw ex;
			}
		}
		public bool SetDirectDepositeFormData(DirectDepositeFormModel model, long UserId)
		{
			try
			{

				using (workorderEMSEntities Context = new workorderEMSEntities())
				{
					var EmployeeId = objworkorderEMSEntities.UserRegistrations.Where(x => x.UserId == UserId).FirstOrDefault()?.EmployeeID;
					var data = GetDirectDepositeDataByEmployeeId(EmployeeId);
					if (data != null)
						return Context.spSetDirectDepositForm("U", EmployeeId, model.Account1.EmployeeBankName, model.Account1.AccountType,
							model.Account1.Account, model.Account1.BankRouting, model.Account1.DepositeAmount, model.Account2.EmployeeBankName, model.Account2.AccountType, model.Account2.Account
							, model.Account2.BankRouting, model.VoidCheck, "Y") > 0 ? true : false;

					return Context.spSetDirectDepositForm("I", EmployeeId, model.Account1.EmployeeBankName, model.Account1.AccountType,
							model.Account1.Account, model.Account1.BankRouting, model.Account1.DepositeAmount.HasValue? model.Account1.DepositeAmount.Value:0, model.Account2.EmployeeBankName, model.Account2.AccountType, model.Account2.Account
							, model.Account2.BankRouting, model.VoidCheck, "Y") > 0 ? true : false;
				}
			}
			catch (Exception ex)
			{
				throw ex;
			}
		}
		public bool SetEmployeeHandbookData(EmployeeHandbookModel model, long UserId)
		{
			try
			{

				using (workorderEMSEntities Context = new workorderEMSEntities())
				{
					var data = GetEmployeeHandBookByUserId(UserId);
					var EmployeeId = objworkorderEMSEntities.UserRegistrations.Where(x => x.UserId == UserId).FirstOrDefault()?.EmployeeID;

					if (data != null)
						return Context.spSetEmployeeHandbook("U", model.EhbId, EmployeeId, model.IsActive) > 0 ? true : false;

					return Context.spSetEmployeeHandbook("I", model.EhbId, EmployeeId, model.IsActive) > 0 ? true : false;
				}
			}
			catch (Exception ex)
			{
				throw ex;
			}
		}
		public EmployeeHandbookModel GetEmployeeHandBookByUserId(long UserId)
		{
			try
			{

				using (workorderEMSEntities Context = new workorderEMSEntities())
				{
					var EmployeeId = objworkorderEMSEntities.UserRegistrations.Where(x => x.UserId == UserId).FirstOrDefault()?.EmployeeID;


					var result = Context.spGetEmployeeHandbook(EmployeeId).Select(x => new EmployeeHandbookModel
					{
						EmployeeName = x

					}).FirstOrDefault();

					return result;
				}
			}
			catch (Exception ex)
			{
				throw ex;
			}
		}
		public EmployeeHandbookModel GetEmployeeHandBookByEmployeeId(string EmployeeId)
		{
			try
			{

				using (workorderEMSEntities Context = new workorderEMSEntities())
				{

					var result = Context.spGetEmployeeHandbook(EmployeeId).Select(x => new EmployeeHandbookModel
					{
						EmployeeName = x

					}).FirstOrDefault();

					return result;
				}
			}
			catch (Exception ex)
			{
				throw ex;
			}
		}
		public string GetEmployeeId(long userId)
		{

			try
			{

				using (workorderEMSEntities Context = new workorderEMSEntities())
				{

					var result = Context.UserRegistrations.Where(x => x.UserId == userId)?.FirstOrDefault().EmployeeID;

					return result;
				}
			}
			catch (Exception ex)
			{
				throw ex;
			}
		}
	}


}
