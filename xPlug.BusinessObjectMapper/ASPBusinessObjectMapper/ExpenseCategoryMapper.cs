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
	///* Date Generated:	25-11-2013 09:26:29
	///*******************************************************************************


	public class ExpenseCategoryMapper
	{

		public ExpenseCategoryMapper()
		{
		}

		public static TR Map<T, TR>(T sourceObject) where T : class where TR : class 
		{
			if(sourceObject == null){return null;}
			Type myType = typeof (T);
			if (myType == typeof(ExpenseCategory))
			{
				var objItem = new ExpenseManager.EF.ExpenseCategory();
				var myItem = sourceObject as ExpenseCategory;
				if(myItem == null){return null;};
				try
				{
					objItem.ExpenseCategoryId = myItem.ExpenseCategoryId;

					objItem.Title = myItem.Title;

					objItem.Code = myItem.Code;

					objItem.Status = myItem.Status;

				}
				catch(Exception ex)
				{
					return new ExpenseCategory() as TR;
				}
				return objItem as TR;
			}
			if (myType == typeof(ExpenseManager.EF.ExpenseCategory))
			{
				var objItem = new ExpenseCategory();
				var myItem = sourceObject as ExpenseManager.EF.ExpenseCategory;
				if(myItem == null){return null;};
				try
				{
					objItem.ExpenseCategoryId = myItem.ExpenseCategoryId;

					objItem.Title = myItem.Title;

					objItem.Code = myItem.Code;

					objItem.Status = myItem.Status;

					#region Included Tables
					#endregion
				}
				catch(Exception ex)
				{
					return new ExpenseCategory() as TR;
				}
				return objItem as TR;
			}
		return null;
		}
	}


	//Class File Generated from Code<->Stripper 1.2.0.0 | All Rights Reserved
}
