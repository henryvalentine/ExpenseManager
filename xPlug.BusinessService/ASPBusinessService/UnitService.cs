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
	///* Date Generated:	25-11-2013 09:26:19
	///*******************************************************************************


	public partial class UnitService : MarshalByRefObject
	{
		private readonly UnitManager  _unitManager;
		public UnitService()
		{
			_unitManager = new UnitManager();
		}

		public int AddUnit(Unit unit)
		{
			try
			{
				return _unitManager.AddUnit(unit);
			}
			catch (Exception ex)
			{
				ErrorManager.LogApplicationError(ex.StackTrace, ex.Source, ex.Message);
				return 0;
			}
		}

		public bool UpdateUnit(Unit unit)
		{
			try
			{
				return _unitManager.UpdateUnit(unit);
				}
			catch (Exception ex)
			{
				ErrorManager.LogApplicationError(ex.StackTrace, ex.Source, ex.Message);
				return false;
			}
		}

		public bool DeleteUnit(Int32 unitId)
		{
			try
			{
				return _unitManager.DeleteUnit(unitId);
				}
			catch (Exception ex)
			{
				ErrorManager.LogApplicationError(ex.StackTrace, ex.Source, ex.Message);
				return false;
			}
		}

		public Unit GetUnit(int unitId)
		{
			try
			{
				return _unitManager.GetUnit(unitId);
			}
			catch (Exception ex)
			{
				ErrorManager.LogApplicationError(ex.StackTrace, ex.Source, ex.Message);
				return new Unit();
			}
		}

		public List<Unit> GetUnits()
		{
			try
			{
				var objList = new List<Unit>();
				objList = _unitManager.GetUnits();
				if(objList == null) {return  new List<Unit>();}
				return objList;
			}
			catch (Exception ex)
			{
				ErrorManager.LogApplicationError(ex.StackTrace, ex.Source, ex.Message);
				return new List<Unit>();
			}
		}

		public List<Unit>  GetUnitsByDepartmentId(Int32 departmentId)
		{
			try
			{
				return _unitManager.GetUnitsByDepartmentId(departmentId);
			}
			catch (Exception ex)
			{
				ErrorManager.LogApplicationError(ex.StackTrace, ex.Source, ex.Message);
				return new List<Unit>();
			}
		}

	}


	//Class File Generated from Code<->Stripper 1.2.0.0 | All Rights Reserved
}
