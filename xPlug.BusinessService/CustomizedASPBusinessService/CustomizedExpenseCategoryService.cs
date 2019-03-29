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
	///* Date Generated:	12-09-2013 10:06:27
	///*******************************************************************************


	public partial class ExpenseCategoryService
	{
        public List<ExpenseCategory> GetFilteredExpenseCategories()
        {
            try
            {
                return _expenseCategoryManager.GetFilteredExpenseCategories();
            }
            catch (Exception ex)
            {
                ErrorManager.LogApplicationError(ex.StackTrace, ex.Source, ex.Message);
                return new List<ExpenseCategory>();
            }
        }

        public List<ExpenseCategory> GetAllActiveExpenseCategories()
        {
            try
            {
                return _expenseCategoryManager.GetAllActiveExpenseCategories();
            }
            catch (Exception ex)
            {
                ErrorManager.LogApplicationError(ex.StackTrace, ex.Source, ex.Message);
                return new List<ExpenseCategory>();
            }
        }

        public List<ExpenseCategory> GetOrderedExpenseCategories()
        {
            try
            {
               return _expenseCategoryManager.GetOrderedExpenseCategories();
            }
            catch (Exception ex)
            {
                ErrorManager.LogApplicationError(ex.StackTrace, ex.Source, ex.Message);
                return new List<ExpenseCategory>();
            }
        }

        public int AddExpenseCategoryCheckDuplicate(ExpenseCategory expenseCategory)
        {
            try
            {
                return  _expenseCategoryManager.AddExpenseCategoryCheckDuplicate(expenseCategory);
            }
            catch (Exception ex)
            {
                ErrorManager.LogApplicationError(ex.StackTrace, ex.Source, ex.Message);
                return 0;
            }
        }

        public int UpdateExpenseCategoryCheckDuplicate(ExpenseCategory expenseCategory)
        {
            try
            {
                return _expenseCategoryManager.UpdateExpenseCategoryCheckDuplicate(expenseCategory);
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
