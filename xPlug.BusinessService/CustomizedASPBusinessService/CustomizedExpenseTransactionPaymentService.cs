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
	///* Date Generated:	12-09-2013 10:06:38
	///*******************************************************************************


	public partial class ExpenseTransactionPaymentService
	{

        public List<ExpenseTransactionPayment> GetOrderedExpenseTransactionPayments()
        {
            try
            {
                return _expenseTransactionPaymentManager.GetOrderedExpenseTransactionPayments();
               
            }
            catch (Exception ex)
            {
                ErrorManager.LogApplicationError(ex.StackTrace, ex.Source, ex.Message);
                return new List<ExpenseTransactionPayment>();
            }
        }

        public long UpdateTransactionPayment(ExpenseTransactionPayment expenseTransactionPayment)
        {
            try
            {
                return _expenseTransactionPaymentManager.UpdateTransactionPayment(expenseTransactionPayment);
               
            }
            catch (Exception ex)
            {
                ErrorManager.LogApplicationError(ex.StackTrace, ex.Source, ex.Message);
                return 0;
            }
        }


        public ExpenseTransactionPayment GetUncompltedTransactionPayment(long expenseTransactionPaymentId)
        {
            try
            {
                return _expenseTransactionPaymentManager.GetUncompltedTransactionPayment(expenseTransactionPaymentId);
               
            }
            catch (Exception ex)
            {
                ErrorManager.LogApplicationError(ex.StackTrace, ex.Source, ex.Message);
                return new ExpenseTransactionPayment();
            }
        }

        public ExpenseTransactionPayment AddExpenseTransactionPaymentReturnObject(ExpenseTransactionPayment expenseTransactionPayment)
        {
            try
            {
                return _expenseTransactionPaymentManager.AddExpenseTransactionPaymentReturnObject(expenseTransactionPayment);
               
            }
            catch (Exception ex)
            {
                ErrorManager.LogApplicationError(ex.StackTrace, ex.Source, ex.Message);

                return new ExpenseTransactionPayment();
            }
        }
        
        public List<ExpenseTransactionPayment> GetBeneficiaryUncompletedExpenseTransactionPayments(int beneficiaryId)
        {
            try
            {
                return _expenseTransactionPaymentManager.GetBeneficiaryUnCompletedExpenseTransactionPayments(beneficiaryId);
                
            }
            catch (Exception ex)
            {
                ErrorManager.LogApplicationError(ex.StackTrace, ex.Source, ex.Message);
                return new List<ExpenseTransactionPayment>();
            }
        }
        
        public List<ExpenseTransactionPayment> GetCompletedExpenseTransactionPayments()
        {
            try
            {
                return _expenseTransactionPaymentManager.GetCompletedExpenseTransactionPayments();
                
            }
            catch (Exception ex)
            {
                ErrorManager.LogApplicationError(ex.StackTrace, ex.Source, ex.Message);
                return new List<ExpenseTransactionPayment>();
            }
        }

        public List<ExpenseTransactionPayment> GetUnCompletedExpenseTransactionPayments()
        {
            try
            {
                return _expenseTransactionPaymentManager.GetUnCompletedExpenseTransactionPayments();
               
            }
            catch (Exception ex)
            {
                ErrorManager.LogApplicationError(ex.StackTrace, ex.Source, ex.Message);
                return new List<ExpenseTransactionPayment>();
            }
        }

        public List<ExpenseTransactionPayment> FilterExpenseTransactionPaymentsByDateRange(DateTime startDate, DateTime endDate)
        {
            try
            {
                return _expenseTransactionPaymentManager.FilterExpenseTransactionPaymentsByDateRange(startDate, endDate);
            }
            catch (Exception ex)
            {
                ErrorManager.LogApplicationError(ex.StackTrace, ex.Source, ex.Message);
                return new List<ExpenseTransactionPayment>();
            }
        }


         public List<ExpenseTransactionPayment> GetApprovedTransactionPaymentsByDateRange(DateTime startDate, DateTime endDate)
        {
            try
            {
                return _expenseTransactionPaymentManager.GetApprovedTransactionPaymentsByDateRange(startDate, endDate);
            }
            catch (Exception ex)
            {
                ErrorManager.LogApplicationError(ex.StackTrace, ex.Source, ex.Message);
                return new List<ExpenseTransactionPayment>();
            }
        }

        public List<ExpenseTransactionPayment> GetVoidedTransactionPaymentsByDateRange(DateTime startDate, DateTime endDate)
        {
            try
            {
                return _expenseTransactionPaymentManager.GetVoidedTransactionPaymentsByDateRange(startDate, endDate);
            }
            catch (Exception ex)
            {
                ErrorManager.LogApplicationError(ex.StackTrace, ex.Source, ex.Message);
                return new List<ExpenseTransactionPayment>();
            }
        }

        public List<ExpenseTransactionPayment> GetUnCompletedExpenseTransactionPaymentsByDateRange(DateTime startDate, DateTime endDate)
        {
            try
            {
                return _expenseTransactionPaymentManager.GetUnCompletedExpenseTransactionPaymentsByDateRange(startDate, endDate);
            }
            catch (Exception ex)
            {
                ErrorManager.LogApplicationError(ex.StackTrace, ex.Source, ex.Message);
                return new List<ExpenseTransactionPayment>();
            }
        }

        public List<ExpenseTransactionPayment> GetCompletedExpenseTransactionPaymentsByDateRange(DateTime startDate, DateTime endDate)
        {
            try
            {
                return _expenseTransactionPaymentManager.GetCompletedExpenseTransactionPaymentsByDateRange(startDate, endDate);
            }
            catch (Exception ex)
            {
                ErrorManager.LogApplicationError(ex.StackTrace, ex.Source, ex.Message);
                return new List<ExpenseTransactionPayment>();
            }
        }

        public List<ExpenseTransactionPayment> GetCurrentTransactionPayments(string date)
        {
            try
            {
                return _expenseTransactionPaymentManager.GetCurrentTransactionPayments(date);
            }
            catch (Exception ex)
            {
                ErrorManager.LogApplicationError(ex.StackTrace, ex.Source, ex.Message);
                return new List<ExpenseTransactionPayment>();
            }
        }

        public List<ExpenseTransactionPayment> GetWeeklyTransactionPayments(int status, int dept, string yrVal, string monthVal, int weeklyVal)
        {
            try
            {

                return _expenseTransactionPaymentManager.GetWeeklyTransactionPayments(status, dept, yrVal, monthVal, weeklyVal);
            }
            catch (Exception ex)
            {
                ErrorManager.LogApplicationError(ex.StackTrace, ex.Source, ex.Message);

                return new List<ExpenseTransactionPayment>();
            }
        }

        public List<ExpenseTransactionPayment> GetMonthlyTransactionPayments(int status, int dept, string yrVal, string monthVal)
        {
            try
            {

                return _expenseTransactionPaymentManager.GetMonthlyTransactionPayments(status, dept, yrVal, monthVal);
            }
            catch (Exception ex)
            {
                ErrorManager.LogApplicationError(ex.StackTrace, ex.Source, ex.Message);

                return new List<ExpenseTransactionPayment>();
            }
        }

        public List<ExpenseTransactionPayment> GetTransactionPaymentsByDateRange(string startDate, string endDate, int deptId, int status)
        {
            try
            {

                return _expenseTransactionPaymentManager.GetTransactionPaymentsByDateRange(startDate, endDate, deptId, status);
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
