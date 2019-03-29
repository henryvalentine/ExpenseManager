using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using xPlug.BusinessObject;



namespace xPlug.BusinessObjectMapper
{
	/// Author:	 Oluwaseyi Adegboyega - sadegboyega@xplugng.com
	/// Copyright © 2014. xPlug Technologies Limited. All Rights Reserved
	///*******************************************************************************
	///* Date Created: 	04-06-2013
	///* Date Generated:	17-01-2014 02:29:15
	///*******************************************************************************


	public class ExpenseTransactionMapper
	{

		public ExpenseTransactionMapper()
		{
		}

		public static TR Map<T, TR>(T sourceObject) where T : class where TR : class 
		{
			if(sourceObject == null){return null;}
			Type myType = typeof (T);
			if (myType == typeof(ExpenseTransaction))
			{
				var objItem = new ExpenseManager.EF.ExpenseTransaction();
				var myItem = sourceObject as ExpenseTransaction;
				if(myItem == null){return null;};
				try
				{
					objItem.ExpenseTransactionId = myItem.ExpenseTransactionId;

					objItem.ExpenseTitle = myItem.ExpenseTitle;

					objItem.Description = myItem.Description;

					objItem.BeneficiaryId = myItem.BeneficiaryId;

					objItem.BeneficiaryTypeId = myItem.BeneficiaryTypeId;

					objItem.RegisteredById = myItem.RegisteredById;

					objItem.TransactionDate = myItem.TransactionDate;

					objItem.TransactionTime = myItem.TransactionTime;

					objItem.Status = myItem.Status;

					objItem.TotalTransactionAmount = myItem.TotalTransactionAmount;

					objItem.ApproverId = myItem.ApproverId;

					objItem.DateApproved = myItem.DateApproved;

					objItem.TimeApproved = myItem.TimeApproved;

					objItem.TotalApprovedAmount = myItem.TotalApprovedAmount;

					objItem.ApproverComment = myItem.ApproverComment;

				}
				catch(Exception ex)
				{
					return new ExpenseTransaction() as TR;
				}
				return objItem as TR;
			}
			if (myType == typeof(ExpenseManager.EF.ExpenseTransaction))
			{
				var objItem = new ExpenseTransaction();
				var myItem = sourceObject as ExpenseManager.EF.ExpenseTransaction;
				if(myItem == null){return null;};
				try
				{
					objItem.ExpenseTransactionId = myItem.ExpenseTransactionId;

					objItem.ExpenseTitle = myItem.ExpenseTitle;

					objItem.Description = myItem.Description;

					objItem.BeneficiaryId = myItem.BeneficiaryId;

					objItem.BeneficiaryTypeId = myItem.BeneficiaryTypeId;

					objItem.RegisteredById = myItem.RegisteredById;

					objItem.TransactionDate = myItem.TransactionDate;

					objItem.TransactionTime = myItem.TransactionTime;

					objItem.Status = myItem.Status;

					objItem.TotalTransactionAmount = myItem.TotalTransactionAmount;

					objItem.ApproverId = myItem.ApproverId;

					objItem.DateApproved = myItem.DateApproved;

					objItem.TimeApproved = myItem.TimeApproved;

					objItem.TotalApprovedAmount = myItem.TotalApprovedAmount;

					objItem.ApproverComment = myItem.ApproverComment;

					#region Included Tables
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
							objItem.BeneficiaryType = new BeneficiaryType();
							objItem.BeneficiaryType.BeneficiaryTypeId = myItem.BeneficiaryType.BeneficiaryTypeId;

							objItem.BeneficiaryType.Name = myItem.BeneficiaryType.Name;

							objItem.BeneficiaryType.Status = myItem.BeneficiaryType.Status;

						}
						catch{}
					#endregion
				}
				catch(Exception ex)
				{
					return new ExpenseTransaction() as TR;
				}
				return objItem as TR;
			}
		return null;
		}
	}


	//Class File Generated from Code<->Stripper 1.2.0.0 | All Rights Reserved
}
