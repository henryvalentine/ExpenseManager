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
	///* Date Generated:	25-11-2013 09:26:18
	///*******************************************************************************


	public class PaymentModeMapper
	{

		public PaymentModeMapper()
		{
		}

		public static TR Map<T, TR>(T sourceObject) where T : class where TR : class 
		{
			if(sourceObject == null){return null;}
			Type myType = typeof (T);
			if (myType == typeof(PaymentMode))
			{
				var objItem = new ExpenseManager.EF.PaymentMode();
				var myItem = sourceObject as PaymentMode;
				if(myItem == null){return null;};
				try
				{
					objItem.PaymentModeId = myItem.PaymentModeId;

					objItem.Name = myItem.Name;

					objItem.Status = myItem.Status;

				}
				catch(Exception ex)
				{
					return new PaymentMode() as TR;
				}
				return objItem as TR;
			}
			if (myType == typeof(ExpenseManager.EF.PaymentMode))
			{
				var objItem = new PaymentMode();
				var myItem = sourceObject as ExpenseManager.EF.PaymentMode;
				if(myItem == null){return null;};
				try
				{
					objItem.PaymentModeId = myItem.PaymentModeId;

					objItem.Name = myItem.Name;

					objItem.Status = myItem.Status;

					#region Included Tables
					#endregion
				}
				catch(Exception ex)
				{
					return new PaymentMode() as TR;
				}
				return objItem as TR;
			}
		return null;
		}
	}


	//Class File Generated from Code<->Stripper 1.2.0.0 | All Rights Reserved
}
