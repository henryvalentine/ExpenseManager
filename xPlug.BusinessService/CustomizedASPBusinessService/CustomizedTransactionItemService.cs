using System;
using System.Collections.Generic;
using kPortal.CoreUtilities;
using xPlug.BusinessObject;

namespace xPlug.BusinessService
{
	/// Author:	 Oluwaseyi Adegboyega - sadegboyega@xplugng.com
	/// Copyright © 2013. xPlug Technologies Limited. All Rights Reserved
	///*******************************************************************************
	///* Date Created: 	04-06-2013
	///* Date Generated:	25-11-2013 09:26:28
	///*******************************************************************************


	public partial class TransactionItemService
	{
        public int AddTransactionItems(List<TransactionItem> transactionItems, ExpenseTransaction expenseTransaction)
		{
			try
			{
                return _transactionItemManager.AddTransactionItems(transactionItems, expenseTransaction);
			}
			catch (Exception ex)
			{
				ErrorManager.LogApplicationError(ex.StackTrace, ex.Source, ex.Message);
				return 0;
			}
		}

        public ExpenseTransaction UpdateTransactionItemAndTotalAmount(TransactionItem transactionItem)
		{
			try
			{
                return _transactionItemManager.UpdateTransactionItemAndTotalAmount(transactionItem);
			}
			catch (Exception ex)
			{
				ErrorManager.LogApplicationError(ex.StackTrace, ex.Source, ex.Message);
				return new ExpenseTransaction();
			}
		}

        public bool UpdateTransactionAndItems(ExpenseTransaction transaction, List<TransactionItem> updatedTransactionItemList)
		{
			try
			{
                return _transactionItemManager.UpdateTransactionAndItems(transaction, updatedTransactionItemList);
			}
			catch (Exception ex)
			{
				ErrorManager.LogApplicationError(ex.StackTrace, ex.Source, ex.Message);
				return false;
			}
		}

        public bool UpdateVoidedTransactionItemsAndTransaction(ExpenseTransaction transaction, List<TransactionItem> voidedTransactionItemList)
		{
			try
			{
                return _transactionItemManager.UpdateVoidedTransactionItemsAndTransaction(transaction, voidedTransactionItemList);
			}
			catch (Exception ex)
			{
				ErrorManager.LogApplicationError(ex.StackTrace, ex.Source, ex.Message);
				return false;
			}
		}

        public bool RevertChanges(List<TransactionItem> transactionItems)
		{
			try
			{
                return _transactionItemManager.RevertChanges(transactionItems);
			}
			catch (Exception ex)
			{
				ErrorManager.LogApplicationError(ex.StackTrace, ex.Source, ex.Message);
				return false;
			}
		}

        public ExpenseTransaction DeleteTransactionItemUpdateTotalAmount(int transactionItemId)
		{
			try
			{
                return _transactionItemManager.DeleteTransactionItemUpdateTotalAmount(transactionItemId);
			}
			catch (Exception ex)
			{
				ErrorManager.LogApplicationError(ex.StackTrace, ex.Source, ex.Message);
				return new ExpenseTransaction();
			}
		}

        public bool DeleteTransactionAndItem(int transactionItemId)
		{
			try
			{
                return _transactionItemManager.DeleteTransactionAndItem(transactionItemId);
			}
			catch (Exception ex)
			{
				ErrorManager.LogApplicationError(ex.StackTrace, ex.Source, ex.Message);
				return false;
			}
		}

        public bool DeleteTransactionAndItems(long expenseTransactionId)
		{
			try
			{
                return _transactionItemManager.DeleteTransactionAndItems(expenseTransactionId);
			}
			catch (Exception ex)
			{
				ErrorManager.LogApplicationError(ex.StackTrace, ex.Source, ex.Message);
				return false;
			}
		}

        public Dictionary<List<TransactionItem>, List<ExpenseItem>> GetTransactionItemsByExpenseTransaction(long expenseTransactionId)
		{
			try
			{
                return _transactionItemManager.GetTransactionItemsByExpenseTransaction(expenseTransactionId);
			}
			catch (Exception ex)
			{
				ErrorManager.LogApplicationError(ex.StackTrace, ex.Source, ex.Message);
				return new Dictionary<List<TransactionItem>, List<ExpenseItem>>();
			}
		}

        public List<TransactionItem> GetApprovedTransactionItemsByExpenseTransaction(long expenseTransactionId)
		{
			try
			{
                return _transactionItemManager.GetApprovedTransactionItemsByExpenseTransaction(expenseTransactionId);
			}
			catch (Exception ex)
			{
				ErrorManager.LogApplicationError(ex.StackTrace, ex.Source, ex.Message);
				return new List<TransactionItem>();
			}
		}


        public bool UpdatePendingTransactionAndItem(ExpenseTransaction transaction, TransactionItem updatedTransactionItem)
		{
			try
			{
                return _transactionItemManager.UpdatePendingTransactionAndItem(transaction, updatedTransactionItem);
			}
			catch (Exception ex)
			{
				ErrorManager.LogApplicationError(ex.StackTrace, ex.Source, ex.Message);
				return false;
			}
		}

        public bool ModifyPendingTransactionAndItem(ExpenseTransaction transaction,int transactionItemId)
		{
			try
			{
                return _transactionItemManager.ModifyPendingTransactionAndItem(transaction, transactionItemId);
			}
			catch (Exception ex)
			{
				ErrorManager.LogApplicationError(ex.StackTrace, ex.Source, ex.Message);
				return false;
			}
		}


        public List<TransactionItem> GetExpenseItemCostsByDateRange(int expenseItemId, string startDate, string endDate)
        {
            try
            {
                return _transactionItemManager.GetExpenseItemCostsByDateRange(expenseItemId, startDate, endDate);
            }
            catch (Exception ex)
            {
                ErrorManager.LogApplicationError(ex.StackTrace, ex.Source, ex.Message);
                return new List<TransactionItem>();
            }
        }


        public List<TransactionItem> GetAccountsHeadsCostsByDateRange(int accountHeadId, string startDate, string endDate)
        {
            try
            {
                return _transactionItemManager.GetAccountsHeadsCostsByDateRange(accountHeadId, startDate, endDate);
            }
            catch (Exception ex)
            {
                ErrorManager.LogApplicationError(ex.StackTrace, ex.Source, ex.Message);
                return new List<TransactionItem>();
            }
        }

         public List<List<TransactionItem>> GetItemsByDateRange(long expenseTransactionId)
        {
            try
            {
                return _transactionItemManager.GetItemsByDateRange(expenseTransactionId);
            }
            catch (Exception ex)
            {
                ErrorManager.LogApplicationError(ex.StackTrace, ex.Source, ex.Message);
                return new List<List<TransactionItem>>();
            }
        }

         public List<TransactionItem> GetSingleTransactionItems(long expenseTransactionId)
         {
             try
             {
                 return _transactionItemManager.GetSingleTransactionItems(expenseTransactionId);
             }
             catch (Exception ex)
             {
                 ErrorManager.LogApplicationError(ex.StackTrace, ex.Source, ex.Message);
                 return new List<TransactionItem>();
             }
         }

         public List<TransactionItem> GetDetailedTransactionItems(int transactionItemId)
         {
             try
             {
                 return _transactionItemManager.GetDetailedTransactionItems(transactionItemId);
             }
             catch (Exception ex)
             {
                 ErrorManager.LogApplicationError(ex.StackTrace, ex.Source, ex.Message);
                 return new List<TransactionItem>();
             }
         }
	}


	//Class File Generated from Code<->Stripper 1.2.0.0 | All Rights Reserved
}
