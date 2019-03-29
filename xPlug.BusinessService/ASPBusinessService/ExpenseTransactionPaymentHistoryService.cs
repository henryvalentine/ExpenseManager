using System;
using System.Linq;
using System.Collections.Generic;
using System.Data;
using XPLUG.WEBTOOLS;
using xPlug.BusinessObject;
using xPlug.BusinessManager;



namespace xPlug.BusinessService
{
	/// Author:	 Oluwaseyi Adegboyega - sadegboyega@xplugng.com
	/// Copyright © 2013. xPlug Technologies Limited. All Rights Reserved
	///*******************************************************************************
	///* Date Created: 	04-06-2013
	///* Date Generated:	01-12-2013 03:58:06
	///*******************************************************************************


	public partial class ExpenseTransactionPaymentHistoryService : MarshalByRefObject
	{
		private readonly ExpenseTransactionPaymentHistoryManager  _expenseTransactionPaymentHistoryManager;
		public ExpenseTransactionPaymentHistoryService()
		{
			_expenseTransactionPaymentHistoryManager = new ExpenseTransactionPaymentHistoryManager();
		}

		public long AddExpenseTransactionPaymentHistory(ExpenseTransactionPaymentHistory expenseTransactionPaymentHistory)
		{
			try
			{
				return _expenseTransactionPaymentHistoryManager.AddExpenseTransactionPaymentHistory(expenseTransactionPaymentHistory);
			}
			catch (Exception ex)
			{
				ErrorManager.LogApplicationError(ex.StackTrace, ex.Source, ex.Message);
				return 0;
			}
		}

		public bool UpdateExpenseTransactionPaymentHistory(ExpenseTransactionPaymentHistory expenseTransactionPaymentHistory)
		{
			try
			{
				return _expenseTransactionPaymentHistoryManager.UpdateExpenseTransactionPaymentHistory(expenseTransactionPaymentHistory);
				}
			catch (Exception ex)
			{
				ErrorManager.LogApplicationError(ex.StackTrace, ex.Source, ex.Message);
				return false;
			}
		}

		public bool DeleteExpenseTransactionPaymentHistory(Int64 expenseTransactionPaymentHistoryId)
		{
			try
			{
				return _expenseTransactionPaymentHistoryManager.DeleteExpenseTransactionPaymentHistory(expenseTransactionPaymentHistoryId);
				}
			catch (Exception ex)
			{
				ErrorManager.LogApplicationError(ex.StackTrace, ex.Source, ex.Message);
				return false;
			}
		}

		public ExpenseTransactionPaymentHistory GetExpenseTransactionPaymentHistory(long expenseTransactionPaymentHistoryId)
		{
			try
			{
				return _expenseTransactionPaymentHistoryManager.GetExpenseTransactionPaymentHistory(expenseTransactionPaymentHistoryId);
			}
			catch (Exception ex)
			{
				ErrorManager.LogApplicationError(ex.StackTrace, ex.Source, ex.Message);
				return new ExpenseTransactionPaymentHistory();
			}
		}

		public List<ExpenseTransactionPaymentHistory> GetExpenseTransactionPaymentHistories()
		{
			try
			{
				var objList = new List<ExpenseTransactionPaymentHistory>();
				objList = _expenseTransactionPaymentHistoryManager.GetExpenseTransactionPaymentHistories();
				if(objList == null) {return  new List<ExpenseTransactionPaymentHistory>();}
				return objList;
			}
			catch (Exception ex)
			{
				ErrorManager.LogApplicationError(ex.StackTrace, ex.Source, ex.Message);
				return new List<ExpenseTransactionPaymentHistory>();
			}
		}

		public List<ExpenseTransactionPaymentHistory>  GetExpenseTransactionPaymentHistoriesByExpenseTransactionId(Int64 expenseTransactionId)
		{
			try
			{
				return _expenseTransactionPaymentHistoryManager.GetExpenseTransactionPaymentHistoriesByExpenseTransactionId(expenseTransactionId);
			}
			catch (Exception ex)
			{
				ErrorManager.LogApplicationError(ex.StackTrace, ex.Source, ex.Message);
				return new List<ExpenseTransactionPaymentHistory>();
			}
		}

		public List<ExpenseTransactionPaymentHistory>  GetExpenseTransactionPaymentHistoriesByExpenseTransactionPaymentId(Int64 expenseTransactionPaymentId)
		{
			try
			{
				return _expenseTransactionPaymentHistoryManager.GetExpenseTransactionPaymentHistoriesByExpenseTransactionPaymentId(expenseTransactionPaymentId);
			}
			catch (Exception ex)
			{
				ErrorManager.LogApplicationError(ex.StackTrace, ex.Source, ex.Message);
				return new List<ExpenseTransactionPaymentHistory>();
			}
		}

		public List<ExpenseTransactionPaymentHistory>  GetExpenseTransactionPaymentHistoriesByPaymentModeId(Int32 paymentModeId)
		{
			try
			{
				return _expenseTransactionPaymentHistoryManager.GetExpenseTransactionPaymentHistoriesByPaymentModeId(paymentModeId);
			}
			catch (Exception ex)
			{
				ErrorManager.LogApplicationError(ex.StackTrace, ex.Source, ex.Message);
				return new List<ExpenseTransactionPaymentHistory>();
			}
		}

		public List<ExpenseTransactionPaymentHistory>  GetExpenseTransactionPaymentHistoriesByBeneficiaryId(Int32 beneficiaryId)
		{
			try
			{
				return _expenseTransactionPaymentHistoryManager.GetExpenseTransactionPaymentHistoriesByBeneficiaryId(beneficiaryId);
			}
			catch (Exception ex)
			{
				ErrorManager.LogApplicationError(ex.StackTrace, ex.Source, ex.Message);
				return new List<ExpenseTransactionPaymentHistory>();
			}
		}

	}


	//Class File Generated from Code<->Stripper 1.2.0.0 | All Rights Reserved
}
