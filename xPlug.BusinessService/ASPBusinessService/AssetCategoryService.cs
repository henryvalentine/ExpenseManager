using System;
using System.Linq;
using System.Collections.Generic;
using System.Data;
using xPlug.BusinessObject;
using xPlug.BusinessManager;
using kPortal.CoreUtilities;



namespace xPlug.BusinessService
{
	/// Author:	 Oluwaseyi Adegboyega - sadegboyega@xplugng.com
	/// Copyright © 2013. xPlug Technologies Limited. All Rights Reserved
	///*******************************************************************************
	///* Date Created: 	04-06-2013
	///* Date Generated:	25-11-2013 09:26:18
	///*******************************************************************************


	public partial class AssetCategoryService : MarshalByRefObject
	{
		private readonly AssetCategoryManager  _assetCategoryManager;
		public AssetCategoryService()
		{
			_assetCategoryManager = new AssetCategoryManager();
		}

		public int AddAssetCategory(AssetCategory assetCategory)
		{
			try
			{
				return _assetCategoryManager.AddAssetCategory(assetCategory);
			}
			catch (Exception ex)
			{
				ErrorManager.LogApplicationError(ex.StackTrace, ex.Source, ex.Message);
				return 0;
			}
		}

		public int UpdateAssetCategory(AssetCategory assetCategory)
		{
			try
			{
				return _assetCategoryManager.UpdateAssetCategory(assetCategory);
				}
			catch (Exception ex)
			{
				ErrorManager.LogApplicationError(ex.StackTrace, ex.Source, ex.Message);
				return 0;
			}
		}

		public bool DeleteAssetCategory(Int32 assetCategoryId)
		{
			try
			{
				return _assetCategoryManager.DeleteAssetCategory(assetCategoryId);
				}
			catch (Exception ex)
			{
				ErrorManager.LogApplicationError(ex.StackTrace, ex.Source, ex.Message);
				return false;
			}
		}

		public AssetCategory GetAssetCategory(int assetCategoryId)
		{
			try
			{
				return _assetCategoryManager.GetAssetCategory(assetCategoryId);
			}
			catch (Exception ex)
			{
				ErrorManager.LogApplicationError(ex.StackTrace, ex.Source, ex.Message);
				return new AssetCategory();
			}
		}

		public List<AssetCategory> GetAssetCategories()
		{
			try
			{
				var objList = new List<AssetCategory>();
				objList = _assetCategoryManager.GetAssetCategories();
				if(objList == null) {return  new List<AssetCategory>();}
				return objList;
			}
			catch (Exception ex)
			{
				ErrorManager.LogApplicationError(ex.StackTrace, ex.Source, ex.Message);
				return new List<AssetCategory>();
			}
		}

	}


	//Class File Generated from Code<->Stripper 1.2.0.0 | All Rights Reserved
}
