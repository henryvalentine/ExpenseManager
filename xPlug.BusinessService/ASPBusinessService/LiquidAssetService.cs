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
	///* Date Generated:	25-11-2013 09:26:23
	///*******************************************************************************


	public partial class LiquidAssetService : MarshalByRefObject
	{
		private readonly LiquidAssetManager  _liquidAssetManager;
		public LiquidAssetService()
		{
			_liquidAssetManager = new LiquidAssetManager();
		}

		public int AddLiquidAsset(LiquidAsset liquidAsset)
		{
			try
			{
				return _liquidAssetManager.AddLiquidAsset(liquidAsset);
			}
			catch (Exception ex)
			{
				ErrorManager.LogApplicationError(ex.StackTrace, ex.Source, ex.Message);
				return 0;
			}
		}

		public bool UpdateLiquidAsset(LiquidAsset liquidAsset)
		{
			try
			{
				return _liquidAssetManager.UpdateLiquidAsset(liquidAsset);
				}
			catch (Exception ex)
			{
				ErrorManager.LogApplicationError(ex.StackTrace, ex.Source, ex.Message);
				return false;
			}
		}

		public bool DeleteLiquidAsset(Int32 liquidAssetId)
		{
			try
			{
				return _liquidAssetManager.DeleteLiquidAsset(liquidAssetId);
				}
			catch (Exception ex)
			{
				ErrorManager.LogApplicationError(ex.StackTrace, ex.Source, ex.Message);
				return false;
			}
		}

		public LiquidAsset GetLiquidAsset(int liquidAssetId)
		{
			try
			{
				return _liquidAssetManager.GetLiquidAsset(liquidAssetId);
			}
			catch (Exception ex)
			{
				ErrorManager.LogApplicationError(ex.StackTrace, ex.Source, ex.Message);
				return new LiquidAsset();
			}
		}

		public List<LiquidAsset> GetLiquidAssets()
		{
			try
			{
				var objList = new List<LiquidAsset>();
				objList = _liquidAssetManager.GetLiquidAssets();
				if(objList == null) {return  new List<LiquidAsset>();}
				return objList;
			}
			catch (Exception ex)
			{
				ErrorManager.LogApplicationError(ex.StackTrace, ex.Source, ex.Message);
				return new List<LiquidAsset>();
			}
		}

		public List<LiquidAsset>  GetLiquidAssetsByAssetTypeId(Int32 assetTypeId)
		{
			try
			{
				return _liquidAssetManager.GetLiquidAssetsByAssetTypeId(assetTypeId);
			}
			catch (Exception ex)
			{
				ErrorManager.LogApplicationError(ex.StackTrace, ex.Source, ex.Message);
				return new List<LiquidAsset>();
			}
		}

		public List<LiquidAsset>  GetLiquidAssetsByAssetCategoryId(Int32 assetCategoryId)
		{
			try
			{
				return _liquidAssetManager.GetLiquidAssetsByAssetCategoryId(assetCategoryId);
			}
			catch (Exception ex)
			{
				ErrorManager.LogApplicationError(ex.StackTrace, ex.Source, ex.Message);
				return new List<LiquidAsset>();
			}
		}

	}


	//Class File Generated from Code<->Stripper 1.2.0.0 | All Rights Reserved
}
