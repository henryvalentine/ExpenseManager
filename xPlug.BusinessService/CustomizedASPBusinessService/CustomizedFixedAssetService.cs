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
	///* Date Generated:	18-09-2013 01:30:47
	///*******************************************************************************


	public partial class FixedAssetService
	{
        public List<FixedAsset> GetFixedAssetsByActiveFixedAssetTypes()
        {
            try
            {
                var activeAssetTypesList = GetFixedAssets();
                if (!activeAssetTypesList.Any())
                {
                    return new List<FixedAsset>();
                }

                foreach (var fixedAsset in activeAssetTypesList)
                {
                    fixedAsset.TotalCost = (fixedAsset.Quantity * fixedAsset.CostOfPurchase) + fixedAsset.CostOfTransportationAndInstallation;
                }
                return activeAssetTypesList.Where(m => m.AssetType.Status == 1).OrderBy(m => m.AssetCategory.Name).ThenBy(m => m.AssetType.Name).ThenByDescending(m => m.Name).ToList();
            }
            catch (Exception ex)
            {
                ErrorManager.LogApplicationError(ex.StackTrace, ex.Source, ex.Message);
                return new List<FixedAsset>();
            }
        }

        public List<FixedAsset> GetFixedAssetsByActiveFixedAssetTypId(int assetTypeId)
        {
            try
            {
                return _fixedAssetManager.GetFixedAssetsByActiveFixedAssetTypId(assetTypeId);
            }
            catch (Exception ex)
            {
                ErrorManager.LogApplicationError(ex.StackTrace, ex.Source, ex.Message);
                return new List<FixedAsset>();
            }
        }

        public FixedAsset GetLastFixedAssetByActiveFixedAssetTypId(int assetTypeId)
        {
            try
            {

                return _fixedAssetManager.GetLastFixedAssetByActiveFixedAssetTypId(assetTypeId);
            }
            catch (Exception ex)
            {
                ErrorManager.LogApplicationError(ex.StackTrace, ex.Source, ex.Message);
                return new FixedAsset();
            }
        }

        public int AddFixedAssetCheckDuplicate(FixedAsset fixedAsset)
        {
            try
            {
                return _fixedAssetManager.AddFixedAssetCheckDuplicate(fixedAsset);
            }
            catch (Exception ex)
            {
                ErrorManager.LogApplicationError(ex.StackTrace, ex.Source, ex.Message);
                return 0;
            }
        }

        public int UpdateFixedAssetCheckDuplicate(FixedAsset fixedAsset)
        {
            try
            {
                return _fixedAssetManager.UpdateFixedAssetCheckDuplicate(fixedAsset);
            }
            catch (Exception ex)
            {
                ErrorManager.LogApplicationError(ex.StackTrace, ex.Source, ex.Message);
                return 0;
            }
        }
    }
    
	//Class File Generated from Code<->Stripper 1.2.0.0 | All Rights Reserved
}
