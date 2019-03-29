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


	public class FixedAssetMapper
	{

		public FixedAssetMapper()
		{
		}

		public static TR Map<T, TR>(T sourceObject) where T : class where TR : class 
		{
			if(sourceObject == null){return null;}
			Type myType = typeof (T);
			if (myType == typeof(FixedAsset))
			{
				var objItem = new ExpenseManager.EF.FixedAsset();
				var myItem = sourceObject as FixedAsset;
				if(myItem == null){return null;};
				try
				{
					objItem.FixedAssetId = myItem.FixedAssetId;

					objItem.AssetTypeId = myItem.AssetTypeId;

					objItem.Name = myItem.Name;

					objItem.Description = myItem.Description;

					objItem.CostOfPurchase = myItem.CostOfPurchase;

					objItem.CostOfTransportationAndInstallation = myItem.CostOfTransportationAndInstallation;

					objItem.DatePurchased = myItem.DatePurchased;

					objItem.ScannedReceipt = myItem.ScannedReceipt;

					objItem.Model = myItem.Model;

					objItem.Make = myItem.Make;

					objItem.Brand = myItem.Brand;

					objItem.Code = myItem.Code;

					objItem.Status = myItem.Status;

					objItem.AssetCategoryId = myItem.AssetCategoryId;

					objItem.Quantity = myItem.Quantity;

				}
				catch(Exception ex)
				{
					return new FixedAsset() as TR;
				}
				return objItem as TR;
			}
			if (myType == typeof(ExpenseManager.EF.FixedAsset))
			{
				var objItem = new FixedAsset();
				var myItem = sourceObject as ExpenseManager.EF.FixedAsset;
				if(myItem == null){return null;};
				try
				{
					objItem.FixedAssetId = myItem.FixedAssetId;

					objItem.AssetTypeId = myItem.AssetTypeId;

					objItem.Name = myItem.Name;

					objItem.Description = myItem.Description;

					objItem.CostOfPurchase = myItem.CostOfPurchase;

					objItem.CostOfTransportationAndInstallation = myItem.CostOfTransportationAndInstallation;

					objItem.DatePurchased = myItem.DatePurchased;

					objItem.ScannedReceipt = myItem.ScannedReceipt;

					objItem.Model = myItem.Model;

					objItem.Make = myItem.Make;

					objItem.Brand = myItem.Brand;

					objItem.Code = myItem.Code;

					objItem.Status = myItem.Status;

					objItem.AssetCategoryId = myItem.AssetCategoryId;

					objItem.Quantity = myItem.Quantity;

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
					return new FixedAsset() as TR;
				}
				return objItem as TR;
			}
		return null;
		}
	}


	//Class File Generated from Code<->Stripper 1.2.0.0 | All Rights Reserved
}
