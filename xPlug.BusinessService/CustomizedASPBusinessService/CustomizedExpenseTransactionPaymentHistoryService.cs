using System;
using System.Collections.Generic;
using kPortal.CoreUtilities;
using xPlug.BusinessObject;
using xPlug.BusinessObject.CustomizedASPBusinessObject;

namespace xPlug.BusinessService
{
	/// Author:	 Oluwaseyi Adegboyega - sadegboyega@xplugng.com
	/// Copyright © 2013. xPlug Technologies Limited. All Rights Reserved
	///*******************************************************************************
	///* Date Created: 	04-06-2013
	///* Date Generated:	12-09-2013 10:06:43
	///*******************************************************************************


	public partial class ExpenseTransactionPaymentHistoryService
	{

        public ExpenseTransactionPaymentHistory GetRecentPaymentInTransactionPaymentHistories(ExpenseTransactionPayment expenseTransactionPayment)
		{
			try
			{
               return _expenseTransactionPaymentHistoryManager.GetRecentExpenseTransactionPaymentHistories(expenseTransactionPayment);
			}
			catch (Exception ex)
			{
				ErrorManager.LogApplicationError(ex.StackTrace, ex.Source, ex.Message);
                return new ExpenseTransactionPaymentHistory();
			}
		}

        public DictObject GetMyGenericVoucherObject(long transactionPaymentHistoryId)
		{
			try
			{
              return  _expenseTransactionPaymentHistoryManager.GetMyGenericVoucherObject(transactionPaymentHistoryId);
			}
			catch (Exception ex)
			{
				ErrorManager.LogApplicationError(ex.StackTrace, ex.Source, ex.Message);

                return new DictObject();
			}
		}

        public long AddTransactionPaymentHistoryAndPcv(ExpenseTransactionPaymentHistory expenseTransactionPaymentHistory)
		{
			try
			{
                return _expenseTransactionPaymentHistoryManager.AddTransactionPaymentHistoryAndPcv(expenseTransactionPaymentHistory);
			}
			catch (Exception ex)
			{
				ErrorManager.LogApplicationError(ex.StackTrace, ex.Source, ex.Message);

                return 0;
			}
		}

        public List<DictObject> GetMyGenericVoucherObjectsByDateRange(DateTime startDate, DateTime endDate)
		{
			try
			{
                return _expenseTransactionPaymentHistoryManager.GetMyGenericVoucherObjectsByDateRange(startDate, endDate);
			}
			catch (Exception ex)
			{
				ErrorManager.LogApplicationError(ex.StackTrace, ex.Source, ex.Message);

                return new  List<DictObject>();
			}
		}

        public List<DictObject> GetApprovedTransactionPaymentVoucherObjectsByDateRange(DateTime startDate, DateTime endDate)
		{
			try
			{
                return _expenseTransactionPaymentHistoryManager.GetApprovedTransactionPaymentVoucherObjectsByDateRange(startDate, endDate);
			}
			catch (Exception ex)
			{
				ErrorManager.LogApplicationError(ex.StackTrace, ex.Source, ex.Message);

                return new  List<DictObject>();
			}
		}

        public List<DictObject> GetVoidedTransactionPaymentVoucherObjectsByDateRange(DateTime startDate, DateTime endDate)
        {
            try
            {
                return _expenseTransactionPaymentHistoryManager.GetVoidedTransactionPaymentVoucherObjectsByDateRange(startDate, endDate);
            }
            catch (Exception ex)
            {
                ErrorManager.LogApplicationError(ex.StackTrace, ex.Source, ex.Message);

                return new List<DictObject>();
            }
        }

        public  List<ExpenseTransactionPaymentHistory> GetVoidedTransactionPaymentsByDateRange(DateTime startDate, DateTime endDate, int dept)
        {
            try
            {
                return _expenseTransactionPaymentHistoryManager.GetVoidedTransactionPaymentsByDateRange(startDate, endDate, dept);
            }
            catch (Exception ex)
            {
                ErrorManager.LogApplicationError(ex.StackTrace, ex.Source, ex.Message);

                return new List<ExpenseTransactionPaymentHistory>();
            }

        }
        public List<DictObject> GetMyGenericVoucherObjectsByIds(List<long> paymentHistoryIds)
        {
            try
            {
                return _expenseTransactionPaymentHistoryManager.GetMyGenericVoucherObjectsByIds(paymentHistoryIds);
            }
            catch (Exception ex)
            {
                ErrorManager.LogApplicationError(ex.StackTrace, ex.Source, ex.Message);

                return new List<DictObject>();
            }

        }

        public List<DictObject> GetVoucherObjects(Dictionary<long, string> dictCollection)
        {
            try
            {
                return _expenseTransactionPaymentHistoryManager.GetVoucherObjects(dictCollection);
            }
            catch (Exception ex)
            {
                ErrorManager.LogApplicationError(ex.StackTrace, ex.Source, ex.Message);

                return new List<DictObject>();
            }

        }
         
	    public List<ExpenseTransactionPaymentHistory> GetApprovedTransactionPaymentsByDateRange(DateTime startDate, DateTime endDate, int dept)
	    {
	         try
            {
                return _expenseTransactionPaymentHistoryManager.GetApprovedTransactionPaymentsByDateRange(startDate, endDate, dept);
            }
            catch (Exception ex)
            {
                ErrorManager.LogApplicationError(ex.StackTrace, ex.Source, ex.Message);

                return new List<ExpenseTransactionPaymentHistory>();
            }
	    
	    }
	    public List<ExpenseTransactionPaymentHistory> GetTransactionPaymentsByDateRange(DateTime startDate, DateTime endDate, int dept)
	    {
	         try
            {
                return _expenseTransactionPaymentHistoryManager.GetTransactionPaymentsByDateRange(startDate, endDate, dept);
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
