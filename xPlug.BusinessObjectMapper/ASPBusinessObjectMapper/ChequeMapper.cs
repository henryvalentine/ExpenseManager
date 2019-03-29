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
	///* Date Generated:	01-12-2013 03:58:07
	///*******************************************************************************


	public class ChequeMapper
	{

		public ChequeMapper()
		{
		}

		public static TR Map<T, TR>(T sourceObject) where T : class where TR : class 
		{
			if(sourceObject == null){return null;}
			Type myType = typeof (T);
			if (myType == typeof(Cheque))
			{
				var objItem = new ExpenseManager.EF.Cheque();
				var myItem = sourceObject as Cheque;
				if(myItem == null){return null;};
				try
				{
					objItem.ChequePaymentId = myItem.ChequePaymentId;

					objItem.Amount = myItem.Amount;

					objItem.ChequeNo = myItem.ChequeNo;

					objItem.ExpenseTransactionPaymentHistoryId = myItem.ExpenseTransactionPaymentHistoryId;

					objItem.BankId = myItem.BankId;

					objItem.ScannedCopy = myItem.ScannedCopy;

				}
				catch(Exception ex)
				{
					return new Cheque() as TR;
				}
				return objItem as TR;
			}
			if (myType == typeof(ExpenseManager.EF.Cheque))
			{
				var objItem = new Cheque();
				var myItem = sourceObject as ExpenseManager.EF.Cheque;
				if(myItem == null){return null;};
				try
				{
					objItem.ChequePaymentId = myItem.ChequePaymentId;

					objItem.Amount = myItem.Amount;

					objItem.ChequeNo = myItem.ChequeNo;

					objItem.ExpenseTransactionPaymentHistoryId = myItem.ExpenseTransactionPaymentHistoryId;

					objItem.BankId = myItem.BankId;

					objItem.ScannedCopy = myItem.ScannedCopy;

					#region Included Tables
						try
						{
							objItem.ExpenseTransactionPaymentHistory = new ExpenseTransactionPaymentHistory();
							objItem.ExpenseTransactionPaymentHistory.ExpenseTransactionPaymentHistoryId = myItem.ExpenseTransactionPaymentHistory.ExpenseTransactionPaymentHistoryId;

							objItem.ExpenseTransactionPaymentHistory.ExpenseTransactionId = myItem.ExpenseTransactionPaymentHistory.ExpenseTransactionId;

							objItem.ExpenseTransactionPaymentHistory.AmountPaid = myItem.ExpenseTransactionPaymentHistory.AmountPaid;

							objItem.ExpenseTransactionPaymentHistory.PaymentDate = myItem.ExpenseTransactionPaymentHistory.PaymentDate;

							objItem.ExpenseTransactionPaymentHistory.PaymentTime = myItem.ExpenseTransactionPaymentHistory.PaymentTime;

							objItem.ExpenseTransactionPaymentHistory.PaidById = myItem.ExpenseTransactionPaymentHistory.PaidById;

							objItem.ExpenseTransactionPaymentHistory.Comment = myItem.ExpenseTransactionPaymentHistory.Comment;

							objItem.ExpenseTransactionPaymentHistory.Status = myItem.ExpenseTransactionPaymentHistory.Status;

							objItem.ExpenseTransactionPaymentHistory.ExpenseTransactionPaymentId = myItem.ExpenseTransactionPaymentHistory.ExpenseTransactionPaymentId;

							objItem.ExpenseTransactionPaymentHistory.PaymentModeId = myItem.ExpenseTransactionPaymentHistory.PaymentModeId;

							objItem.ExpenseTransactionPaymentHistory.BeneficiaryId = myItem.ExpenseTransactionPaymentHistory.BeneficiaryId;

						}
						catch{}
						try
						{
							objItem.Bank = new Bank();
							objItem.Bank.BankId = myItem.Bank.BankId;

							objItem.Bank.BankName = myItem.Bank.BankName;

						}
						catch{}
					#endregion
				}
				catch(Exception ex)
				{
					return new Cheque() as TR;
				}
				return objItem as TR;
			}
		return null;
		}
	}


	//Class File Generated from Code<->Stripper 1.2.0.0 | All Rights Reserved
}
