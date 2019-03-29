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
	///* Date Generated:	25-11-2013 09:26:27
	///*******************************************************************************


	public class ExpenseItemMapper
	{

		public ExpenseItemMapper()
		{
		}

		public static TR Map<T, TR>(T sourceObject) where T : class where TR : class 
		{
			if(sourceObject == null){return null;}
			Type myType = typeof (T);
			if (myType == typeof(ExpenseItem))
			{
				var objItem = new ExpenseManager.EF.ExpenseItem();
				var myItem = sourceObject as ExpenseItem;
				if(myItem == null){return null;};
				try
				{
					objItem.ExpenseItemId = myItem.ExpenseItemId;

					objItem.ExpenseCategoryId = myItem.ExpenseCategoryId;

					objItem.AccountsHeadId = myItem.AccountsHeadId;

					objItem.Title = myItem.Title;

					objItem.Description = myItem.Description;

					objItem.Code = myItem.Code;

					objItem.Status = myItem.Status;

				}
				catch(Exception ex)
				{
					return new ExpenseItem() as TR;
				}
				return objItem as TR;
			}
			if (myType == typeof(ExpenseManager.EF.ExpenseItem))
			{
				var objItem = new ExpenseItem();
				var myItem = sourceObject as ExpenseManager.EF.ExpenseItem;
				if(myItem == null){return null;};
				try
				{
					objItem.ExpenseItemId = myItem.ExpenseItemId;

					objItem.ExpenseCategoryId = myItem.ExpenseCategoryId;

					objItem.AccountsHeadId = myItem.AccountsHeadId;

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
						try
						{
							objItem.AccountsHead = new AccountsHead();
							objItem.AccountsHead.AccountsHeadId = myItem.AccountsHead.AccountsHeadId;

							objItem.AccountsHead.ExpenseCategoryId = myItem.AccountsHead.ExpenseCategoryId;

							objItem.AccountsHead.Title = myItem.AccountsHead.Title;

							objItem.AccountsHead.Description = myItem.AccountsHead.Description;

							objItem.AccountsHead.Code = myItem.AccountsHead.Code;

							objItem.AccountsHead.Status = myItem.AccountsHead.Status;

						}
						catch{}
					#endregion
				}
				catch(Exception ex)
				{
					return new ExpenseItem() as TR;
				}
				return objItem as TR;
			}
		return null;
		}
	}


	//Class File Generated from Code<->Stripper 1.2.0.0 | All Rights Reserved
}
