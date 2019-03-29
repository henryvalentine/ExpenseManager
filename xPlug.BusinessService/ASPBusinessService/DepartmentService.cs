using System;
using System.Linq;
using System.Collections.Generic;
using System.Data;
using xPlug.BusinessObject;
using xPlug.BusinessManager;
using kPortal.CoreUtilities;



namespace xPlug.BusinessService
{
	/// Author:	 Oluwaseyi Adegboyega - sadegboyega@xplugng.com
	/// Copyright © 2013. xPlug Technologies Limited. All Rights Reserved
	///*******************************************************************************
	///* Date Created: 	04-06-2013
	///* Date Generated:	25-11-2013 09:26:18
	///*******************************************************************************


	public partial class DepartmentService : MarshalByRefObject
	{
		private readonly DepartmentManager  _departmentManager;
		public DepartmentService()
		{
			_departmentManager = new DepartmentManager();
		}

		public int AddDepartment(Department department)
		{
			try
			{
				return _departmentManager.AddDepartment(department);
			}
			catch (Exception ex)
			{
				ErrorManager.LogApplicationError(ex.StackTrace, ex.Source, ex.Message);
				return 0;
			}
		}

		public bool UpdateDepartment(Department department)
		{
			try
			{
				return _departmentManager.UpdateDepartment(department);
				}
			catch (Exception ex)
			{
				ErrorManager.LogApplicationError(ex.StackTrace, ex.Source, ex.Message);
				return false;
			}
		}

		public bool DeleteDepartment(Int32 departmentId)
		{
			try
			{
				return _departmentManager.DeleteDepartment(departmentId);
				}
			catch (Exception ex)
			{
				ErrorManager.LogApplicationError(ex.StackTrace, ex.Source, ex.Message);
				return false;
			}
		}

		public Department GetDepartment(int departmentId)
		{
			try
			{
				return _departmentManager.GetDepartment(departmentId);
			}
			catch (Exception ex)
			{
				ErrorManager.LogApplicationError(ex.StackTrace, ex.Source, ex.Message);
				return new Department();
			}
		}

		public List<Department> GetDepartments()
		{
			try
			{
				var objList = new List<Department>();
				objList = _departmentManager.GetDepartments();
				if(objList == null) {return  new List<Department>();}
				return objList;
			}
			catch (Exception ex)
			{
				ErrorManager.LogApplicationError(ex.StackTrace, ex.Source, ex.Message);
				return new List<Department>();
			}
		}

	}


	//Class File Generated from Code<->Stripper 1.2.0.0 | All Rights Reserved
}
