using System;
using System.Collections.Generic;
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
	///* Date Generated:	18-09-2013 01:30:41
	///*******************************************************************************


	public partial class AssetTypeManager
	{
        public List<BusinessObject.AssetType> GetAssetTypesByActiveAssetCategory(int assetCategoryId)
      {
          try
          {
              using (var db = new ExpenseManagerDBEntities())
              {
                  var myObjList = db.AssetTypes.Where(m => m.AssetCategoryId == assetCategoryId && m.AssetCategory.Status == 1).ToList();
                  var myBusinessObjList = new List<BusinessObject.AssetType>();
                  if (!myObjList.Any())
                  {
                      return myBusinessObjList;
                  }
                  //Re-Map each Entity Object to Business Object
                  foreach (var item in myObjList)
                  {
                      var myBusinessObj = AssetTypeMapper.Map<AssetType, BusinessObject.AssetType>(item);
                      if (myBusinessObj == null)
                      {
                          continue;
                      }
                      myBusinessObjList.Add(myBusinessObj);
                  }
                  return myBusinessObjList;
              }
          }
          catch (Exception ex)
          {
              ErrorManager.LogApplicationError(ex.StackTrace, ex.Source, ex.Message);
              return new List<xPlug.BusinessObject.AssetType>();
          }
      }

        public List<BusinessObject.AssetType> GetAllAssetTypes()
        {
            try
            {
                using (var db = new ExpenseManagerDBEntities())
                {
                    var myObjList = db.AssetTypes.ToList();
                    var myBusinessObjList = new List<BusinessObject.AssetType>();
                    if (!myObjList.Any())
                    {
                        return myBusinessObjList;
                    }
                    //Re-Map each Entity Object to Business Object
                    foreach (var item in myObjList)
                    {
                        var myBusinessObj = AssetTypeMapper.Map<AssetType, BusinessObject.AssetType>(item);
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
                return new List<xPlug.BusinessObject.AssetType>();
            }
        }

        public BusinessObject.AssetType GetLastInsertedAssetTypeByAssetCategory(int assetCategoryId)
         {
           try
            {
              using (var db = new ExpenseManagerDBEntities())
              {
                  var myObjList = db.AssetTypes.Where(m => m.AssetCategoryId == assetCategoryId).ToList();
                  var myBusinessObjList = new List<BusinessObject.AssetType>();
                  if (!myObjList.Any())
                  {
                      return new BusinessObject.AssetType();
                  }
                  //Re-Map each Entity Object to Business Object
                  foreach (var item in myObjList)
                  {
                      var myBusinessObj = AssetTypeMapper.Map<AssetType, BusinessObject.AssetType>(item);
                      
                      if (myBusinessObj == null)
                      {
                          continue;
                      }

                      myBusinessObjList.Add(myBusinessObj);
                  }

                  if (!myBusinessObjList.Any())
                  {
                      return new BusinessObject.AssetType();
                  }
                  var obj = myBusinessObjList.OrderByDescending(m => m.Code).ElementAt(0);
                  return obj;
              }
          }
          catch (Exception ex)
          {
              ErrorManager.LogApplicationError(ex.StackTrace, ex.Source, ex.Message);
              return new BusinessObject.AssetType();
          }
      }

        public List<BusinessObject.AssetType> GetOrderedAssetTypeByAssetCategory(int assetCategoryId)
        {
            try
            {
                using (var db = new ExpenseManagerDBEntities())
                {
                    var myObjList = db.AssetTypes.Where(m => m.AssetCategoryId == assetCategoryId).ToList();
                    var myBusinessObjList = new List<BusinessObject.AssetType>();
                    if (!myObjList.Any())
                    {
                        return new List<BusinessObject.AssetType>();
                    }
                    //Re-Map each Entity Object to Business Object
                    foreach (var item in myObjList)
                    {
                        var myBusinessObj = AssetTypeMapper.Map<AssetType, BusinessObject.AssetType>(item);

                        if (myBusinessObj == null)
                        {
                            continue;
                        }

                        myBusinessObjList.Add(myBusinessObj);
                    }

                    if (!myBusinessObjList.Any())
                    {
                        return new List<BusinessObject.AssetType>();
                    }
                    return myBusinessObjList.OrderByDescending(m => m.Name).ToList();
                   
                }
            }
            catch (Exception ex)
            {
                ErrorManager.LogApplicationError(ex.StackTrace, ex.Source, ex.Message);
                return new List<BusinessObject.AssetType>();
            }
        }
	}

    
	//Class File Generated from Code<->Stripper 1.2.0.0 | All Rights Reserved
}
