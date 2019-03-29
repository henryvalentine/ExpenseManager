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
	///* Date Generated:	12-09-2013 10:06:33
	///*******************************************************************************


	public partial class ExpenseItemService
	{
       public int AddExpenseItemCheckDuplicate(ExpenseItem expenseItem)
		{
			try
			{
                return _expenseItemManager.AddExpenseItemCheckDuplicate(expenseItem);
			}
			catch (Exception ex)
			{
				ErrorManager.LogApplicationError(ex.StackTrace, ex.Source, ex.Message);
				return 0;
			}
		}

       public int UpdateExpenseItemCheckDuplicate(ExpenseItem expenseItem)
        {
            try
            {
                return _expenseItemManager.UpdateExpenseItemCheckDuplicate(expenseItem);
            }
            catch (Exception ex)
            {
                ErrorManager.LogApplicationError(ex.StackTrace, ex.Source, ex.Message);
                return 0;
            }
        }

       public bool DeleteExpenseItemCheckReference(int expenseItemId)
       {
           try
           {
               return _expenseItemManager.DeleteExpenseItemCheckReference(expenseItemId);
           }
           catch (Exception ex)
           {
               ErrorManager.LogApplicationError(ex.StackTrace, ex.Source, ex.Message);
               return false;
           }
       }

       public List<ExpenseItem> GetActiveOrderedExpenseItems()
		{
			try
			{
                return _expenseItemManager.GetActiveOrderedExpenseItems();
			}
			catch (Exception ex)
			{
				ErrorManager.LogApplicationError(ex.StackTrace, ex.Source, ex.Message);
				return new List<ExpenseItem>();
			}
		}

       public List<ExpenseItem> GetLastInsertedExpenseItem(int accountHead )
       {
           try
           {
              return _expenseItemManager.GetLastInsertedExpenseItem(accountHead);
               
           }
           catch (Exception ex)
           {
               ErrorManager.LogApplicationError(ex.StackTrace, ex.Source, ex.Message);
               return new List<ExpenseItem>();
           }
       }

       public List<ExpenseItem> GetOrderedExpenseItemsByExpenseCategoryId(Int32 expenseCategoryId)
       {
           try
           {
               return _expenseItemManager.GetOrderedExpenseItemsByExpenseCategoryId(expenseCategoryId);
              
           }
           catch (Exception ex)
           {
               ErrorManager.LogApplicationError(ex.StackTrace, ex.Source, ex.Message);
               return new List<ExpenseItem>();
           }
       }

       public List<ExpenseItem> GetAllOrderedExpenseItems()
       {
           try
           {
               return _expenseItemManager.GetAllOrderedExpenseItems(); ;
           }
           catch (Exception ex)
           {
               ErrorManager.LogApplicationError(ex.StackTrace, ex.Source, ex.Message);
               return new List<ExpenseItem>();
           }
       }

       public List<ExpenseItem> GetOrderedExpenseItemsByAccountsHeadId(Int32 accountsHeadId)
       {
           try
           {
               return _expenseItemManager.GetOrderedExpenseItemsByAccountsHeadId(accountsHeadId);
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
