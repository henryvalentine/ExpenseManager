using System;
using System.Collections.Generic;
using System.Linq;
using kPortal.CoreUtilities;
using xPlug.BusinessObject;

namespace xPlug.BusinessService
{
	/// Author:	 Oluwaseyi Adegboyega - sadegboyega@xplugng.com
	/// Copyright © 2013. xPlug Technologies Limited. All Rights Reserved
	///*******************************************************************************
	///* Date Created: 	04-06-2013
	///* Date Generated:	04-11-2013 03:10:17
	///*******************************************************************************


	public partial class UnitService
	{
        public List<Unit> GetAllOrderedUnits()
        {
            try
            {
                return _unitManager.GetAllOrderedUnits();
            }
            catch (Exception ex)
            {
                ErrorManager.LogApplicationError(ex.StackTrace, ex.Source, ex.Message);
                return new List<Unit>();
            }
        }

        public List<Unit> GetActiveOrderedUnitsByDepartment(int departmentId)
        {
            try
            {
                return _unitManager.GetActiveOrderedUnitsByDepartment(departmentId);

            }
            catch (Exception ex)
            {
                ErrorManager.LogApplicationError(ex.StackTrace, ex.Source, ex.Message);
                return new List<Unit>();
            }
        }

        public List<Unit> GetActiveFilteredOrderedUnits()
        {
            try
            {
                return _unitManager.GetActiveFilteredOrderedUnits();

            }
            catch (Exception ex)
            {
                ErrorManager.LogApplicationError(ex.StackTrace, ex.Source, ex.Message);
                return new List<Unit>();
            }
        }

        public int UpdateUnitCheckDuplicate(Unit unit)
        {
            try
            {
                return _unitManager.UpdateUnitCheckDuplicate(unit);
            }
            catch (Exception ex)
            {
                ErrorManager.LogApplicationError(ex.StackTrace, ex.Source, ex.Message);
                return 0;
            }
        }

        public int AddUnitCheckDuplicate(Unit unit)
        {
            try
            {
                return _unitManager.AddUnitCheckDuplicate(unit);
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
