using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using ExpenseManager.EF;
using xPlug.BusinessObjectMapper;
using XPLUG.WEBTOOLS;

namespace xPlug.BusinessManager
{
	/// Author:	 Oluwaseyi Adegboyega - sadegboyega@xplugng.com
	/// Copyright © 2013. xPlug Technologies Limited. All Rights Reserved
	///*******************************************************************************
	///* Date Created: 	04-06-2013
	///* Date Generated:	25-11-2013 09:26:27
	///*******************************************************************************


	public partial class BeneficiaryManager
	{
	    public int AddBeneficiary(BusinessObject.Beneficiary beneficiary)
		{
			try
			{
				//Re-Map Object to Entity Object
				var myEntityObj = BeneficiaryMapper.Map<BusinessObject.Beneficiary, Beneficiary>(beneficiary);
				if(myEntityObj == null)
				{return -2;}
				using (var db = new ExpenseManagerDBEntities())
				{
					db.AddToBeneficiaries(myEntityObj);
					db.SaveChanges();
					beneficiary.BeneficiaryId = myEntityObj.BeneficiaryId;
					return beneficiary.BeneficiaryId;
				}
			}
			catch (Exception ex)
			{
				ErrorManager.LogApplicationError(ex.StackTrace, ex.Source, ex.Message);
				return 0;
			}
		}

		public bool UpdateBeneficiary(xPlug.BusinessObject.Beneficiary beneficiary)
		{
			try
			{
				//Re-Map Object to Entity Object
				var myEntityObj = BeneficiaryMapper.Map<xPlug.BusinessObject.Beneficiary, Beneficiary>(beneficiary);
				if(myEntityObj == null)
				{return false;}
				using (var db = new ExpenseManagerDBEntities())
				{
					db.Beneficiaries.Attach(myEntityObj);
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

		public bool DeleteBeneficiary(int beneficiaryId)
		{
			try
			{
				using (var db = new ExpenseManagerDBEntities())
				{
					var myObj = db.Beneficiaries.Single(s => s.BeneficiaryId == beneficiaryId);
					if (myObj == null) { return false; };
					db.Beneficiaries.DeleteObject(myObj);
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

		public BusinessObject.Beneficiary GetBeneficiary(int beneficiaryId)
		{
			try
			{
				using (var db = new ExpenseManagerDBEntities())
				{
					var myObj = db.Beneficiaries.SingleOrDefault(s => s.BeneficiaryId == beneficiaryId);
					if(myObj == null){return new xPlug.BusinessObject.Beneficiary();}
					//Re-Map Entity Object to Business Object
					var myBusinessObj = BeneficiaryMapper.Map<Beneficiary, xPlug.BusinessObject.Beneficiary>(myObj);
					if(myBusinessObj == null){return new xPlug.BusinessObject.Beneficiary();}
					{return myBusinessObj;}
				}
			}
			catch (Exception ex)
			{
				ErrorManager.LogApplicationError(ex.StackTrace, ex.Source, ex.Message);
				return new xPlug.BusinessObject.Beneficiary();
			}
		}

		public List<BusinessObject.Beneficiary> GetBeneficiaries()
		{
			try
			{
				using (var db = new ExpenseManagerDBEntities())
				{
					var myObjList = db.Beneficiaries.ToList();
					var myBusinessObjList = new List<xPlug.BusinessObject.Beneficiary>();
					if(myObjList == null){return myBusinessObjList;}
					//Re-Map each Entity Object to Business Object
					foreach (var item in myObjList)
					{
					var myBusinessObj = BeneficiaryMapper.Map<Beneficiary, xPlug.BusinessObject.Beneficiary>(item);
						if(myBusinessObj == null){continue;}
						myBusinessObjList.Add(myBusinessObj);
					}
					return myBusinessObjList;
				}
			}
			catch (Exception ex)
			{
				ErrorManager.LogApplicationError(ex.StackTrace, ex.Source, ex.Message);
				return new List<xPlug.BusinessObject.Beneficiary>();
			}
		}

		public List<BusinessObject.Beneficiary>  GetBeneficiariesByDepartmentId(Int32 departmentId)
		{
			try
			{
				using (var db = new ExpenseManagerDBEntities())
				{
					var myObjList = db.Beneficiaries.ToList().FindAll(m => m.DepartmentId == departmentId);
					var myBusinessObjList = new List<BusinessObject.Beneficiary>();
					if(!myObjList.Any())
					{
					    return myBusinessObjList;
					}
					//Re-Map each Entity Object to Business Object
					foreach (var item in myObjList)
					{
					var myBusinessObj = BeneficiaryMapper.Map<Beneficiary, BusinessObject.Beneficiary>(item);
						if(myBusinessObj == null){continue;}
						myBusinessObjList.Add(myBusinessObj);
					}
					return myBusinessObjList;
				}
			}
			catch (Exception ex)
			{
				ErrorManager.LogApplicationError(ex.StackTrace, ex.Source, ex.Message);
				return new List<BusinessObject.Beneficiary>();
			}
		}

		public List<BusinessObject.Beneficiary>  GetBeneficiariesByUnitId(Int32 unitId)
		{
			try
			{
				using (var db = new ExpenseManagerDBEntities())
				{
					var myObjList = db.Beneficiaries.ToList().FindAll(m => m.UnitId == unitId);
					var myBusinessObjList = new List<xPlug.BusinessObject.Beneficiary>();
					if(myObjList == null){return myBusinessObjList;}
					//Re-Map each Entity Object to Business Object
					foreach (var item in myObjList)
					{
					var myBusinessObj = BeneficiaryMapper.Map<Beneficiary, xPlug.BusinessObject.Beneficiary>(item);
						if(myBusinessObj == null){continue;}
						myBusinessObjList.Add(myBusinessObj);
					}
					return myBusinessObjList;
				}
			}
			catch (Exception ex)
			{
				ErrorManager.LogApplicationError(ex.StackTrace, ex.Source, ex.Message);
				return new List<xPlug.BusinessObject.Beneficiary>();
			}
		}

		public List<xPlug.BusinessObject.Beneficiary>  GetBeneficiariesByBeneficiaryTypeId(Int32 beneficiaryTypeId)
		{
			try
			{
				using (var db = new ExpenseManagerDBEntities())
				{
					var myObjList = db.Beneficiaries.ToList().FindAll(m => m.BeneficiaryTypeId == beneficiaryTypeId);
					var myBusinessObjList = new List<xPlug.BusinessObject.Beneficiary>();
					if(myObjList == null){return myBusinessObjList;}
					//Re-Map each Entity Object to Business Object
					foreach (var item in myObjList)
					{
					var myBusinessObj = BeneficiaryMapper.Map<Beneficiary, xPlug.BusinessObject.Beneficiary>(item);
						if(myBusinessObj == null){continue;}
						myBusinessObjList.Add(myBusinessObj);
					}
					return myBusinessObjList;
				}
			}
			catch (Exception ex)
			{
				ErrorManager.LogApplicationError(ex.StackTrace, ex.Source, ex.Message);
				return new List<xPlug.BusinessObject.Beneficiary>();
			}
		}

	}


	//Class File Generated from Code<->Stripper 1.2.0.0 | All Rights Reserved
}
