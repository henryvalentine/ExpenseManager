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
	///* Date Generated:	05-12-2013 11:55:57
	///*******************************************************************************


	public class PaymentVoucherNumberMapper
	{

		public PaymentVoucherNumberMapper()
		{
		}

		public static TR Map<T, TR>(T sourceObject) where T : class where TR : class 
		{
			if(sourceObject == null){return null;}
			Type myType = typeof (T);
			if (myType == typeof(PaymentVoucherNumber))
			{
				var objItem = new ExpenseManager.EF.PaymentVoucherNumber();
				var myItem = sourceObject as PaymentVoucherNumber;
				if(myItem == null){return null;};
				try
				{
					objItem.PaymentVoucherNumberId = myItem.PaymentVoucherNumberId;

					objItem.TransactionId = myItem.TransactionId;

					objItem.TransactionTotalAmount = myItem.TransactionTotalAmount;

					objItem.PaymentDate = myItem.PaymentDate;

					objItem.DateSubmitted = myItem.DateSubmitted;

				}
				catch(Exception ex)
				{
					return new PaymentVoucherNumber() as TR;
				}
				return objItem as TR;
			}
			if (myType == typeof(ExpenseManager.EF.PaymentVoucherNumber))
			{
				var objItem = new PaymentVoucherNumber();
				var myItem = sourceObject as ExpenseManager.EF.PaymentVoucherNumber;
				if(myItem == null){return null;};
				try
				{
					objItem.PaymentVoucherNumberId = myItem.PaymentVoucherNumberId;

					objItem.TransactionId = myItem.TransactionId;

					objItem.TransactionTotalAmount = myItem.TransactionTotalAmount;

					objItem.PaymentDate = myItem.PaymentDate;

					objItem.DateSubmitted = myItem.DateSubmitted;

					#region Included Tables
					#endregion
				}
				catch(Exception ex)
				{
					return new PaymentVoucherNumber() as TR;
				}
				return objItem as TR;
			}
		return null;
		}
	}


	//Class File Generated from Code<->Stripper 1.2.0.0 | All Rights Reserved
}
