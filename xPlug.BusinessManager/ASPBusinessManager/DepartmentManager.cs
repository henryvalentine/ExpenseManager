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


	public partial class DepartmentManager
	{
		public DepartmentManager()
		{
		}

		public int AddDepartment(xPlug.BusinessObject.Department department)
		{
			try
			{
				//Re-Map Object to Entity Object
				var myEntityObj = DepartmentMapper.Map<xPlug.BusinessObject.Department, Department>(department);
				if(myEntityObj == null)
				{return -2;}
				using (var db = new ExpenseManagerDBEntities())
				{
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

		public bool UpdateDepartment(xPlug.BusinessObject.Department department)
		{
			try
			{
				//Re-Map Object to Entity Object
				var myEntityObj = DepartmentMapper.Map<xPlug.BusinessObject.Department, Department>(department);
				if(myEntityObj == null)
				{return false;}
				using (var db = new ExpenseManagerDBEntities())
				{
					db.Departments.Attach(myEntityObj);
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

		public bool DeleteDepartment(int departmentId)
		{
			try
			{
				using (var db = new ExpenseManagerDBEntities())
				{
					var myObj = db.Departments.Single(s => s.DepartmentId == departmentId);
					if (myObj == null) { return false; };
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

		public xPlug.BusinessObject.Department GetDepartment(int departmentId)
		{
			try
			{
				using (var db = new ExpenseManagerDBEntities())
				{
					var myObj = db.Departments.SingleOrDefault(s => s.DepartmentId == departmentId);
					if(myObj == null){return new xPlug.BusinessObject.Department();}
					//Re-Map Entity Object to Business Object
					var myBusinessObj = DepartmentMapper.Map<Department, xPlug.BusinessObject.Department>(myObj);
					if(myBusinessObj == null){return new xPlug.BusinessObject.Department();}
					{return myBusinessObj;}
				}
			}
			catch (Exception ex)
			{
				ErrorManager.LogApplicationError(ex.StackTrace, ex.Source, ex.Message);
				return new xPlug.BusinessObject.Department();
			}
		}

		public List<xPlug.BusinessObject.Department> GetDepartments()
		{
			try
			{
				using (var db = new ExpenseManagerDBEntities())
				{
					var myObjList = db.Departments.ToList();
					var myBusinessObjList = new List<xPlug.BusinessObject.Department>();
					if(myObjList == null){return myBusinessObjList;}
					//Re-Map each Entity Object to Business Object
					foreach (var item in myObjList)
					{
					var myBusinessObj = DepartmentMapper.Map<Department, xPlug.BusinessObject.Department>(item);
						if(myBusinessObj == null){continue;}
						myBusinessObjList.Add(myBusinessObj);
					}
					return myBusinessObjList;
				}
			}
			catch (Exception ex)
			{
				ErrorManager.LogApplicationError(ex.StackTrace, ex.Source, ex.Message);
				return new List<xPlug.BusinessObject.Department>();
			}
		}

	}


	//Class File Generated from Code<->Stripper 1.2.0.0 | All Rights Reserved
}
