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
	///* Date Generated:	25-11-2013 09:26:28
	///*******************************************************************************


	public class ExpenseTypeMapper
	{

		public ExpenseTypeMapper()
		{
		}

		public static TR Map<T, TR>(T sourceObject) where T : class where TR : class 
		{
			if(sourceObject == null){return null;}
			Type myType = typeof (T);
			if (myType == typeof(ExpenseType))
			{
				var objItem = new ExpenseManager.EF.ExpenseType();
				var myItem = sourceObject as ExpenseType;
				if(myItem == null){return null;};
				try
				{
					objItem.ExpenseTypeId = myItem.ExpenseTypeId;

					objItem.Name = myItem.Name;

					objItem.Status = myItem.Status;

				}
				catch(Exception ex)
				{
					return new ExpenseType() as TR;
				}
				return objItem as TR;
			}
			if (myType == typeof(ExpenseManager.EF.ExpenseType))
			{
				var objItem = new ExpenseType();
				var myItem = sourceObject as ExpenseManager.EF.ExpenseType;
				if(myItem == null){return null;};
				try
				{
					objItem.ExpenseTypeId = myItem.ExpenseTypeId;

					objItem.Name = myItem.Name;

					objItem.Status = myItem.Status;

					#region Included Tables
					#endregion
				}
				catch(Exception ex)
				{
					return new ExpenseType() as TR;
				}
				return objItem as TR;
			}
		return null;
		}
	}


	//Class File Generated from Code<->Stripper 1.2.0.0 | All Rights Reserved
}
