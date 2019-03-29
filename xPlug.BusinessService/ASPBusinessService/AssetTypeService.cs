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
	///* Date Generated:	25-11-2013 09:26:21
	///*******************************************************************************


	public partial class AssetTypeService : MarshalByRefObject
	{
		private readonly AssetTypeManager  _assetTypeManager;
		public AssetTypeService()
		{
			_assetTypeManager = new AssetTypeManager();
		}

		public int AddAssetType(AssetType assetType)
		{
			try
			{
				return _assetTypeManager.AddAssetType(assetType);
			}
			catch (Exception ex)
			{
				ErrorManager.LogApplicationError(ex.StackTrace, ex.Source, ex.Message);
				return 0;
			}
		}

		public int UpdateAssetType(AssetType assetType)
		{
			try
			{
				return _assetTypeManager.UpdateAssetType(assetType);
				}
			catch (Exception ex)
			{
				ErrorManager.LogApplicationError(ex.StackTrace, ex.Source, ex.Message);
				return 0;
			}
		}

		public bool DeleteAssetType(Int32 assetTypeId)
		{
			try
			{
				return _assetTypeManager.DeleteAssetType(assetTypeId);
				}
			catch (Exception ex)
			{
				ErrorManager.LogApplicationError(ex.StackTrace, ex.Source, ex.Message);
				return false;
			}
		}

		public AssetType GetAssetType(int assetTypeId)
		{
			try
			{
				return _assetTypeManager.GetAssetType(assetTypeId);
			}
			catch (Exception ex)
			{
				ErrorManager.LogApplicationError(ex.StackTrace, ex.Source, ex.Message);
				return new AssetType();
			}
		}

		public List<AssetType> GetAssetTypes()
		{
			try
			{
				var objList = new List<AssetType>();
				objList = _assetTypeManager.GetAssetTypes();
				if(objList == null) {return  new List<AssetType>();}
				return objList;
			}
			catch (Exception ex)
			{
				ErrorManager.LogApplicationError(ex.StackTrace, ex.Source, ex.Message);
				return new List<AssetType>();
			}
		}

		public List<AssetType>  GetAssetTypesByAssetCategoryId(Int32 assetCategoryId)
		{
			try
			{
				return _assetTypeManager.GetAssetTypesByAssetCategoryId(assetCategoryId);
			}
			catch (Exception ex)
			{
				ErrorManager.LogApplicationError(ex.StackTrace, ex.Source, ex.Message);
				return new List<AssetType>();
			}
		}

	}


	//Class File Generated from Code<->Stripper 1.2.0.0 | All Rights Reserved
}
