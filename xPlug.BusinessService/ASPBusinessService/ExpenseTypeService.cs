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
	///* Date Generated:	25-11-2013 09:26:28
	///*******************************************************************************


	public partial class ExpenseTypeService : MarshalByRefObject
	{
		private readonly ExpenseTypeManager  _expenseTypeManager;
		public ExpenseTypeService()
		{
			_expenseTypeManager = new ExpenseTypeManager();
		}

		public int AddExpenseType(ExpenseType expenseType)
		{
			try
			{
				return _expenseTypeManager.AddExpenseType(expenseType);
			}
			catch (Exception ex)
			{
				ErrorManager.LogApplicationError(ex.StackTrace, ex.Source, ex.Message);
				return 0;
			}
		}

		public bool UpdateExpenseType(ExpenseType expenseType)
		{
			try
			{
				return _expenseTypeManager.UpdateExpenseType(expenseType);
				}
			catch (Exception ex)
			{
				ErrorManager.LogApplicationError(ex.StackTrace, ex.Source, ex.Message);
				return false;
			}
		}

		public bool DeleteExpenseType(Int32 expenseTypeId)
		{
			try
			{
				return _expenseTypeManager.DeleteExpenseType(expenseTypeId);
				}
			catch (Exception ex)
			{
				ErrorManager.LogApplicationError(ex.StackTrace, ex.Source, ex.Message);
				return false;
			}
		}

		public ExpenseType GetExpenseType(int expenseTypeId)
		{
			try
			{
				return _expenseTypeManager.GetExpenseType(expenseTypeId);
			}
			catch (Exception ex)
			{
				ErrorManager.LogApplicationError(ex.StackTrace, ex.Source, ex.Message);
				return new ExpenseType();
			}
		}

		public List<ExpenseType> GetExpenseTypes()
		{
			try
			{
				var objList = new List<ExpenseType>();
				objList = _expenseTypeManager.GetExpenseTypes();
				if(objList == null) {return  new List<ExpenseType>();}
				return objList;
			}
			catch (Exception ex)
			{
				ErrorManager.LogApplicationError(ex.StackTrace, ex.Source, ex.Message);
				return new List<ExpenseType>();
			}
		}

	}


	//Class File Generated from Code<->Stripper 1.2.0.0 | All Rights Reserved
}
