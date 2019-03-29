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


	public partial class AssetCategoryService
	{
        public List<AssetCategory> GetAssetCategoryList()
        {
            try
            {
               return GetAssetCategories().OrderBy(m => m.Name).ToList();
            }
            catch (Exception ex)
            {
                ErrorManager.LogApplicationError(ex.StackTrace, ex.Source, ex.Message);
                return new List<AssetCategory>();
            }
        }

        public int AddAssetCategoryCheckDuplicate(AssetCategory assetCategory)
        {
            try
            {
                return _assetCategoryManager.AddAssetCategoryCheckDuplicate(assetCategory);
            }
            catch (Exception ex)
            {
                ErrorManager.LogApplicationError(ex.StackTrace, ex.Source, ex.Message);
                return 0;
            }
        }

        public int UpdateAssetCategoryCheckDuplicate(AssetCategory assetCategory)
        {
            try
            {
                return _assetCategoryManager.UpdateAssetCategoryCheckDuplicate(assetCategory);
            }
            catch (Exception ex)
            {
                ErrorManager.LogApplicationError(ex.StackTrace, ex.Source, ex.Message);
                return 0;
            }
        }

        public List<AssetCategory> GetActiveAssetCategories()
        {
            try
            {
                return _assetCategoryManager.GetActiveAssetCategories();
            }
            catch (Exception ex)
            {
                ErrorManager.LogApplicationError(ex.StackTrace, ex.Source, ex.Message);
                return new List<AssetCategory>();
            }
        }

        public List<AssetCategory> LoadFilterdAssetCategories()
        {
            try
            {
                return _assetCategoryManager.GetFilterdAssetCategories();
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
