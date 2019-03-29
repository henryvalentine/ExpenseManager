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
	///* Date Generated:	25-11-2013 09:26:27
	///*******************************************************************************


	public partial class FixedAssetService : MarshalByRefObject
	{
		private readonly FixedAssetManager  _fixedAssetManager;
		public FixedAssetService()
		{
			_fixedAssetManager = new FixedAssetManager();
		}

		public int AddFixedAsset(FixedAsset fixedAsset)
		{
			try
			{
				return _fixedAssetManager.AddFixedAsset(fixedAsset);
			}
			catch (Exception ex)
			{
				ErrorManager.LogApplicationError(ex.StackTrace, ex.Source, ex.Message);
				return 0;
			}
		}

		public int UpdateFixedAsset(FixedAsset fixedAsset)
		{
			try
			{
				return _fixedAssetManager.UpdateFixedAsset(fixedAsset);
				}
			catch (Exception ex)
			{
				ErrorManager.LogApplicationError(ex.StackTrace, ex.Source, ex.Message);
				return 0;
			}
		}

		public bool DeleteFixedAsset(Int32 fixedAssetId)
		{
			try
			{
				return _fixedAssetManager.DeleteFixedAsset(fixedAssetId);
				}
			catch (Exception ex)
			{
				ErrorManager.LogApplicationError(ex.StackTrace, ex.Source, ex.Message);
				return false;
			}
		}

		public FixedAsset GetFixedAsset(int fixedAssetId)
		{
			try
			{
				return _fixedAssetManager.GetFixedAsset(fixedAssetId);
			}
			catch (Exception ex)
			{
				ErrorManager.LogApplicationError(ex.StackTrace, ex.Source, ex.Message);
				return new FixedAsset();
			}
		}

		public List<FixedAsset> GetFixedAssets()
		{
			try
			{
				var objList = new List<FixedAsset>();
				objList = _fixedAssetManager.GetFixedAssets();
				if(objList == null) {return  new List<FixedAsset>();}
				return objList;
			}
			catch (Exception ex)
			{
				ErrorManager.LogApplicationError(ex.StackTrace, ex.Source, ex.Message);
				return new List<FixedAsset>();
			}
		}

		public List<FixedAsset>  GetFixedAssetsByAssetTypeId(Int32 assetTypeId)
		{
			try
			{
				return _fixedAssetManager.GetFixedAssetsByAssetTypeId(assetTypeId);
			}
			catch (Exception ex)
			{
				ErrorManager.LogApplicationError(ex.StackTrace, ex.Source, ex.Message);
				return new List<FixedAsset>();
			}
		}

		public List<FixedAsset>  GetFixedAssetsByAssetCategoryId(Int32 assetCategoryId)
		{
			try
			{
				return _fixedAssetManager.GetFixedAssetsByAssetCategoryId(assetCategoryId);
			}
			catch (Exception ex)
			{
				ErrorManager.LogApplicationError(ex.StackTrace, ex.Source, ex.Message);
				return new List<FixedAsset>();
			}
		}

	}


	//Class File Generated from Code<->Stripper 1.2.0.0 | All Rights Reserved
}
