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


	public partial class TransactionItemService : MarshalByRefObject
	{
		private readonly TransactionItemManager  _transactionItemManager;
		public TransactionItemService()
		{
			_transactionItemManager = new TransactionItemManager();
		}

		public int AddTransactionItem(TransactionItem transactionItem)
		{
			try
			{
				return _transactionItemManager.AddTransactionItem(transactionItem);
			}
			catch (Exception ex)
			{
				ErrorManager.LogApplicationError(ex.StackTrace, ex.Source, ex.Message);
				return 0;
			}
		}

		public bool UpdateTransactionItem(TransactionItem transactionItem)
		{
			try
			{
				return _transactionItemManager.UpdateTransactionItem(transactionItem);
				}
			catch (Exception ex)
			{
				ErrorManager.LogApplicationError(ex.StackTrace, ex.Source, ex.Message);
                return false;
			}
		}

		public bool DeleteTransactionItem(Int32 transactionItemId)
		{
			try
			{
				return _transactionItemManager.DeleteTransactionItem(transactionItemId);
				}
			catch (Exception ex)
			{
				ErrorManager.LogApplicationError(ex.StackTrace, ex.Source, ex.Message);
				return false;
			}
		}

		public TransactionItem GetTransactionItem(int transactionItemId)
		{
			try
			{
				return _transactionItemManager.GetTransactionItem(transactionItemId);
			}
			catch (Exception ex)
			{
				ErrorManager.LogApplicationError(ex.StackTrace, ex.Source, ex.Message);
				return new TransactionItem();
			}
		}

		public List<TransactionItem> GetTransactionItems()
		{
			try
			{
				var objList = new List<TransactionItem>();
				objList = _transactionItemManager.GetTransactionItems();
				if(objList == null) {return  new List<TransactionItem>();}
				return objList;
			}
			catch (Exception ex)
			{
				ErrorManager.LogApplicationError(ex.StackTrace, ex.Source, ex.Message);
				return new List<TransactionItem>();
			}
		}

		public List<TransactionItem>  GetTransactionItemsByExpensenseItemId(Int32 expensenseItemId)
		{
			try
			{
				return _transactionItemManager.GetTransactionItemsByExpensenseItemId(expensenseItemId);
			}
			catch (Exception ex)
			{
				ErrorManager.LogApplicationError(ex.StackTrace, ex.Source, ex.Message);
				return new List<TransactionItem>();
			}
		}

		public List<TransactionItem>  GetTransactionItemsByExpenseTransactionId(Int64 expenseTransactionId)
		{
			try
			{
				return _transactionItemManager.GetTransactionItemsByExpenseTransactionId(expenseTransactionId);
			}
			catch (Exception ex)
			{
				ErrorManager.LogApplicationError(ex.StackTrace, ex.Source, ex.Message);
				return new List<TransactionItem>();
			}
		}

		public List<TransactionItem>  GetTransactionItemsByExpenseCategoryId(Int32 expenseCategoryId)
		{
			try
			{
				return _transactionItemManager.GetTransactionItemsByExpenseCategoryId(expenseCategoryId);
			}
			catch (Exception ex)
			{
				ErrorManager.LogApplicationError(ex.StackTrace, ex.Source, ex.Message);
				return new List<TransactionItem>();
			}
		}

		public List<TransactionItem>  GetTransactionItemsByExpenseTypeId(Int32 expenseTypeId)
		{
			try
			{
				return _transactionItemManager.GetTransactionItemsByExpenseTypeId(expenseTypeId);
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
