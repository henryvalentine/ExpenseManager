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
	///* Date Generated:	25-11-2013 09:26:29
	///*******************************************************************************


	public partial class ExpenseCategoryService : MarshalByRefObject
	{
		private readonly ExpenseCategoryManager  _expenseCategoryManager;
		public ExpenseCategoryService()
		{
			_expenseCategoryManager = new ExpenseCategoryManager();
		}

		public int AddExpenseCategory(ExpenseCategory expenseCategory)
		{
			try
			{
				return _expenseCategoryManager.AddExpenseCategory(expenseCategory);
			}
			catch (Exception ex)
			{
				ErrorManager.LogApplicationError(ex.StackTrace, ex.Source, ex.Message);
				return 0;
			}
		}

		public bool UpdateExpenseCategory(ExpenseCategory expenseCategory)
		{
			try
			{
				return _expenseCategoryManager.UpdateExpenseCategory(expenseCategory);
				}
			catch (Exception ex)
			{
				ErrorManager.LogApplicationError(ex.StackTrace, ex.Source, ex.Message);
				return false;
			}
		}

		public bool DeleteExpenseCategory(Int32 expenseCategoryId)
		{
			try
			{
				return _expenseCategoryManager.DeleteExpenseCategory(expenseCategoryId);
				}
			catch (Exception ex)
			{
				ErrorManager.LogApplicationError(ex.StackTrace, ex.Source, ex.Message);
				return false;
			}
		}

		public ExpenseCategory GetExpenseCategory(int expenseCategoryId)
		{
			try
			{
				return _expenseCategoryManager.GetExpenseCategory(expenseCategoryId);
			}
			catch (Exception ex)
			{
				ErrorManager.LogApplicationError(ex.StackTrace, ex.Source, ex.Message);
				return new ExpenseCategory();
			}
		}

		public List<ExpenseCategory> GetExpenseCategories()
		{
			try
			{
				var objList = new List<ExpenseCategory>();
				objList = _expenseCategoryManager.GetExpenseCategories();
				if(objList == null) {return  new List<ExpenseCategory>();}
				return objList;
			}
			catch (Exception ex)
			{
				ErrorManager.LogApplicationError(ex.StackTrace, ex.Source, ex.Message);
				return new List<ExpenseCategory>();
			}
		}

	}


	//Class File Generated from Code<->Stripper 1.2.0.0 | All Rights Reserved
}
