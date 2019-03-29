using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.Linq;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using ExpenseManager.EF;
using ExpenseManager.EF.Helpers;
using XPLUG.WEBTOOLS;
using xPlug.BusinessObject.CustomizedASPBusinessObject;
using xPlug.BusinessObject.CustomizedASPBusinessObject.Enum;
using xPlug.BusinessObjectMapper;
using Beneficiary = xPlug.BusinessObject.Beneficiary;

namespace xPlug.BusinessManager
{
	/// Author:	 Oluwaseyi Adegboyega - sadegboyega@xplugng.com
	/// Copyright © 2013. xPlug Technologies Limited. All Rights Reserved
	///*******************************************************************************
	///* Date Created: 	04-06-2013
	///* Date Generated:	12-09-2013 10:06:35
	///*******************************************************************************


	public partial class ExpenseTransactionManager
	{
        public Dictionary<List<int>, List<int>> GetFilteredPortalUsers()
        {
            try
            {
                var portalUserList = new Dictionary<List<int>, List<int>>();
                var idsForUserWithUnApprovedTransactions = new List<int>();
                var idsForUserWithApprovedTransactions = new List<int>();
                using (var db = new ExpenseManagerDBEntities())
                {
                    var myObjList = db.ExpenseTransactions.ToList();

                    if (!myObjList.Any())
                    {
                        return new Dictionary<List<int>, List<int>>();
                    }

                    foreach (var item in myObjList)
                    {

                        if (idsForUserWithUnApprovedTransactions.All(m => m != item.RegisteredById))
                        {
                            idsForUserWithUnApprovedTransactions.Add(item.RegisteredById);
                        }

                        if (idsForUserWithApprovedTransactions.All(m => m != item.RegisteredById && item.Status == 1))
                        {
                            idsForUserWithApprovedTransactions.Add(item.RegisteredById);
                        }
                    }
                }

                if (!idsForUserWithUnApprovedTransactions.Any() && !idsForUserWithApprovedTransactions.Any())
                {
                    return portalUserList;
                }
                portalUserList.Add(idsForUserWithUnApprovedTransactions, idsForUserWithApprovedTransactions);

                return portalUserList;
            }
            catch (Exception ex)
            {
                ErrorManager.LogApplicationError(ex.StackTrace, ex.Source, ex.Message);
                return new Dictionary<List<int>, List<int>>();
            }
        }
        public List<int> GetPortalUsersWithUnApprovedTransactions()
        {
            try
            {
                var portalUserList = new List<int>();

                using (var db = new ExpenseManagerDBEntities())
                {
                    var myObjList = db.ExpenseTransactions.Where(m => m.Status == 0).ToList();

                    if (!myObjList.Any())
                    {
                        return new List<int>();
                    }

                    foreach (var item in myObjList)
                    {
                        bool any = portalUserList.Any(m => m == item.RegisteredById);
                        if (!any)
                        {
                            portalUserList.Add(item.RegisteredById);
                        }
                    }

                }

                if (!portalUserList.Any())
                {
                    return new List<int>();
                }

                return portalUserList;
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
                var portalUserList = new List<int>();

                using (var db = new ExpenseManagerDBEntities())
                {
                    var myObjList = db.ExpenseTransactions.Where(m => m.Status == 1 || m.Status == 3).ToList();
                    if (myObjList == null || !myObjList.Any())
                    {
                        return new List<int>();
                    }

                    foreach (var item in myObjList)
                    {
                        bool any = portalUserList.Any(m => m == item.RegisteredById);
                        if (!any)
                        {
                            portalUserList.Add(item.RegisteredById);
                        }
                    }

                }

                if (!portalUserList.Any())
                {
                    return new List<int>();
                }

                return portalUserList;
            }
            catch (Exception ex)
            {
                ErrorManager.LogApplicationError(ex.StackTrace, ex.Source, ex.Message);
                return new List<int>();
            }

        }
        public List<BusinessObject.ExpenseTransaction> GetPendingExpenseTransactionsByCurrentDate()
        {
            try
            {
                var currentDate = DateMap.GetLocalDate();
                using (var db = new ExpenseManagerDBEntities())
                {
                    var objList = db.ExpenseTransactions.Where(m => (m.Status == 0) && m.TransactionDate == currentDate).ToList();
                    var myBusinessObjList = new List<BusinessObject.ExpenseTransaction>();
                    if (!objList.Any())
                    {
                        return myBusinessObjList;
                    }
                    var paymentList = db.ExpenseTransactionPayments.ToList();

                    if (!paymentList.Any())
                    {
                        paymentList = new List<ExpenseTransactionPayment>();
                    }
                    //Re-Map each Entity Object to Business Object
                    foreach (var item in objList)
                    {
                        var myBusinessObj = ExpenseTransactionMapper.Map<ExpenseTransaction, BusinessObject.ExpenseTransaction>(item);
                        if (myBusinessObj == null)
                        {
                            continue;
                        }

                        if (!paymentList.Any())
                        {
                            myBusinessObj.PaymentStatus = Enum.GetName(typeof(ExpensePaymentStatus), ExpensePaymentStatus.Not_Paid).Replace("_", " ");
                        }
                        else
                        {

                            var payment = paymentList.Find(m => m.ExpenseTransactionId == myBusinessObj.ExpenseTransactionId);

                            if (payment == null || payment.ExpenseTransactionPaymentId < 1)
                            {
                                myBusinessObj.PaymentStatus = Enum.GetName(typeof(ExpensePaymentStatus), ExpensePaymentStatus.Not_Paid).Replace("_", " ");
                            }
                            else
                            {
                                if (payment.AmountPaid.Equals(0))
                                {
                                    myBusinessObj.PaymentStatus = Enum.GetName(typeof(ExpensePaymentStatus), ExpensePaymentStatus.Not_Paid).Replace("_", " ");
                                }

                                if (!payment.AmountPaid.Equals(0) && payment.AmountPaid < payment.TotalAmountPayable)
                                {
                                    myBusinessObj.PaymentStatus = Enum.GetName(typeof(ExpensePaymentStatus), ExpensePaymentStatus.Partly_Paid).Replace("_", " ");
                                }

                                if (payment.AmountPaid.Equals(payment.TotalAmountPayable))
                                {
                                    myBusinessObj.PaymentStatus = Enum.GetName(typeof(ExpensePaymentStatus), ExpensePaymentStatus.Fully_Paid).Replace("_", " ");
                                }
                            }

                        }
                        myBusinessObj.ApprovalStatus = Enum.GetName(typeof(ExpenseApprovalStatus), myBusinessObj.Status);
                        myBusinessObj.TransactionDate = DateMap.ReverseToGeneralDate(myBusinessObj.TransactionDate);
                        myBusinessObjList.Add(myBusinessObj);
                    }

                    if (!myBusinessObjList.Any())
                    {
                        return new List<BusinessObject.ExpenseTransaction>();
                    }
                    
                    return myBusinessObjList.OrderBy(m => m.ExpenseTitle).ToList();
                }
            }
            catch(Exception ex)
            {
                ErrorManager.LogApplicationError(ex.StackTrace, ex.Source, ex.Message);
                return new List<BusinessObject.ExpenseTransaction>();
            }
        }
        public List<Beneficiary> GetDepartmentsWithApprovedExpenseTransactions()
        {
            try
            {
                using (var db = new ExpenseManagerDBEntities())
                {
                    var objList = db.ExpenseTransactions.Where(m => m.Status == 1).ToList();
                    var myBusinessObjList = new List<Beneficiary>();
                    var expenseTransactionList = new List<BusinessObject.ExpenseTransaction>();
                    if (!objList.Any())
                    {
                        return myBusinessObjList;
                    }

                    //Re-Map each Entity Object to Business Object
                    foreach (var item in objList)
                    {
                        var myBusinessObj = ExpenseTransactionMapper.Map<ExpenseTransaction, BusinessObject.ExpenseTransaction>(item);
                        if (myBusinessObj == null)
                        {
                            continue;
                        }
                        expenseTransactionList.Add(myBusinessObj);
                    }

                    foreach (var item in expenseTransactionList)
                    {
                        bool any = myBusinessObjList.Any(m => m.BeneficiaryId == item.BeneficiaryId);
                        if (!any)
                        {
                            myBusinessObjList.Add(item.Beneficiary);
                        }
                    }

                    if (!myBusinessObjList.Any())
                    {
                        return new List<BusinessObject.Beneficiary>();
                    }

                    return myBusinessObjList.OrderBy(m => m.FullName).ToList();
                }
            }
            catch (Exception ex)
            {
                ErrorManager.LogApplicationError(ex.StackTrace, ex.Source, ex.Message);
                return new List<BusinessObject.Beneficiary>();
            }
        }
        public List<BusinessObject.ExpenseTransaction> GetApprovedOrVoidedExpenseTransactionsByDateRange(DateTime startDate, DateTime endDate)
        {
            try
            {
                using (var db = new ExpenseManagerDBEntities())
                {
                    var objList = db.ExpenseTransactions.Where(m => (m.Status == 1 || m.Status == 3)).ToList();
                    var newTransactionList = new List<ExpenseTransaction>();
                    newTransactionList.AddRange(from transaction in objList
                                                let transactionDate = DateTime.Parse(transaction.DateApproved)
                                                where
                                                    transactionDate == startDate ||
                                                    (transactionDate > startDate && transactionDate < endDate) ||
                                                    transactionDate == endDate
                                                select transaction);

                    var expenseTransactionList = new List<BusinessObject.ExpenseTransaction>();

                    if (!newTransactionList.Any())
                    {
                        return expenseTransactionList;
                    }
                    var paymentList = db.ExpenseTransactionPayments.ToList();

                    if (!paymentList.Any())
                    {
                        paymentList = new List<ExpenseTransactionPayment>();
                    }
                    //Re-Map each Entity Object to Business Object
                    foreach (var item in newTransactionList)
                    {
                        var myBusinessObj =
                            ExpenseTransactionMapper.Map<ExpenseTransaction, BusinessObject.ExpenseTransaction>(item);

                        if (myBusinessObj == null)
                        {
                            continue;
                        }


                        if (!paymentList.Any())
                        {
                            myBusinessObj.PaymentStatus =
                                Enum.GetName(typeof(ExpensePaymentStatus), ExpensePaymentStatus.Not_Paid).Replace("_",
                                                                                                                   " ");
                        }
                        else
                        {
                            var payment =
                                paymentList.Find(m => m.ExpenseTransactionId == myBusinessObj.ExpenseTransactionId);

                            if (payment == null || payment.ExpenseTransactionPaymentId < 1)
                            {
                                myBusinessObj.PaymentStatus = Enum.GetName(typeof(ExpensePaymentStatus), ExpensePaymentStatus.Not_Paid).Replace("_", " ");
                            }
                            else
                            {
                                if (payment.AmountPaid.Equals(0))
                                {
                                    myBusinessObj.PaymentStatus = Enum.GetName(typeof(ExpensePaymentStatus), ExpensePaymentStatus.Not_Paid).
                                            Replace("_", " ");
                                }

                                if (!payment.AmountPaid.Equals(0) && payment.AmountPaid < payment.TotalAmountPayable)
                                {
                                    myBusinessObj.PaymentStatus =
                                        Enum.GetName(typeof(ExpensePaymentStatus), ExpensePaymentStatus.Partly_Paid).
                                            Replace("_", " ");
                                }

                                if (payment.AmountPaid.Equals(payment.TotalAmountPayable))
                                {
                                    myBusinessObj.PaymentStatus =
                                        Enum.GetName(typeof(ExpensePaymentStatus), ExpensePaymentStatus.Fully_Paid).
                                            Replace("_", " ");
                                }
                            }
                        }
                        myBusinessObj.ApprovalStatus = Enum.GetName(typeof(ExpenseApprovalStatus), myBusinessObj.Status);
                        expenseTransactionList.Add(myBusinessObj);
                    }

                    if (!expenseTransactionList.Any())
                    {
                        return new List<BusinessObject.ExpenseTransaction>();
                    }
                    return expenseTransactionList.OrderByDescending(m => m.TransactionDate).ToList();
                }
            }
            catch (Exception ex)
            {
                ErrorManager.LogApplicationError(ex.StackTrace, ex.Source, ex.Message);
                return new List<BusinessObject.ExpenseTransaction>();
            }
        }
        public List<BusinessObject.ExpenseTransaction> GetCurrentApprovedOrVoidedExpenseTransactions()  
        {
            try
            {
                var currentDate = DateMap.GetLocalDate();
                using (var db = new ExpenseManagerDBEntities())
                {
                    var objList = db.ExpenseTransactions.Where(m => (m.Status == 1 || m.Status == 3) && m.DateApproved == currentDate).ToList();
                    var expenseTransactionList = new List<BusinessObject.ExpenseTransaction>();
                    if (!objList.Any())
                    {
                        return expenseTransactionList;
                    }

                    var paymentList = db.ExpenseTransactionPayments.ToList();

                    if (!paymentList.Any())
                    {
                        paymentList = new List<ExpenseTransactionPayment>();
                    }

                    //Re-Map each Entity Object to Business Object
                    foreach (var item in objList)
                    {
                        var myBusinessObj =
                            ExpenseTransactionMapper.Map<ExpenseTransaction, BusinessObject.ExpenseTransaction>(item);

                        if (myBusinessObj == null)
                        {
                            continue;
                        }
                        if (!paymentList.Any())
                        {
                            myBusinessObj.PaymentStatus =
                                Enum.GetName(typeof(ExpensePaymentStatus), ExpensePaymentStatus.Not_Paid).Replace("_", " ");
                        }
                        else
                        {
                            var payment = paymentList.Find(m => m.ExpenseTransactionId == myBusinessObj.ExpenseTransactionId);

                            if (payment == null || payment.ExpenseTransactionPaymentId < 1)
                            {
                                myBusinessObj.PaymentStatus = Enum.GetName(typeof(ExpensePaymentStatus), ExpensePaymentStatus.Not_Paid).Replace("_", " ");
                            }
                            else
                            {
                                if (payment.AmountPaid.Equals(0))
                                {
                                    myBusinessObj.PaymentStatus =
                                        Enum.GetName(typeof(ExpensePaymentStatus), ExpensePaymentStatus.Not_Paid).Replace("_", " ");
                                }

                                if (!payment.AmountPaid.Equals(0) && payment.AmountPaid < payment.TotalAmountPayable)
                                {
                                    myBusinessObj.PaymentStatus = Enum.GetName(typeof(ExpensePaymentStatus), ExpensePaymentStatus.Partly_Paid).Replace("_", " ");
                                }

                                if (payment.AmountPaid.Equals(payment.TotalAmountPayable))
                                {
                                    myBusinessObj.PaymentStatus = Enum.GetName(typeof(ExpensePaymentStatus), ExpensePaymentStatus.Fully_Paid).Replace("_", " ");
                                }
                            }

                        }
                        myBusinessObj.ApprovalStatus = Enum.GetName(typeof(ExpenseApprovalStatus), myBusinessObj.Status);
                        expenseTransactionList.Add(myBusinessObj);
                    }

                    if (!expenseTransactionList.Any())
                    {
                        return new List<BusinessObject.ExpenseTransaction>();
                    }
                    return expenseTransactionList.OrderBy(m => m.ExpenseTitle).ToList();
                }
            }
            catch (Exception ex)
            {
                ErrorManager.LogApplicationError(ex.StackTrace, ex.Source, ex.Message);
                return new List<BusinessObject.ExpenseTransaction>();
            }
        }
        public List<BusinessObject.ExpenseTransaction> GetPortalUserCurrentApprovedOrVoidedExpenseTransactions(int userId)  
        {
            try
            {
                var currentDate = DateMap.GetLocalDate();
                using (var db = new ExpenseManagerDBEntities())
                {
                    var objList = db.ExpenseTransactions.Where(m => (m.Status == 1 || m.Status == 3) && m.RegisteredById == userId && m.DateApproved == currentDate).ToList();
                    var expenseTransactionList = new List<BusinessObject.ExpenseTransaction>();
                    if (!objList.Any())
                    {
                        return expenseTransactionList;
                    }
                    var paymentList = db.ExpenseTransactionPayments.ToList();

                    if (!paymentList.Any())
                    {
                        paymentList = new List<ExpenseTransactionPayment>();
                    }
                    //Re-Map each Entity Object to Business Object
                    foreach (var item in objList)
                    {
                        var myBusinessObj = ExpenseTransactionMapper.Map<ExpenseTransaction, BusinessObject.ExpenseTransaction>(item);

                        if (myBusinessObj == null)
                        {
                            continue;
                        }

                        if (!paymentList.Any())
                        {
                            myBusinessObj.PaymentStatus =
                                Enum.GetName(typeof (ExpensePaymentStatus), ExpensePaymentStatus.Not_Paid).Replace("_"," ");
                        }
                        else
                        {
                            var payment = paymentList.Find(m => m.ExpenseTransactionId == myBusinessObj.ExpenseTransactionId);

                            if (payment == null || payment.ExpenseTransactionPaymentId < 1)
                            {
                                myBusinessObj.PaymentStatus = Enum.GetName(typeof (ExpensePaymentStatus), ExpensePaymentStatus.Not_Paid).Replace("_", " ");
                            }
                            else
                            {
                                if (payment.AmountPaid.Equals(0))
                                {
                                    myBusinessObj.PaymentStatus = Enum.GetName(typeof (ExpensePaymentStatus), ExpensePaymentStatus.Not_Paid).Replace("_", " ");
                                }

                                if (!payment.AmountPaid.Equals(0) && payment.AmountPaid < payment.TotalAmountPayable)
                                {
                                    myBusinessObj.PaymentStatus = Enum.GetName(typeof (ExpensePaymentStatus), ExpensePaymentStatus.Partly_Paid).Replace("_", " ");
                                }

                                if (payment.AmountPaid.Equals(payment.TotalAmountPayable))
                                {
                                    myBusinessObj.PaymentStatus = Enum.GetName(typeof (ExpensePaymentStatus), ExpensePaymentStatus.Fully_Paid).Replace("_", " ");
                                }
                            }

                        }
                        myBusinessObj.ApprovalStatus = Enum.GetName(typeof (ExpenseApprovalStatus), myBusinessObj.Status);
                        if (myBusinessObj.Status == 0)
                        {
                            var nAStatus = Enum.GetName(typeof(PendingStatus), PendingStatus.N_A).Replace("_", "/");
                            myBusinessObj.DateApproved = nAStatus;
                            myBusinessObj.TimeApproved = nAStatus;
                        }
                        expenseTransactionList.Add(myBusinessObj);
                    }

                    return expenseTransactionList.OrderBy(m => m.ExpenseTitle).ToList();
                }
            }

            catch (Exception ex)
            {
                ErrorManager.LogApplicationError(ex.StackTrace, ex.Source, ex.Message);
                return new List<BusinessObject.ExpenseTransaction>();
            }
        }
        public List<BusinessObject.ExpenseTransaction> GetPortalUserApprovedOrVoidedExpenseTransactionsByDateRange(int userId, DateTime startDate, DateTime endDate)
        {
            try
            {
                using (var db = new ExpenseManagerDBEntities())
                {
                    var objList = db.ExpenseTransactions.Where(m => (m.Status == 1 || m.Status == 3) && m.RegisteredById == userId).ToList();
                    var expenseTransactionList = new List<BusinessObject.ExpenseTransaction>();
                    if (!objList.Any())
                    {
                        return expenseTransactionList;
                    }

                    var newTransactionList = new List<ExpenseTransaction>();
                    newTransactionList.AddRange(from transaction in objList
                                                let transactionDate = DateTime.Parse(transaction.DateApproved)
                                                where
                                                    transactionDate == startDate ||
                                                    (transactionDate > startDate && transactionDate < endDate) ||
                                                    transactionDate == endDate
                                                select transaction);
                   
                    if (!newTransactionList.Any())
                    {
                        return expenseTransactionList;
                    }

                    var paymentList = db.ExpenseTransactionPayments.ToList();

                    if (!paymentList.Any())
                    {
                        paymentList = new List<ExpenseTransactionPayment>();
                    }
                    //Re-Map each Entity Object to Business Object
                    foreach (var item in newTransactionList)
                    {
                        var myBusinessObj =
                            ExpenseTransactionMapper.Map<ExpenseTransaction, BusinessObject.ExpenseTransaction>(item);

                        if (myBusinessObj == null)
                        {
                            continue;
                        }


                        if (!paymentList.Any())
                        {
                            myBusinessObj.PaymentStatus =
                                Enum.GetName(typeof(ExpensePaymentStatus), ExpensePaymentStatus.Not_Paid).Replace("_", " ");
                        }
                        else
                        {
                            var payment = paymentList.Find(m => m.ExpenseTransactionId == myBusinessObj.ExpenseTransactionId);

                            if (payment == null || payment.ExpenseTransactionPaymentId < 1)
                            {
                                myBusinessObj.PaymentStatus = Enum.GetName(typeof(ExpensePaymentStatus), ExpensePaymentStatus.Not_Paid).Replace("_", " ");
                            }
                            else
                            {
                                if (payment.AmountPaid.Equals(0))
                                {
                                    myBusinessObj.PaymentStatus =
                                        Enum.GetName(typeof(ExpensePaymentStatus), ExpensePaymentStatus.Not_Paid).Replace("_", " ");
                                }

                                if (!payment.AmountPaid.Equals(0) && payment.AmountPaid < payment.TotalAmountPayable)
                                {
                                    myBusinessObj.PaymentStatus = Enum.GetName(typeof(ExpensePaymentStatus), ExpensePaymentStatus.Partly_Paid).Replace("_", " ");
                                }

                                if (payment.AmountPaid.Equals(payment.TotalAmountPayable))
                                {
                                    myBusinessObj.PaymentStatus = Enum.GetName(typeof(ExpensePaymentStatus), ExpensePaymentStatus.Fully_Paid).Replace("_", " ");
                                }
                            }

                        }
                        myBusinessObj.ApprovalStatus = Enum.GetName(typeof(ExpenseApprovalStatus), myBusinessObj.Status);
                        expenseTransactionList.Add(myBusinessObj);
                    }

                    if (!expenseTransactionList.Any())
                    {
                        return new List<BusinessObject.ExpenseTransaction>();
                    }
                    return expenseTransactionList.OrderBy(m => m.ExpenseTitle).ToList();
                }
            }
            catch (Exception ex)
            {
                ErrorManager.LogApplicationError(ex.StackTrace, ex.Source, ex.Message);
                return new List<BusinessObject.ExpenseTransaction>();
            }
        }
        public List<BusinessObject.ExpenseTransaction> GetApprovedExpenseTransactions( int beneficiaryId)
        {
            try
            {
                using (var db = new ExpenseManagerDBEntities())
                {
                    var objList = db.ExpenseTransactions.Where(m => m.Status == 1 && m.BeneficiaryId == beneficiaryId).ToList();
                    var expenseTransactionList = new List<BusinessObject.ExpenseTransaction>();
                    if (!objList.Any())
                    {
                        return expenseTransactionList;
                    }

                    //Re-Map each Entity Object to Business Object
                    expenseTransactionList.AddRange(objList.Select(item => ExpenseTransactionMapper.Map<ExpenseTransaction, BusinessObject.ExpenseTransaction>(item)).Where(myBusinessObj => myBusinessObj != null));

                    foreach (var expenseTransaction in expenseTransactionList)
                    {
                       expenseTransaction.ApprovalStatus = Enum.GetName(typeof (ExpenseApprovalStatus),expenseTransaction.Status);
                      
                    }

                    return expenseTransactionList.OrderBy(m => m.ExpenseTitle).ToList();
                }
            }
            catch (Exception ex)
            {
                ErrorManager.LogApplicationError(ex.StackTrace, ex.Source, ex.Message);
                return new List<BusinessObject.ExpenseTransaction>();
            }
        }
        public BusinessObject.ExpenseTransaction GetExpenseTransactionInfo(long expenseTransactionId)
        {
            try
            {
                using (var db = new ExpenseManagerDBEntities())
                {
                    var obj = db.ExpenseTransactions.Where(m => m.Status == 1 && m.ExpenseTransactionId == expenseTransactionId).ToList();
                    if (!obj.Any())
                    {
                        return new BusinessObject.ExpenseTransaction();
                    }

                    //Re-Map each Entity Object to Business Object
                    var myBusinessObj = ExpenseTransactionMapper.Map<ExpenseTransaction, BusinessObject.ExpenseTransaction>(obj.ElementAt(0));
                    if (myBusinessObj == null)
                    {
                        return new BusinessObject.ExpenseTransaction();
                    }
                    
                    myBusinessObj.ApprovalStatus = Enum.GetName(typeof (ExpenseApprovalStatus),myBusinessObj.Status);
                    myBusinessObj.TotalApprovedQuantity = db.TransactionItems.Where(m => m.ExpenseTransactionId == myBusinessObj.ExpenseTransactionId && m.ApprovedQuantity > 0).Sum(m => m.ApprovedQuantity);  
                    
                    return myBusinessObj;
                }
            }
            catch (Exception ex)
            {
                ErrorManager.LogApplicationError(ex.StackTrace, ex.Source, ex.Message);
                return new BusinessObject.ExpenseTransaction();
            }
        }
        public int GetTransactionTotalApprovedQyuantity(long expenseTransactionId)
        {
            try
            {
                using (var db = new ExpenseManagerDBEntities())
                {
                    var totalApprovedQuantity = db.TransactionItems.Where(m => m.ExpenseTransactionId == expenseTransactionId && m.ApprovedQuantity > 0).Sum(m => m.ApprovedQuantity);  
                    
                    if(totalApprovedQuantity < 1)
                    {
                        return 0;
                    }

                    return totalApprovedQuantity;
                }
            }
            catch (Exception ex)
            {
                ErrorManager.LogApplicationError(ex.StackTrace, ex.Source, ex.Message);
                return 0;
            }
        }
        public List<BusinessObject.ExpenseTransaction> GetCurrentApprovedExpenseTransactionsWithoutPayments()
        {
            try
            {
                using (var db = new ExpenseManagerDBEntities())
                {

                    var objList = (from x in db.ExpenseTransactions
                                    where x.Status == 1 && x.DateApproved == DateMap.GetLocalDate()
                                    from y in x.ExpenseTransactionPayments
                                    where y.ExpenseTransactionId != x.ExpenseTransactionId
                                    select x).ToList();

                    var expenseTransactionList = new List<BusinessObject.ExpenseTransaction>();
                    
                    if (!objList.Any())
                    {
                        return expenseTransactionList;
                    }
                 
                    //Re-Map each Entity Object to Business Object
                    expenseTransactionList.AddRange(objList.Select(item => ExpenseTransactionMapper.Map<ExpenseTransaction, BusinessObject.ExpenseTransaction>(item)).Where(myBusinessObj => myBusinessObj != null));

                    if (!expenseTransactionList.Any())
                    {
                        return expenseTransactionList;
                    }

                    foreach (var expenseTransaction in expenseTransactionList)
                    {
                        expenseTransaction.ApprovalStatus = Enum.GetName(typeof(ExpenseApprovalStatus), expenseTransaction.Status);

                        
                    }

                    return expenseTransactionList.OrderBy(m => m.ExpenseTitle).ToList();
                }
            }
            catch (Exception ex)
            {
                ErrorManager.LogApplicationError(ex.StackTrace, ex.Source, ex.Message);
                return new List<BusinessObject.ExpenseTransaction>();
            }
        }
        public List<BusinessObject.ExpenseTransaction> GetCurrentApprovedUnpaidExpenseTransactions(string date)
        {
            try
            {
                using (var db = new ExpenseManagerDBEntities())
                {

                    var objList = db.ExpenseTransactions.Where(m => (m.Status == 1 && !m.ExpenseTransactionPayments.Any() && m.DateApproved == date)).ToList();

                    var expenseTransactionList = new List<BusinessObject.ExpenseTransaction>();

                    if (!objList.Any())
                    {
                        return expenseTransactionList;
                    }

                    //Re-Map each Entity Object to Business Object
                    expenseTransactionList.AddRange(objList.Select(item => ExpenseTransactionMapper.Map<ExpenseTransaction, BusinessObject.ExpenseTransaction>(item)).Where(myBusinessObj => myBusinessObj != null));

                    if (!expenseTransactionList.Any())
                    {
                        return expenseTransactionList;
                    }

                    foreach (var expenseTransaction in expenseTransactionList)
                    {
                        expenseTransaction.ApprovalStatus = Enum.GetName(typeof(ExpenseApprovalStatus), expenseTransaction.Status);


                    }

                    return expenseTransactionList.OrderBy(m => m.DateApproved).ToList();
                }
            }
            catch (Exception ex)
            {
                ErrorManager.LogApplicationError(ex.StackTrace, ex.Source, ex.Message);
                return new List<BusinessObject.ExpenseTransaction>();
            }
        }
        public List<BusinessObject.ExpenseTransaction> GetApprovedUnpaidExpenseTransactionsByDateRange(DateTime startDate, DateTime endDate)
        {
            try
            {
                using (var db = new ExpenseManagerDBEntities())
                {

                    var objList = db.ExpenseTransactions.Where(m => m.Status == 1 && !m.ExpenseTransactionPayments.Any()).ToList();
                    var newTransactionList = new List<ExpenseTransaction>();
                    newTransactionList.AddRange(from transaction in objList
                                                let transactionDate = DateTime.Parse(transaction.DateApproved)
                                                where
                                                    transactionDate == startDate ||
                                                    (transactionDate > startDate && transactionDate < endDate) ||
                                                    transactionDate == endDate
                                                select transaction);

                    var expenseTransactionList = new List<BusinessObject.ExpenseTransaction>();

                    if (!newTransactionList.Any())
                    {
                        return expenseTransactionList;
                    }

                    //Re-Map each Entity Object to Business Object
                    expenseTransactionList.AddRange(newTransactionList.Select(item => ExpenseTransactionMapper.Map<ExpenseTransaction, BusinessObject.ExpenseTransaction>(item)).Where(myBusinessObj => myBusinessObj != null));

                    if (!expenseTransactionList.Any())
                    {
                        return expenseTransactionList;
                    }

                    foreach (var expenseTransaction in expenseTransactionList)
                    {
                        expenseTransaction.ApprovalStatus = Enum.GetName(typeof(ExpenseApprovalStatus), expenseTransaction.Status);


                    }

                    return expenseTransactionList.OrderBy(m => m.DateApproved).ToList();
                }
            }
            catch (Exception ex)
            {
                ErrorManager.LogApplicationError(ex.StackTrace, ex.Source, ex.Message);
                return new List<BusinessObject.ExpenseTransaction>();
            }
        }
        public List<BusinessObject.ExpenseTransaction> GetAllApprovedUnpaidExpenseTransactions()
        {
            try
            {
                using (var db = new ExpenseManagerDBEntities())
                {

                    var objList = db.ExpenseTransactions.Where(m => m.Status == 1 && !m.ExpenseTransactionPayments.Any()).ToList();
                    var expenseTransactionList = new List<BusinessObject.ExpenseTransaction>();
                    if (!objList.Any())
                    {
                        return expenseTransactionList;
                    }

                    //Re-Map each Entity Object to Business Object
                    expenseTransactionList.AddRange(objList.Select(item => ExpenseTransactionMapper.Map<ExpenseTransaction, BusinessObject.ExpenseTransaction>(item)).Where(myBusinessObj => myBusinessObj != null));

                    if (!expenseTransactionList.Any())
                    {
                        return expenseTransactionList;
                    }

                    foreach (var expenseTransaction in expenseTransactionList)
                    {
                        expenseTransaction.ApprovalStatus = Enum.GetName(typeof(ExpenseApprovalStatus), expenseTransaction.Status);
                        
                    }

                    return expenseTransactionList.OrderBy(m => m.ExpenseTitle).ToList();
                }
            }
            catch (Exception ex)
            {
                ErrorManager.LogApplicationError(ex.StackTrace, ex.Source, ex.Message);
                return new List<BusinessObject.ExpenseTransaction>();
            }
        }
        public List<BusinessObject.ExpenseTransaction> GetApprovedTransactionsByPortalUser(int userId)
        {
            try
            {
                using (var db = new ExpenseManagerDBEntities())
                {
                    var objList =
                        db.ExpenseTransactions.Where(
                            m => m.RegisteredById == userId && (m.Status == 1)).ToList();

                    var expenseTransactionList = new List<BusinessObject.ExpenseTransaction>();

                    if (!objList.Any())
                    {
                        return expenseTransactionList;
                    }

                    //Re-Map each Entity Object to Business Object
                    foreach (var item in objList)
                    {
                        var myBusinessObj =
                            ExpenseTransactionMapper.Map<ExpenseTransaction, BusinessObject.ExpenseTransaction>(item);

                        if (myBusinessObj == null)
                        {
                            continue;
                        }
                        myBusinessObj.ApprovalStatus = Enum.GetName(typeof(ExpenseApprovalStatus), myBusinessObj.Status);
                        
                        expenseTransactionList.Add(myBusinessObj);
                    }

                    return expenseTransactionList.OrderBy(m => m.ExpenseTitle).ToList();
                }
            }
            catch (Exception ex)
            {
                ErrorManager.LogApplicationError(ex.StackTrace, ex.Source, ex.Message);
                return new List<BusinessObject.ExpenseTransaction>();
            }
        }
        public List<BusinessObject.ExpenseTransaction> GetUnApprovedTransactionsByPortalUser(int userId)
        {
            try
            {
                using (var db = new ExpenseManagerDBEntities())
                {
                    var objList =
                        db.ExpenseTransactions.Where(
                            m => m.RegisteredById == userId && m.Status == 0).ToList();

                    var expenseTransactionList = new List<BusinessObject.ExpenseTransaction>();

                    if (!objList.Any())
                    {
                        return expenseTransactionList;
                    }
                    var paymentList = db.ExpenseTransactionPayments.ToList();

                    if (!paymentList.Any())
                    {
                        paymentList = new List<ExpenseTransactionPayment>();
                    }
                    //Re-Map each Entity Object to Business Object
                    foreach (var item in objList)
                    {
                        var myBusinessObj =
                            ExpenseTransactionMapper.Map<ExpenseTransaction, BusinessObject.ExpenseTransaction>(item);

                        if (myBusinessObj == null)
                        {
                            continue;
                        }

                        if (!paymentList.Any())
                        {
                            myBusinessObj.PaymentStatus = Enum.GetName(typeof(ExpensePaymentStatus), ExpensePaymentStatus.Not_Paid).Replace("_", " ");
                        }
                        else
                        {
                            var payment = paymentList.Find(m => m.ExpenseTransactionId == myBusinessObj.ExpenseTransactionId);

                            if (payment == null || payment.ExpenseTransactionPaymentId < 1)
                            {
                                myBusinessObj.PaymentStatus = Enum.GetName(typeof(ExpensePaymentStatus), ExpensePaymentStatus.Not_Paid).Replace("_", " ");
                            }
                            else
                            {
                                if (payment.AmountPaid.Equals(0))
                                {
                                    myBusinessObj.PaymentStatus = Enum.GetName(typeof(ExpensePaymentStatus), ExpensePaymentStatus.Not_Paid).Replace("_", " ");
                                }

                                if (!payment.AmountPaid.Equals(0) && payment.AmountPaid < payment.TotalAmountPayable)
                                {
                                    myBusinessObj.PaymentStatus = Enum.GetName(typeof(ExpensePaymentStatus), ExpensePaymentStatus.Partly_Paid).Replace("_", " ");
                                }

                                if (payment.AmountPaid.Equals(payment.TotalAmountPayable))
                                {
                                    myBusinessObj.PaymentStatus = Enum.GetName(typeof(ExpensePaymentStatus), ExpensePaymentStatus.Fully_Paid).Replace("_", " ");
                                }
                            }

                        }
                        myBusinessObj.ApprovalStatus = Enum.GetName(typeof(ExpenseApprovalStatus), myBusinessObj.Status);
                        
                        expenseTransactionList.Add(myBusinessObj);
                    }
                    
                    return expenseTransactionList.OrderBy(m => m.ExpenseTitle).ToList();
                }
            }
            catch (Exception ex)
            {
                ErrorManager.LogApplicationError(ex.StackTrace, ex.Source, ex.Message);
                return new List<BusinessObject.ExpenseTransaction>();
            }
        }
        public List<BusinessObject.ExpenseTransaction> GetTransactionsByPortalUser(int userId)
        {
            try
            {
                using (var db = new ExpenseManagerDBEntities())
                {
                    var objList =
                        db.ExpenseTransactions.Where(m => m.RegisteredById == userId).ToList();

                    var expenseTransactionList = new List<BusinessObject.ExpenseTransaction>();

                    if (!objList.Any())
                    {
                        return expenseTransactionList;
                    }
                    var paymentList = db.ExpenseTransactionPayments.ToList();

                    if (!paymentList.Any())
                    {
                        paymentList = new List<ExpenseTransactionPayment>();
                    }
                    //Re-Map each Entity Object to Business Object
                    foreach (var item in objList)
                    {
                        var myBusinessObj =
                            ExpenseTransactionMapper.Map<ExpenseTransaction, BusinessObject.ExpenseTransaction>(item);

                        if (myBusinessObj == null)
                        {
                            continue;
                        }

                        if (!paymentList.Any())
                        {
                            myBusinessObj.PaymentStatus = Enum.GetName(typeof(ExpensePaymentStatus), ExpensePaymentStatus.Not_Paid).Replace("_", " ");
                        }
                        else
                        {
                            var payment = paymentList.Find(m => m.ExpenseTransactionId == myBusinessObj.ExpenseTransactionId);

                            if (payment == null || payment.ExpenseTransactionPaymentId < 1)
                            {
                                var name = Enum.GetName(typeof(ExpensePaymentStatus), ExpensePaymentStatus.Not_Paid);
                                if (name != null)
                                    myBusinessObj.PaymentStatus = name.Replace("_", " ");
                            }
                            else
                            {
                                if (payment.AmountPaid.Equals(0))
                                {
                                    var name = Enum.GetName(typeof(ExpensePaymentStatus), ExpensePaymentStatus.Not_Paid);
                                    if (name !=
                                        null)
                                        myBusinessObj.PaymentStatus = name.Replace("_", " ");
                                }

                                if (!payment.AmountPaid.Equals(0) && payment.AmountPaid < payment.TotalAmountPayable)
                                {
                                    var name = Enum.GetName(typeof(ExpensePaymentStatus), ExpensePaymentStatus.Partly_Paid);
                                    if (name !=
                                        null)
                                        myBusinessObj.PaymentStatus = name.Replace("_", " ");
                                }

                                if (payment.AmountPaid.Equals(payment.TotalAmountPayable))
                                {
                                    var name = Enum.GetName(typeof(ExpensePaymentStatus), ExpensePaymentStatus.Fully_Paid);
                                    if (name !=
                                        null)
                                        myBusinessObj.PaymentStatus = name.Replace("_", " ");
                                }
                            }
                        }
                        myBusinessObj.ApprovalStatus = Enum.GetName(typeof(ExpenseApprovalStatus), myBusinessObj.Status);
                        
                        expenseTransactionList.Add(myBusinessObj);
                    }
                    return expenseTransactionList.OrderBy(m => m.ExpenseTitle).ToList();
                }
            }
            catch (Exception ex)
            {
                ErrorManager.LogApplicationError(ex.StackTrace, ex.Source, ex.Message);
                return new List<BusinessObject.ExpenseTransaction>();
            }
        }
        public List<BusinessObject.ExpenseTransaction> GetPortalUserTransactionsByDate(int userId, string transactionDate)
        {
            try
            {
                using (var db = new ExpenseManagerDBEntities())
                {
                    var objList =
                        db.ExpenseTransactions.Where(
                            m => m.RegisteredById == userId && m.TransactionDate == transactionDate).ToList();
                    var expenseTransactionList = new List<BusinessObject.ExpenseTransaction>();

                    if (!objList.Any())
                    {
                        return expenseTransactionList;
                    }
                    var paymentList = db.ExpenseTransactionPayments.ToList();

                    if (!paymentList.Any())
                    {
                        paymentList = new List<ExpenseTransactionPayment>();
                    }
                    //Re-Map each Entity Object to Business Object
                    foreach (var item in objList)
                    {
                        var myBusinessObj =
                            ExpenseTransactionMapper.Map<ExpenseTransaction, BusinessObject.ExpenseTransaction>(item);

                        if (myBusinessObj == null)
                        {
                            continue;
                        }

                        if (!paymentList.Any())
                        {
                            myBusinessObj.PaymentStatus = Enum.GetName(typeof(ExpensePaymentStatus), ExpensePaymentStatus.Not_Paid).Replace("_", " ");
                        }
                        else
                        {
                            var payment = paymentList.Find(m => m.ExpenseTransactionId == myBusinessObj.ExpenseTransactionId);

                            if (payment == null || payment.ExpenseTransactionPaymentId < 1)
                            {
                                myBusinessObj.PaymentStatus = Enum.GetName(typeof(ExpensePaymentStatus), ExpensePaymentStatus.Not_Paid).Replace("_", " ");
                            }
                            else
                            {
                                if (payment.AmountPaid.Equals(0))
                                {
                                    myBusinessObj.PaymentStatus = Enum.GetName(typeof(ExpensePaymentStatus), ExpensePaymentStatus.Not_Paid).Replace("_", " ");
                                }

                                if (!payment.AmountPaid.Equals(0) && payment.AmountPaid < payment.TotalAmountPayable)
                                {
                                    myBusinessObj.PaymentStatus = Enum.GetName(typeof(ExpensePaymentStatus), ExpensePaymentStatus.Partly_Paid).Replace("_", " ");
                                }

                                if (payment.AmountPaid.Equals(payment.TotalAmountPayable))
                                {
                                    myBusinessObj.PaymentStatus = Enum.GetName(typeof(ExpensePaymentStatus), ExpensePaymentStatus.Fully_Paid).Replace("_", " ");
                                }
                            }
                        }
                        if (myBusinessObj.Status == 0)
                        {
                            myBusinessObj.DateApproved = Enum.GetName(typeof(ExpenseApprovalDateTime), myBusinessObj.Status);
                            myBusinessObj.TimeApproved = Enum.GetName(typeof(ExpenseApprovalDateTime), myBusinessObj.Status);
                        }
                        myBusinessObj.ApprovalStatus = Enum.GetName(typeof(ExpenseApprovalStatus), myBusinessObj.Status);
                        
                        expenseTransactionList.Add(myBusinessObj);
                    }
                    return expenseTransactionList.OrderBy(m => m.ExpenseTitle).ToList();
                }
            }
            catch (Exception ex)
            {
                ErrorManager.LogApplicationError(ex.StackTrace, ex.Source, ex.Message);
                return new List<BusinessObject.ExpenseTransaction>();
            }
        }
        public List<BusinessObject.ExpenseTransaction> GetPortalUserPendingAndRejectedTransactionsByDate(int userId, string transactionDate)
        {
            try
            {
                using (var db = new ExpenseManagerDBEntities())
                {
                    var objList =
                        db.ExpenseTransactions.Where(m => m.RegisteredById == userId && ((m.Status == 0 && m.TransactionDate == transactionDate) || (m.Status == 2 && m.DateApproved == transactionDate))).ToList();
                    var expenseTransactionList = new List<BusinessObject.ExpenseTransaction>();

                    if (!objList.Any())
                    {
                        return expenseTransactionList;
                    }
                    
                    //Re-Map each Entity Object to Business Object
                    foreach (var item in objList)
                    {
                        var myBusinessObj =
                            ExpenseTransactionMapper.Map<ExpenseTransaction, BusinessObject.ExpenseTransaction>(item);

                        if (myBusinessObj == null)
                        {
                            continue;
                        }

                        if (myBusinessObj.Status == 0)
                        {
                            var nAStatus = Enum.GetName(typeof(PendingStatus), PendingStatus.N_A).Replace("_", "/");
                            myBusinessObj.PaymentStatus = nAStatus;
                            myBusinessObj.ApproverComment = nAStatus;
                            myBusinessObj.DateApproved = nAStatus;
                            myBusinessObj.TimeApproved = nAStatus;
                        }
                        if (myBusinessObj.Status == 2)
                        {
                            myBusinessObj.PaymentStatus =
                                        Enum.GetName(typeof(ExpensePaymentStatus), ExpensePaymentStatus.Not_Paid).
                                            Replace("_", " ");
                        }
                        myBusinessObj.ApprovalStatus = Enum.GetName(typeof(ExpenseApprovalStatus), myBusinessObj.Status);

                        expenseTransactionList.Add(myBusinessObj);
                    }
                    return expenseTransactionList.OrderBy(m => m.ExpenseTitle).ToList();
                }
            }
            catch (Exception ex)
            {
                ErrorManager.LogApplicationError(ex.StackTrace, ex.Source, ex.Message);
                return new List<BusinessObject.ExpenseTransaction>();
            }
        }
        public List<BusinessObject.ExpenseTransaction> GetPendingAndRejectedTransactionsByDate(string date)
        {
            try
            {
                using (var db = new ExpenseManagerDBEntities())
                {
                    var objList = db.ExpenseTransactions.Where(m => (m.Status == 0 && m.TransactionDate == date) || (m.Status == 2 && m.DateApproved == date)).ToList();

                    var expenseTransactionList = new List<BusinessObject.ExpenseTransaction>();

                    if (!objList.Any())
                    {
                        return expenseTransactionList;
                    }
                    //Re-Map each Entity Object to Business Object
                    foreach (var item in objList)
                    {
                        var myBusinessObj =
                            ExpenseTransactionMapper.Map<ExpenseTransaction, BusinessObject.ExpenseTransaction>(item);

                        if (myBusinessObj == null)
                        {
                            continue;
                        }
                        myBusinessObj.ApprovalStatus = Enum.GetName(typeof(ExpenseApprovalStatus), myBusinessObj.Status);

                        if (myBusinessObj.Status == 0)
                        {
                            var name = Enum.GetName(typeof(PendingStatus), PendingStatus.N_A);
                            if (name != null)
                            {
                                var nAStatus = name.Replace("_", "/");
                                myBusinessObj.PaymentStatus = nAStatus;
                                myBusinessObj.ApproverComment = nAStatus;
                                myBusinessObj.DateApproved = nAStatus;
                                myBusinessObj.TimeApproved = nAStatus;
                            }
                        }
                        if (myBusinessObj.Status == 2)
                        {
                            myBusinessObj.PaymentStatus =
                                        Enum.GetName(typeof(ExpensePaymentStatus), ExpensePaymentStatus.Not_Paid).
                                            Replace("_", " ");
                        }
                        expenseTransactionList.Add(myBusinessObj);
                    }

                    if (!expenseTransactionList.Any())
                    {
                        return new List<BusinessObject.ExpenseTransaction>();
                    }
                    return expenseTransactionList.OrderBy(m => m.ExpenseTitle).ToList();
                }
            }
            catch (Exception ex)
            {
                ErrorManager.LogApplicationError(ex.StackTrace, ex.Source, ex.Message);
                return new List<BusinessObject.ExpenseTransaction>();
            }
        }
        public List<BusinessObject.ExpenseTransaction> GetPendingAndRejectedTransactionsByDateRange(DateTime startDate, DateTime endDate)
        {
            try
            {
                using (var db = new ExpenseManagerDBEntities())
                {
                    var objList = db.ExpenseTransactions.Where(m => m.Status == 2 || m.Status == 0).ToList();
                    var newTransactionList = new List<ExpenseTransaction>();
                    newTransactionList.AddRange(from transaction in objList
                                                let transactionDate = DateTime.Parse(transaction.TransactionDate)
                                                where
                                                    transactionDate == startDate ||
                                                    (transactionDate > startDate && transactionDate < endDate) ||
                                                    transactionDate == endDate
                                                select transaction);

                    var expenseTransactionList = new List<BusinessObject.ExpenseTransaction>();

                    if (!newTransactionList.Any())
                    {
                        return expenseTransactionList;
                    }
                    //Re-Map each Entity Object to Business Object
                    foreach (var item in newTransactionList)
                    {
                        var myBusinessObj =
                            ExpenseTransactionMapper.Map<ExpenseTransaction, BusinessObject.ExpenseTransaction>(item);

                        if (myBusinessObj == null)
                        {
                            continue;
                        }
                        if (myBusinessObj.Status == 0)
                        {
                            var nAStatus = Enum.GetName(typeof(PendingStatus), PendingStatus.N_A).Replace("_", "/");
                            myBusinessObj.PaymentStatus = nAStatus;
                            myBusinessObj.ApproverComment = nAStatus;
                            myBusinessObj.DateApproved = nAStatus;
                            myBusinessObj.TimeApproved = nAStatus;
                        }
                        if (myBusinessObj.Status == 2)
                        {
                            myBusinessObj.PaymentStatus =
                                        Enum.GetName(typeof(ExpensePaymentStatus), ExpensePaymentStatus.Not_Paid).
                                            Replace("_", " ");
                        }
                        myBusinessObj.ApprovalStatus = Enum.GetName(typeof(ExpenseApprovalStatus), myBusinessObj.Status);
                        expenseTransactionList.Add(myBusinessObj);
                    }

                    if (!expenseTransactionList.Any())
                    {
                        return new List<BusinessObject.ExpenseTransaction>();
                    }
                    return expenseTransactionList.OrderByDescending(m => m.TransactionDate).ToList();
                }
            }
            catch (Exception ex)
            {
                ErrorManager.LogApplicationError(ex.StackTrace, ex.Source, ex.Message);
                return new List<BusinessObject.ExpenseTransaction>();
            }
        }
        public List<BusinessObject.ExpenseTransaction> GetPortalUserPendingAndRejectedTransactionsByDateRange(int userId, DateTime startDate, DateTime endDate)
        {
            try
            {
                using (var db = new ExpenseManagerDBEntities())
                {
                    var objList = db.ExpenseTransactions.Where(m => (m.Status == 2 || m.Status == 0) && m.RegisteredById == userId).ToList();
                    var newTransactionList = new List<ExpenseTransaction>();
                    newTransactionList.AddRange(from transaction in objList
                                                let transactionDate = DateTime.Parse(transaction.TransactionDate)
                                                where
                                                    transactionDate == startDate ||
                                                    (transactionDate > startDate && transactionDate < endDate) ||
                                                    transactionDate == endDate
                                                select transaction);

                    var expenseTransactionList = new List<BusinessObject.ExpenseTransaction>();

                    if (!newTransactionList.Any())
                    {
                        return expenseTransactionList;
                    }
                    //Re-Map each Entity Object to Business Object
                    foreach (var item in newTransactionList)
                    {
                        var myBusinessObj =
                            ExpenseTransactionMapper.Map<ExpenseTransaction, BusinessObject.ExpenseTransaction>(item);

                        if (myBusinessObj == null)
                        {
                            continue;
                        }
                        if (myBusinessObj.Status == 0)
                        {
                            var nAStatus = Enum.GetName(typeof(PendingStatus), PendingStatus.N_A).Replace("_", "/");
                            myBusinessObj.PaymentStatus = nAStatus;
                            myBusinessObj.ApproverComment = nAStatus;
                            myBusinessObj.DateApproved = nAStatus;
                            myBusinessObj.TimeApproved = nAStatus;
                        }
                        if (myBusinessObj.Status == 2)
                        {
                            myBusinessObj.PaymentStatus =
                                        Enum.GetName(typeof(ExpensePaymentStatus), ExpensePaymentStatus.Not_Paid).
                                            Replace("_", " ");
                        }
                        myBusinessObj.ApprovalStatus = Enum.GetName(typeof(ExpenseApprovalStatus), myBusinessObj.Status);
                        expenseTransactionList.Add(myBusinessObj);
                    }

                    if (!expenseTransactionList.Any())
                    {
                        return new List<BusinessObject.ExpenseTransaction>();
                    }
                    return expenseTransactionList.OrderByDescending(m => m.TransactionDate).ToList();
                }
            }
            catch (Exception ex)
            {
                ErrorManager.LogApplicationError(ex.StackTrace, ex.Source, ex.Message);
                return new List<BusinessObject.ExpenseTransaction>();
            }
        }
        public List<BusinessObject.ExpenseTransaction> GetPortalUserRejectedTransactionsByDateRange(int userId, DateTime startDate, DateTime endDate)
        {
            try
            {
                using (var db = new ExpenseManagerDBEntities())
                {
                    var objList = db.ExpenseTransactions.Where(m => (m.Status == 2) && m.RegisteredById == userId).ToList();
                    var newTransactionList = new List<ExpenseTransaction>();
                    newTransactionList.AddRange(from transaction in objList
                                                let transactionDate = DateTime.Parse(transaction.TransactionDate)
                                                where
                                                    transactionDate == startDate ||
                                                    (transactionDate > startDate && transactionDate < endDate) ||
                                                    transactionDate == endDate
                                                select transaction);

                    var expenseTransactionList = new List<BusinessObject.ExpenseTransaction>();

                    if (!newTransactionList.Any())
                    {
                        return expenseTransactionList;
                    }
                    //Re-Map each Entity Object to Business Object
                    foreach (var item in newTransactionList)
                    {
                        var myBusinessObj =
                            ExpenseTransactionMapper.Map<ExpenseTransaction, BusinessObject.ExpenseTransaction>(item);

                        if (myBusinessObj == null)
                        {
                            continue;
                        }
                        if (myBusinessObj.Status == 0)
                        {
                            var nAStatus = Enum.GetName(typeof(PendingStatus), PendingStatus.N_A).Replace("_", "/");
                            myBusinessObj.PaymentStatus = nAStatus;
                            myBusinessObj.ApproverComment = nAStatus;
                            myBusinessObj.DateApproved = nAStatus;
                            myBusinessObj.TimeApproved = nAStatus;
                        }
                        if (myBusinessObj.Status == 2)
                        {
                            myBusinessObj.PaymentStatus =
                                        Enum.GetName(typeof(ExpensePaymentStatus), ExpensePaymentStatus.Not_Paid).
                                            Replace("_", " ");
                        }
                        myBusinessObj.ApprovalStatus = Enum.GetName(typeof(ExpenseApprovalStatus), myBusinessObj.Status);
                        expenseTransactionList.Add(myBusinessObj);
                    }

                    if (!expenseTransactionList.Any())
                    {
                        return new List<BusinessObject.ExpenseTransaction>();
                    }
                    return expenseTransactionList.OrderByDescending(m => m.TransactionDate).ToList();
                }
            }
            catch (Exception ex)
            {
                ErrorManager.LogApplicationError(ex.StackTrace, ex.Source, ex.Message);
                return new List<BusinessObject.ExpenseTransaction>();
            }
        }
        public List<BusinessObject.ExpenseTransaction> GetPortalUserPendingTransactionsByDateRange(int userId, DateTime startDate, DateTime endDate)
        {
            try
            {
                using (var db = new ExpenseManagerDBEntities())
                {
                    var objList = db.ExpenseTransactions.Where(m => (m.Status == 0) && m.RegisteredById == userId).ToList();
                    var newTransactionList = new List<ExpenseTransaction>();
                    newTransactionList.AddRange(from transaction in objList
                                                let transactionDate = DateTime.Parse(transaction.TransactionDate)
                                                where
                                                    transactionDate == startDate ||
                                                    (transactionDate > startDate && transactionDate < endDate) ||
                                                    transactionDate == endDate
                                                select transaction);

                    var expenseTransactionList = new List<BusinessObject.ExpenseTransaction>();

                    if (!newTransactionList.Any())
                    {
                        return expenseTransactionList;
                    }
                    //Re-Map each Entity Object to Business Object
                    foreach (var item in newTransactionList)
                    {
                        var myBusinessObj =
                            ExpenseTransactionMapper.Map<ExpenseTransaction, BusinessObject.ExpenseTransaction>(item);

                        if (myBusinessObj == null)
                        {
                            continue;
                        }
                        if (myBusinessObj.Status == 0)
                        {
                            var nAStatus = Enum.GetName(typeof(PendingStatus), PendingStatus.N_A).Replace("_", "/");
                            myBusinessObj.PaymentStatus = nAStatus;
                            myBusinessObj.ApproverComment = nAStatus;
                            myBusinessObj.DateApproved = nAStatus;
                            myBusinessObj.TimeApproved = nAStatus;
                        }
                        if (myBusinessObj.Status == 2)
                        {
                            myBusinessObj.PaymentStatus =
                                        Enum.GetName(typeof(ExpensePaymentStatus), ExpensePaymentStatus.Not_Paid).
                                            Replace("_", " ");
                        }
                        myBusinessObj.ApprovalStatus = Enum.GetName(typeof(ExpenseApprovalStatus), myBusinessObj.Status);
                        expenseTransactionList.Add(myBusinessObj);
                    }

                    if (!expenseTransactionList.Any())
                    {
                        return new List<BusinessObject.ExpenseTransaction>();
                    }
                    return expenseTransactionList.OrderByDescending(m => m.TransactionDate).ToList();
                }
            }
            catch (Exception ex)
            {
                ErrorManager.LogApplicationError(ex.StackTrace, ex.Source, ex.Message);
                return new List<BusinessObject.ExpenseTransaction>();
            }
        }
        public List<BusinessObject.ExpenseTransaction> GetExpenseTransactionsByDateRange(DateTime startDate, DateTime endDate)
        {
            try
            {
                using (var db = new ExpenseManagerDBEntities())
                {
                    var objList = db.ExpenseTransactions.ToList();
                    var newTransactionList = new List<ExpenseTransaction>();
                    newTransactionList.AddRange(from transaction in objList
                                                let transactionDate = DateTime.Parse(transaction.TransactionDate)
                                                where
                                                    transactionDate == startDate ||
                                                    (transactionDate > startDate && transactionDate < endDate) ||
                                                    transactionDate == endDate
                                                select transaction);

                    var expenseTransactionList = new List<BusinessObject.ExpenseTransaction>();

                    if (!newTransactionList.Any())
                    {
                        return expenseTransactionList;
                    }
                    var paymentList = db.ExpenseTransactionPayments.ToList();

                    if (!paymentList.Any())
                    {
                        paymentList = new List<ExpenseTransactionPayment>();
                    }
                    //Re-Map each Entity Object to Business Object
                    foreach (var item in newTransactionList)
                    {
                        var myBusinessObj = ExpenseTransactionMapper.Map<ExpenseTransaction, BusinessObject.ExpenseTransaction>(item);

                        if (myBusinessObj == null)
                        {
                            continue;
                        }


                        if (!paymentList.Any())
                        {
                            myBusinessObj.PaymentStatus =
                                Enum.GetName(typeof (ExpensePaymentStatus), ExpensePaymentStatus.Not_Paid).Replace("_",
                                                                                                                   " ");
                        }
                        else
                        {
                            var payment =
                                paymentList.Find(m => m.ExpenseTransactionId == myBusinessObj.ExpenseTransactionId);

                            if (payment == null || payment.ExpenseTransactionPaymentId < 1)
                            {
                                myBusinessObj.PaymentStatus =
                                    Enum.GetName(typeof (ExpensePaymentStatus), ExpensePaymentStatus.Not_Paid).Replace(
                                        "_", " ");
                            }
                            else
                            {
                                if (payment.AmountPaid.Equals(0))
                                {
                                    myBusinessObj.PaymentStatus =
                                        Enum.GetName(typeof (ExpensePaymentStatus), ExpensePaymentStatus.Not_Paid).
                                            Replace("_", " ");
                                }

                                if (!payment.AmountPaid.Equals(0) && payment.AmountPaid < payment.TotalAmountPayable)
                                {
                                    myBusinessObj.PaymentStatus =
                                        Enum.GetName(typeof (ExpensePaymentStatus), ExpensePaymentStatus.Partly_Paid).
                                            Replace("_", " ");
                                }

                                if (payment.AmountPaid.Equals(payment.TotalAmountPayable))
                                {
                                    myBusinessObj.PaymentStatus =
                                        Enum.GetName(typeof (ExpensePaymentStatus), ExpensePaymentStatus.Fully_Paid).
                                            Replace("_", " ");
                                }
                            }

                        }
                        myBusinessObj.ApprovalStatus = Enum.GetName(typeof (ExpenseApprovalStatus), myBusinessObj.Status);
                        if (myBusinessObj.Status == 0)
                        {
                            var nAStatus = Enum.GetName(typeof(PendingStatus), PendingStatus.N_A).Replace("_", "/");
                            myBusinessObj.DateApproved = nAStatus;
                            myBusinessObj.TimeApproved = nAStatus;
                        }
                        expenseTransactionList.Add(myBusinessObj);
                    }

                    if (!expenseTransactionList.Any())
                    {
                        return new List<BusinessObject.ExpenseTransaction>();
                    }
                    return expenseTransactionList.OrderByDescending(m => m.TransactionDate).ToList();
                }
            }
            catch (Exception ex)
            {
                ErrorManager.LogApplicationError(ex.StackTrace, ex.Source, ex.Message);
                return new List<BusinessObject.ExpenseTransaction>();
            }
        }
        public List<BusinessObject.ExpenseTransaction> GetPortalUserTransactionsByDateRange(int userId, DateTime startDate, DateTime endDate)
        {
            try
            {
                using (var db = new ExpenseManagerDBEntities())
                {
                    var objList = db.ExpenseTransactions.ToList();
                    var newTransactionList = new List<ExpenseTransaction>();
                    newTransactionList.AddRange(from transaction in objList
                                                let transactionDate = DateTime.Parse(transaction.TransactionDate)
                                                where
                                                    (transactionDate == startDate ||
                                                    (transactionDate > startDate && transactionDate < endDate) ||
                                                    transactionDate == endDate) && transaction.RegisteredById == userId
                                                select transaction);

                    var expenseTransactionList = new List<BusinessObject.ExpenseTransaction>();

                    if (!newTransactionList.Any())
                    {
                        return expenseTransactionList;
                    }
                    var paymentList = db.ExpenseTransactionPayments.ToList();

                    if (!paymentList.Any())
                    {
                        paymentList = new List<ExpenseTransactionPayment>();
                    }
                    //Re-Map each Entity Object to Business Object
                    foreach (var item in newTransactionList)
                    {
                        var myBusinessObj =
                            ExpenseTransactionMapper.Map<ExpenseTransaction, BusinessObject.ExpenseTransaction>(item);

                        if (myBusinessObj == null)
                        {
                            continue;
                        }


                        if (!paymentList.Any())
                        {
                            myBusinessObj.PaymentStatus = Enum.GetName(typeof(ExpensePaymentStatus), ExpensePaymentStatus.Not_Paid).Replace("_", " ");
                        }
                        else
                        {
                            var payment = paymentList.Find(m => m.ExpenseTransactionId == myBusinessObj.ExpenseTransactionId);

                            if (payment == null || payment.ExpenseTransactionPaymentId < 1)
                            {
                                myBusinessObj.PaymentStatus = Enum.GetName(typeof(ExpensePaymentStatus), ExpensePaymentStatus.Not_Paid).Replace("_", " ");
                            }
                            else
                            {
                                if (payment.AmountPaid.Equals(0))
                                {
                                    myBusinessObj.PaymentStatus = Enum.GetName(typeof(ExpensePaymentStatus), ExpensePaymentStatus.Not_Paid).Replace("_", " ");
                                }

                                if (!payment.AmountPaid.Equals(0) && payment.AmountPaid < payment.TotalAmountPayable)
                                {
                                    myBusinessObj.PaymentStatus = Enum.GetName(typeof(ExpensePaymentStatus), ExpensePaymentStatus.Partly_Paid).Replace("_", " ");
                                }

                                if (payment.AmountPaid.Equals(payment.TotalAmountPayable))
                                {
                                    myBusinessObj.PaymentStatus = Enum.GetName(typeof(ExpensePaymentStatus), ExpensePaymentStatus.Fully_Paid).Replace("_", " ");
                                }
                            }

                        }
                        myBusinessObj.ApprovalStatus = Enum.GetName(typeof(ExpenseApprovalStatus), myBusinessObj.Status);
                        
                        expenseTransactionList.Add(myBusinessObj);
                    }

                    if (!expenseTransactionList.Any())
                    {
                        return new List<BusinessObject.ExpenseTransaction>();
                    }
                    return expenseTransactionList.OrderBy(m => m.ExpenseTitle).ToList();
                }
            }
            catch (Exception ex)
            {
                ErrorManager.LogApplicationError(ex.StackTrace, ex.Source, ex.Message);
                return new List<BusinessObject.ExpenseTransaction>();
            }
        }
        public List<BusinessObject.ExpenseTransaction> GetPortalUserApprovedTransactionsByDate(int userId, string date)
        {
            try
            {

                using (var db = new ExpenseManagerDBEntities())
                {
                    var objList = db.ExpenseTransactions.Where(m => m.RegisteredById == userId && (m.Status == 1 || m.Status == 3) && m.TransactionDate == date).ToList();

                    var expenseTransactionList = new List<BusinessObject.ExpenseTransaction>();

                    if (!objList.Any())
                    {
                        return expenseTransactionList;
                    }
                    var paymentList = db.ExpenseTransactionPayments.ToList();

                    if (!paymentList.Any())
                    {
                        paymentList = new List<ExpenseTransactionPayment>();
                    }
                    //Re-Map each Entity Object to Business Object
                    foreach (var item in objList)
                    {
                        var myBusinessObj =
                            ExpenseTransactionMapper.Map<ExpenseTransaction, BusinessObject.ExpenseTransaction>(item);

                        if (myBusinessObj == null)
                        {
                            continue;
                        }


                        if (!paymentList.Any())
                        {
                            myBusinessObj.PaymentStatus = Enum.GetName(typeof(ExpensePaymentStatus), ExpensePaymentStatus.Not_Paid).Replace("_", " ");
                        }
                        else
                        {
                            var payment = paymentList.Find(m => m.ExpenseTransactionId == myBusinessObj.ExpenseTransactionId);

                            if (payment == null || payment.ExpenseTransactionPaymentId < 1)
                            {
                                myBusinessObj.PaymentStatus = Enum.GetName(typeof(ExpensePaymentStatus), ExpensePaymentStatus.Not_Paid).Replace("_", " ");
                            }
                            else
                            {
                                if (payment.AmountPaid.Equals(0))
                                {
                                    myBusinessObj.PaymentStatus = Enum.GetName(typeof(ExpensePaymentStatus), ExpensePaymentStatus.Not_Paid).Replace("_", " ");
                                }

                                if (!payment.AmountPaid.Equals(0) && payment.AmountPaid < payment.TotalAmountPayable)
                                {
                                    myBusinessObj.PaymentStatus = Enum.GetName(typeof(ExpensePaymentStatus), ExpensePaymentStatus.Partly_Paid).Replace("_", " ");
                                }

                                if (payment.AmountPaid.Equals(payment.TotalAmountPayable))
                                {
                                    myBusinessObj.PaymentStatus = Enum.GetName(typeof(ExpensePaymentStatus), ExpensePaymentStatus.Fully_Paid).Replace("_", " ");
                                }
                            }

                        }
                        myBusinessObj.ApprovalStatus = Enum.GetName(typeof(ExpenseApprovalStatus), myBusinessObj.Status);
                        
                        expenseTransactionList.Add(myBusinessObj);
                    }

                    if (!expenseTransactionList.Any())
                    {
                        return new List<BusinessObject.ExpenseTransaction>();
                    }
                    return expenseTransactionList.OrderBy(m => m.ExpenseTitle).ToList();
                }
            }
            catch (Exception ex)
            {
                ErrorManager.LogApplicationError(ex.StackTrace, ex.Source, ex.Message);
                return new List<BusinessObject.ExpenseTransaction>();
            }
        }
        public List<BusinessObject.ExpenseTransaction> GetPortalUserUnApprovedTransactionsByDate(int userId, string date)
        {
            try
            {
                 using (var db = new ExpenseManagerDBEntities())
                {
                    var objList = db.ExpenseTransactions.Where(m => m.RegisteredById == userId && (m.Status == 0) && m.TransactionDate == date).ToList();

                    var expenseTransactionList = new List<BusinessObject.ExpenseTransaction>();

                    if (!objList.Any())
                    {
                        return expenseTransactionList;
                    }
                    var paymentList = db.ExpenseTransactionPayments.ToList();

                    if (!paymentList.Any())
                    {
                        paymentList = new List<ExpenseTransactionPayment>();
                    }
                    //Re-Map each Entity Object to Business Object
                    foreach (var item in objList)
                    {
                        var myBusinessObj =
                            ExpenseTransactionMapper.Map<ExpenseTransaction, BusinessObject.ExpenseTransaction>(item);

                        if (myBusinessObj == null)
                        {
                            continue;
                        }


                        if (!paymentList.Any())
                        {
                            var name = Enum.GetName(typeof(ExpensePaymentStatus), ExpensePaymentStatus.Not_Paid);
                            if (name != null)
                                myBusinessObj.PaymentStatus = name.Replace("_", " ");
                        }
                        else
                        {
                            var payment = paymentList.Find(m => m.ExpenseTransactionId == myBusinessObj.ExpenseTransactionId);

                            if (payment == null || payment.ExpenseTransactionPaymentId < 1)
                            {
                                var name = Enum.GetName(typeof(ExpensePaymentStatus), ExpensePaymentStatus.Not_Paid);
                                if (name != null)
                                    myBusinessObj.PaymentStatus = name.Replace("_", " ");
                            }
                            else
                            {
                                if (payment.AmountPaid.Equals(0))
                                {
                                    var name = Enum.GetName(typeof(ExpensePaymentStatus), ExpensePaymentStatus.Not_Paid);
                                    if (name !=
                                        null)
                                        myBusinessObj.PaymentStatus = name.Replace("_", " ");
                                }

                                if (!payment.AmountPaid.Equals(0) && payment.AmountPaid < payment.TotalAmountPayable)
                                {
                                    var name = Enum.GetName(typeof(ExpensePaymentStatus), ExpensePaymentStatus.Partly_Paid);
                                    if (name !=
                                        null)
                                        myBusinessObj.PaymentStatus = name.Replace("_", " ");
                                }

                                if (payment.AmountPaid.Equals(payment.TotalAmountPayable))
                                {
                                    var name = Enum.GetName(typeof(ExpensePaymentStatus), ExpensePaymentStatus.Fully_Paid);
                                    if (name !=
                                        null)
                                        myBusinessObj.PaymentStatus = name.Replace("_", " ");
                                }
                            }

                        }
                        myBusinessObj.ApprovalStatus = Enum.GetName(typeof(ExpenseApprovalStatus), myBusinessObj.Status);
                        
                        expenseTransactionList.Add(myBusinessObj);
                    }

                    if (!expenseTransactionList.Any())
                    {
                        return new List<BusinessObject.ExpenseTransaction>();
                    }
                    return expenseTransactionList.OrderBy(m => m.ExpenseTitle).ToList();
                }
            }
            catch (Exception ex)
            {
                ErrorManager.LogApplicationError(ex.StackTrace, ex.Source, ex.Message);
                return new List<BusinessObject.ExpenseTransaction>();
            }
        }
        public List<BusinessObject.ExpenseTransaction> GetBeneficiaryTransactionsByDateRange(int beneficiaryId, DateTime startDate, DateTime endDate)
        {
            try
            {
                using (var db = new ExpenseManagerDBEntities())
                {
                    var objList = db.ExpenseTransactions.ToList();
                    var myBusinessObjList = new List<BusinessObject.ExpenseTransaction>();
                    if (!objList.Any())
                    {
                        return myBusinessObjList;
                    }

                    var newTransactionList = new List<ExpenseTransaction>();
                    newTransactionList.AddRange(from transaction in objList
                                                let transactionDate = DateTime.Parse(transaction.TransactionDate)
                                                where
                                                    (transactionDate == startDate || (transactionDate > startDate && transactionDate < endDate) || transactionDate == endDate) && transaction.BeneficiaryId == beneficiaryId
                                                select transaction);
                    if (!newTransactionList.Any())
                    {
                        return myBusinessObjList;
                    }
                    var paymentList = db.ExpenseTransactionPayments.ToList();

                    if (!paymentList.Any())
                    {
                        paymentList = new List<ExpenseTransactionPayment>();
                    }

                    //Re-Map each Entity Object to Business Object
                    foreach (var item in newTransactionList)
                    {
                        var myBusinessObj =
                            ExpenseTransactionMapper.Map
                                <ExpenseTransaction, BusinessObject.ExpenseTransaction>(item);
                        if (myBusinessObj == null)
                        {
                            continue;
                        }

                        if (!paymentList.Any())
                        {
                            var name = Enum.GetName(typeof(ExpensePaymentStatus), ExpensePaymentStatus.Not_Paid);
                            if (name != null)
                                myBusinessObj.PaymentStatus = name.Replace("_", " ");
                        }
                        else
                        {
                            var payment = paymentList.Find(m => m.ExpenseTransactionId == myBusinessObj.ExpenseTransactionId);

                            if (payment == null || payment.ExpenseTransactionPaymentId < 1)
                            {
                                var name = Enum.GetName(typeof(ExpensePaymentStatus), ExpensePaymentStatus.Not_Paid);
                                if (name != null)
                                    myBusinessObj.PaymentStatus = name.Replace("_", " ");
                            }
                            else
                            {
                                if (payment.AmountPaid.Equals(0))
                                {
                                    var name = Enum.GetName(typeof(ExpensePaymentStatus), ExpensePaymentStatus.Not_Paid);
                                    if (name !=
                                        null)
                                        myBusinessObj.PaymentStatus = name.Replace("_", " ");
                                }

                                if (!payment.AmountPaid.Equals(0) && payment.AmountPaid < payment.TotalAmountPayable)
                                {
                                    var name = Enum.GetName(typeof(ExpensePaymentStatus), ExpensePaymentStatus.Partly_Paid);
                                    if (name !=
                                        null)
                                        myBusinessObj.PaymentStatus = name.Replace("_", " ");
                                }

                                if (payment.AmountPaid.Equals(payment.TotalAmountPayable))
                                {
                                    var name = Enum.GetName(typeof(ExpensePaymentStatus), ExpensePaymentStatus.Fully_Paid);
                                    if (name !=
                                        null)
                                        myBusinessObj.PaymentStatus = name.Replace("_", " ");
                                }
                            }

                        }
                        myBusinessObj.ApprovalStatus = Enum.GetName(typeof(ExpenseApprovalStatus), myBusinessObj.Status);
                        
                        myBusinessObjList.Add(myBusinessObj);
                    }
                    if (!myBusinessObjList.Any())
                    {
                        return new List<BusinessObject.ExpenseTransaction>();
                    }

                    return myBusinessObjList.OrderBy(m => m.ExpenseTitle).ToList();
                }
            }
            catch (Exception ex)
            {
                ErrorManager.LogApplicationError(ex.StackTrace, ex.Source, ex.Message);
                return new List<BusinessObject.ExpenseTransaction>();
            }
        }
        public List<BusinessObject.ExpenseTransaction> GetBeneficiaryransactionsByDate(int beneficiaryId, string transactionDate)
        {
            try
            {
                using (var db = new ExpenseManagerDBEntities())
                {
                    var objList = db.ExpenseTransactions.Where(m => m.BeneficiaryId == beneficiaryId && m.TransactionDate == transactionDate).ToList();
                    var myBusinessObjList = new List<BusinessObject.ExpenseTransaction>();
                    if (!objList.Any())
                    {
                        return myBusinessObjList;
                    }

                    var paymentList = db.ExpenseTransactionPayments.ToList();

                    if (!paymentList.Any())
                    {
                        paymentList = new List<ExpenseTransactionPayment>();
                    }

                    //Re-Map each Entity Object to Business Object
                    foreach (var item in objList)
                    {
                        var myBusinessObj = ExpenseTransactionMapper.Map
                                <ExpenseTransaction, BusinessObject.ExpenseTransaction>(item);
                        if (myBusinessObj == null)
                        {
                            continue;
                        }

                        if (!paymentList.Any())
                        {
                            myBusinessObj.PaymentStatus = Enum.GetName(typeof(ExpensePaymentStatus), ExpensePaymentStatus.Not_Paid).Replace("_", " ");
                        }
                        else
                        {
                            var payment = paymentList.Find(m => m.ExpenseTransactionId == myBusinessObj.ExpenseTransactionId);

                            if (payment == null || payment.ExpenseTransactionPaymentId < 1)
                            {
                                myBusinessObj.PaymentStatus = Enum.GetName(typeof(ExpensePaymentStatus), ExpensePaymentStatus.Not_Paid).Replace("_", " ");
                            }
                            else
                            {
                                if (payment.AmountPaid.Equals(0))
                                {
                                    myBusinessObj.PaymentStatus = Enum.GetName(typeof(ExpensePaymentStatus), ExpensePaymentStatus.Not_Paid).Replace("_", " ");
                                }

                                if (!payment.AmountPaid.Equals(0) && payment.AmountPaid < payment.TotalAmountPayable)
                                {
                                    myBusinessObj.PaymentStatus = Enum.GetName(typeof(ExpensePaymentStatus), ExpensePaymentStatus.Partly_Paid).Replace("_", " ");
                                }

                                if (payment.AmountPaid.Equals(payment.TotalAmountPayable))
                                {
                                    myBusinessObj.PaymentStatus = Enum.GetName(typeof(ExpensePaymentStatus), ExpensePaymentStatus.Fully_Paid).Replace("_", " ");
                                }
                            }

                        }
                        myBusinessObj.ApprovalStatus = Enum.GetName(typeof(ExpenseApprovalStatus), myBusinessObj.Status);
                        
                        myBusinessObjList.Add(myBusinessObj);
                    }
                    if (!myBusinessObjList.Any())
                    {
                        return new List<BusinessObject.ExpenseTransaction>();
                    }

                    return myBusinessObjList.OrderBy(m => m.ExpenseTitle).ToList();
                }
            }
            catch (Exception ex)
            {
                ErrorManager.LogApplicationError(ex.StackTrace, ex.Source, ex.Message);
                return new List<BusinessObject.ExpenseTransaction>();
            }
        }
        public List<BusinessObject.ExpenseTransaction> GetExpenseTransactionsByDate(string date)
        {
            try
            {
                using (var db = new ExpenseManagerDBEntities())
                {
                    var objList = db.ExpenseTransactions.Where(m => m.TransactionDate == date).ToList();

                    var expenseTransactionList = new List<BusinessObject.ExpenseTransaction>();

                    if (!objList.Any())
                    {
                        return expenseTransactionList;
                    }
                    var paymentList = db.ExpenseTransactionPayments.ToList();

                    if (!paymentList.Any())
                    {
                        paymentList = new List<ExpenseTransactionPayment>();
                    }
                    //Re-Map each Entity Object to Business Object
                    foreach (var item in objList)
                    {
                        var myBusinessObj =
                            ExpenseTransactionMapper.Map<ExpenseTransaction, BusinessObject.ExpenseTransaction>(item);

                        if (myBusinessObj == null)
                        {
                            continue;
                        }


                        if (!paymentList.Any())
                        {
                            myBusinessObj.PaymentStatus = Enum.GetName(typeof(ExpensePaymentStatus), ExpensePaymentStatus.Not_Paid).Replace("_", " ");
                        }
                        else
                        {
                            var payment = paymentList.Find(m => m.ExpenseTransactionId == myBusinessObj.ExpenseTransactionId);

                            if (payment == null || payment.ExpenseTransactionPaymentId < 1)
                            {
                                myBusinessObj.PaymentStatus = Enum.GetName(typeof(ExpensePaymentStatus), ExpensePaymentStatus.Not_Paid).Replace("_", " ");
                            }
                            else
                            {
                                if (payment.AmountPaid.Equals(0))
                                {
                                    myBusinessObj.PaymentStatus = Enum.GetName(typeof(ExpensePaymentStatus), ExpensePaymentStatus.Not_Paid).Replace("_", " ");
                                    
                                }

                                if (!payment.AmountPaid.Equals(0) && payment.AmountPaid < payment.TotalAmountPayable)
                                {
                                    myBusinessObj.PaymentStatus = Enum.GetName(typeof(ExpensePaymentStatus), ExpensePaymentStatus.Partly_Paid).Replace("_", " ");
                                }

                                if (payment.AmountPaid.Equals(payment.TotalAmountPayable))
                                {
                                    myBusinessObj.PaymentStatus = Enum.GetName(typeof(ExpensePaymentStatus), ExpensePaymentStatus.Fully_Paid).Replace("_", " ");
                                }
                            }

                        }

                        if(myBusinessObj.Status == 0)
                        {
                            var nAStatus = Enum.GetName(typeof(PendingStatus), PendingStatus.N_A).Replace("_", "/");
                            myBusinessObj.DateApproved = nAStatus;
                            myBusinessObj.TimeApproved = nAStatus;
                        }
                        myBusinessObj.ApprovalStatus = Enum.GetName(typeof(ExpenseApprovalStatus), myBusinessObj.Status);
                        
                        expenseTransactionList.Add(myBusinessObj);
                    }

                    if (!expenseTransactionList.Any())
                    {
                        return new List<BusinessObject.ExpenseTransaction>();
                    }
                    return expenseTransactionList.OrderBy(m => m.ExpenseTitle).ToList();
                }
            }
            catch (Exception ex)
            {
                ErrorManager.LogApplicationError(ex.StackTrace, ex.Source, ex.Message);
                return new List<BusinessObject.ExpenseTransaction>();
            }
        }
        public List<BusinessObject.ExpenseTransaction> GetExpenseTransactionsByDate(string date, int flag)
        {
            try
            {
                using (var db = new ExpenseManagerDBEntities())
                {
                    var objList = new List<ExpenseTransaction>();

                    var expenseTransactionList = new List<BusinessObject.ExpenseTransaction>();

                    if (flag == 1)
                    {
                        objList = db.ExpenseTransactions.Where(m => m.TransactionDate == date && (m.Status == 1 || m.Status == 3)).ToList();
                    }

                    if (flag == 2)
                    {
                        objList = db.ExpenseTransactions.Where(m => m.TransactionDate == date && (m.Status == 0)).ToList();
                    }

                    if (!objList.Any())
                    {
                        return new List<BusinessObject.ExpenseTransaction>();
                    }
                    var paymentList = db.ExpenseTransactionPayments.ToList();

                    if (!paymentList.Any())
                    {
                        paymentList = new List<ExpenseTransactionPayment>();
                    }
                    //Re-Map each Entity Object to Business Object
                    foreach (var item in objList)
                    {
                        var myBusinessObj =
                            ExpenseTransactionMapper.Map<ExpenseTransaction, BusinessObject.ExpenseTransaction>(item);

                        if (myBusinessObj == null)
                        {
                            continue;
                        }

                        if (!paymentList.Any())
                        {
                            myBusinessObj.PaymentStatus = Enum.GetName(typeof(ExpensePaymentStatus), ExpensePaymentStatus.Not_Paid).Replace("_", " ");
                        }
                        else
                        {
                            var payment = paymentList.Find(m => m.ExpenseTransactionId == myBusinessObj.ExpenseTransactionId);

                            if (payment == null || payment.ExpenseTransactionPaymentId < 1)
                            {
                                myBusinessObj.PaymentStatus = Enum.GetName(typeof(ExpensePaymentStatus), ExpensePaymentStatus.Not_Paid).Replace("_", " ");
                            }
                            else
                            {
                                if (payment.AmountPaid.Equals(0))
                                {
                                    myBusinessObj.PaymentStatus = Enum.GetName(typeof(ExpensePaymentStatus), ExpensePaymentStatus.Not_Paid).Replace("_", " ");
                                }

                                if (!payment.AmountPaid.Equals(0) && payment.AmountPaid < payment.TotalAmountPayable)
                                {
                                    myBusinessObj.PaymentStatus = Enum.GetName(typeof(ExpensePaymentStatus), ExpensePaymentStatus.Partly_Paid).Replace("_", " ");
                                }

                                if (payment.AmountPaid.Equals(payment.TotalAmountPayable))
                                {
                                    myBusinessObj.PaymentStatus = Enum.GetName(typeof(ExpensePaymentStatus), ExpensePaymentStatus.Fully_Paid).Replace("_", " ");
                                }
                            }

                        }
                        myBusinessObj.ApprovalStatus = Enum.GetName(typeof(ExpenseApprovalStatus), myBusinessObj.Status);
                        
                        expenseTransactionList.Add(myBusinessObj);
                    }

                    if (!expenseTransactionList.Any())
                    {
                        return new List<BusinessObject.ExpenseTransaction>();
                    }

                    return
                        expenseTransactionList.OrderBy(m => m.ExpenseTitle).ToList();
                }
            }
            catch (Exception ex)
            {
                ErrorManager.LogApplicationError(ex.StackTrace, ex.Source, ex.Message);
                return new List<BusinessObject.ExpenseTransaction>();
            }
        }
        public List<BusinessObject.ExpenseTransaction> GetAllExpenseTransactions()
        {
            try
            {
               using (var db = new ExpenseManagerDBEntities())
                {
                    var objList = db.ExpenseTransactions.ToList();

                    var expenseTransactionList = new List<BusinessObject.ExpenseTransaction>();

                    if (!objList.Any())
                    {
                        return expenseTransactionList;
                    }
                    var paymentList = db.ExpenseTransactionPayments.ToList();

                    if (!paymentList.Any())
                    {
                        paymentList = new List<ExpenseTransactionPayment>();
                    }
                    //Re-Map each Entity Object to Business Object
                    foreach (var item in objList)
                    {
                        var myBusinessObj =
                            ExpenseTransactionMapper.Map<ExpenseTransaction, BusinessObject.ExpenseTransaction>(item);

                        if (myBusinessObj == null)
                        {
                            continue;
                        }

                        if (!paymentList.Any())
                        {
                            myBusinessObj.PaymentStatus = Enum.GetName(typeof(ExpensePaymentStatus), ExpensePaymentStatus.Not_Paid).Replace("_", " ");
                        }
                        else
                        {
                            var payment = paymentList.Find(m => m.ExpenseTransactionId == myBusinessObj.ExpenseTransactionId);

                            if (payment == null || payment.ExpenseTransactionPaymentId < 1)
                            {
                                myBusinessObj.PaymentStatus = Enum.GetName(typeof(ExpensePaymentStatus), ExpensePaymentStatus.Not_Paid).Replace("_", " ");
                            }
                            else
                            {
                                if (payment.AmountPaid.Equals(0))
                                {
                                    myBusinessObj.PaymentStatus = Enum.GetName(typeof(ExpensePaymentStatus), ExpensePaymentStatus.Not_Paid).Replace("_", " ");
                                }

                                if (!payment.AmountPaid.Equals(0) && payment.AmountPaid < payment.TotalAmountPayable)
                                {
                                    myBusinessObj.PaymentStatus = Enum.GetName(typeof(ExpensePaymentStatus), ExpensePaymentStatus.Partly_Paid).Replace("_", " ");
                                }

                                if (payment.AmountPaid.Equals(payment.TotalAmountPayable))
                                {
                                    myBusinessObj.PaymentStatus = Enum.GetName(typeof(ExpensePaymentStatus), ExpensePaymentStatus.Fully_Paid).Replace("_", " ");
                                }
                            }

                        }
                        myBusinessObj.ApprovalStatus = Enum.GetName(typeof(ExpenseApprovalStatus), myBusinessObj.Status);
                        expenseTransactionList.Add(myBusinessObj);
                    }

                    if (!expenseTransactionList.Any())
                    {
                        return new List<BusinessObject.ExpenseTransaction>();
                    }
                    return expenseTransactionList.OrderBy(m => m.ExpenseTitle).ToList();
                }
            }
            catch (Exception ex)
            {
                ErrorManager.LogApplicationError(ex.StackTrace, ex.Source, ex.Message);
                return new List<BusinessObject.ExpenseTransaction>();
            }
        }
        public List<BusinessObject.ExpenseTransaction> GetTransactionsByBeneficiary(int beneficiaryId)
        {
            try
            {
                using (var db = new ExpenseManagerDBEntities())
                {
                    var objList = db.ExpenseTransactions.Where(m => m.BeneficiaryId == beneficiaryId).ToList();

                    var expenseTransactionList = new List<BusinessObject.ExpenseTransaction>();

                    if (!objList.Any())
                    {
                        return expenseTransactionList;
                    }
                    var paymentList = db.ExpenseTransactionPayments.ToList();

                    if (!paymentList.Any())
                    {
                        paymentList = new List<ExpenseTransactionPayment>();
                    }
                    //Re-Map each Entity Object to Business Object
                    foreach (var item in objList)
                    {
                        var myBusinessObj =
                            ExpenseTransactionMapper.Map<ExpenseTransaction, BusinessObject.ExpenseTransaction>(item);

                        if (myBusinessObj == null)
                        {
                            continue;
                        }

                        if (!paymentList.Any())
                        {
                            var name = Enum.GetName(typeof(ExpensePaymentStatus), ExpensePaymentStatus.Not_Paid);
                            if (name != null)
                                myBusinessObj.PaymentStatus = name.Replace("_", " ");
                        }
                        else
                        {
                            var payment = paymentList.Find(m => m.ExpenseTransactionId == myBusinessObj.ExpenseTransactionId);

                            if (payment == null || payment.ExpenseTransactionPaymentId < 1)
                            {
                                var name = Enum.GetName(typeof(ExpensePaymentStatus), ExpensePaymentStatus.Not_Paid);
                                if (name != null)
                                    myBusinessObj.PaymentStatus = name.Replace("_", " ");
                            }
                            else
                            {
                                if (payment.AmountPaid.Equals(0))
                                {
                                    var name = Enum.GetName(typeof(ExpensePaymentStatus), ExpensePaymentStatus.Not_Paid);
                                    if (name !=
                                        null)
                                        myBusinessObj.PaymentStatus = name.Replace("_", " ");
                                }

                                if (!payment.AmountPaid.Equals(0) && payment.AmountPaid < payment.TotalAmountPayable)
                                {
                                    var name = Enum.GetName(typeof(ExpensePaymentStatus), ExpensePaymentStatus.Partly_Paid);
                                    if (name !=
                                        null)
                                        myBusinessObj.PaymentStatus = name.Replace("_", " ");
                                }

                                if (payment.AmountPaid.Equals(payment.TotalAmountPayable))
                                {
                                    var name = Enum.GetName(typeof(ExpensePaymentStatus), ExpensePaymentStatus.Fully_Paid);
                                    if (name !=
                                        null)
                                        myBusinessObj.PaymentStatus = name.Replace("_", " ");
                                }
                            }

                        }
                        myBusinessObj.ApprovalStatus = Enum.GetName(typeof(ExpenseApprovalStatus), myBusinessObj.Status);
                        
                        expenseTransactionList.Add(myBusinessObj);

                    }

                    if (!expenseTransactionList.Any())
                    {
                        return new List<BusinessObject.ExpenseTransaction>();
                    }
                    return expenseTransactionList.OrderBy(m => m.ExpenseTitle).ToList();
                }
            }
            catch (Exception ex)
            {
                ErrorManager.LogApplicationError(ex.StackTrace, ex.Source, ex.Message);
                return new List<BusinessObject.ExpenseTransaction>();
            }
        }
        public List<BusinessObject.ExpenseTransaction> GetApprovedTransactionsByDateRange(DateTime start, DateTime stop)
        {
            try
            {
                using (var db = new ExpenseManagerDBEntities())
                {
                    if (stop < start || start > stop)
                    {
                        return null;
                    }

                    var objList = db.ExpenseTransactions.Where(m => m.Status == 1).ToList();

                    var myBusinessObjList = new List<BusinessObject.ExpenseTransaction>();

                    if (!objList.Any())
                    {
                        return myBusinessObjList;
                    }

                    var newTransactionList = new List<ExpenseTransaction>();
                    newTransactionList.AddRange(from transaction in objList
                                                let transactionDate = DateTime.Parse(transaction.DateApproved)
                                                where
                                                    transactionDate == start ||
                                                    (transactionDate > start && transactionDate < stop) ||
                                                    transactionDate == stop
                                                select transaction);
                    if (!newTransactionList.Any())
                    {
                        return myBusinessObjList;
                    }

                    var paymentList = db.ExpenseTransactionPayments.ToList();

                    if (!paymentList.Any())
                    {
                        paymentList = new List<ExpenseTransactionPayment>();
                    }

                    //Re-Map each Entity Object to Business Object
                    foreach (var item in newTransactionList)
                    {
                        var myBusinessObj =
                            ExpenseTransactionMapper.Map<ExpenseTransaction, BusinessObject.ExpenseTransaction>(item);
                        if (myBusinessObj == null)
                        {
                            continue;
                        }

                        if (!paymentList.Any())
                        {
                            var name = Enum.GetName(typeof(ExpensePaymentStatus), ExpensePaymentStatus.Not_Paid);
                            if (name != null)
                                myBusinessObj.PaymentStatus =
                                    name.Replace("_", " ");
                        }
                        else
                        {
                            var payment =
                                paymentList.Find(m => m.ExpenseTransactionId == myBusinessObj.ExpenseTransactionId);

                            if (payment == null || payment.ExpenseTransactionPaymentId < 1)
                            {
                                myBusinessObj.PaymentStatus =
                                    Enum.GetName(typeof(ExpensePaymentStatus), ExpensePaymentStatus.Not_Paid).Replace("_", " ");
                            }
                            else
                            {
                                if (payment.AmountPaid.Equals(0))
                                {
                                    myBusinessObj.PaymentStatus =
                                        Enum.GetName(typeof(ExpensePaymentStatus), ExpensePaymentStatus.Not_Paid).Replace("_", " ");
                                }

                                if (!payment.AmountPaid.Equals(0) && payment.AmountPaid < payment.TotalAmountPayable)
                                {
                                    myBusinessObj.PaymentStatus = Enum.GetName(typeof(ExpensePaymentStatus), ExpensePaymentStatus.Partly_Paid).Replace("_", " ");
                                }

                                if (payment.AmountPaid.Equals(payment.TotalAmountPayable))
                                {
                                    myBusinessObj.PaymentStatus = Enum.GetName(typeof(ExpensePaymentStatus), ExpensePaymentStatus.Fully_Paid).Replace("_", " ");
                                }
                            }

                        }
                        myBusinessObj.ApprovalStatus = Enum.GetName(typeof(ExpenseApprovalStatus), myBusinessObj.Status);
                        myBusinessObjList.Add(myBusinessObj);
                    }

                    if (!myBusinessObjList.Any())
                    {
                        return new List<BusinessObject.ExpenseTransaction>();
                    }

                    foreach (var expenseTransaction in myBusinessObjList)
                    {
                        expenseTransaction.TransactionDate = DateMap.ReverseToGeneralDate(expenseTransaction.TransactionDate);
                    }

                    return myBusinessObjList.OrderByDescending(m=>m.TransactionDate).ToList();
                }
            }
            catch (Exception ex)
            {
                ErrorManager.LogApplicationError(ex.StackTrace, ex.Source, ex.Message);
                return new List<BusinessObject.ExpenseTransaction>();
            }
        }
        public List<BusinessObject.ExpenseTransaction> GetRejectedTransactionsByDateRange(DateTime start, DateTime stop)
        {
            try
            {
                using (var db = new ExpenseManagerDBEntities())
                {
                    if (stop < start || start > stop)
                    {
                        return null;
                    }

                    var objList = db.ExpenseTransactions.Where(m => m.Status == 2).ToList();

                    var myBusinessObjList = new List<BusinessObject.ExpenseTransaction>();

                    if (!objList.Any())
                    {
                        return myBusinessObjList;
                    }

                    var newTransactionList = new List<ExpenseTransaction>();
                    newTransactionList.AddRange(from transaction in objList
                                                let transactionDate = DateTime.Parse(transaction.DateApproved)
                                                where
                                                    transactionDate == start ||
                                                    (transactionDate > start && transactionDate < stop) ||
                                                    transactionDate == stop
                                                select transaction);
                    if (!newTransactionList.Any())
                    {
                        return myBusinessObjList;
                    }

                    var paymentList = db.ExpenseTransactionPayments.ToList();

                    if (!paymentList.Any())
                    {
                        paymentList = new List<ExpenseTransactionPayment>();
                    }

                    //Re-Map each Entity Object to Business Object
                    foreach (var item in newTransactionList)
                    {
                        var myBusinessObj =
                            ExpenseTransactionMapper.Map<ExpenseTransaction, BusinessObject.ExpenseTransaction>(item);
                        if (myBusinessObj == null)
                        {
                            continue;
                        }

                        if (!paymentList.Any())
                        {
                            myBusinessObj.PaymentStatus =
                                Enum.GetName(typeof(ExpensePaymentStatus), ExpensePaymentStatus.Not_Paid).Replace("_"," ");
                        }
                        else
                        {
                            var payment =
                                paymentList.Find(m => m.ExpenseTransactionId == myBusinessObj.ExpenseTransactionId);

                            if (payment == null || payment.ExpenseTransactionPaymentId < 1)
                            {
                                myBusinessObj.PaymentStatus =
                                    Enum.GetName(typeof(ExpensePaymentStatus), ExpensePaymentStatus.Not_Paid).Replace("_", " ");
                            }
                            else
                            {
                                if (payment.AmountPaid.Equals(0))
                                {
                                    myBusinessObj.PaymentStatus =
                                        Enum.GetName(typeof(ExpensePaymentStatus), ExpensePaymentStatus.Not_Paid).Replace("_", " ");
                                }

                                if (!payment.AmountPaid.Equals(0) && payment.AmountPaid < payment.TotalAmountPayable)
                                {
                                    myBusinessObj.PaymentStatus = Enum.GetName(typeof(ExpensePaymentStatus), ExpensePaymentStatus.Partly_Paid).Replace("_", " ");
                                }

                                if (payment.AmountPaid.Equals(payment.TotalAmountPayable))
                                {
                                    myBusinessObj.PaymentStatus = Enum.GetName(typeof(ExpensePaymentStatus), ExpensePaymentStatus.Fully_Paid).Replace("_", " ");
                                }
                            }

                        }
                        myBusinessObj.ApprovalStatus = Enum.GetName(typeof(ExpenseApprovalStatus), myBusinessObj.Status);
                        myBusinessObjList.Add(myBusinessObj);
                    }

                    if (!myBusinessObjList.Any())
                    {
                        return new List<BusinessObject.ExpenseTransaction>();
                    }

                    foreach (var expenseTransaction in myBusinessObjList)
                    {
                        expenseTransaction.TransactionDate = DateMap.ReverseToGeneralDate(expenseTransaction.TransactionDate);
                    }

                    return myBusinessObjList.OrderByDescending(m=>m.TransactionDate).ToList();
                }
            }
            catch (Exception ex)
            {
                ErrorManager.LogApplicationError(ex.StackTrace, ex.Source, ex.Message);
                return new List<BusinessObject.ExpenseTransaction>();
            }
        }
        public List<BusinessObject.ExpenseTransaction> GetPendingTransactionsByDateRange(DateTime startDate, DateTime endDate)
        {
            try
            {
                using (var db = new ExpenseManagerDBEntities())
                {
                    var objList = db.ExpenseTransactions.Where(m => m.Status == 0).ToList();
                    var newTransactionList = new List<ExpenseTransaction>();
                    newTransactionList.AddRange(from transaction in objList
                                                let transactionDate = DateTime.Parse(transaction.TransactionDate)
                                                where
                                                    transactionDate == startDate ||
                                                    (transactionDate > startDate && transactionDate < endDate) ||
                                                    transactionDate == endDate
                                                select transaction);

                    var expenseTransactionList = new List<BusinessObject.ExpenseTransaction>();

                    if (!newTransactionList.Any())
                    {
                        return expenseTransactionList;
                    }
                    var paymentList = db.ExpenseTransactionPayments.ToList();

                    if (!paymentList.Any())
                    {
                        paymentList = new List<ExpenseTransactionPayment>();
                    }
                    //Re-Map each Entity Object to Business Object
                    foreach (var item in newTransactionList)
                    {
                        var myBusinessObj =
                            ExpenseTransactionMapper.Map<ExpenseTransaction, BusinessObject.ExpenseTransaction>(item);

                        if (myBusinessObj == null)
                        {
                            continue;
                        }


                        if (!paymentList.Any())
                        {
                            myBusinessObj.PaymentStatus = Enum.GetName(typeof(ExpensePaymentStatus), ExpensePaymentStatus.Not_Paid).Replace("_", " ");
                        }
                        else
                        {
                            var payment = paymentList.Find(m => m.ExpenseTransactionId == myBusinessObj.ExpenseTransactionId);

                            if (payment == null || payment.ExpenseTransactionPaymentId < 1)
                            {
                                myBusinessObj.PaymentStatus = Enum.GetName(typeof(ExpensePaymentStatus), ExpensePaymentStatus.Not_Paid).Replace("_", " ");
                            }
                            else
                            {
                                if (payment.AmountPaid.Equals(0))
                                {
                                    myBusinessObj.PaymentStatus = Enum.GetName(typeof(ExpensePaymentStatus), ExpensePaymentStatus.Not_Paid).Replace("_", " ");
                                }

                                if (!payment.AmountPaid.Equals(0) && payment.AmountPaid < payment.TotalAmountPayable)
                                {
                                    myBusinessObj.PaymentStatus = Enum.GetName(typeof(ExpensePaymentStatus), ExpensePaymentStatus.Partly_Paid).Replace("_", " ");
                                }

                                if (payment.AmountPaid.Equals(payment.TotalAmountPayable))
                                {
                                    myBusinessObj.PaymentStatus = Enum.GetName(typeof(ExpensePaymentStatus), ExpensePaymentStatus.Fully_Paid).Replace("_", " ");
                                }
                            }

                        }
                       
                        if (myBusinessObj.Status == 0)
                        {
                            var nAStatus = Enum.GetName(typeof(PendingStatus), PendingStatus.N_A).Replace("_", "/");
                            myBusinessObj.PaymentStatus = nAStatus;
                            myBusinessObj.ApproverComment = nAStatus;
                            myBusinessObj.DateApproved = nAStatus;
                            myBusinessObj.TimeApproved = nAStatus;
                        }
                        
                        myBusinessObj.ApprovalStatus = Enum.GetName(typeof(ExpenseApprovalStatus), myBusinessObj.Status);
                        expenseTransactionList.Add(myBusinessObj);
                    }

                    if (!expenseTransactionList.Any())
                    {
                        return new List<BusinessObject.ExpenseTransaction>();
                    }
                    return expenseTransactionList.OrderByDescending(m => m.TransactionDate).ToList();
                }
            }
            catch (Exception ex)
            {
                ErrorManager.LogApplicationError(ex.StackTrace, ex.Source, ex.Message);
                return new List<BusinessObject.ExpenseTransaction>();
            }
        }
        public List<BusinessObject.ExpenseTransaction> GetVoidedTransactionsByDateRange(DateTime startDate, DateTime endDate)
        {
            try
            {
                using (var db = new ExpenseManagerDBEntities())
                {
                    var objList = db.ExpenseTransactions.ToList().FindAll(m => (String.CompareOrdinal(m.TransactionDate, startDate.ToString("yyyy/MM/dd")) >= 0) &&
                         (String.CompareOrdinal(m.TransactionDate, endDate.ToString("yyyy/MM/dd")) <= 0) && m.Status == 3).ToList();

                    var expenseTransactionList = new List<BusinessObject.ExpenseTransaction>();

                    if (!objList.Any())
                    {
                        return expenseTransactionList;
                    }

                    //Re-Map each Entity Object to Business Object
                    Parallel.ForEach(objList, item =>
                                            {
                                                var myBusinessObj = ExpenseTransactionMapper.Map<ExpenseTransaction, BusinessObject.ExpenseTransaction>(item);

                                                if (myBusinessObj.Status == 0)
                                                {

                                                    var name = Enum.GetName(typeof(ExpenseApprovalDateTime), ExpenseApprovalDateTime.N_A);
                                                    if (name != null)
                                                    {
                                                        myBusinessObj.DateApproved = name.Replace("_", "/");
                                                        myBusinessObj.TimeApproved = name.Replace("_", "/");
                                                    }
                                                }

                                                myBusinessObj.ApprovalStatus = Enum.GetName(typeof(ExpenseApprovalStatus), myBusinessObj.Status);
                                        
                                                var paymentStatus =
                                                    db.ExpenseTransactionPayments.Where(
                                                        m => m.ExpenseTransactionId == myBusinessObj.ExpenseTransactionId).ToList();
                                               
                                                if (!paymentStatus.Any())
                                                {
                                                    var name = Enum.GetName(typeof(ExpensePaymentStatus), ExpensePaymentStatus.Not_Paid);
                                                    if (name != null)
                                                        myBusinessObj.PaymentStatus = name.Replace("_", "/");
                                                }

                                                if ( paymentStatus.Any() && paymentStatus.ElementAt(0).Status == 0)
                                                {
                                                    var name = Enum.GetName(typeof(ExpensePaymentStatus), ExpensePaymentStatus.Partly_Paid);
                                                    if (
                                                        name != null)
                                                        myBusinessObj.PaymentStatus = name.Replace("_", "/");
                                                }

                                                if (paymentStatus.Any() && paymentStatus.ElementAt(0).Status == 1)
                                                {
                                                    var name = Enum.GetName(typeof(ExpensePaymentStatus), ExpensePaymentStatus.Fully_Paid);
                                                    if (
                                                        name != null)
                                                        myBusinessObj.PaymentStatus = name.Replace("_", "/");
                                                }
                                                expenseTransactionList.Add(myBusinessObj);

                                            });
                      
                    if (!expenseTransactionList.Any())
                    {
                        return new List<BusinessObject.ExpenseTransaction>();
                    }
                    return expenseTransactionList.OrderByDescending(m => m.TransactionDate).ToList();
                }
            }
            catch (Exception ex)
            {
                ErrorManager.LogApplicationError(ex.StackTrace, ex.Source, ex.Message);
                return new List<BusinessObject.ExpenseTransaction>();
            }
        }
        public List<BusinessObject.ExpenseTransaction> GetApprovedTransactionsByDate(string date)
        {
            try
            {
                using (var db = new ExpenseManagerDBEntities())
                {
                    var objList = db.ExpenseTransactions.Where(m => m.DateApproved == date && m.Status == 1).ToList();

                    if (!objList.Any())
                    {
                        return new List<BusinessObject.ExpenseTransaction>();
                    }

                    var myBusinessObjList = new List<BusinessObject.ExpenseTransaction>();
                    if (!objList.Any())
                    {
                        return myBusinessObjList;
                    }
                   
                    //Re-Map each Entity Object to Business Object
                    foreach (var item in objList)
                    {
                        var myBusinessObj =
                            ExpenseTransactionMapper.Map
                                <ExpenseTransaction, BusinessObject.ExpenseTransaction>(item);
                        if (myBusinessObj == null)
                        {
                            continue;
                        }
                       myBusinessObj.ApprovalStatus = Enum.GetName(typeof(ExpenseApprovalStatus), myBusinessObj.Status);
                       myBusinessObjList.Add(myBusinessObj);
                    }

                    if (!myBusinessObjList.Any())
                    {
                        return new List<BusinessObject.ExpenseTransaction>();
                    }

                    foreach (var expenseTransaction in myBusinessObjList)
                    {
                        expenseTransaction.TransactionDate = DateMap.ReverseToGeneralDate(expenseTransaction.TransactionDate);
                    }

                    return myBusinessObjList;
                }
            }
            catch (Exception ex)
            {
                ErrorManager.LogApplicationError(ex.StackTrace, ex.Source, ex.Message);
                return new List<BusinessObject.ExpenseTransaction>();
            }
        }
        public List<BusinessObject.ExpenseTransaction> GetPortalUserApprovedTransactionsByDateRange(int portalUserId, DateTime startDate, DateTime endDate)
        {
            try
            {
                using (var db = new ExpenseManagerDBEntities())
                {
                    var objList = db.ExpenseTransactions.Where(m => m.Status == 1 && m.RegisteredById == portalUserId).ToList();
                    var newObjList = new List<ExpenseTransaction>();
                    var myBusinessObjList = new List<BusinessObject.ExpenseTransaction>();
                    if (!objList.Any())
                    {
                        return myBusinessObjList;
                    }

                    var paymentList = db.ExpenseTransactionPayments.ToList();

                    if (!paymentList.Any())
                    {
                        paymentList = new List<ExpenseTransactionPayment>();
                    }

                    newObjList.AddRange(from transaction in objList
                                        let transactionDate = DateTime.Parse(transaction.TransactionDate)
                                        where
                                            transactionDate == startDate ||
                                            (transactionDate > startDate && transactionDate < endDate) ||
                                            transactionDate == endDate
                                        select transaction
                                                           );


                    if (!newObjList.Any())
                    {
                        return myBusinessObjList;
                    }

                    //Re-Map each Entity Object to Business Object
                    foreach (var item in newObjList)
                    {
                        var myBusinessObj =
                            ExpenseTransactionMapper.Map
                                <ExpenseTransaction, BusinessObject.ExpenseTransaction>(item);
                        if (myBusinessObj == null)
                        {
                            continue;
                        }
                        if (!paymentList.Any())
                        {
                            myBusinessObj.PaymentStatus = Enum.GetName(typeof(ExpensePaymentStatus), ExpensePaymentStatus.Not_Paid).Replace("_", " ");
                        }
                        else
                        {
                            var payment = paymentList.Find(m => m.ExpenseTransactionId == myBusinessObj.ExpenseTransactionId);

                            if (payment == null || payment.ExpenseTransactionPaymentId < 1)
                            {
                                myBusinessObj.PaymentStatus = Enum.GetName(typeof(ExpensePaymentStatus), ExpensePaymentStatus.Not_Paid).Replace("_", " ");
                            }
                            else
                            {
                                if (payment.AmountPaid.Equals(0))
                                {
                                    myBusinessObj.PaymentStatus = Enum.GetName(typeof(ExpensePaymentStatus), ExpensePaymentStatus.Not_Paid).Replace("_", " ");
                                }

                                if (!payment.AmountPaid.Equals(0) && payment.AmountPaid < payment.TotalAmountPayable)
                                {
                                    myBusinessObj.PaymentStatus = Enum.GetName(typeof(ExpensePaymentStatus), ExpensePaymentStatus.Partly_Paid).Replace("_", " ");
                                }

                                if (payment.AmountPaid.Equals(payment.TotalAmountPayable))
                                {
                                    myBusinessObj.PaymentStatus = Enum.GetName(typeof(ExpensePaymentStatus), ExpensePaymentStatus.Fully_Paid).Replace("_", " ");
                                }
                            }

                        }
                        if (myBusinessObj.Status == 0)
                        {
                            myBusinessObj.DateApproved = Enum.GetName(typeof(ExpenseApprovalDateTime), myBusinessObj.Status);
                            myBusinessObj.TimeApproved = Enum.GetName(typeof(ExpenseApprovalDateTime), myBusinessObj.Status);
                        }
                        myBusinessObj.ApprovalStatus = Enum.GetName(typeof(ExpenseApprovalStatus), myBusinessObj.Status);
                        myBusinessObjList.Add(myBusinessObj);
                    }
                    if (!myBusinessObjList.Any())
                    {
                        return new List<BusinessObject.ExpenseTransaction>();
                    }

                    return myBusinessObjList.OrderBy(m => m.ExpenseTitle).ToList();
                }
            }
            catch (Exception ex)
            {
                ErrorManager.LogApplicationError(ex.StackTrace, ex.Source, ex.Message);
                return new List<BusinessObject.ExpenseTransaction>();
            }
        }
        public List<BusinessObject.ExpenseTransaction> GetPortalUserVoidedTransactionsByDateRange(int portalUserId, DateTime startDate, DateTime endDate)
        {
            try
            {
                using (var db = new ExpenseManagerDBEntities())
                {
                    var objList = db.ExpenseTransactions.Where(m => m.Status == 3 && m.RegisteredById == portalUserId).ToList();
                    var newObjList = new List<ExpenseManager.EF.ExpenseTransaction>();
                    var myBusinessObjList = new List<BusinessObject.ExpenseTransaction>();
                    if (!objList.Any())
                    {
                        return myBusinessObjList;
                    }
                    
                    newObjList.AddRange(from transaction in objList
                                        let transactionDate = DateTime.Parse(transaction.TransactionDate)
                                        where
                                            transactionDate == startDate ||
                                            (transactionDate > startDate && transactionDate < endDate) ||
                                            transactionDate == endDate
                                        select transaction
                                                           );


                    if (!newObjList.Any())
                    {
                        return myBusinessObjList;
                    }

                    var paymentList = db.ExpenseTransactionPayments.ToList();

                    if (!paymentList.Any())
                    {
                        paymentList = new List<ExpenseTransactionPayment>();
                    }

                    //Re-Map each Entity Object to Business Object
                    foreach (var item in newObjList)
                    {
                        var myBusinessObj =
                            ExpenseTransactionMapper.Map
                                <ExpenseTransaction, BusinessObject.ExpenseTransaction>(item);
                        if (myBusinessObj == null)
                        {
                            continue;
                        }
                        if (!paymentList.Any())
                        {
                            myBusinessObj.PaymentStatus = Enum.GetName(typeof(ExpensePaymentStatus), ExpensePaymentStatus.Not_Paid).Replace("_", " ");
                        }
                        else
                        {
                            var payment = paymentList.Find(m => m.ExpenseTransactionId == myBusinessObj.ExpenseTransactionId);

                            if (payment == null || payment.ExpenseTransactionPaymentId < 1)
                            {
                                myBusinessObj.PaymentStatus = Enum.GetName(typeof(ExpensePaymentStatus), ExpensePaymentStatus.Not_Paid).Replace("_", " ");
                            }
                            else
                            {
                                if (payment.AmountPaid.Equals(0))
                                {
                                    myBusinessObj.PaymentStatus = Enum.GetName(typeof(ExpensePaymentStatus), ExpensePaymentStatus.Not_Paid).Replace("_", " ");
                                }

                                if (!payment.AmountPaid.Equals(0) && payment.AmountPaid < payment.TotalAmountPayable)
                                {
                                    myBusinessObj.PaymentStatus = Enum.GetName(typeof(ExpensePaymentStatus), ExpensePaymentStatus.Partly_Paid).Replace("_", " ");
                                }

                                if (payment.AmountPaid.Equals(payment.TotalAmountPayable))
                                {
                                    myBusinessObj.PaymentStatus = Enum.GetName(typeof(ExpensePaymentStatus), ExpensePaymentStatus.Fully_Paid).Replace("_", " ");
                                }
                            }

                        }
                        if (myBusinessObj.Status == 0)
                        {
                            myBusinessObj.DateApproved = Enum.GetName(typeof(ExpenseApprovalDateTime), myBusinessObj.Status);
                            myBusinessObj.TimeApproved = Enum.GetName(typeof(ExpenseApprovalDateTime), myBusinessObj.Status);
                        }
                        myBusinessObj.ApprovalStatus = Enum.GetName(typeof(ExpenseApprovalStatus), myBusinessObj.Status);
                        myBusinessObjList.Add(myBusinessObj);
                    }
                    if (!myBusinessObjList.Any())
                    {
                        return new List<BusinessObject.ExpenseTransaction>();
                    }

                    return myBusinessObjList.OrderBy(m => m.ExpenseTitle).ToList();
                }
            }
            catch (Exception ex)
            {
                ErrorManager.LogApplicationError(ex.StackTrace, ex.Source, ex.Message);
                return new List<BusinessObject.ExpenseTransaction>();
            }
        }
        public List<BusinessObject.ExpenseTransaction> GetBeneficiaryApprovedTransactionsByDateRange(int beneficiaryId, string startQueryDate, string endQueryDate)
        {
            try
            {
                using (var db = new ExpenseManagerDBEntities())
                {
                    var objList = db.ExpenseTransactions.Where(m => m.Status == 1 && m.BeneficiaryId == beneficiaryId).ToList();
                    var newObjList = new List<ExpenseTransaction>();
                    var myBusinessObjList = new List<BusinessObject.ExpenseTransaction>();
                    if (!objList.Any())
                    {
                        return myBusinessObjList;
                    }

                    var startDate = DateTime.Parse(startQueryDate.Trim());
                    var endDate = DateTime.Parse(endQueryDate.Trim());
                    if (endDate < startDate || startDate > endDate)
                    {
                        return null;
                    }

                    newObjList.AddRange(from transaction in objList
                                        let transactionDate = DateTime.Parse(transaction.TransactionDate)
                                        where
                                            transactionDate == startDate ||
                                            (transactionDate > startDate && transactionDate < endDate) ||
                                            transactionDate == endDate
                                        select transaction
                                                           );


                    if (!newObjList.Any())
                    {
                        return myBusinessObjList;
                    }

                    //Re-Map each Entity Object to Business Object
                    foreach (var item in newObjList)
                    {
                        var myBusinessObj =
                            ExpenseTransactionMapper.Map
                                <ExpenseTransaction, BusinessObject.ExpenseTransaction>(item);
                        if (myBusinessObj == null)
                        {
                            continue;
                        }
                        myBusinessObjList.Add(myBusinessObj);
                    }
                    if (!myBusinessObjList.Any())
                    {
                        return new List<BusinessObject.ExpenseTransaction>();
                    }

                    return myBusinessObjList.OrderBy(m => m.ExpenseTitle).ToList();
                }
            }
            catch (Exception ex)
            {
                ErrorManager.LogApplicationError(ex.StackTrace, ex.Source, ex.Message);
                return new List<BusinessObject.ExpenseTransaction>();
            }
        }
        public List<BusinessObject.ExpenseTransaction> GetBeneficiaryApprovedTransactionsByDate(int beneficiaryId, string transactionDate)
        {
            try
            {
                using (var db = new ExpenseManagerDBEntities())
                {
                    var objList = db.ExpenseTransactions.Where(m => m.Status == 1 && m.BeneficiaryId == beneficiaryId && m.TransactionDate == transactionDate).ToList();
                  var myBusinessObjList = new List<BusinessObject.ExpenseTransaction>();
                    if (!objList.Any())
                    {
                        return myBusinessObjList;
                    }
                    //Re-Map each Entity Object to Business Object
                    foreach (var item in objList)
                    {
                        var myBusinessObj =
                            ExpenseTransactionMapper.Map
                                <ExpenseTransaction, BusinessObject.ExpenseTransaction>(item);
                        if (myBusinessObj == null)
                        {
                            continue;
                        }
                        myBusinessObjList.Add(myBusinessObj);
                    }
                    if (!myBusinessObjList.Any())
                    {
                        return new List<BusinessObject.ExpenseTransaction>();
                    }

                    return myBusinessObjList.OrderBy(m => m.ExpenseTitle).ToList();
                }
            }
            catch (Exception ex)
            {
                ErrorManager.LogApplicationError(ex.StackTrace, ex.Source, ex.Message);
                return new List<BusinessObject.ExpenseTransaction>();
            }
        }
        public List<BusinessObject.ExpenseTransaction> GetUserApprovedTransactionsByDate(int userId, string date)
        {
            try
            {

                using (var db = new ExpenseManagerDBEntities())
                {
                    var objList = db.ExpenseTransactions.Where(m => m.RegisteredById == userId && (m.Status == 1) && m.TransactionDate == date).ToList();

                    var expenseTransactionList = new List<BusinessObject.ExpenseTransaction>();

                    if (!objList.Any())
                    {
                        return expenseTransactionList;
                    }
                   
                    //Re-Map each Entity Object to Business Object
                    foreach (var item in objList)
                    {
                        var myBusinessObj =
                            ExpenseTransactionMapper.Map<ExpenseTransaction, BusinessObject.ExpenseTransaction>(item);

                        if (myBusinessObj == null)
                        {
                            continue;
                        }
                        expenseTransactionList.Add(myBusinessObj);
                    }

                    if (!expenseTransactionList.Any())
                    {
                        return new List<BusinessObject.ExpenseTransaction>();
                    }
                    return expenseTransactionList.OrderBy(m => m.ExpenseTitle).ToList();
                }
            }
            catch (Exception ex)
            {
                ErrorManager.LogApplicationError(ex.StackTrace, ex.Source, ex.Message);
                return new List<BusinessObject.ExpenseTransaction>();
            }
        }
        public double GetTransactionTotalAmount(long expenseTransactionId)
        {
            try
            {
                using (var db = new ExpenseManagerDBEntities())
                {
                    var entityList = db.TransactionItems.Where(m => m.ExpenseTransactionId == expenseTransactionId).ToList();
                    if (!entityList.Any())
                      {
                        return 0;
                      }

                    foreach (var transactionItem in entityList)
                    {
                        if(transactionItem.ApprovedQuantity < 1)
                        {
                            transactionItem.ApprovedQuantity = transactionItem.RequestedQuantity;
                        }

                        if(transactionItem.ApprovedUnitPrice < 1)
                        {
                            transactionItem.ApprovedUnitPrice = transactionItem.RequestedUnitPrice;
                        }

                        if(transactionItem.ApprovedTotalPrice < 1)
                        {
                            transactionItem.ApprovedTotalPrice = transactionItem.ApprovedQuantity * transactionItem.ApprovedUnitPrice;
                        }

                        db.ObjectStateManager.ChangeObjectState(transactionItem, EntityState.Modified);
                        db.SaveChanges();
                    }
                    var totalApprovedPrice = entityList.Sum(m => m.ApprovedTotalPrice);
                    if (totalApprovedPrice < 1)
                    {
                        return 0;
                    }
                    return totalApprovedPrice;
                }
                
            }
            catch (Exception ex)
            {
                ErrorManager.LogApplicationError(ex.StackTrace, ex.Source, ex.Message);
                return 0;
            }
        }
        public List<TransactionObject> GetTransObject(int expenseItemId)
        {
            try
            {
                using (var db = new ExpenseManagerDBEntities())
                {
                    var transObjList = new List<TransactionObject>();
                    var newEntityList = new List<TransactionItem>();
                    var entityList = db.TransactionItems.Where(m => m.ExpenseItem.ExpenseItemId == expenseItemId && m.ExpenseTransaction.Status == 1).ToList();
                    
                    if (!entityList.Any())
                      {
                          return transObjList;
                      }

                    newEntityList.AddRange(entityList.Count > 5 ? entityList.OrderByDescending(m => Convert.ToDateTime(m.ExpenseTransaction.DateApproved)).Take(5) : entityList);

                    transObjList.AddRange(newEntityList.Select(transactionItem => new TransactionObject
                                                                                   {
                                                                                       Quantity = transactionItem.ApprovedQuantity, 
                                                                                       UnitPrice = transactionItem.ApprovedUnitPrice, 
                                                                                       TransactionApprovalDate = transactionItem.ExpenseTransaction.DateApproved, 
                                                                                       TransactionTitle = transactionItem.ExpenseTransaction.ExpenseTitle, 
                                                                                       TransactionItem = transactionItem.ExpenseItem.Title
                                                                                   }));

                    return !transObjList.Any() ? new List<TransactionObject>() : transObjList.OrderByDescending(m => Convert.ToDateTime(m.TransactionApprovalDate)).ToList();
                }
                
            }
            catch (Exception ex)
            {
                ErrorManager.LogApplicationError(ex.StackTrace, ex.Source, ex.Message);
                return new List<TransactionObject>();
            }
        }

        public List<BusinessObject.ExpenseTransaction> GetWeeklyTransactions(int status, int dept, string yrVal, string monthVal, int weeklyVal)
	    {
            try
            {
                using (var db = new ExpenseManagerDBEntities())
                {
                    var weeklyTransactionList = new List<BusinessObject.ExpenseTransaction>();
                    var entityList = new List<ExpenseTransaction>();
                    if (status == 0)
                    {
                        entityList = db.ExpenseTransactions.Where(m =>  m.Beneficiary.DepartmentId == dept && m.TransactionDate.Contains(yrVal) && m.TransactionDate.Contains(monthVal)).ToList();
                    }

                    if (status == 1)
                    {
                        entityList = db.ExpenseTransactions.Where(m => m.Status == 0 && m.Beneficiary.DepartmentId == dept && m.TransactionDate.Contains(yrVal) && m.TransactionDate.Contains(monthVal)).ToList();
                    }

                    if (status == 2)
                    {
                        entityList = db.ExpenseTransactions.Where(m => m.Status == 1  && m.Beneficiary.DepartmentId == dept && m.TransactionDate.Contains(yrVal) && m.TransactionDate.Contains(monthVal)).ToList();
                    }

                    if (status == 3)
                    {
                        entityList = db.ExpenseTransactions.Where(m => m.Status == 2 && m.Beneficiary.DepartmentId == dept && m.TransactionDate.Contains(yrVal) && m.TransactionDate.Contains(monthVal)).ToList();
                    }

                    if (status == 4)
                    {
                        entityList = db.ExpenseTransactions.Where(m => m.Status == 3 && m.Beneficiary.DepartmentId == dept && m.TransactionDate.Contains(yrVal) && m.TransactionDate.Contains(monthVal)).ToList();
                    }

                    if (!entityList.Any())
                    {
                        return weeklyTransactionList;
                    }

                    entityList.ForEachx(item =>
                                                {
                                                    var xVal = new WeekOfMonthDeterminant().GetWeekOfMonth(DateTime.Parse(item.TransactionDate));
                                                    
                                                    if (xVal == weeklyVal)
                                                    {
                                                        var myBusinessObj = ExpenseTransactionMapper.Map<ExpenseTransaction, BusinessObject.ExpenseTransaction>(item);
                                                        if (myBusinessObj.Status == 0)
                                                        {

                                                            var name = Enum.GetName(typeof(ExpenseApprovalDateTime), ExpenseApprovalDateTime.N_A);
                                                            if (name != null)
                                                            {
                                                                myBusinessObj.DateApproved = name.Replace("_", "/");
                                                                myBusinessObj.TimeApproved = name.Replace("_", "/");
                                                            }
                                                        }

                                                        myBusinessObj.ApprovalStatus = Enum.GetName(typeof(ExpenseApprovalStatus), myBusinessObj.Status);

                                                        var paymentStatus =
                                                            db.ExpenseTransactionPayments.Where(
                                                                m => m.ExpenseTransactionId == myBusinessObj.ExpenseTransactionId).ToList();

                                                        if (!paymentStatus.Any())
                                                        {
                                                            var name = Enum.GetName(typeof(ExpensePaymentStatus), ExpensePaymentStatus.Not_Paid);
                                                            if (name != null)
                                                                myBusinessObj.PaymentStatus = name.Replace("_", " ");
                                                        }

                                                        if (paymentStatus.Any() && paymentStatus.ElementAt(0).Status == 0)
                                                        {
                                                            var name = Enum.GetName(typeof(ExpensePaymentStatus), ExpensePaymentStatus.Partly_Paid);
                                                            if (
                                                                name != null)
                                                                myBusinessObj.PaymentStatus = name.Replace("_", " ");
                                                        }

                                                        if (paymentStatus.Any() && paymentStatus.ElementAt(0).Status == 1)
                                                        {
                                                            var name = Enum.GetName(typeof(ExpensePaymentStatus), ExpensePaymentStatus.Fully_Paid);
                                                            if (
                                                                name != null)
                                                                myBusinessObj.PaymentStatus = name.Replace("_", " ");
                                                        }
                                                        
                                                        weeklyTransactionList.Add(myBusinessObj);
                                                    }
                                                    
                                                });


                    return !weeklyTransactionList.Any() ? new List<BusinessObject.ExpenseTransaction>() : weeklyTransactionList.OrderByDescending(m => Convert.ToDateTime(m.TransactionDate)).ToList();
                }

            }
            catch (Exception ex)
            {
                ErrorManager.LogApplicationError(ex.StackTrace, ex.Source, ex.Message);
                return new List<BusinessObject.ExpenseTransaction>();
            }
	    }

        public List<BusinessObject.ExpenseTransaction> GetMonthlyTransactions(int status, int dept, string yrVal, string monthVal)
        {
            try
            {
                using (var db = new ExpenseManagerDBEntities())
                {
                    var monthlyTransactionList = new List<BusinessObject.ExpenseTransaction>();
                    var entityList = new List<ExpenseTransaction>();
                    if (status == 0)
                    {
                        entityList = db.ExpenseTransactions.Where(m => m.Beneficiary.DepartmentId == dept && m.TransactionDate.Contains(yrVal) && m.TransactionDate.Contains(monthVal)).ToList();
                    }

                    if (status == 1)
                    {
                        entityList = db.ExpenseTransactions.Where(m => m.Status == 0 && m.Beneficiary.DepartmentId == dept && m.TransactionDate.Contains(yrVal) && m.TransactionDate.Contains(monthVal)).ToList();
                    }

                    if (status == 2)
                    {
                        entityList = db.ExpenseTransactions.Where(m => m.Status == 1 && m.Beneficiary.DepartmentId == dept && m.TransactionDate.Contains(yrVal) && m.TransactionDate.Contains(monthVal)).ToList();
                    }

                    if (status == 3)
                    {
                        entityList = db.ExpenseTransactions.Where(m => m.Status == 2 && m.Beneficiary.DepartmentId == dept && m.TransactionDate.Contains(yrVal) && m.TransactionDate.Contains(monthVal)).ToList();
                    }

                    if (status == 4)
                    {
                        entityList = db.ExpenseTransactions.Where(m => m.Status == 3 && m.Beneficiary.DepartmentId == dept && m.TransactionDate.Contains(yrVal) && m.TransactionDate.Contains(monthVal)).ToList();
                    }

                    if (!entityList.Any())
                    {
                        return monthlyTransactionList;
                    }

                    entityList.ForEachx( item =>
                                            {
                                                var myBusinessObj = ExpenseTransactionMapper.Map<ExpenseTransaction, BusinessObject.ExpenseTransaction>(item);

                                                if (myBusinessObj.Status == 0)
                                                {

                                                    var name = Enum.GetName(typeof(ExpenseApprovalDateTime), ExpenseApprovalDateTime.N_A);
                                                    if (name != null)
                                                    {
                                                        myBusinessObj.DateApproved = name.Replace("_", "/");
                                                        myBusinessObj.TimeApproved = name.Replace("_", "/");
                                                    }
                                                }

                                                myBusinessObj.ApprovalStatus = Enum.GetName(typeof(ExpenseApprovalStatus), myBusinessObj.Status);
                                        
                                                var paymentStatus =
                                                    db.ExpenseTransactionPayments.Where(
                                                        m => m.ExpenseTransactionId == myBusinessObj.ExpenseTransactionId).ToList();
                                               
                                                if (!paymentStatus.Any())
                                                {
                                                    var name = Enum.GetName(typeof(ExpensePaymentStatus), ExpensePaymentStatus.Not_Paid);
                                                    if (name != null)
                                                        myBusinessObj.PaymentStatus = name.Replace("_", " ");
                                                }

                                                if ( paymentStatus.Any() && paymentStatus.ElementAt(0).Status == 0)
                                                {
                                                    var name = Enum.GetName(typeof(ExpensePaymentStatus), ExpensePaymentStatus.Partly_Paid);
                                                    if (
                                                        name != null)
                                                        myBusinessObj.PaymentStatus = name.Replace("_", " ");
                                                }

                                                if (paymentStatus.Any() && paymentStatus.ElementAt(0).Status == 1)
                                                {
                                                    var name = Enum.GetName(typeof(ExpensePaymentStatus), ExpensePaymentStatus.Fully_Paid);
                                                    if (
                                                        name != null)
                                                        myBusinessObj.PaymentStatus = name.Replace("_", " ");
                                                }
                                                monthlyTransactionList.Add(myBusinessObj);

                                            });


                    return !monthlyTransactionList.Any() ? new List<BusinessObject.ExpenseTransaction>() : monthlyTransactionList.OrderByDescending(m => Convert.ToDateTime(m.TransactionDate)).ToList();
                }

            }
            catch (Exception ex)
            {
                ErrorManager.LogApplicationError(ex.StackTrace, ex.Source, ex.Message);
                return new List<BusinessObject.ExpenseTransaction>();
            }
        }

        public List<BusinessObject.ExpenseTransaction> GetTransactionsByDateRange(string startDate, string endDate, int deptId, int status)
        {
            try
            {
                using (var db = new ExpenseManagerDBEntities())
                {
                    var objList = new List<ExpenseTransaction>();

                    var expenseTransactionList = new List<BusinessObject.ExpenseTransaction>();

                    if (status == 0)
                    {
                        objList = db.ExpenseTransactions.ToList().FindAll(m => m.Beneficiary.DepartmentId == deptId && (String.CompareOrdinal(m.TransactionDate, startDate) >= 0) &&
                        (String.CompareOrdinal(m.TransactionDate, endDate) <= 0)).ToList();
                    }

                    if (status == 1)
                    {
                        objList = db.ExpenseTransactions.ToList().FindAll(m => m.Beneficiary.DepartmentId == deptId && (String.CompareOrdinal(m.TransactionDate, startDate) >= 0) &&
                        (String.CompareOrdinal(m.TransactionDate, endDate) <= 0) && m.Status == 0).ToList();
                    }

                    if (status == 2)
                    {
                        objList = db.ExpenseTransactions.ToList().FindAll(m => m.Beneficiary.DepartmentId == deptId && (String.CompareOrdinal(m.TransactionDate, startDate) >= 0) &&
                        (String.CompareOrdinal(m.TransactionDate, endDate) <= 0) && m.Status == 1).ToList();
                    }

                    if (status == 3)
                    {
                        objList = db.ExpenseTransactions.ToList().FindAll(m => m.Beneficiary.DepartmentId == deptId && (String.CompareOrdinal(m.TransactionDate, startDate) >= 0) &&
                        (String.CompareOrdinal(m.TransactionDate, endDate) <= 0) && m.Status == 2).ToList();
                    }

                    if (status == 4)
                    {
                        objList = db.ExpenseTransactions.ToList().FindAll(m => m.Beneficiary.DepartmentId == deptId && (String.CompareOrdinal(m.TransactionDate, startDate) >= 0) &&
                        (String.CompareOrdinal(m.TransactionDate, endDate) <= 0) && m.Status == 3).ToList();
                    }

                    if (!objList.Any())
                    {
                        return expenseTransactionList;
                    }

                    //Re-Map each Entity Object to Business Object
                    //Parallel.ForEach(objList, item =>
                    //{
                       

                    //});

                    objList.ForEachx
                        (
                            m =>
                            {
                                var myBusinessObj = ExpenseTransactionMapper.Map<ExpenseTransaction, BusinessObject.ExpenseTransaction>(m);

                                if (myBusinessObj.Status == 0)
                                {

                                    var name = Enum.GetName(typeof(ExpenseApprovalDateTime), ExpenseApprovalDateTime.N_A);
                                    if (name != null)
                                    {
                                        myBusinessObj.DateApproved = name.Replace("_", "/");
                                        myBusinessObj.TimeApproved = name.Replace("_", "/");
                                    }
                                }

                                myBusinessObj.ApprovalStatus = Enum.GetName(typeof(ExpenseApprovalStatus), myBusinessObj.Status);

                                var paymentStatus = db.ExpenseTransactionPayments.Where(x => x.ExpenseTransactionId == myBusinessObj.ExpenseTransactionId).ToList();

                                if (!paymentStatus.Any())
                                {
                                    var name = Enum.GetName(typeof(ExpensePaymentStatus), ExpensePaymentStatus.Not_Paid);
                                    if (name != null)
                                        myBusinessObj.PaymentStatus = name.Replace("_", " ");
                                }

                                if (paymentStatus.Any() && paymentStatus.ElementAt(0).Status == 0)
                                {
                                    var name = Enum.GetName(typeof(ExpensePaymentStatus), ExpensePaymentStatus.Partly_Paid);
                                    if (
                                        name != null)
                                        myBusinessObj.PaymentStatus = name.Replace("_", " ");
                                }

                                if (paymentStatus.Any() && paymentStatus.ElementAt(0).Status == 1)
                                {
                                    var name = Enum.GetName(typeof(ExpensePaymentStatus), ExpensePaymentStatus.Fully_Paid);
                                    if (
                                        name != null)
                                        myBusinessObj.PaymentStatus = name.Replace("_", " ");
                                }
                                expenseTransactionList.Add(myBusinessObj);
                            }
                        );

                    if (!expenseTransactionList.Any())
                    {
                        return new List<BusinessObject.ExpenseTransaction>();
                    }
                    return expenseTransactionList.OrderByDescending(m => m.TransactionDate).ToList();
                }
            }
            catch (Exception ex)
            {
                ErrorManager.LogApplicationError(ex.StackTrace, ex.Source, ex.Message);
                return new List<BusinessObject.ExpenseTransaction>();
            }
        }
    }
   
  }

	//Class File Generated from Code<->Stripper 1.2.0.0 | All Rights Reserved

