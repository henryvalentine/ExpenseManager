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
	///* Date Generated:	18-09-2013 05:32:17
	///*******************************************************************************


	public partial class LiquidAssetManager
	{
		public LiquidAssetManager()
		{
		}

		public int AddLiquidAsset(xPlug.BusinessObject.LiquidAsset liquidAsset)
		{
			try
			{
				//Re-Map Object to Entity Object
				var myEntityObj = LiquidAssetMapper.Map<xPlug.BusinessObject.LiquidAsset, LiquidAsset>(liquidAsset);
				if(myEntityObj == null)
				{return -2;}
				using (var db = new ExpenseManagerDBEntities())
				{
					db.AddToLiquidAssets(myEntityObj);
					db.SaveChanges();
					liquidAsset.LiquidAssetId = myEntityObj.LiquidAssetId;
					return liquidAsset.LiquidAssetId;
				}
			}
			catch (Exception ex)
			{
				ErrorManager.LogApplicationError(ex.StackTrace, ex.Source, ex.Message);
				return 0;
			}
		}

		public bool UpdateLiquidAsset(xPlug.BusinessObject.LiquidAsset liquidAsset)
		{
			try
			{
				//Re-Map Object to Entity Object
				var myEntityObj = LiquidAssetMapper.Map<xPlug.BusinessObject.LiquidAsset, LiquidAsset>(liquidAsset);
				if(myEntityObj == null)
				{return false;}
				using (var db = new ExpenseManagerDBEntities())
				{
					db.LiquidAssets.Attach(myEntityObj);
					 db.ObjectStateManager.ChangeObjectState(myEntityObj, EntityState.Modified);
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

		public bool DeleteLiquidAsset(int liquidAssetId)
		{
			try
			{
				using (var db = new ExpenseManagerDBEntities())
				{
					var myObj = db.LiquidAssets.Single(s => s.LiquidAssetId == liquidAssetId);
					if (myObj == null) { return false; };
					db.LiquidAssets.DeleteObject(myObj);
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

		public xPlug.BusinessObject.LiquidAsset GetLiquidAsset(int liquidAssetId)
		{
			try
			{
				using (var db = new ExpenseManagerDBEntities())
				{
					var myObj = db.LiquidAssets.SingleOrDefault(s => s.LiquidAssetId == liquidAssetId);
					if(myObj == null){return new xPlug.BusinessObject.LiquidAsset();}
					//Re-Map Entity Object to Business Object
					var myBusinessObj = LiquidAssetMapper.Map<LiquidAsset, xPlug.BusinessObject.LiquidAsset>(myObj);
					if(myBusinessObj == null){return new xPlug.BusinessObject.LiquidAsset();}
					{return myBusinessObj;}
				}
			}
			catch (Exception ex)
			{
				ErrorManager.LogApplicationError(ex.StackTrace, ex.Source, ex.Message);
				return new xPlug.BusinessObject.LiquidAsset();
			}
		}

		public List<xPlug.BusinessObject.LiquidAsset> GetLiquidAssets()
		{
			try
			{
				using (var db = new ExpenseManagerDBEntities())
				{
					var myObjList = db.LiquidAssets.ToList();
					var myBusinessObjList = new List<xPlug.BusinessObject.LiquidAsset>();
					if(myObjList == null){return myBusinessObjList;}
					//Re-Map each Entity Object to Business Object
					foreach (var item in myObjList)
					{
					var myBusinessObj = LiquidAssetMapper.Map<LiquidAsset, xPlug.BusinessObject.LiquidAsset>(item);
						if(myBusinessObj == null){continue;}
						myBusinessObjList.Add(myBusinessObj);
					}
					return myBusinessObjList;
				}
			}
			catch (Exception ex)
			{
				ErrorManager.LogApplicationError(ex.StackTrace, ex.Source, ex.Message);
				return new List<xPlug.BusinessObject.LiquidAsset>();
			}
		}

		public List<xPlug.BusinessObject.LiquidAsset>  GetLiquidAssetsByAssetTypeId(Int32 assetTypeId)
		{
			try
			{
				using (var db = new ExpenseManagerDBEntities())
				{
					var myObjList = db.LiquidAssets.ToList().FindAll(m => m.AssetTypeId == assetTypeId);
					var myBusinessObjList = new List<xPlug.BusinessObject.LiquidAsset>();
					if(myObjList == null){return myBusinessObjList;}
					//Re-Map each Entity Object to Business Object
					foreach (var item in myObjList)
					{
					var myBusinessObj = LiquidAssetMapper.Map<LiquidAsset, xPlug.BusinessObject.LiquidAsset>(item);
						if(myBusinessObj == null){continue;}
						myBusinessObjList.Add(myBusinessObj);
					}
					return myBusinessObjList;
				}
			}
			catch (Exception ex)
			{
				ErrorManager.LogApplicationError(ex.StackTrace, ex.Source, ex.Message);
				return new List<xPlug.BusinessObject.LiquidAsset>();
			}
		}

		public List<xPlug.BusinessObject.LiquidAsset>  GetLiquidAssetsByAssetCategoryId(Int32 assetCategoryId)
		{
			try
			{
				using (var db = new ExpenseManagerDBEntities())
				{
					var myObjList = db.LiquidAssets.ToList().FindAll(m => m.AssetCategoryId == assetCategoryId);
					var myBusinessObjList = new List<xPlug.BusinessObject.LiquidAsset>();
					if(myObjList == null){return myBusinessObjList;}
					//Re-Map each Entity Object to Business Object
					foreach (var item in myObjList)
					{
					var myBusinessObj = LiquidAssetMapper.Map<LiquidAsset, xPlug.BusinessObject.LiquidAsset>(item);
						if(myBusinessObj == null){continue;}
						myBusinessObjList.Add(myBusinessObj);
					}
					return myBusinessObjList;
				}
			}
			catch (Exception ex)
			{
				ErrorManager.LogApplicationError(ex.StackTrace, ex.Source, ex.Message);
				return new List<xPlug.BusinessObject.LiquidAsset>();
			}
		}

	}


	//Class File Generated from Code<->Stripper 1.2.0.0 | All Rights Reserved
}
