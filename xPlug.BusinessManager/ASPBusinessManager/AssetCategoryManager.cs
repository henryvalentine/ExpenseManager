using System;
using System.Linq;
using System.Collections.Generic;
using System.Data;
using XPLUG.WEBTOOLS;
using xPlug.BusinessObjectMapper;

using ExpenseManager.EF;


namespace xPlug.BusinessManager
{
	/// Author:	 Oluwaseyi Adegboyega - sadegboyega@xplugng.com
	/// Copyright © 2013. xPlug Technologies Limited. All Rights Reserved
	///*******************************************************************************
	///* Date Created: 	04-06-2013
	///* Date Generated:	18-09-2013 05:32:13
	///*******************************************************************************


	public partial class AssetCategoryManager
	{
		public AssetCategoryManager()
		{
		}

		public int AddAssetCategory(xPlug.BusinessObject.AssetCategory assetCategory)
		{
			try
			{
				//Re-Map Object to Entity Object
				var myEntityObj = AssetCategoryMapper.Map<xPlug.BusinessObject.AssetCategory, AssetCategory>(assetCategory);
				if(myEntityObj == null)
				{return -2;}
				using (var db = new ExpenseManagerDBEntities())
				{
                    if (db.AssetCategories.Any())
                    {
                        if (db.AssetCategories.Count(m => m.Name == myEntityObj.Name) > 0)
                        {
                            return -3;
                        }
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

		public int UpdateAssetCategory(xPlug.BusinessObject.AssetCategory assetCategory)
		{
			try
			{
				//Re-Map Object to Entity Object
				var myEntityObj = AssetCategoryMapper.Map<xPlug.BusinessObject.AssetCategory, AssetCategory>(assetCategory);
				if(myEntityObj == null)
				{return -2;}
				using (var db = new ExpenseManagerDBEntities())
				{

                    if (db.AssetCategories.Any())
                    {
                        if (db.AssetCategories.Count(m => m.Name == myEntityObj.Name && m.AssetCategoryId != myEntityObj.AssetCategoryId) > 0)
                        {
                            return -3;
                        }
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

		public bool DeleteAssetCategory(int assetCategoryId)
		{
			try
			{
				using (var db = new ExpenseManagerDBEntities())
				{
					var myObj = db.AssetCategories.Single(s => s.AssetCategoryId == assetCategoryId);
					if (myObj == null) { return false; };
					db.AssetCategories.DeleteObject(myObj);
					db.SaveChanges();
					return true;
				}
			}
			catch (Exception ex)
			{
				ErrorManager.LogApplicationError(ex.StackTrace, ex.Source, ex.Message);
				return false;
			}
		}

		public xPlug.BusinessObject.AssetCategory GetAssetCategory(int assetCategoryId)
		{
			try
			{
				using (var db = new ExpenseManagerDBEntities())
				{
					var myObj = db.AssetCategories.SingleOrDefault(s => s.AssetCategoryId == assetCategoryId);
					if(myObj == null){return new xPlug.BusinessObject.AssetCategory();}
					//Re-Map Entity Object to Business Object
					var myBusinessObj = AssetCategoryMapper.Map<AssetCategory, xPlug.BusinessObject.AssetCategory>(myObj);
					if(myBusinessObj == null){return new xPlug.BusinessObject.AssetCategory();}
					{return myBusinessObj;}
				}
			}
			catch (Exception ex)
			{
				ErrorManager.LogApplicationError(ex.StackTrace, ex.Source, ex.Message);
				return new xPlug.BusinessObject.AssetCategory();
			}
		}

		public List<xPlug.BusinessObject.AssetCategory> GetAssetCategories()
		{
			try
			{
				using (var db = new ExpenseManagerDBEntities())
				{
					var myObjList = db.AssetCategories.ToList();
					var myBusinessObjList = new List<xPlug.BusinessObject.AssetCategory>();
					if(myObjList == null){return myBusinessObjList;}
					//Re-Map each Entity Object to Business Object
					foreach (var item in myObjList)
					{
					var myBusinessObj = AssetCategoryMapper.Map<AssetCategory, xPlug.BusinessObject.AssetCategory>(item);
						if(myBusinessObj == null){continue;}
						myBusinessObjList.Add(myBusinessObj);
					}
					return myBusinessObjList;
				}
			}
			catch (Exception ex)
			{
				ErrorManager.LogApplicationError(ex.StackTrace, ex.Source, ex.Message);
				return new List<xPlug.BusinessObject.AssetCategory>();
			}
		}

	}


	//Class File Generated from Code<->Stripper 1.2.0.0 | All Rights Reserved
}
