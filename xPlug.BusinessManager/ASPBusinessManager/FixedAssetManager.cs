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
	///* Date Generated:	18-09-2013 05:32:15
	///*******************************************************************************


	public partial class FixedAssetManager
	{
		public FixedAssetManager()
		{
		}

		public int AddFixedAsset(xPlug.BusinessObject.FixedAsset fixedAsset)
		{
			try
			{
				//Re-Map Object to Entity Object
				var myEntityObj = FixedAssetMapper.Map<BusinessObject.FixedAsset, FixedAsset>(fixedAsset);
				if(myEntityObj == null)
				{return -2;}
				using (var db = new ExpenseManagerDBEntities())
				{
                    if(db.FixedAssets.Any())
                    {
                        if (db.FixedAssets.Count(m => m.Name.ToLower() == myEntityObj.Name.ToLower() && m.DatePurchased == myEntityObj.DatePurchased) > 0)
                        {
                            if (!string.IsNullOrEmpty(fixedAsset.Model) && !string.IsNullOrEmpty(fixedAsset.Brand) && !string.IsNullOrEmpty(fixedAsset.Make))
                            {
                                if(db.FixedAssets.Count(m => m.Model == myEntityObj.Model && m.Make == myEntityObj.Make && m.Brand == myEntityObj.Brand) > 0)
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

		public int UpdateFixedAsset(BusinessObject.FixedAsset fixedAsset)
		{
			try
			{
				//Re-Map Object to Entity Object
				var myEntityObj = FixedAssetMapper.Map<BusinessObject.FixedAsset, FixedAsset>(fixedAsset);
				if(myEntityObj == null)
				{return -2;}
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

		public bool DeleteFixedAsset(int fixedAssetId)
		{
			try
			{
				using (var db = new ExpenseManagerDBEntities())
				{
					var myObj = db.FixedAssets.Single(s => s.FixedAssetId == fixedAssetId);
					if (myObj == null) { return false; };
					db.FixedAssets.DeleteObject(myObj);
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

		public xPlug.BusinessObject.FixedAsset GetFixedAsset(int fixedAssetId)
		{
			try
			{
				using (var db = new ExpenseManagerDBEntities())
				{
					var myObj = db.FixedAssets.SingleOrDefault(s => s.FixedAssetId == fixedAssetId);
					if(myObj == null){return new xPlug.BusinessObject.FixedAsset();}
					//Re-Map Entity Object to Business Object
					var myBusinessObj = FixedAssetMapper.Map<FixedAsset, xPlug.BusinessObject.FixedAsset>(myObj);
					if(myBusinessObj == null){return new xPlug.BusinessObject.FixedAsset();}
					{return myBusinessObj;}
				}
			}
			catch (Exception ex)
			{
				ErrorManager.LogApplicationError(ex.StackTrace, ex.Source, ex.Message);
				return new xPlug.BusinessObject.FixedAsset();
			}
		}

		public List<xPlug.BusinessObject.FixedAsset> GetFixedAssets()
		{
			try
			{
				using (var db = new ExpenseManagerDBEntities())
				{
					var myObjList = db.FixedAssets.ToList();
					var myBusinessObjList = new List<xPlug.BusinessObject.FixedAsset>();
					if(myObjList == null){return myBusinessObjList;}
					//Re-Map each Entity Object to Business Object
					foreach (var item in myObjList)
					{
					var myBusinessObj = FixedAssetMapper.Map<FixedAsset, xPlug.BusinessObject.FixedAsset>(item);
						if(myBusinessObj == null){continue;}
						myBusinessObjList.Add(myBusinessObj);
					}
					return myBusinessObjList;
				}
			}
			catch (Exception ex)
			{
				ErrorManager.LogApplicationError(ex.StackTrace, ex.Source, ex.Message);
				return new List<xPlug.BusinessObject.FixedAsset>();
			}
		}

		public List<xPlug.BusinessObject.FixedAsset>  GetFixedAssetsByAssetTypeId(Int32 assetTypeId)
		{
			try
			{
				using (var db = new ExpenseManagerDBEntities())
				{
					var myObjList = db.FixedAssets.ToList().FindAll(m => m.AssetTypeId == assetTypeId);
					var myBusinessObjList = new List<xPlug.BusinessObject.FixedAsset>();
					if(myObjList == null){return myBusinessObjList;}
					//Re-Map each Entity Object to Business Object
					foreach (var item in myObjList)
					{
					var myBusinessObj = FixedAssetMapper.Map<FixedAsset, xPlug.BusinessObject.FixedAsset>(item);
						if(myBusinessObj == null){continue;}
						myBusinessObjList.Add(myBusinessObj);
					}
					return myBusinessObjList;
				}
			}
			catch (Exception ex)
			{
				ErrorManager.LogApplicationError(ex.StackTrace, ex.Source, ex.Message);
				return new List<xPlug.BusinessObject.FixedAsset>();
			}
		}

		public List<xPlug.BusinessObject.FixedAsset>  GetFixedAssetsByAssetCategoryId(Int32 assetCategoryId)
		{
			try
			{
				using (var db = new ExpenseManagerDBEntities())
				{
					var myObjList = db.FixedAssets.ToList().FindAll(m => m.AssetCategoryId == assetCategoryId);
					var myBusinessObjList = new List<xPlug.BusinessObject.FixedAsset>();
					if(myObjList == null){return myBusinessObjList;}
					//Re-Map each Entity Object to Business Object
					foreach (var item in myObjList)
					{
					var myBusinessObj = FixedAssetMapper.Map<FixedAsset, xPlug.BusinessObject.FixedAsset>(item);
						if(myBusinessObj == null){continue;}
						myBusinessObjList.Add(myBusinessObj);
					}
					return myBusinessObjList;
				}
			}
			catch (Exception ex)
			{
				ErrorManager.LogApplicationError(ex.StackTrace, ex.Source, ex.Message);
				return new List<xPlug.BusinessObject.FixedAsset>();
			}
		}

	}


	//Class File Generated from Code<->Stripper 1.2.0.0 | All Rights Reserved
}
