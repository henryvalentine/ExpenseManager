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
	///* Date Generated:	17-01-2014 02:29:16
	///*******************************************************************************


	public class TransactionItemMapper
	{

		public TransactionItemMapper()
		{
		}

		public static TR Map<T, TR>(T sourceObject) where T : class where TR : class 
		{
			if(sourceObject == null){return null;}
			Type myType = typeof (T);
			if (myType == typeof(TransactionItem))
			{
				var objItem = new ExpenseManager.EF.TransactionItem();
				var myItem = sourceObject as TransactionItem;
				if(myItem == null){return null;};
				try
				{
					objItem.TransactionItemId = myItem.TransactionItemId;

					objItem.ExpensenseItemId = myItem.ExpensenseItemId;

					objItem.RequestedQuantity = myItem.RequestedQuantity;

					objItem.RequestedUnitPrice = myItem.RequestedUnitPrice;

					objItem.ExpenseTransactionId = myItem.ExpenseTransactionId;

					objItem.ExpenseCategoryId = myItem.ExpenseCategoryId;

					objItem.ExpenseTypeId = myItem.ExpenseTypeId;

					objItem.ApprovedQuantity = myItem.ApprovedQuantity;

					objItem.ApprovedUnitPrice = myItem.ApprovedUnitPrice;

					objItem.ApprovedTotalPrice = myItem.ApprovedTotalPrice;

					objItem.Description = myItem.Description;

					objItem.Status = myItem.Status;

				}
				catch(Exception ex)
				{
					return new TransactionItem() as TR;
				}
				return objItem as TR;
			}
			if (myType == typeof(ExpenseManager.EF.TransactionItem))
			{
				var objItem = new TransactionItem();
				var myItem = sourceObject as ExpenseManager.EF.TransactionItem;
				if(myItem == null){return null;};
				try
				{
					objItem.TransactionItemId = myItem.TransactionItemId;

					objItem.ExpensenseItemId = myItem.ExpensenseItemId;

					objItem.RequestedQuantity = myItem.RequestedQuantity;

					objItem.RequestedUnitPrice = myItem.RequestedUnitPrice;

					objItem.ExpenseTransactionId = myItem.ExpenseTransactionId;

					objItem.ExpenseCategoryId = myItem.ExpenseCategoryId;

					objItem.ExpenseTypeId = myItem.ExpenseTypeId;

					objItem.ApprovedQuantity = myItem.ApprovedQuantity;

					objItem.ApprovedUnitPrice = myItem.ApprovedUnitPrice;

					objItem.ApprovedTotalPrice = myItem.ApprovedTotalPrice;

					objItem.Description = myItem.Description;

					objItem.Status = myItem.Status;

					#region Included Tables
						try
						{
							objItem.ExpenseItem = new ExpenseItem();
							objItem.ExpenseItem.ExpenseItemId = myItem.ExpenseItem.ExpenseItemId;

							objItem.ExpenseItem.ExpenseCategoryId = myItem.ExpenseItem.ExpenseCategoryId;

							objItem.ExpenseItem.AccountsHeadId = myItem.ExpenseItem.AccountsHeadId;

							objItem.ExpenseItem.Title = myItem.ExpenseItem.Title;

							objItem.ExpenseItem.Description = myItem.ExpenseItem.Description;

							objItem.ExpenseItem.Code = myItem.ExpenseItem.Code;

							objItem.ExpenseItem.Status = myItem.ExpenseItem.Status;

						}
						catch{}
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
							objItem.ExpenseCategory = new ExpenseCategory();
							objItem.ExpenseCategory.ExpenseCategoryId = myItem.ExpenseCategory.ExpenseCategoryId;

							objItem.ExpenseCategory.Title = myItem.ExpenseCategory.Title;

							objItem.ExpenseCategory.Code = myItem.ExpenseCategory.Code;

							objItem.ExpenseCategory.Status = myItem.ExpenseCategory.Status;

						}
						catch{}
						try
						{
							objItem.ExpenseType = new ExpenseType();
							objItem.ExpenseType.ExpenseTypeId = myItem.ExpenseType.ExpenseTypeId;

							objItem.ExpenseType.Name = myItem.ExpenseType.Name;

							objItem.ExpenseType.Status = myItem.ExpenseType.Status;

						}
						catch{}
					#endregion
				}
				catch(Exception ex)
				{
					return new TransactionItem() as TR;
				}
				return objItem as TR;
			}
		return null;
		}
	}


	//Class File Generated from Code<->Stripper 1.2.0.0 | All Rights Reserved
}
