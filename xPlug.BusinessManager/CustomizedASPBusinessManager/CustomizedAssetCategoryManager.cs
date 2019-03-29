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
	///* Date Generated:	18-09-2013 01:30:43
	///*******************************************************************************


	public partial class AssetCategoryManager
	{
        public int AddAssetCategoryCheckDuplicate(BusinessObject.AssetCategory assetCategory)
        {
            try
            {
                //Re-Map Object to Entity Object
                var myEntityObj = AssetCategoryMapper.Map<BusinessObject.AssetCategory, AssetCategory>(assetCategory);
                if (myEntityObj == null)
                {
                    return -2;
                }
                using (var db = new ExpenseManagerDBEntities())
                {
                    if (db.AssetCategories.Count(m => m.Name.ToLower().Replace(" ", string.Empty) == assetCategory.Name.ToLower().Replace(" ", string.Empty)) > 0)
                    {
                        return -3;
                    }
                    db.AddToAssetCategories(myEntityObj);
                    db.SaveChanges();
                    assetCategory.AssetCategoryId = myEntityObj.AssetCategoryId;
                    return assetCategory.AssetCategoryId;
                }
            }
            catch (Exception ex)
            {
                ErrorManager.LogApplicationError(ex.StackTrace, ex.Source, ex.Message);
                return 0;
            }
        }

        public int UpdateAssetCategoryCheckDuplicate(BusinessObject.AssetCategory assetCategory)
        {
            try
            {
                //Re-Map Object to Entity Object
                var myEntityObj = AssetCategoryMapper.Map<BusinessObject.AssetCategory, AssetCategory>(assetCategory);
                if (myEntityObj == null)
                {
                    return -2;
                }
                using (var db = new ExpenseManagerDBEntities())
                {
                    if (db.AssetCategories.Count(m => m.Name.ToLower().Replace(" ", string.Empty) == assetCategory.Name.ToLower().Replace(" ", string.Empty) && m.AssetCategoryId != assetCategory.AssetCategoryId) > 0)
                    {
                        return -3;
                    }
                    db.AssetCategories.Attach(myEntityObj);
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

        public List<BusinessObject.AssetCategory> GetActiveAssetCategories()
        {
            try
            {
                using (var db = new ExpenseManagerDBEntities())
                {
                    var myObjList = db.AssetCategories.Where(m => m.Status == 1).ToList();
                    var myBusinessObjList = new List<BusinessObject.AssetCategory>();
                    if (!myObjList.Any())
                    {
                        return myBusinessObjList;
                    }
                    //Re-Map each Entity Object to Business Object
                    foreach (var item in myObjList)
                    {
                        var myBusinessObj = AssetCategoryMapper.Map<AssetCategory, BusinessObject.AssetCategory>(item);
                        if (myBusinessObj == null)
                        {
                            continue;
                        }
                        myBusinessObjList.Add(myBusinessObj);
                    }
                    return myBusinessObjList.OrderBy(m => m.Name).ToList();
                }
            }
            catch (Exception ex)
            {
                ErrorManager.LogApplicationError(ex.StackTrace, ex.Source, ex.Message);
                return new List<BusinessObject.AssetCategory>();
            }
        }

        public List<BusinessObject.AssetCategory> GetFilterdAssetCategories()
        {
            try
            {
                using (var db = new ExpenseManagerDBEntities())
                {
                    var myObjList = db.AssetCategories.Where(m => m.Status == 1 && m.AssetTypes.Any()).ToList();
                    var myBusinessObjList = new List<BusinessObject.AssetCategory>();
                    if (!myObjList.Any())
                    {
                        return myBusinessObjList;
                    }
                    //Re-Map each Entity Object to Business Object
                    foreach (var item in myObjList)
                    {
                        var myBusinessObj = AssetCategoryMapper.Map<AssetCategory, BusinessObject.AssetCategory>(item);
                        if (myBusinessObj == null)
                        {
                            continue;
                        }
                        myBusinessObjList.Add(myBusinessObj);
                    }
                    return myBusinessObjList.OrderBy(m => m.Name).ToList();
                }
            }
            catch (Exception ex)
            {
                ErrorManager.LogApplicationError(ex.StackTrace, ex.Source, ex.Message);
                return new List<BusinessObject.AssetCategory>();
            }
        }
	}

    
	//Class File Generated from Code<->Stripper 1.2.0.0 | All Rights Reserved
}
