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
	///* Date Generated:	25-11-2013 09:26:24
	///*******************************************************************************


	public partial class ExpenseTransactionService : MarshalByRefObject
	{
		private readonly ExpenseTransactionManager  _expenseTransactionManager;
		public ExpenseTransactionService()
		{
			_expenseTransactionManager = new ExpenseTransactionManager();
		}

		public long AddExpenseTransaction(ExpenseTransaction expenseTransaction)
		{
			try
			{
				return _expenseTransactionManager.AddExpenseTransaction(expenseTransaction);
			}
			catch (Exception ex)
			{
				ErrorManager.LogApplicationError(ex.StackTrace, ex.Source, ex.Message);
				return 0;
			}
		}

		public bool UpdateExpenseTransaction(ExpenseTransaction expenseTransaction)
		{
			try
			{
				return _expenseTransactionManager.UpdateExpenseTransaction(expenseTransaction);
				}
			catch (Exception ex)
			{
				ErrorManager.LogApplicationError(ex.StackTrace, ex.Source, ex.Message);
				return false;
			}
		}

		public bool DeleteExpenseTransaction(Int64 expenseTransactionId)
		{
			try
			{
				return _expenseTransactionManager.DeleteExpenseTransaction(expenseTransactionId);
				}
			catch (Exception ex)
			{
				ErrorManager.LogApplicationError(ex.StackTrace, ex.Source, ex.Message);
				return false;
			}
		}

		public ExpenseTransaction GetExpenseTransaction(long expenseTransactionId)
		{
			try
			{
				return _expenseTransactionManager.GetExpenseTransaction(expenseTransactionId);
			}
			catch (Exception ex)
			{
				ErrorManager.LogApplicationError(ex.StackTrace, ex.Source, ex.Message);
				return new ExpenseTransaction();
			}
		}

		public List<ExpenseTransaction> GetExpenseTransactions()
		{
			try
			{
				var objList = new List<ExpenseTransaction>();
				objList = _expenseTransactionManager.GetExpenseTransactions();
				if(objList == null) {return  new List<ExpenseTransaction>();}
				return objList;
			}
			catch (Exception ex)
			{
				ErrorManager.LogApplicationError(ex.StackTrace, ex.Source, ex.Message);
				return new List<ExpenseTransaction>();
			}
		}

		public List<ExpenseTransaction>  GetExpenseTransactionsByBeneficiaryId(Int32 beneficiaryId)
		{
			try
			{
				return _expenseTransactionManager.GetExpenseTransactionsByBeneficiaryId(beneficiaryId);
			}
			catch (Exception ex)
			{
				ErrorManager.LogApplicationError(ex.StackTrace, ex.Source, ex.Message);
				return new List<ExpenseTransaction>();
			}
		}

		public List<ExpenseTransaction>  GetExpenseTransactionsByBeneficiaryTypeId(Int32 beneficiaryTypeId)
		{
			try
			{
				return _expenseTransactionManager.GetExpenseTransactionsByBeneficiaryTypeId(beneficiaryTypeId);
			}
			catch (Exception ex)
			{
				ErrorManager.LogApplicationError(ex.StackTrace, ex.Source, ex.Message);
				return new List<ExpenseTransaction>();
			}
		}

	}


	//Class File Generated from Code<->Stripper 1.2.0.0 | All Rights Reserved
}
