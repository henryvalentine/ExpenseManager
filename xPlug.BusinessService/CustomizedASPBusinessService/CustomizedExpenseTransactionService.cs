using System;
using System.Collections.Generic;
using System.Linq;
using XPLUG.WEBTOOLS;
using xPlug.BusinessObject;
using xPlug.BusinessObject.CustomizedASPBusinessObject;

namespace xPlug.BusinessService
{
	/// Author:	 Oluwaseyi Adegboyega - sadegboyega@xplugng.com
	/// Copyright © 2013. xPlug Technologies Limited. All Rights Reserved
	///*******************************************************************************
	///* Date Created: 	04-06-2013
	///* Date Generated:	12-09-2013 10:06:36
	///*******************************************************************************


	public partial class ExpenseTransactionService
	{
        public Dictionary<List<int>, List<int>> GetFilteredPortalUsers()
                {
                    try
                    {
                        return _expenseTransactionManager.GetFilteredPortalUsers();
                    }
                    catch (Exception ex)
                    {
                        ErrorManager.LogApplicationError(ex.StackTrace, ex.Source, ex.Message);
                        return new Dictionary<List<int>, List<int>>();
                    }
           
                }

        public List<ExpenseTransaction> GetCurrentApprovedOrVoidedExpenseTransactions()
        {
            try
            {
                return _expenseTransactionManager.GetCurrentApprovedOrVoidedExpenseTransactions();
               
            }
            catch (Exception ex)
            {
                ErrorManager.LogApplicationError(ex.StackTrace, ex.Source, ex.Message);
                return new List<ExpenseTransaction>();
            }
        }
        public List<ExpenseTransaction> GetPortalUserApprovedOrVoidedExpenseTransactionsByDateRange(int userId, DateTime startDate, DateTime endDate)
        {
            try
            {
                return _expenseTransactionManager.GetPortalUserApprovedOrVoidedExpenseTransactionsByDateRange(userId, startDate, endDate);
               
            }
            catch (Exception ex)
            {
                ErrorManager.LogApplicationError(ex.StackTrace, ex.Source, ex.Message);
                return new List<ExpenseTransaction>();
            }
        }

        public List<ExpenseTransaction> GetApprovedOrVoidedExpenseTransactionsByDateRange(DateTime startDate, DateTime endDate)
        {
            try
            {
                return _expenseTransactionManager.GetApprovedOrVoidedExpenseTransactionsByDateRange(startDate, endDate);
               
            }
            catch (Exception ex)
            {
                ErrorManager.LogApplicationError(ex.StackTrace, ex.Source, ex.Message);
                return new List<ExpenseTransaction>();
            }
        }
        
        public List<ExpenseTransaction> GetPortalUserCurrentApprovedOrVoidedExpenseTransactions(int userId)
        {
            try
            {
                return _expenseTransactionManager.GetPortalUserCurrentApprovedOrVoidedExpenseTransactions(userId);
               
            }
            catch (Exception ex)
            {
                ErrorManager.LogApplicationError(ex.StackTrace, ex.Source, ex.Message);
                return new List<ExpenseTransaction>();
            }
        }
        public List<ExpenseTransaction> GetOnlyApprovedExpenseTransactionsWithoutPayments()
        {
            try
            {
                return _expenseTransactionManager.GetCurrentApprovedExpenseTransactionsWithoutPayments();
               
            }
            catch (Exception ex)
            {
                ErrorManager.LogApplicationError(ex.StackTrace, ex.Source, ex.Message);
                return new List<ExpenseTransaction>();
            }
        }
        public List<ExpenseTransaction> GetCurrentApprovedUnpaidExpenseTransactions(String date)
        {
            try
            {
                return _expenseTransactionManager.GetCurrentApprovedUnpaidExpenseTransactions(date);
               
            }
            catch (Exception ex)
            {
                ErrorManager.LogApplicationError(ex.StackTrace, ex.Source, ex.Message);
                return new List<ExpenseTransaction>();
            }
        }
        public List<ExpenseTransaction> GetApprovedUnpaidExpenseTransactionsByDateRange(DateTime startDate, DateTime endDate)
        {
            try
            {
                return _expenseTransactionManager.GetApprovedUnpaidExpenseTransactionsByDateRange(startDate, endDate);
               
            }
            catch (Exception ex)
            {
                ErrorManager.LogApplicationError(ex.StackTrace, ex.Source, ex.Message);
                return new List<ExpenseTransaction>();
            }
        }

        public List<ExpenseTransaction> GetAllApprovedUnpaidExpenseTransactions()
        {
            try
            {
                return _expenseTransactionManager.GetAllApprovedUnpaidExpenseTransactions();
               
            }
            catch (Exception ex)
            {
                ErrorManager.LogApplicationError(ex.StackTrace, ex.Source, ex.Message);
                return new List<ExpenseTransaction>();
            }
        }
        
        public ExpenseTransaction GetExpenseTransactionInfo(long expenseTransactionId)
        {
            try
            {
                return _expenseTransactionManager.GetExpenseTransactionInfo(expenseTransactionId);
               
            }
            catch (Exception ex)
            {
                ErrorManager.LogApplicationError(ex.StackTrace, ex.Source, ex.Message);
                return new ExpenseTransaction();
            }
        }
        public int GetTransactionTotalApprovedQyuantity(long expenseTransactionId)
        {
            try
            {
                return _expenseTransactionManager.GetTransactionTotalApprovedQyuantity(expenseTransactionId);
            }
            catch (Exception ex)
            {
                ErrorManager.LogApplicationError(ex.StackTrace, ex.Source, ex.Message);
                return 0;
            }
        }
        public List<ExpenseTransaction> GetBeneficiaryOnlyApprovedExpenseTransactions(int bebeficiaryId)
        {
            try
            {
                return _expenseTransactionManager.GetApprovedExpenseTransactions(bebeficiaryId);
               
            }
            catch (Exception ex)
            {
                ErrorManager.LogApplicationError(ex.StackTrace, ex.Source, ex.Message);
                return new List<ExpenseTransaction>();
            }
        }
        public List<ExpenseTransaction> GetPendingExpenseTransactionsByCurrentDate()
        {
            try
            {
                return _expenseTransactionManager.GetPendingExpenseTransactionsByCurrentDate();
            }
            catch (Exception ex)
            {
                ErrorManager.LogApplicationError(ex.StackTrace, ex.Source, ex.Message);
                return new List<ExpenseTransaction>();
            }
        }
        public List<Beneficiary> GetDepartmentsWithApprovedExpenseTransactions()
        {
            try
            {
                var objList = _expenseTransactionManager.GetDepartmentsWithApprovedExpenseTransactions();
                if (objList == null)
                {
                    return new List<Beneficiary>();
                }
                return objList;
            }
            catch (Exception ex)
            {
                ErrorManager.LogApplicationError(ex.StackTrace, ex.Source, ex.Message);
                return new List<Beneficiary>();
            }
           
        }
        public List<int> GetPortalUsersWithUnApprovedTransactions()
        {
            try
            {
                var objList = _expenseTransactionManager.GetPortalUsersWithUnApprovedTransactions();
                if (objList == null)
                {
                    return new List<int>();
                }
                return objList;
            }
            catch (Exception ex)
            {
                ErrorManager.LogApplicationError(ex.StackTrace, ex.Source, ex.Message);
                return new List<int>();
            }

        }
        public List<int> GetPortalUsersWithApprovedTransactions()
        {
            try
            {
                var objList = _expenseTransactionManager.GetPortalUsersWithApprovedTransactions();
                if (objList == null)
                {
                    return new List<int>();
                }
                return objList;
            }
            catch (Exception ex)
            {
                ErrorManager.LogApplicationError(ex.StackTrace, ex.Source, ex.Message);
                return new List<int>();
            }

        }
        public List<ExpenseTransaction> GetApprovedTransactionsByPortalUser(int userId)
        {
            try
            {
                return _expenseTransactionManager.GetApprovedTransactionsByPortalUser(userId);
                
            }
            catch (Exception ex)
            {
                ErrorManager.LogApplicationError(ex.StackTrace, ex.Source, ex.Message);
                return new List<ExpenseTransaction>();
            }
        }
        public List<ExpenseTransaction> GetUnApprovedTransactionsByPortalUser(int userId)
        {
            try
            {
                return _expenseTransactionManager.GetUnApprovedTransactionsByPortalUser(userId);
                
            }
            catch (Exception ex)
            {
                ErrorManager.LogApplicationError(ex.StackTrace, ex.Source, ex.Message);
                return new List<ExpenseTransaction>();
            }
        }
        public List<ExpenseTransaction> GetTransactionsByPortalUser(int userId)
        {
            try
            {
                return _expenseTransactionManager.GetTransactionsByPortalUser(userId);
            }
            catch (Exception ex)
            {
                ErrorManager.LogApplicationError(ex.StackTrace, ex.Source, ex.Message);
                return new List<ExpenseTransaction>();
            }
        }
        public List<ExpenseTransaction> GetPortalUserTransactionsByDate(int userId, string transactionDate)
        {
            try
            {
                return _expenseTransactionManager.GetPortalUserTransactionsByDate(userId, transactionDate);
            }
            catch (Exception ex)
            {
                ErrorManager.LogApplicationError(ex.StackTrace, ex.Source, ex.Message);
                return new List<ExpenseTransaction>();
            }
        }
        public List<ExpenseTransaction> GetPendingAndRejectedTransactionsByDateRange(DateTime startDate, DateTime endDate)
        {
            try
            {
                return _expenseTransactionManager.GetPendingAndRejectedTransactionsByDateRange(startDate, endDate);
            }
            catch (Exception ex)
            {
                ErrorManager.LogApplicationError(ex.StackTrace, ex.Source, ex.Message);
                return new List<ExpenseTransaction>();
            }
        }

        public List<ExpenseTransaction> GetPortalUserPendingAndRejectedTransactionsByDateRange(int userId, DateTime startDate, DateTime endDate)
        {
            try
            {
                return _expenseTransactionManager.GetPortalUserPendingAndRejectedTransactionsByDateRange(userId, startDate, endDate);
            }
            catch (Exception ex)
            {
                ErrorManager.LogApplicationError(ex.StackTrace, ex.Source, ex.Message);
                return new List<ExpenseTransaction>();
            }
        }

        public List<ExpenseTransaction> GetPortalUserRejectedTransactionsByDateRange(int userId, DateTime startDate, DateTime endDate)
        {
            try
            {
                return _expenseTransactionManager.GetPortalUserRejectedTransactionsByDateRange(userId, startDate, endDate);
            }
            catch (Exception ex)
            {
                ErrorManager.LogApplicationError(ex.StackTrace, ex.Source, ex.Message);
                return new List<ExpenseTransaction>();
            }
        }
       
        public List<ExpenseTransaction> GetPortalUserPendingTransactionsByDateRange(int userId, DateTime startDate, DateTime endDate)
        {
            try
            {
                return _expenseTransactionManager.GetPortalUserPendingTransactionsByDateRange(userId, startDate, endDate);
            }
            catch (Exception ex)
            {
                ErrorManager.LogApplicationError(ex.StackTrace, ex.Source, ex.Message);
                return new List<ExpenseTransaction>();
            }
        }
      
        public List<ExpenseTransaction> GetPortalUserTransactionsByDateRange(int userId, DateTime startDate, DateTime endDate)
        {
            try
            {
                return _expenseTransactionManager.GetPortalUserTransactionsByDateRange(userId, startDate, endDate);
            }
            catch (Exception ex)
            {
                ErrorManager.LogApplicationError(ex.StackTrace, ex.Source, ex.Message);
                return new List<ExpenseTransaction>();
            }
        }
        public List<ExpenseTransaction> GetBeneficiaryTransactionsByDateRange(int beneficiaryId, DateTime startDate, DateTime endDate)
        {
            try
            {
                return _expenseTransactionManager.GetBeneficiaryTransactionsByDateRange(beneficiaryId, startDate, endDate);
            }
            catch (Exception ex)
            {
                ErrorManager.LogApplicationError(ex.StackTrace, ex.Source, ex.Message);
                return new List<ExpenseTransaction>();
            }
        }
        public List<ExpenseTransaction> GetBeneficiaryransactionsByDate(int beneficiaryId, string transactionDate)
        {
            try
            {
                return _expenseTransactionManager.GetBeneficiaryransactionsByDate(beneficiaryId, transactionDate);
            }
            catch (Exception ex)
            {
                ErrorManager.LogApplicationError(ex.StackTrace, ex.Source, ex.Message);
                return new List<ExpenseTransaction>();
            }
        }
        public List<ExpenseTransaction> GetPortalUserRejectedTransactionsByDate(int userId, string transactionDate)
        {
            try
            {
                return _expenseTransactionManager.GetPortalUserPendingAndRejectedTransactionsByDate(userId, transactionDate);
            }
            catch (Exception ex)
            {
                ErrorManager.LogApplicationError(ex.StackTrace, ex.Source, ex.Message);
                return new List<ExpenseTransaction>();
            }
        }
        public List<ExpenseTransaction> GetExpenseTransactionsByDateRange(DateTime startDate, DateTime endDate)
        {
            try
            {
                return _expenseTransactionManager.GetExpenseTransactionsByDateRange(startDate, endDate);
            }
            catch (Exception ex)
            {
                ErrorManager.LogApplicationError(ex.StackTrace, ex.Source, ex.Message);
                return new List<ExpenseTransaction>();
            }
        }
        public List<ExpenseTransaction> GetPortalUserApprovedTransactionsByDate(int userId, string date)
        {
            try
            {

                return _expenseTransactionManager.GetPortalUserApprovedTransactionsByDate(userId, date);
                
            }
            catch (Exception ex)
            {
                ErrorManager.LogApplicationError(ex.StackTrace, ex.Source, ex.Message);
                return new List<ExpenseTransaction>();
            }
        }
        public List<ExpenseTransaction> GetPortalUserUnApprovedTransactionsByDate(int userId, string date)
        {
            try
            {

                return _expenseTransactionManager.GetPortalUserUnApprovedTransactionsByDate(userId, date);
            }
            catch (Exception ex)
            {
                ErrorManager.LogApplicationError(ex.StackTrace, ex.Source, ex.Message);
                return new List<ExpenseTransaction>();
            }
        }
        public List<ExpenseTransaction> GetExpenseTransactionsByDate(string date)
        {
            try
            {
                return _expenseTransactionManager.GetExpenseTransactionsByDate(date);
            }
            catch (Exception ex)
            {
                ErrorManager.LogApplicationError(ex.StackTrace, ex.Source, ex.Message);
                return new List<ExpenseTransaction>();
            }
        }
        public List<ExpenseTransaction> GetPendingAndRejectedTransactionsByDate(string date)
        {
            try
            {
                return _expenseTransactionManager.GetPendingAndRejectedTransactionsByDate(date);
            }
            catch (Exception ex)
            {
                ErrorManager.LogApplicationError(ex.StackTrace, ex.Source, ex.Message);
                return new List<ExpenseTransaction>();
            }
        }
        public List<ExpenseTransaction> GetExpenseTransactionsByDate(string date, int flag)
        {
            try
            {
                return _expenseTransactionManager.GetExpenseTransactionsByDate(date, flag);
            }
            catch (Exception ex)
            {
                ErrorManager.LogApplicationError(ex.StackTrace, ex.Source, ex.Message);
                return new List<ExpenseTransaction>();
            }
        }
        public List<ExpenseTransaction> GetAllExpenseTransactions()
        {
            try
            {
                return _expenseTransactionManager.GetAllExpenseTransactions();
                 
            }
            catch (Exception ex)
            {
                ErrorManager.LogApplicationError(ex.StackTrace, ex.Source, ex.Message);
                return new List<ExpenseTransaction>();
            }
        }
        public List<ExpenseTransaction> GetTransactionsByBeneficiary(int beneficiaryId)
        {
            try
            {
                return _expenseTransactionManager.GetTransactionsByBeneficiary(beneficiaryId);
                
            }
            catch (Exception ex)
            {
                ErrorManager.LogApplicationError(ex.StackTrace, ex.Source, ex.Message);
                return new List<ExpenseTransaction>();
            }
        }
        public List<ExpenseTransaction> GetApprovedTransactionsByDate(string date)
        {
            try
            {
                return _expenseTransactionManager.GetApprovedTransactionsByDate(date);
            }
            catch (Exception ex)
            {
                ErrorManager.LogApplicationError(ex.StackTrace, ex.Source, ex.Message);
                return new List<ExpenseTransaction>();
            }
        }
        public List<ExpenseTransaction> GetBeneficiaryApprovedTransactionsByDateRange(int beneficiaryId, string startQueryDate, string endQueryDate)
        {
            try
            {
                return _expenseTransactionManager.GetBeneficiaryApprovedTransactionsByDateRange(beneficiaryId, startQueryDate, endQueryDate);
            }
            catch (Exception ex)
            {
                ErrorManager.LogApplicationError(ex.StackTrace, ex.Source, ex.Message);
                return new List<ExpenseTransaction>();
            }
        }
        public List<ExpenseTransaction> GetPortalUserApprovedTransactionsByDateRange(int portalUserId, DateTime startQueryDate, DateTime endQueryDate)
        {
            try
            {
                return _expenseTransactionManager.GetPortalUserApprovedTransactionsByDateRange(portalUserId, startQueryDate, endQueryDate);
            }
            catch (Exception ex)
            {
                ErrorManager.LogApplicationError(ex.StackTrace, ex.Source, ex.Message);
                return new List<ExpenseTransaction>();
            }
        }

        public List<ExpenseTransaction> GetPortalUserVoidedTransactionsByDateRange(int portalUserId, DateTime startQueryDate, DateTime endQueryDate)
        {
            try
            {
                return _expenseTransactionManager.GetPortalUserVoidedTransactionsByDateRange(portalUserId, startQueryDate, endQueryDate);
            }
            catch (Exception ex)
            {
                ErrorManager.LogApplicationError(ex.StackTrace, ex.Source, ex.Message);
                return new List<ExpenseTransaction>();
            }
        }
        
        public List<ExpenseTransaction> GetVoidedTransactionsByDateRange(DateTime startDate, DateTime endDate)
        {
            try
            {
                return _expenseTransactionManager.GetVoidedTransactionsByDateRange(startDate, endDate);
            }
            catch (Exception ex)
            {
                ErrorManager.LogApplicationError(ex.StackTrace, ex.Source, ex.Message);
                return new List<ExpenseTransaction>();
            }
        }
        public List<ExpenseTransaction> GetRejectedTransactionsByDateRange(DateTime startDate, DateTime endDate)
         {
             try
             {
                 return _expenseTransactionManager.GetRejectedTransactionsByDateRange(startDate, endDate);
             }
             catch (Exception ex)
             {
                 ErrorManager.LogApplicationError(ex.StackTrace, ex.Source, ex.Message);
                 return new List<ExpenseTransaction>();
             }
         }
        public List<ExpenseTransaction> GetApprovedTransactionsByDateRange(DateTime start, DateTime stop)
         {
             try
             {
                 return _expenseTransactionManager.GetApprovedTransactionsByDateRange(start, stop);
             }
             catch (Exception ex)
             {
                 ErrorManager.LogApplicationError(ex.StackTrace, ex.Source, ex.Message);
                 return new List<ExpenseTransaction>();
             }
         }
        public List<ExpenseTransaction> GetPendingTransactionsByDateRange(DateTime startDate, DateTime endDate)
        {
            try
            {

                return _expenseTransactionManager.GetPendingTransactionsByDateRange(startDate, endDate);
            }
            catch (Exception ex)
            {
                ErrorManager.LogApplicationError(ex.StackTrace, ex.Source, ex.Message);
                return new List<ExpenseTransaction>();
            }
        }

        public List<ExpenseTransaction> GetBeneficiaryApprovedTransactionsByDate(int beneficiaryId, string transactionDate)
        {
            try
            {

                return _expenseTransactionManager.GetBeneficiaryApprovedTransactionsByDate(beneficiaryId, transactionDate);
            }
            catch (Exception ex)
            {
                ErrorManager.LogApplicationError(ex.StackTrace, ex.Source, ex.Message);
                return new List<ExpenseTransaction>();
            }
        }
        public List<ExpenseTransaction> GetUserApprovedTransactionsByDate(int userId, string date)
        {
            try
            {

                return _expenseTransactionManager.GetUserApprovedTransactionsByDate(userId, date);
            }
            catch (Exception ex)
            {
                ErrorManager.LogApplicationError(ex.StackTrace, ex.Source, ex.Message);
                return new List<ExpenseTransaction>();
            }
        }

        public List<TransactionObject> GetTransObject(int expenseItemId)
        {
            try
            {

                return _expenseTransactionManager.GetTransObject(expenseItemId);
            }
            catch (Exception ex)
            {
                ErrorManager.LogApplicationError(ex.StackTrace, ex.Source, ex.Message);
                return new List<TransactionObject>();
            }
        }
         
        public double GetTransactionTotalAmount(long expenseTransactionId)
        {
            try
            {

                return _expenseTransactionManager.GetTransactionTotalAmount(expenseTransactionId);
            }
            catch (Exception ex)
            {
                ErrorManager.LogApplicationError(ex.StackTrace, ex.Source, ex.Message);
                return 0;
            }
        }
        
         public List<ExpenseTransaction> GetWeeklyTransactions(int status, int dept, string yrVal, string monthVal, int weeklyVal)
        {
            try
            {

                return _expenseTransactionManager.GetWeeklyTransactions(status, dept, yrVal, monthVal, weeklyVal);
            }
            catch (Exception ex)
            {
                ErrorManager.LogApplicationError(ex.StackTrace, ex.Source, ex.Message);

                return new List<ExpenseTransaction>();
            }
        }

         public List<ExpenseTransaction> GetMonthlyTransactions(int status, int dept, string yrVal, string monthVal)
         {
             try
             {

                 return _expenseTransactionManager.GetMonthlyTransactions(status, dept, yrVal, monthVal);
             }
             catch (Exception ex)
             {
                 ErrorManager.LogApplicationError(ex.StackTrace, ex.Source, ex.Message);

                 return new List<ExpenseTransaction>();
             }
         }

        public List<ExpenseTransaction> GetTransactionsByDateRange(string startDate, string endDate, int deptId, int status)
         {
             try
             {

                 return _expenseTransactionManager.GetTransactionsByDateRange(startDate, endDate, deptId, status);
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
