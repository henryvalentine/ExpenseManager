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
	///* Date Generated:	25-11-2013 09:26:20
	///*******************************************************************************


	public class AssetTypeMapper
	{

		public AssetTypeMapper()
		{
		}

		public static TR Map<T, TR>(T sourceObject) where T : class where TR : class 
		{
			if(sourceObject == null){return null;}
			Type myType = typeof (T);
			if (myType == typeof(AssetType))
			{
				var objItem = new ExpenseManager.EF.AssetType();
				var myItem = sourceObject as AssetType;
				if(myItem == null){return null;};
				try
				{
					objItem.AssetTypeId = myItem.AssetTypeId;

					objItem.AssetCategoryId = myItem.AssetCategoryId;

					objItem.Name = myItem.Name;

					objItem.Code = myItem.Code;

					objItem.Status = myItem.Status;

				}
				catch(Exception ex)
				{
					return new AssetType() as TR;
				}
				return objItem as TR;
			}
			if (myType == typeof(ExpenseManager.EF.AssetType))
			{
				var objItem = new AssetType();
				var myItem = sourceObject as ExpenseManager.EF.AssetType;
				if(myItem == null){return null;};
				try
				{
					objItem.AssetTypeId = myItem.AssetTypeId;

					objItem.AssetCategoryId = myItem.AssetCategoryId;

					objItem.Name = myItem.Name;

					objItem.Code = myItem.Code;

					objItem.Status = myItem.Status;

					#region Included Tables
						try
						{
							objItem.AssetCategory = new AssetCategory();
							objItem.AssetCategory.AssetCategoryId = myItem.AssetCategory.AssetCategoryId;

							objItem.AssetCategory.Name = myItem.AssetCategory.Name;

							objItem.AssetCategory.Code = myItem.AssetCategory.Code;

							objItem.AssetCategory.Status = myItem.AssetCategory.Status;

						}
						catch{}
					#endregion
				}
				catch(Exception ex)
				{
					return new AssetType() as TR;
				}
				return objItem as TR;
			}
		return null;
		}
	}


	//Class File Generated from Code<->Stripper 1.2.0.0 | All Rights Reserved
}
