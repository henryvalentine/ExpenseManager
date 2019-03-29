using System;
using xPlug.BusinessObject;

namespace xPlug.BusinessObjectMapper
{
	/// Author:	 Oluwaseyi Adegboyega - sadegboyega@xplugng.com
	/// Copyright © 2013. xPlug Technologies Limited. All Rights Reserved
	///*******************************************************************************
	///* Date Created: 	04-06-2013
	///* Date Generated:	28-11-2013 04:15:14
	///*******************************************************************************


	public class ExpenseTransactionPaymentMapper
	{

		public ExpenseTransactionPaymentMapper()
		{}

		public static TR Map<T, TR>(T sourceObject) where T : class where TR : class 
		{
			if(sourceObject == null){return null;}
			Type myType = typeof (T);
			if (myType == typeof(ExpenseTransactionPayment))
			{
				var objItem = new ExpenseManager.EF.ExpenseTransactionPayment();
				var myItem = sourceObject as ExpenseTransactionPayment;
				if(myItem == null){return null;};
				try
				{
					objItem.ExpenseTransactionPaymentId = myItem.ExpenseTransactionPaymentId;

					objItem.ExpenseTransactionId = myItem.ExpenseTransactionId;

					objItem.TotalAmountPayable = myItem.TotalAmountPayable;

					objItem.Balance = myItem.Balance;

					objItem.LastPaymentDate = myItem.LastPaymentDate;

					objItem.LastPaymentTime = myItem.LastPaymentTime;

					objItem.Status = myItem.Status;

					objItem.AmountPaid = myItem.AmountPaid;

					objItem.BeneficiaryId = myItem.BeneficiaryId;

                    objItem.DepartmentId = myItem.DepartmentId;

				}
				catch(Exception ex)
				{
					return new ExpenseTransactionPayment() as TR;
				}
				return objItem as TR;
			}
			if (myType == typeof(ExpenseManager.EF.ExpenseTransactionPayment))
			{
				var objItem = new ExpenseTransactionPayment();
				var myItem = sourceObject as ExpenseManager.EF.ExpenseTransactionPayment;
				if(myItem == null){return null;};
				try
				{
					objItem.ExpenseTransactionPaymentId = myItem.ExpenseTransactionPaymentId;

					objItem.ExpenseTransactionId = myItem.ExpenseTransactionId;

					objItem.TotalAmountPayable = myItem.TotalAmountPayable;

					objItem.Balance = myItem.Balance;

					objItem.LastPaymentDate = myItem.LastPaymentDate;

					objItem.LastPaymentTime = myItem.LastPaymentTime;

					objItem.Status = myItem.Status;

					objItem.AmountPaid = myItem.AmountPaid;

					objItem.BeneficiaryId = myItem.BeneficiaryId;

                    objItem.DepartmentId = myItem.DepartmentId;

					#region Included Tables
						try
						{
							objItem.ExpenseTransaction = new ExpenseTransaction();
							objItem.ExpenseTransaction.ExpenseTransactionId = myItem.ExpenseTransaction.ExpenseTransactionId;

							objItem.ExpenseTransaction.ExpenseTitle = myItem.ExpenseTransaction.ExpenseTitle;

							objItem.ExpenseTransaction.Description = myItem.ExpenseTransaction.Description;

							objItem.ExpenseTransaction.BeneficiaryId = myItem.ExpenseTransaction.BeneficiaryId;

							objItem.ExpenseTransaction.BeneficiaryTypeId = myItem.ExpenseTransaction.BeneficiaryTypeId;

							objItem.ExpenseTransaction.RegisteredById = myItem.ExpenseTransaction.RegisteredById;

							objItem.ExpenseTransaction.TransactionDate = myItem.ExpenseTransaction.TransactionDate;

							objItem.ExpenseTransaction.TransactionTime = myItem.ExpenseTransaction.TransactionTime;

							objItem.ExpenseTransaction.Status = myItem.ExpenseTransaction.Status;

							objItem.ExpenseTransaction.TotalTransactionAmount = myItem.ExpenseTransaction.TotalTransactionAmount;

							objItem.ExpenseTransaction.ApproverId = myItem.ExpenseTransaction.ApproverId;

							objItem.ExpenseTransaction.DateApproved = myItem.ExpenseTransaction.DateApproved;

							objItem.ExpenseTransaction.TimeApproved = myItem.ExpenseTransaction.TimeApproved;

							objItem.ExpenseTransaction.TotalApprovedAmount = myItem.ExpenseTransaction.TotalApprovedAmount;

							objItem.ExpenseTransaction.ApproverComment = myItem.ExpenseTransaction.ApproverComment;

						}
						catch{}
						try
						{
							objItem.Beneficiary = new Beneficiary();
							objItem.Beneficiary.BeneficiaryId = myItem.Beneficiary.BeneficiaryId;

							objItem.Beneficiary.FullName = myItem.Beneficiary.FullName;

							objItem.Beneficiary.GSMNO2 = myItem.Beneficiary.GSMNO2;

							objItem.Beneficiary.GSMNO1 = myItem.Beneficiary.GSMNO1;

							objItem.Beneficiary.DateRegistered = myItem.Beneficiary.DateRegistered;

							objItem.Beneficiary.TimeRegistered = myItem.Beneficiary.TimeRegistered;

							objItem.Beneficiary.Sex = myItem.Beneficiary.Sex;

							objItem.Beneficiary.Email = myItem.Beneficiary.Email;

							objItem.Beneficiary.Status = myItem.Beneficiary.Status;

							objItem.Beneficiary.CompanyName = myItem.Beneficiary.CompanyName;

							objItem.Beneficiary.DepartmentId = myItem.Beneficiary.DepartmentId;

							objItem.Beneficiary.UnitId = myItem.Beneficiary.UnitId;

							objItem.Beneficiary.BeneficiaryTypeId = myItem.Beneficiary.BeneficiaryTypeId;

						}
						catch{}
                        try
                        {
                            objItem.Department = new BusinessObject.Department();

                            objItem.Department.DepartmentId = myItem.Department.DepartmentId;

                            objItem.Department.Name = myItem.Department.Name;

                            objItem.Department.Status = myItem.Department.Status;
                        }
                        catch { }
					#endregion
				}
				catch(Exception ex)
				{
					return new ExpenseTransactionPayment() as TR;
				}
				return objItem as TR;
			}
		return null;
		}
	}


	//Class File Generated from Code<->Stripper 1.2.0.0 | All Rights Reserved
}
