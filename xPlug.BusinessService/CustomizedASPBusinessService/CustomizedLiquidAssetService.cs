using System.Collections.Generic;
using System.Linq;
using xPlug.BusinessObject;

namespace xPlug.BusinessService
{
	/// Author:	 Oluwaseyi Adegboyega - sadegboyega@xplugng.com
	/// Copyright © 2013. xPlug Technologies Limited. All Rights Reserved
	///*******************************************************************************
	///* Date Created: 	04-06-2013
	///* Date Generated:	18-09-2013 01:30:48
	///*******************************************************************************


	public partial class LiquidAssetService
	{
        public List<LiquidAsset> GetLasInsertedLiquidAsset(List<LiquidAsset> liquidAssetsList, int liquidAssetTypeId)
        {
            if (liquidAssetsList == null || !liquidAssetsList.Any())
            {
                return new List<LiquidAsset>();
            }
            {
                
            }
            var newLiquidAssetList = liquidAssetsList.FindAll(m => m.AssetTypeId == liquidAssetTypeId).OrderByDescending(m => m.Code).Take(1).ToList();

            if (!liquidAssetsList.Any())
            {
                return new List<LiquidAsset>();
            }

            return newLiquidAssetList;
        }
        
	}


	//Class File Generated from Code<->Stripper 1.2.0.0 | All Rights Reserved
}
