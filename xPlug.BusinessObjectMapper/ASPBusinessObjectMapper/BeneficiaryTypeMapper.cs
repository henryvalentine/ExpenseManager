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


	public class BeneficiaryTypeMapper
	{

		public BeneficiaryTypeMapper()
		{
		}

		public static TR Map<T, TR>(T sourceObject) where T : class where TR : class 
		{
			if(sourceObject == null){return null;}
			Type myType = typeof (T);
			if (myType == typeof(BeneficiaryType))
			{
				var objItem = new ExpenseManager.EF.BeneficiaryType();
				var myItem = sourceObject as BeneficiaryType;
				if(myItem == null){return null;};
				try
				{
					objItem.BeneficiaryTypeId = myItem.BeneficiaryTypeId;

					objItem.Name = myItem.Name;

					objItem.Status = myItem.Status;

				}
				catch(Exception ex)
				{
					return new BeneficiaryType() as TR;
				}
				return objItem as TR;
			}
			if (myType == typeof(ExpenseManager.EF.BeneficiaryType))
			{
				var objItem = new BeneficiaryType();
				var myItem = sourceObject as ExpenseManager.EF.BeneficiaryType;
				if(myItem == null){return null;};
				try
				{
					objItem.BeneficiaryTypeId = myItem.BeneficiaryTypeId;

					objItem.Name = myItem.Name;

					objItem.Status = myItem.Status;

					#region Included Tables
					#endregion
				}
				catch(Exception ex)
				{
					return new BeneficiaryType() as TR;
				}
				return objItem as TR;
			}
		return null;
		}
	}


	//Class File Generated from Code<->Stripper 1.2.0.0 | All Rights Reserved
}
