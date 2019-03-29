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


	public partial class ExpenseTransactionPaymentService : MarshalByRefObject
	{
		private readonly ExpenseTransactionPaymentManager  _expenseTransactionPaymentManager;
		public ExpenseTransactionPaymentService()
		{
			_expenseTransactionPaymentManager = new ExpenseTransactionPaymentManager();
		}

		public long AddExpenseTransactionPayment(ExpenseTransactionPayment expenseTransactionPayment)
		{
			try
			{
				return _expenseTransactionPaymentManager.AddExpenseTransactionPayment(expenseTransactionPayment);
			}
			catch (Exception ex)
			{
				ErrorManager.LogApplicationError(ex.StackTrace, ex.Source, ex.Message);
				return 0;
			}
		}

		public bool UpdateExpenseTransactionPayment(ExpenseTransactionPayment expenseTransactionPayment)
		{
			try
			{
				return _expenseTransactionPaymentManager.UpdateExpenseTransactionPayment(expenseTransactionPayment);
				}
			catch (Exception ex)
			{
				ErrorManager.LogApplicationError(ex.StackTrace, ex.Source, ex.Message);
				return false;
			}
		}

		public bool DeleteExpenseTransactionPayment(Int64 expenseTransactionPaymentId)
		{
			try
			{
				return _expenseTransactionPaymentManager.DeleteExpenseTransactionPayment(expenseTransactionPaymentId);
				}
			catch (Exception ex)
			{
				ErrorManager.LogApplicationError(ex.StackTrace, ex.Source, ex.Message);
				return false;
			}
		}

		public ExpenseTransactionPayment GetExpenseTransactionPayment(long expenseTransactionPaymentId)
		{
			try
			{
				return _expenseTransactionPaymentManager.GetExpenseTransactionPayment(expenseTransactionPaymentId);
			}
			catch (Exception ex)
			{
				ErrorManager.LogApplicationError(ex.StackTrace, ex.Source, ex.Message);
				return new ExpenseTransactionPayment();
			}
		}

		public List<ExpenseTransactionPayment> GetExpenseTransactionPayments()
		{
			try
			{
				var objList = new List<ExpenseTransactionPayment>();
				objList = _expenseTransactionPaymentManager.GetExpenseTransactionPayments();
				if(objList == null) {return  new List<ExpenseTransactionPayment>();}
				return objList;
			}
			catch (Exception ex)
			{
				ErrorManager.LogApplicationError(ex.StackTrace, ex.Source, ex.Message);
				return new List<ExpenseTransactionPayment>();
			}
		}

		public List<ExpenseTransactionPayment>  GetExpenseTransactionPaymentsByExpenseTransactionId(Int64 expenseTransactionId)
		{
			try
			{
				return _expenseTransactionPaymentManager.GetExpenseTransactionPaymentsByExpenseTransactionId(expenseTransactionId);
			}
			catch (Exception ex)
			{
				ErrorManager.LogApplicationError(ex.StackTrace, ex.Source, ex.Message);
				return new List<ExpenseTransactionPayment>();
			}
		}

		public List<ExpenseTransactionPayment>  GetExpenseTransactionPaymentsByBeneficiaryId(Int32 beneficiaryId)
		{
			try
			{
				return _expenseTransactionPaymentManager.GetExpenseTransactionPaymentsByBeneficiaryId(beneficiaryId);
			}
			catch (Exception ex)
			{
				ErrorManager.LogApplicationError(ex.StackTrace, ex.Source, ex.Message);
				return new List<ExpenseTransactionPayment>();
			}
		}

	}


	//Class File Generated from Code<->Stripper 1.2.0.0 | All Rights Reserved
}
