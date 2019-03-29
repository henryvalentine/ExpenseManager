using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using ExpenseManager.EF;
using XPLUG.WEBTOOLS;
using xPlug.BusinessObjectMapper;

namespace xPlug.BusinessManager
{
	/// Author:	 Oluwaseyi Adegboyega - sadegboyega@xplugng.com
	/// Copyright © 2013. xPlug Technologies Limited. All Rights Reserved
	///*******************************************************************************
	///* Date Created: 	04-06-2013
	///* Date Generated:	12-10-2013 11:16:48
	///*******************************************************************************


	public partial class DepartmentManager
    {
        public int AddDepartmentCheckDuplicate(BusinessObject.Department department)
        {
            try
            {
                //Re-Map Object to Entity Object
                var myEntityObj = DepartmentMapper.Map<BusinessObject.Department, Department>(department);
                if (myEntityObj == null)
                {
                    return -2;
                }
                using (var db = new ExpenseManagerDBEntities())
                {
                    if (db.Departments.Count(m => m.Name.ToLower().Replace(" ", string.Empty) == department.Name.ToLower().Replace(" ", string.Empty)) > 0)
                    {
                        return -3;
                    }
                    db.AddToDepartments(myEntityObj);
                    db.SaveChanges();
                    department.DepartmentId = myEntityObj.DepartmentId;
                    return department.DepartmentId;
                }
            }
            catch (Exception ex)
            {
                ErrorManager.LogApplicationError(ex.StackTrace, ex.Source, ex.Message);
                return 0;
            }
        }

        public int UpdateDepartmentCheckDuplicate(BusinessObject.Department department)
        {
            try
            {
                //Re-Map Object to Entity Object
                var myEntityObj = DepartmentMapper.Map<BusinessObject.Department, Department>(department);
                if (myEntityObj == null)
                {
                    return -2;
                }
                using (var db = new ExpenseManagerDBEntities())
                {
                    if (db.Departments.Count(m => m.Name.ToLower().Replace(" ", string.Empty) == department.Name.ToLower().Replace(" ", string.Empty) && m.DepartmentId != department.DepartmentId) > 0)
                    {
                        return -3;
                    }
                    db.Departments.Attach(myEntityObj);
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

        public bool DeleteDepartmentCheckReference(int departmentId)
        {
            try
            {
                using (var db = new ExpenseManagerDBEntities())
                {
                    if (db.Units.Any(m => m.UnitId == departmentId))
                    {
                        return false;
                    }
                    var myObj = db.Departments.Single(s => s.DepartmentId == departmentId);
                    if (myObj == null)
                    {
                        return false;
                    }
                    db.Departments.DeleteObject(myObj);
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

        //public Dictionary<List<BusinessObject.Department>, List<BusinessObject.Department>> GetFilteredDepartments()
        //{
        //    try
        //    {
        //        var departmentList = new Dictionary<List<BusinessObject.Department>, List<BusinessObject.Department>>();
        //        var departmentsWithUnApprovedTransactions = new List<BusinessObject.Department>();
        //        var departmentsWithApprovedTransactions = new List<BusinessObject.Department>();
        //        var myBusinessObjList = new List<BusinessObject.ExpenseTransaction>();

        //        using (var db = new ExpenseManagerDBEntities())
        //        {
        //            var myObjList = db.ExpenseTransactions.ToList();

        //            if (!myObjList.Any())
        //            {
        //                return new Dictionary<List<BusinessObject.Department>, List<BusinessObject.Department>>();
        //            }

        //            foreach (var item in myObjList)
        //            {
        //                var myBusinessObj =
        //                    ExpenseTransactionMapper.Map<ExpenseTransaction, BusinessObject.ExpenseTransaction>(item);
        //                if (myBusinessObj == null)
        //                {
        //                    continue;
        //                }

        //                myBusinessObjList.Add(myBusinessObj);
        //            }

        //        }

        //        foreach (var item in myBusinessObjList)
        //        {
        //            if (departmentsWithUnApprovedTransactions.All(m => m.DepartmentId != item.DepartmentId))
        //            {
        //                departmentsWithUnApprovedTransactions.Add(item.Department);
        //            }

        //            if (departmentsWithApprovedTransactions.All(m => m.DepartmentId != item.DepartmentId && item.Status == 1))
        //            {
        //                departmentsWithApprovedTransactions.Add(item.Department);
        //            }
        //        }

        //        if (!departmentsWithUnApprovedTransactions.Any() && !departmentsWithApprovedTransactions.Any())
        //        {
        //            return departmentList;
        //        }
        //        departmentList.Add(departmentsWithUnApprovedTransactions.OrderBy(m => m.Name).ToList(), departmentsWithApprovedTransactions.OrderBy(m => m.Name).ToList());

        //        return departmentList;
        //    }
        //    catch (Exception ex)
        //    {
        //        ErrorManager.LogApplicationError(ex.StackTrace, ex.Source, ex.Message);
        //        return new Dictionary<List<BusinessObject.Department>, List<BusinessObject.Department>>();
        //    }
        //}

        public List<BusinessObject.Department> GetActiveOrderedDepartments()
        {
            try
            {
                using (var db = new ExpenseManagerDBEntities())
                {
                    var myObjList = db.Departments.Where(m => m.Status == 1).ToList();
                    var myBusinessObjList = new List<BusinessObject.Department>();
                    if (!myObjList.Any())
                    {
                        return myBusinessObjList;
                    }
                    //Re-Map each Entity Object to Business Object
                    foreach (var item in myObjList)
                    {
                        var myBusinessObj = DepartmentMapper.Map<Department, BusinessObject.Department>(item);
                        if (myBusinessObj == null) { continue; }
                        myBusinessObjList.Add(myBusinessObj);
                    }
                    return myBusinessObjList.OrderBy(m => m.Name).ToList();
                }
            }
            catch (Exception ex)
            {
                ErrorManager.LogApplicationError(ex.StackTrace, ex.Source, ex.Message);
                return new List<BusinessObject.Department>();
            }
        }

        public List<BusinessObject.Department> GetActiveFilteredOrderedDepartments()
        {
            try
            {
                using (var db = new ExpenseManagerDBEntities())
                {
                    var myObjList = db.Departments.Where(m => m.DepartmentId != 8).ToList();
                    var myBusinessObjList = new List<BusinessObject.Department>();
                    if (!myObjList.Any())
                    {
                        return myBusinessObjList;
                    }
                    //Re-Map each Entity Object to Business Object
                    foreach (var item in myObjList)
                    {
                        var myBusinessObj = DepartmentMapper.Map<Department, BusinessObject.Department>(item);
                        if (myBusinessObj == null) { continue; }
                        myBusinessObjList.Add(myBusinessObj);
                    }
                    return myBusinessObjList.OrderBy(m => m.Name).ToList();
                }
            }
            catch (Exception ex)
            {
                ErrorManager.LogApplicationError(ex.StackTrace, ex.Source, ex.Message);
                return new List<BusinessObject.Department>();
            }
        }

        public List<BusinessObject.Department> GetOrderedDepartments()
        {
            try
            {
                using (var db = new ExpenseManagerDBEntities())
                {
                    var myObjList = db.Departments.ToList();
                    var myBusinessObjList = new List<BusinessObject.Department>();
                    if (!myObjList.Any())
                    {
                        return myBusinessObjList;
                    }
                    //Re-Map each Entity Object to Business Object
                    foreach (var item in myObjList)
                    {
                        var myBusinessObj = DepartmentMapper.Map<Department, BusinessObject.Department>(item);
                        if (myBusinessObj == null) { continue; }
                        myBusinessObjList.Add(myBusinessObj);
                    }
                    return myBusinessObjList.OrderBy(m => m.Name).ToList();
                }
            }
            catch (Exception ex)
            {
                ErrorManager.LogApplicationError(ex.StackTrace, ex.Source, ex.Message);
                return new List<BusinessObject.Department>();
            }
        }
	}


	//Class File Generated from Code<->Stripper 1.2.0.0 | All Rights Reserved
}
