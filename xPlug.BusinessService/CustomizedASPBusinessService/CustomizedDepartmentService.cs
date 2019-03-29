using System;
using System.Collections.Generic;
using System.Linq;
using XPLUG.WEBTOOLS;
using xPlug.BusinessObject;

namespace xPlug.BusinessService
{
	/// Author:	 Oluwaseyi Adegboyega - sadegboyega@xplugng.com
	/// Copyright © 2013. xPlug Technologies Limited. All Rights Reserved
	///*******************************************************************************
	///* Date Created: 	04-06-2013
	///* Date Generated:	12-10-2013 11:16:48
	///*******************************************************************************


	public partial class DepartmentService
	{
        //public Dictionary<List<Department>, List<Department>> GetFilteredDepartments()
        //{
        //    try
        //    {
        //        return _departmentManager.GetFilteredDepartments();

        //    }
        //    catch (Exception ex)
        //    {
        //        ErrorManager.LogApplicationError(ex.StackTrace, ex.Source, ex.Message);
        //        return new Dictionary<List<Department>, List<Department>>();
        //    }
        //}


        public int AddDepartmentCheckDuplicate(Department department)
        {
            try
            {
                return _departmentManager.AddDepartmentCheckDuplicate(department);

            }
            catch (Exception ex)
            {
                ErrorManager.LogApplicationError(ex.StackTrace, ex.Source, ex.Message);
                return 0;
            }
        }
        public int UpdateDepartmentCheckDuplicate(Department department)
        {
            try
            {
                return _departmentManager.UpdateDepartmentCheckDuplicate(department);

            }
            catch (Exception ex)
            {
                ErrorManager.LogApplicationError(ex.StackTrace, ex.Source, ex.Message);
                return 0;
            }
        }
        public List<Department> GetActiveOrderedDepartments()
        {
            try
            {
                return _departmentManager.GetActiveOrderedDepartments();

            }
            catch (Exception ex)
            {
                ErrorManager.LogApplicationError(ex.StackTrace, ex.Source, ex.Message);
                return new List<Department>();
            }
        }
        public List<Department> GetOrderedDepartments()
        {
            try
            {
                return _departmentManager.GetOrderedDepartments();
            }
            catch (Exception ex)
            {
                ErrorManager.LogApplicationError(ex.StackTrace, ex.Source, ex.Message);
                return new List<Department>();
            }
        }
        public List<Department> GetActiveFilteredOrderedDepartments()
        {
            try
            {
                return _departmentManager.GetActiveFilteredOrderedDepartments();

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
