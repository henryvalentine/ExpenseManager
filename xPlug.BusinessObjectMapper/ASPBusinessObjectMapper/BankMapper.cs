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


	public class BankMapper
	{

		public BankMapper()
		{
		}

		public static TR Map<T, TR>(T sourceObject) where T : class where TR : class 
		{
			if(sourceObject == null){return null;}
			Type myType = typeof (T);
			if (myType == typeof(Bank))
			{
				var objItem = new ExpenseManager.EF.Bank();
				var myItem = sourceObject as Bank;
				if(myItem == null){return null;};
				try
				{
					objItem.BankId = myItem.BankId;

					objItem.BankName = myItem.BankName;

				}
				catch(Exception ex)
				{
					return new Bank() as TR;
				}
				return objItem as TR;
			}
			if (myType == typeof(ExpenseManager.EF.Bank))
			{
				var objItem = new Bank();
				var myItem = sourceObject as ExpenseManager.EF.Bank;
				if(myItem == null){return null;};
				try
				{
					objItem.BankId = myItem.BankId;

					objItem.BankName = myItem.BankName;

					#region Included Tables
					#endregion
				}
				catch(Exception ex)
				{
					return new Bank() as TR;
				}
				return objItem as TR;
			}
		return null;
		}
	}


	//Class File Generated from Code<->Stripper 1.2.0.0 | All Rights Reserved
}
