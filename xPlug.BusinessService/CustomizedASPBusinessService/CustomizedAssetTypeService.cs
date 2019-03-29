using System;
using System.Collections.Generic;
using System.Linq;
using XPLUG.WEBTOOLS;
using xPlug.BusinessObject;

namespace xPlug.BusinessService
{
	/// Author:	 Oluwaseyi Adegboyega - sadegboyega@xplugng.com
	/// Copyright © 2013. xPlug Technologies Limited. All Rights Reserved
	///*******************************************************************************
	///* Date Created: 	04-06-2013
	///* Date Generated:	18-09-2013 01:30:43
	///*******************************************************************************


	public partial class AssetTypeService
	{
        public List<AssetType> AssetTypesByActiveAssetCategory(Int32 assetCategoryId)
        {
            try
            {
                var asseTypesByAssetCategoryId = GetAssetTypesByAssetCategoryId(assetCategoryId);

                return asseTypesByAssetCategoryId.Any() ? asseTypesByAssetCategoryId.Where(m => m.AssetCategory.Status == 1 && m.Status == 1).OrderBy(m => m.Name).ToList() : new List<AssetType>();
            }
            catch (Exception ex)
            {
                ErrorManager.LogApplicationError(ex.StackTrace, ex.Source, ex.Message);
                return new List<AssetType>();
            }
        }

        public List<AssetType> GetAssetTypesByActiveAssetCategory(int assetCategoryId)
        {
            try
            {

                return _assetTypeManager.GetAssetTypesByActiveAssetCategory(assetCategoryId);
            }
            catch (Exception ex)
            {
                ErrorManager.LogApplicationError(ex.StackTrace, ex.Source, ex.Message);
                return new List<AssetType>();
            }
        }

        public List<AssetType> GetAllAssetTypes()
        {
            try
            {
                return _assetTypeManager.GetAllAssetTypes();
            }
            catch (Exception ex)
            {
                ErrorManager.LogApplicationError(ex.StackTrace, ex.Source, ex.Message);
                return new List<AssetType>();
            }
        }
        
        public AssetType GetLastInsertedAssetTypeByAssetCategory(int assetCategoryId)
        {
            try
            {
                return _assetTypeManager.GetLastInsertedAssetTypeByAssetCategory(assetCategoryId);
            }
            catch (Exception ex)
            {
                ErrorManager.LogApplicationError(ex.StackTrace, ex.Source, ex.Message);
                return new AssetType();
            }
        }

        public List<AssetType> GetOrderedAssetTypeByAssetCategory(int assetCategoryId)
        {
            try
            {
                return _assetTypeManager.GetOrderedAssetTypeByAssetCategory(assetCategoryId);
            }
            catch (Exception ex)
            {
                ErrorManager.LogApplicationError(ex.StackTrace, ex.Source, ex.Message);
                return new List<AssetType>();
            }
        }

        

         public List<AssetType> GetAssetTypesByAssetCategory(List<AssetType> assetTypeList, int assetCategoryId)
         {
             try
             {

                 return assetTypeList.Any() ? assetTypeList.FindAll(m => m.AssetCategoryId == assetCategoryId).OrderBy(m => m.Name).ToList() : new List<AssetType>();
             }
             catch (Exception ex)
             {
                 ErrorManager.LogApplicationError(ex.StackTrace, ex.Source, ex.Message);
                 return new List<AssetType>();
             }
         }

        public AssetType AssetTypeByAssetTypeId(List<AssetType> assetTypes, int assetTypeId)
        {
            try
            {
                var assetType = assetTypes.Find(m => m.AssetTypeId == assetTypeId);

                if (assetType == null || assetType.AssetTypeId < 1)
                {
                    return new AssetType();
                }
                return assetTypes.Find(m => m.AssetTypeId == assetTypeId);
            }
            catch (Exception ex)
            {
                ErrorManager.LogApplicationError(ex.StackTrace, ex.Source, ex.Message);
                return new AssetType();
            }
        }


	}


	//Class File Generated from Code<->Stripper 1.2.0.0 | All Rights Reserved
}
