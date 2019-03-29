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
	///* Date Generated:	04-11-2013 03:10:17
	///*******************************************************************************


	public partial class UnitManager
	{
        public List<BusinessObject.Unit> GetActiveOrderedUnitsByDepartment(int departmentId)
        {
            try
            {
                using (var db = new ExpenseManagerDBEntities())
                {
                    var myObjList = db.Units.Where(m => m.DepartmentId == departmentId && m.Status == 1).ToList();
                    var myBusinessObjList = new List<BusinessObject.Unit>();
                    if (!myObjList.Any())
                    {
                        return myBusinessObjList;
                    }
                    //Re-Map each Entity Object to Business Object
                    foreach (var item in myObjList)
                    {
                        var myBusinessObj = UnitMapper.Map<Unit, BusinessObject.Unit>(item);
                        if (myBusinessObj == null) { continue; }
                        myBusinessObjList.Add(myBusinessObj);
                    }
                    return myBusinessObjList.OrderBy(m => m.Name).ToList();
                }
            }
            catch (Exception ex)
            {
                ErrorManager.LogApplicationError(ex.StackTrace, ex.Source, ex.Message);
                return new List<BusinessObject.Unit>();
            }
        }
        public List<BusinessObject.Unit> GetAllOrderedUnits()
        {
            try
            {
                using (var db = new ExpenseManagerDBEntities())
                {
                    var myObjList = db.Units.ToList();
                    var myBusinessObjList = new List<BusinessObject.Unit>();
                    if (!myObjList.Any())
                    {
                        return myBusinessObjList;
                    }
                    //Re-Map each Entity Object to Business Object
                    foreach (var item in myObjList)
                    {
                        var myBusinessObj = UnitMapper.Map<Unit, BusinessObject.Unit>(item);
                        if (myBusinessObj == null) { continue; }
                        myBusinessObjList.Add(myBusinessObj);
                    }
                    return myBusinessObjList.OrderBy(m => m.Name).ToList();
                }
            }
            catch (Exception ex)
            {
                ErrorManager.LogApplicationError(ex.StackTrace, ex.Source, ex.Message);
                return new List<BusinessObject.Unit>();
            }
        }
        public List<BusinessObject.Unit> GetActiveFilteredOrderedUnits()
        {
            try
            {
                using (var db = new ExpenseManagerDBEntities())
                {
                    var myObjList = db.Units.Where(m => m.DepartmentId != 8).ToList();
                    var myBusinessObjList = new List<BusinessObject.Unit>();
                    if (!myObjList.Any())
                    {
                        return myBusinessObjList;
                    }
                    //Re-Map each Entity Object to Business Object
                    foreach (var item in myObjList)
                    {
                        var myBusinessObj = DepartmentMapper.Map<Unit, BusinessObject.Unit>(item);
                        if (myBusinessObj == null) { continue; }
                        myBusinessObjList.Add(myBusinessObj);
                    }
                    return myBusinessObjList.OrderBy(m => m.Name).ToList();
                }
            }
            catch (Exception ex)
            {
                ErrorManager.LogApplicationError(ex.StackTrace, ex.Source, ex.Message);
                return new List<BusinessObject.Unit>();
            }
        }
        public int UpdateUnitCheckDuplicate(BusinessObject.Unit unit)
        {
            try
            {
                //Re-Map Object to Entity Object
                var myEntityObj = UnitMapper.Map<BusinessObject.Unit, Unit>(unit);
                if (myEntityObj == null)
                { return 0; }
                using (var db = new ExpenseManagerDBEntities())
                {
                    if (db.Units.Count(m => m.Name.ToLower().Replace(" ", string.Empty) == myEntityObj.Name.ToLower().Replace(" ", string.Empty) && m.UnitId != myEntityObj.UnitId) > 0)
                    {
                        return -3;
                    }
                    db.Units.Attach(myEntityObj);
                    db.ObjectStateManager.ChangeObjectState(myEntityObj, EntityState.Modified);
                    db.SaveChanges();
                    return myEntityObj.UnitId;
                }
            }
            catch (Exception ex)
            {
                ErrorManager.LogApplicationError(ex.StackTrace, ex.Source, ex.Message);
                return 0;
            }
        }
        public int AddUnitCheckDuplicate(BusinessObject.Unit unit)
        {
            try
            {
                //Re-Map Object to Entity Object
                var myEntityObj = UnitMapper.Map<BusinessObject.Unit, Unit>(unit);
                if (myEntityObj == null)
                { return 0; }
                using (var db = new ExpenseManagerDBEntities())
                {
                    if (db.Units.Count(m => m.Name.ToLower().Replace(" ", string.Empty) == myEntityObj.Name.ToLower().Replace(" ", string.Empty)) > 0)
                    {
                        return -3;
                    }

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
	}


	//Class File Generated from Code<->Stripper 1.2.0.0 | All Rights Reserved
}
