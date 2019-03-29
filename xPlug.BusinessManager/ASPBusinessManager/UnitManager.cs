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
	///* Date Generated:	25-11-2013 09:26:18
	///*******************************************************************************


	public partial class UnitManager
	{
		public UnitManager()
		{
		}

		public int AddUnit(xPlug.BusinessObject.Unit unit)
		{
			try
			{
				//Re-Map Object to Entity Object
				var myEntityObj = UnitMapper.Map<xPlug.BusinessObject.Unit, Unit>(unit);
				if(myEntityObj == null)
				{return -2;}
				using (var db = new ExpenseManagerDBEntities())
				{
					db.AddToUnits(myEntityObj);
					db.SaveChanges();
					unit.UnitId = myEntityObj.UnitId;
					return unit.UnitId;
				}
			}
			catch (Exception ex)
			{
				ErrorManager.LogApplicationError(ex.StackTrace, ex.Source, ex.Message);
				return 0;
			}
		}

		public bool UpdateUnit(xPlug.BusinessObject.Unit unit)
		{
			try
			{
				//Re-Map Object to Entity Object
				var myEntityObj = UnitMapper.Map<xPlug.BusinessObject.Unit, Unit>(unit);
				if(myEntityObj == null)
				{return false;}
				using (var db = new ExpenseManagerDBEntities())
				{
					db.Units.Attach(myEntityObj);
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

		public bool DeleteUnit(int unitId)
		{
			try
			{
				using (var db = new ExpenseManagerDBEntities())
				{
					var myObj = db.Units.Single(s => s.UnitId == unitId);
					if (myObj == null) { return false; };
					db.Units.DeleteObject(myObj);
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

		public xPlug.BusinessObject.Unit GetUnit(int unitId)
		{
			try
			{
				using (var db = new ExpenseManagerDBEntities())
				{
					var myObj = db.Units.SingleOrDefault(s => s.UnitId == unitId);
					if(myObj == null){return new xPlug.BusinessObject.Unit();}
					//Re-Map Entity Object to Business Object
					var myBusinessObj = UnitMapper.Map<Unit, xPlug.BusinessObject.Unit>(myObj);
					if(myBusinessObj == null){return new xPlug.BusinessObject.Unit();}
					{return myBusinessObj;}
				}
			}
			catch (Exception ex)
			{
				ErrorManager.LogApplicationError(ex.StackTrace, ex.Source, ex.Message);
				return new xPlug.BusinessObject.Unit();
			}
		}

		public List<xPlug.BusinessObject.Unit> GetUnits()
		{
			try
			{
				using (var db = new ExpenseManagerDBEntities())
				{
					var myObjList = db.Units.ToList();
					var myBusinessObjList = new List<xPlug.BusinessObject.Unit>();
					if(myObjList == null){return myBusinessObjList;}
					//Re-Map each Entity Object to Business Object
					foreach (var item in myObjList)
					{
					var myBusinessObj = UnitMapper.Map<Unit, xPlug.BusinessObject.Unit>(item);
						if(myBusinessObj == null){continue;}
						myBusinessObjList.Add(myBusinessObj);
					}
					return myBusinessObjList;
				}
			}
			catch (Exception ex)
			{
				ErrorManager.LogApplicationError(ex.StackTrace, ex.Source, ex.Message);
				return new List<xPlug.BusinessObject.Unit>();
			}
		}

		public List<xPlug.BusinessObject.Unit>  GetUnitsByDepartmentId(Int32 departmentId)
		{
			try
			{
				using (var db = new ExpenseManagerDBEntities())
				{
					var myObjList = db.Units.ToList().FindAll(m => m.DepartmentId == departmentId);
					var myBusinessObjList = new List<xPlug.BusinessObject.Unit>();
					if(myObjList == null){return myBusinessObjList;}
					//Re-Map each Entity Object to Business Object
					foreach (var item in myObjList)
					{
					var myBusinessObj = UnitMapper.Map<Unit, xPlug.BusinessObject.Unit>(item);
						if(myBusinessObj == null){continue;}
						myBusinessObjList.Add(myBusinessObj);
					}
					return myBusinessObjList;
				}
			}
			catch (Exception ex)
			{
				ErrorManager.LogApplicationError(ex.StackTrace, ex.Source, ex.Message);
				return new List<xPlug.BusinessObject.Unit>();
			}
		}

	}


	//Class File Generated from Code<->Stripper 1.2.0.0 | All Rights Reserved
}
