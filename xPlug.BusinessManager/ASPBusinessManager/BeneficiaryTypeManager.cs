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
	///* Date Generated:	25-11-2013 09:26:28
	///*******************************************************************************


	public partial class BeneficiaryTypeManager
	{
		public BeneficiaryTypeManager()
		{
		}

		public int AddBeneficiaryType(xPlug.BusinessObject.BeneficiaryType beneficiaryType)
		{
			try
			{
				//Re-Map Object to Entity Object
				var myEntityObj = BeneficiaryTypeMapper.Map<xPlug.BusinessObject.BeneficiaryType, BeneficiaryType>(beneficiaryType);
				if(myEntityObj == null)
				{return -2;}
				using (var db = new ExpenseManagerDBEntities())
				{
					db.AddToBeneficiaryTypes(myEntityObj);
					db.SaveChanges();
					beneficiaryType.BeneficiaryTypeId = myEntityObj.BeneficiaryTypeId;
					return beneficiaryType.BeneficiaryTypeId;
				}
			}
			catch (Exception ex)
			{
				ErrorManager.LogApplicationError(ex.StackTrace, ex.Source, ex.Message);
				return 0;
			}
		}

		public bool UpdateBeneficiaryType(xPlug.BusinessObject.BeneficiaryType beneficiaryType)
		{
			try
			{
				//Re-Map Object to Entity Object
				var myEntityObj = BeneficiaryTypeMapper.Map<xPlug.BusinessObject.BeneficiaryType, BeneficiaryType>(beneficiaryType);
				if(myEntityObj == null)
				{return false;}
				using (var db = new ExpenseManagerDBEntities())
				{
					db.BeneficiaryTypes.Attach(myEntityObj);
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

		public bool DeleteBeneficiaryType(int beneficiaryTypeId)
		{
			try
			{
				using (var db = new ExpenseManagerDBEntities())
				{
					var myObj = db.BeneficiaryTypes.Single(s => s.BeneficiaryTypeId == beneficiaryTypeId);
					if (myObj == null) { return false; };
					db.BeneficiaryTypes.DeleteObject(myObj);
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

		public xPlug.BusinessObject.BeneficiaryType GetBeneficiaryType(int beneficiaryTypeId)
		{
			try
			{
				using (var db = new ExpenseManagerDBEntities())
				{
					var myObj = db.BeneficiaryTypes.SingleOrDefault(s => s.BeneficiaryTypeId == beneficiaryTypeId);
					if(myObj == null){return new xPlug.BusinessObject.BeneficiaryType();}
					//Re-Map Entity Object to Business Object
					var myBusinessObj = BeneficiaryTypeMapper.Map<BeneficiaryType, xPlug.BusinessObject.BeneficiaryType>(myObj);
					if(myBusinessObj == null){return new xPlug.BusinessObject.BeneficiaryType();}
					{return myBusinessObj;}
				}
			}
			catch (Exception ex)
			{
				ErrorManager.LogApplicationError(ex.StackTrace, ex.Source, ex.Message);
				return new BusinessObject.BeneficiaryType();
			}
		}

		public List<xPlug.BusinessObject.BeneficiaryType> GetBeneficiaryTypes()
		{
			try
			{
				using (var db = new ExpenseManagerDBEntities())
				{
					var myObjList = db.BeneficiaryTypes.ToList();
					var myBusinessObjList = new List<xPlug.BusinessObject.BeneficiaryType>();
					if(myObjList == null){return myBusinessObjList;}
					//Re-Map each Entity Object to Business Object
					foreach (var item in myObjList)
					{
					var myBusinessObj = BeneficiaryTypeMapper.Map<BeneficiaryType, xPlug.BusinessObject.BeneficiaryType>(item);
						if(myBusinessObj == null){continue;}
						myBusinessObjList.Add(myBusinessObj);
					}
					return myBusinessObjList;
				}
			}
			catch (Exception ex)
			{
				ErrorManager.LogApplicationError(ex.StackTrace, ex.Source, ex.Message);
				return new List<xPlug.BusinessObject.BeneficiaryType>();
			}
		}

	}


	//Class File Generated from Code<->Stripper 1.2.0.0 | All Rights Reserved
}
