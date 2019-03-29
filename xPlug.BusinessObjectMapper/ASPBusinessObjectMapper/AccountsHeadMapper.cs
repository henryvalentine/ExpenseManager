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
	///* Date Generated:	25-11-2013 09:26:21
	///*******************************************************************************


	public class AccountsHeadMapper
	{

		public AccountsHeadMapper()
		{
		}

		public static TR Map<T, TR>(T sourceObject) where T : class where TR : class 
		{
			if(sourceObject == null){return null;}
			Type myType = typeof (T);
			if (myType == typeof(AccountsHead))
			{
				var objItem = new ExpenseManager.EF.AccountsHead();
				var myItem = sourceObject as AccountsHead;
				if(myItem == null){return null;};
				try
				{
					objItem.AccountsHeadId = myItem.AccountsHeadId;

					objItem.ExpenseCategoryId = myItem.ExpenseCategoryId;

					objItem.Title = myItem.Title;

					objItem.Description = myItem.Description;

					objItem.Code = myItem.Code;

					objItem.Status = myItem.Status;

				}
				catch(Exception ex)
				{
					return new AccountsHead() as TR;
				}
				return objItem as TR;
			}
			if (myType == typeof(ExpenseManager.EF.AccountsHead))
			{
				var objItem = new AccountsHead();
				var myItem = sourceObject as ExpenseManager.EF.AccountsHead;
				if(myItem == null){return null;};
				try
				{
					objItem.AccountsHeadId = myItem.AccountsHeadId;

					objItem.ExpenseCategoryId = myItem.ExpenseCategoryId;

					objItem.Title = myItem.Title;

					objItem.Description = myItem.Description;

					objItem.Code = myItem.Code;

					objItem.Status = myItem.Status;

					#region Included Tables
						try
						{
							objItem.ExpenseCategory = new ExpenseCategory();
							objItem.ExpenseCategory.ExpenseCategoryId = myItem.ExpenseCategory.ExpenseCategoryId;

							objItem.ExpenseCategory.Title = myItem.ExpenseCategory.Title;

							objItem.ExpenseCategory.Code = myItem.ExpenseCategory.Code;

							objItem.ExpenseCategory.Status = myItem.ExpenseCategory.Status;

						}
						catch{}
					#endregion
				}
				catch(Exception ex)
				{
					return new AccountsHead() as TR;
				}
				return objItem as TR;
			}
		return null;
		}
	}


	//Class File Generated from Code<->Stripper 1.2.0.0 | All Rights Reserved
}
