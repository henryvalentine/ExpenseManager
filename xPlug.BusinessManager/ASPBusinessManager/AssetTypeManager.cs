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
	///* Date Generated:	18-09-2013 05:32:11
	///*******************************************************************************


	public partial class AssetTypeManager
	{
		public AssetTypeManager()
		{
		}

		public int AddAssetType(xPlug.BusinessObject.AssetType assetType)
		{
			try
			{
				//Re-Map Object to Entity Object
				var myEntityObj = AssetTypeMapper.Map<xPlug.BusinessObject.AssetType, AssetType>(assetType);
				if(myEntityObj == null)
				{return -2;}
				using (var db = new ExpenseManagerDBEntities())
				{
                    if (db.AssetTypes.Any())
                    {
                        if (db.AssetTypes.Count(m => m.Name == myEntityObj.Name) > 0)
                        {
                            return -3;
                        }
                    }

                    db.AddToAssetTypes(myEntityObj);
					db.SaveChanges();
					assetType.AssetTypeId = myEntityObj.AssetTypeId;
					return assetType.AssetTypeId;
				}
			}
			catch (Exception ex)
			{
				ErrorManager.LogApplicationError(ex.StackTrace, ex.Source, ex.Message);
				return 0;
			}
		}

		public int UpdateAssetType(xPlug.BusinessObject.AssetType assetType)
		{
			try
			{
				//Re-Map Object to Entity Object
				var myEntityObj = AssetTypeMapper.Map<xPlug.BusinessObject.AssetType, AssetType>(assetType);
				if(myEntityObj == null)
				{return 12;}
				using (var db = new ExpenseManagerDBEntities())
				{
                    if(db.AssetTypes.Any())
                    {
                        if(db.AssetTypes.Count(m => m.Name == myEntityObj.Name && m.AssetTypeId != myEntityObj.AssetTypeId) > 0)
                        {
                            return -3;
                        }
                    }
					db.AssetTypes.Attach(myEntityObj);
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

		public bool DeleteAssetType(int assetTypeId)
		{
			try
			{
				using (var db = new ExpenseManagerDBEntities())
				{
					var myObj = db.AssetTypes.Single(s => s.AssetTypeId == assetTypeId);
					if (myObj == null) { return false; };
					db.AssetTypes.DeleteObject(myObj);
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

		public xPlug.BusinessObject.AssetType GetAssetType(int assetTypeId)
		{
			try
			{
				using (var db = new ExpenseManagerDBEntities())
				{
					var myObj = db.AssetTypes.SingleOrDefault(s => s.AssetTypeId == assetTypeId);
					if(myObj == null){return new xPlug.BusinessObject.AssetType();}
					//Re-Map Entity Object to Business Object
					var myBusinessObj = AssetTypeMapper.Map<AssetType, xPlug.BusinessObject.AssetType>(myObj);
					if(myBusinessObj == null){return new xPlug.BusinessObject.AssetType();}
					{return myBusinessObj;}
				}
			}
			catch (Exception ex)
			{
				ErrorManager.LogApplicationError(ex.StackTrace, ex.Source, ex.Message);
				return new xPlug.BusinessObject.AssetType();
			}
		}

		public List<xPlug.BusinessObject.AssetType> GetAssetTypes()
		{
			try
			{
				using (var db = new ExpenseManagerDBEntities())
				{
					var myObjList = db.AssetTypes.ToList();
					var myBusinessObjList = new List<xPlug.BusinessObject.AssetType>();
					if(myObjList == null){return myBusinessObjList;}
					//Re-Map each Entity Object to Business Object
					foreach (var item in myObjList)
					{
					var myBusinessObj = AssetTypeMapper.Map<AssetType, xPlug.BusinessObject.AssetType>(item);
						if(myBusinessObj == null){continue;}
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

		public List<xPlug.BusinessObject.AssetType>  GetAssetTypesByAssetCategoryId(Int32 assetCategoryId)
		{
			try
			{
				using (var db = new ExpenseManagerDBEntities())
				{
					var myObjList = db.AssetTypes.ToList().FindAll(m => m.AssetCategoryId == assetCategoryId);
					var myBusinessObjList = new List<xPlug.BusinessObject.AssetType>();
					if(myObjList == null){return myBusinessObjList;}
					//Re-Map each Entity Object to Business Object
					foreach (var item in myObjList)
					{
					var myBusinessObj = AssetTypeMapper.Map<AssetType, xPlug.BusinessObject.AssetType>(item);
						if(myBusinessObj == null){continue;}
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

	}


	//Class File Generated from Code<->Stripper 1.2.0.0 | All Rights Reserved
}
