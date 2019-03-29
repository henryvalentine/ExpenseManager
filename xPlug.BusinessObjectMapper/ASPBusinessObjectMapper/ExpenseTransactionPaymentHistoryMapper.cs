using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using xPlug.BusinessObject;



namespace xPlug.BusinessObjectMapper
{
	/// Author:	 Oluwaseyi Adegboyega - sadegboyega@xplugng.com
	/// Copyright © 2013. xPlug Technologies Limited. All Rights Reserved
	///*******************************************************************************
	///* Date Created: 	04-06-2013
	///* Date Generated:	01-12-2013 03:58:06
	///*******************************************************************************


	public class ExpenseTransactionPaymentHistoryMapper
	{

		public ExpenseTransactionPaymentHistoryMapper()
		{
		}

		public static TR Map<T, TR>(T sourceObject) where T : class where TR : class 
		{
			if(sourceObject == null){return null;}
			Type myType = typeof (T);
			if (myType == typeof(ExpenseTransactionPaymentHistory))
			{
				var objItem = new ExpenseManager.EF.ExpenseTransactionPaymentHistory();
				var myItem = sourceObject as ExpenseTransactionPaymentHistory;
				if(myItem == null){return null;};
				try
				{
					objItem.ExpenseTransactionPaymentHistoryId = myItem.ExpenseTransactionPaymentHistoryId;

					objItem.ExpenseTransactionId = myItem.ExpenseTransactionId;

					objItem.AmountPaid = myItem.AmountPaid;

					objItem.PaymentDate = myItem.PaymentDate;

					objItem.PaymentTime = myItem.PaymentTime;

					objItem.PaidById = myItem.PaidById;

					objItem.Comment = myItem.Comment;

					objItem.Status = myItem.Status;

					objItem.ExpenseTransactionPaymentId = myItem.ExpenseTransactionPaymentId;

					objItem.PaymentModeId = myItem.PaymentModeId;

					objItem.BeneficiaryId = myItem.BeneficiaryId;

				}
				catch(Exception ex)
				{
					return new ExpenseTransactionPaymentHistory() as TR;
				}
				return objItem as TR;
			}
			if (myType == typeof(ExpenseManager.EF.ExpenseTransactionPaymentHistory))
			{
				var objItem = new ExpenseTransactionPaymentHistory();
				var myItem = sourceObject as ExpenseManager.EF.ExpenseTransactionPaymentHistory;
				if(myItem == null){return null;};
				try
				{
					objItem.ExpenseTransactionPaymentHistoryId = myItem.ExpenseTransactionPaymentHistoryId;

					objItem.ExpenseTransactionId = myItem.ExpenseTransactionId;

					objItem.AmountPaid = myItem.AmountPaid;

					objItem.PaymentDate = myItem.PaymentDate;

					objItem.PaymentTime = myItem.PaymentTime;

					objItem.PaidById = myItem.PaidById;

					objItem.Comment = myItem.Comment;

					objItem.Status = myItem.Status;

					objItem.ExpenseTransactionPaymentId = myItem.ExpenseTransactionPaymentId;

					objItem.PaymentModeId = myItem.PaymentModeId;

					objItem.BeneficiaryId = myItem.BeneficiaryId;

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
							objItem.ExpenseTransactionPayment = new ExpenseTransactionPayment();
							objItem.ExpenseTransactionPayment.ExpenseTransactionPaymentId = myItem.ExpenseTransactionPayment.ExpenseTransactionPaymentId;

							objItem.ExpenseTransactionPayment.ExpenseTransactionId = myItem.ExpenseTransactionPayment.ExpenseTransactionId;

							objItem.ExpenseTransactionPayment.TotalAmountPayable = myItem.ExpenseTransactionPayment.TotalAmountPayable;

							objItem.ExpenseTransactionPayment.Balance = myItem.ExpenseTransactionPayment.Balance;

							objItem.ExpenseTransactionPayment.LastPaymentDate = myItem.ExpenseTransactionPayment.LastPaymentDate;

							objItem.ExpenseTransactionPayment.LastPaymentTime = myItem.ExpenseTransactionPayment.LastPaymentTime;

							objItem.ExpenseTransactionPayment.Status = myItem.ExpenseTransactionPayment.Status;

							objItem.ExpenseTransactionPayment.AmountPaid = myItem.ExpenseTransactionPayment.AmountPaid;

							objItem.ExpenseTransactionPayment.BeneficiaryId = myItem.ExpenseTransactionPayment.BeneficiaryId;

                            objItem.ExpenseTransactionPayment.DepartmentId = myItem.ExpenseTransactionPayment.DepartmentId;
						}
						catch{}
						try
						{
							objItem.PaymentMode = new PaymentMode();
							objItem.PaymentMode.PaymentModeId = myItem.PaymentMode.PaymentModeId;

							objItem.PaymentMode.Name = myItem.PaymentMode.Name;

							objItem.PaymentMode.Status = myItem.PaymentMode.Status;

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
					#endregion
				}
				catch(Exception ex)
				{
					return new ExpenseTransactionPaymentHistory() as TR;
				}
				return objItem as TR;
			}
		return null;
		}
	}


	//Class File Generated from Code<->Stripper 1.2.0.0 | All Rights Reserved
}
