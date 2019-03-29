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
	///* Date Generated:	25-11-2013 09:26:23
	///*******************************************************************************


	public class LiquidAssetMapper
	{

		public LiquidAssetMapper()
		{
		}

		public static TR Map<T, TR>(T sourceObject) where T : class where TR : class 
		{
			if(sourceObject == null){return null;}
			Type myType = typeof (T);
			if (myType == typeof(LiquidAsset))
			{
				var objItem = new ExpenseManager.EF.LiquidAsset();
				var myItem = sourceObject as LiquidAsset;
				if(myItem == null){return null;};
				try
				{
					objItem.LiquidAssetId = myItem.LiquidAssetId;

					objItem.AssetTypeId = myItem.AssetTypeId;

					objItem.Name = myItem.Name;

					objItem.Code = myItem.Code;

					objItem.Status = myItem.Status;

					objItem.Amount = myItem.Amount;

					objItem.AssetCategoryId = myItem.AssetCategoryId;

				}
				catch(Exception ex)
				{
					return new LiquidAsset() as TR;
				}
				return objItem as TR;
			}
			if (myType == typeof(ExpenseManager.EF.LiquidAsset))
			{
				var objItem = new LiquidAsset();
				var myItem = sourceObject as ExpenseManager.EF.LiquidAsset;
				if(myItem == null){return null;};
				try
				{
					objItem.LiquidAssetId = myItem.LiquidAssetId;

					objItem.AssetTypeId = myItem.AssetTypeId;

					objItem.Name = myItem.Name;

					objItem.Code = myItem.Code;

					objItem.Status = myItem.Status;

					objItem.Amount = myItem.Amount;

					objItem.AssetCategoryId = myItem.AssetCategoryId;

					#region Included Tables
						try
						{
							objItem.AssetType = new AssetType();
							objItem.AssetType.AssetTypeId = myItem.AssetType.AssetTypeId;

							objItem.AssetType.AssetCategoryId = myItem.AssetType.AssetCategoryId;

							objItem.AssetType.Name = myItem.AssetType.Name;

							objItem.AssetType.Code = myItem.AssetType.Code;

							objItem.AssetType.Status = myItem.AssetType.Status;

						}
						catch{}
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
					return new LiquidAsset() as TR;
				}
				return objItem as TR;
			}
		return null;
		}
	}


	//Class File Generated from Code<->Stripper 1.2.0.0 | All Rights Reserved
}
