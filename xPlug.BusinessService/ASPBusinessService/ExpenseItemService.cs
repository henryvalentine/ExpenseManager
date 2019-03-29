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
	///* Date Generated:	25-11-2013 09:26:27
	///*******************************************************************************


	public partial class ExpenseItemService : MarshalByRefObject
	{
		private readonly ExpenseItemManager  _expenseItemManager;
		public ExpenseItemService()
		{
			_expenseItemManager = new ExpenseItemManager();
		}

		public int AddExpenseItem(ExpenseItem expenseItem)
		{
			try
			{
				return _expenseItemManager.AddExpenseItem(expenseItem);
			}
			catch (Exception ex)
			{
				ErrorManager.LogApplicationError(ex.StackTrace, ex.Source, ex.Message);
				return 0;
			}
		}

		public bool UpdateExpenseItem(ExpenseItem expenseItem)
		{
			try
			{
				return _expenseItemManager.UpdateExpenseItem(expenseItem);
				}
			catch (Exception ex)
			{
				ErrorManager.LogApplicationError(ex.StackTrace, ex.Source, ex.Message);
				return false;
			}
		}

		public bool DeleteExpenseItem(Int32 expenseItemId)
		{
			try
			{
				return _expenseItemManager.DeleteExpenseItem(expenseItemId);
				}
			catch (Exception ex)
			{
				ErrorManager.LogApplicationError(ex.StackTrace, ex.Source, ex.Message);
				return false;
			}
		}

		public ExpenseItem GetExpenseItem(int expenseItemId)
		{
			try
			{
				return _expenseItemManager.GetExpenseItem(expenseItemId);
			}
			catch (Exception ex)
			{
				ErrorManager.LogApplicationError(ex.StackTrace, ex.Source, ex.Message);
				return new ExpenseItem();
			}
		}

		public List<ExpenseItem> GetExpenseItems()
		{
			try
			{
				var objList = new List<ExpenseItem>();
				objList = _expenseItemManager.GetExpenseItems();
				if(objList == null) {return  new List<ExpenseItem>();}
				return objList;
			}
			catch (Exception ex)
			{
				ErrorManager.LogApplicationError(ex.StackTrace, ex.Source, ex.Message);
				return new List<ExpenseItem>();
			}
		}

		public List<ExpenseItem>  GetExpenseItemsByExpenseCategoryId(Int32 expenseCategoryId)
		{
			try
			{
				return _expenseItemManager.GetExpenseItemsByExpenseCategoryId(expenseCategoryId);
			}
			catch (Exception ex)
			{
				ErrorManager.LogApplicationError(ex.StackTrace, ex.Source, ex.Message);
				return new List<ExpenseItem>();
			}
		}

		public List<ExpenseItem>  GetExpenseItemsByAccountsHeadId(Int32 accountsHeadId)
		{
			try
			{
				return _expenseItemManager.GetExpenseItemsByAccountsHeadId(accountsHeadId);
			}
			catch (Exception ex)
			{
				ErrorManager.LogApplicationError(ex.StackTrace, ex.Source, ex.Message);
				return new List<ExpenseItem>();
			}
		}

	}


	//Class File Generated from Code<->Stripper 1.2.0.0 | All Rights Reserved
}
