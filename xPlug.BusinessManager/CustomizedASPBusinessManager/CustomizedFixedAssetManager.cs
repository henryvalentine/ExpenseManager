using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using ExpenseManager.EF;
using XPLUG.WEBTOOLS;
using xPlug.BusinessObjectMapper;

namespace xPlug.BusinessManager
{
	/// Author:	 Oluwaseyi Adegboyega - sadegboyega@xplugng.com
	/// Copyright © 2013. xPlug Technologies Limited. All Rights Reserved
	///*******************************************************************************
	///* Date Created: 	04-06-2013
	///* Date Generated:	18-09-2013 01:30:45
	///*******************************************************************************


	public partial class FixedAssetManager
	{
        public int AddFixedAssetCheckDuplicate(BusinessObject.FixedAsset fixedAsset)
        {
            try
            {
                //Re-Map Object to Entity Object
                var myEntityObj = FixedAssetMapper.Map<BusinessObject.FixedAsset, FixedAsset>(fixedAsset);
                if (myEntityObj == null)
                { return -2; }
                using (var db = new ExpenseManagerDBEntities())
                {
                    if (db.FixedAssets.Any())
                    {
                        if (db.FixedAssets.Count(m => m.Name.ToLower() == myEntityObj.Name.ToLower() && m.DatePurchased == myEntityObj.DatePurchased) > 0)
                        {
                            if (!string.IsNullOrEmpty(fixedAsset.Model) && !string.IsNullOrEmpty(fixedAsset.Brand) && !string.IsNullOrEmpty(fixedAsset.Make))
                            {
                                if (db.FixedAssets.Count(m => m.Model == myEntityObj.Model && m.Make == myEntityObj.Make && m.Brand == myEntityObj.Brand) > 0)
                                {
                                    return -3;
                                }
                            }
                        }
                    }
                    db.AddToFixedAssets(myEntityObj);
                    db.SaveChanges();
                    fixedAsset.FixedAssetId = myEntityObj.FixedAssetId;
                    return fixedAsset.FixedAssetId;
                }
            }
            catch (Exception ex)
            {
                ErrorManager.LogApplicationError(ex.StackTrace, ex.Source, ex.Message);
                return 0;
            }
        }

        public int UpdateFixedAssetCheckDuplicate(BusinessObject.FixedAsset fixedAsset)
        {
            try
            {
                //Re-Map Object to Entity Object
                var myEntityObj = FixedAssetMapper.Map<BusinessObject.FixedAsset, FixedAsset>(fixedAsset);
                if (myEntityObj == null)
                { return -2; }
                using (var db = new ExpenseManagerDBEntities())
                {
                    if (db.FixedAssets.Any())
                    {

                        if (db.FixedAssets.Count(m => m.Name.ToLower() == myEntityObj.Name.ToLower() && m.DatePurchased == myEntityObj.DatePurchased && m.FixedAssetId != myEntityObj.FixedAssetId) > 0)
                        {
                            if (!string.IsNullOrEmpty(fixedAsset.Model) && !string.IsNullOrEmpty(fixedAsset.Brand) && !string.IsNullOrEmpty(fixedAsset.Make))
                            {
                                if (db.FixedAssets.Count(m => m.Model == myEntityObj.Model && m.Make == myEntityObj.Make && m.Brand == myEntityObj.Brand && m.FixedAssetId != myEntityObj.FixedAssetId) > 0)
                                {
                                    return -3;
                                }
                            }

                        }
                    }
                    db.FixedAssets.Attach(myEntityObj);
                    db.ObjectStateManager.ChangeObjectState(myEntityObj, EntityState.Modified);
                    db.SaveChanges();
                    return 1;
                }
            }
            catch (Exception ex)
            {
                ErrorManager.LogApplicationError(ex.StackTrace, ex.Source, ex.Message);
                return 0;
            }
        }

        public BusinessObject.FixedAsset GetLastFixedAssetByActiveFixedAssetTypId(int assetTypeId)
        {
            try
            {
                using (var db = new ExpenseManagerDBEntities())
                {
                    var myEntityObjList = db.FixedAssets.Where(m => m.AssetTypeId == assetTypeId).ToList();
                    var objList = new List<BusinessObject.FixedAsset>();
                    if (!myEntityObjList.Any())
                    {
                        return new BusinessObject.FixedAsset();
                    }

                    var newItemList = myEntityObjList.OrderByDescending(m => m.Code).Take(1).ToList();

                    if (!newItemList.Any())
                    {
                        return new BusinessObject.FixedAsset();
                    }

                    foreach (var fixedAsset in newItemList)
                    {
                        var myObj = FixedAssetMapper.Map<FixedAsset, BusinessObject.FixedAsset>(fixedAsset);
                        myObj.TotalCost = (fixedAsset.Quantity * fixedAsset.CostOfPurchase) + fixedAsset.CostOfTransportationAndInstallation;
                        objList.Add(myObj);
                    }

                    if (!objList.Any())
                    {
                        return new BusinessObject.FixedAsset();
                    }

                    return objList.ElementAtOrDefault(0);
                }
                
            }
            catch (Exception ex)
            {
                ErrorManager.LogApplicationError(ex.StackTrace, ex.Source, ex.Message);
                return new BusinessObject.FixedAsset();
            }
        }

        public List<BusinessObject.FixedAsset> GetFixedAssetsByActiveFixedAssetTypId(int assetTypeId)
		{
			try
			{
				using (var db = new ExpenseManagerDBEntities())
				{
					var myObjList = db.FixedAssets.ToList().FindAll(m => m.AssetTypeId == assetTypeId);
					var myBusinessObjList = new List<BusinessObject.FixedAsset>();
					if(!myObjList.Any())
					{
					    return myBusinessObjList;
					}
					//Re-Map each Entity Object to Business Object
					foreach (var item in myObjList)
					{
					    var myBusinessObj = FixedAssetMapper.Map<FixedAsset, BusinessObject.FixedAsset>(item);
						if(myBusinessObj == null)
						{
						    continue;
						}
					    myBusinessObj.TotalCost = (myBusinessObj.Quantity*myBusinessObj.CostOfPurchase) + myBusinessObj.CostOfTransportationAndInstallation;
						myBusinessObjList.Add(myBusinessObj);
					}
                    if (!myBusinessObjList.Any())
                    {
                        return myBusinessObjList;
                    }
					return myBusinessObjList.OrderBy(m => m.AssetType.Name).ThenBy(m => m.Name).ToList();
				}
			}
			catch (Exception ex)
			{
				ErrorManager.LogApplicationError(ex.StackTrace, ex.Source, ex.Message);
				return new List<BusinessObject.FixedAsset>();
			}
		}
        
        
	}

	//Class File Generated from Code<->Stripper 1.2.0.0 | All Rights Reserved
}
