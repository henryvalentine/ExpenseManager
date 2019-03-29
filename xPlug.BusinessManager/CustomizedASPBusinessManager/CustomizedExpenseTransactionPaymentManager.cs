using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using ExpenseManager.EF;
using ExpenseManager.EF.Helpers;
using xPlug.BusinessObject.CustomizedASPBusinessObject.Enum;
using XPLUG.WEBTOOLS;
using xPlug.BusinessObjectMapper;

namespace xPlug.BusinessManager
{
	/// Author:	 Oluwaseyi Adegboyega - sadegboyega@xplugng.com
	/// Copyright © 2013. xPlug Technologies Limited. All Rights Reserved
	///*******************************************************************************
	///* Date Created: 	04-06-2013
	///* Date Generated:	12-09-2013 10:06:37
	///*******************************************************************************


	public partial class ExpenseTransactionPaymentManager
	{
        public List<BusinessObject.ExpenseTransactionPayment> GetBeneficiaryUnCompletedExpenseTransactionPayments(int beneficiaryId)
        {
            try
            {
                using (var db = new ExpenseManagerDBEntities())
                {
                    var myObjList = db.ExpenseTransactionPayments.Where(m => m.BeneficiaryId == beneficiaryId && m.Status == 0 && m.ExpenseTransaction.Status == 1);
                    var myBusinessObjList = new List<BusinessObject.ExpenseTransactionPayment>();
                    if (!myObjList.Any())
                    {
                        return myBusinessObjList;
                    }
                    //Re-Map each Entity Object to Business Object
                    foreach (var item in myObjList)
                    {
                        var myBusinessObj = ExpenseTransactionPaymentMapper.Map<ExpenseTransactionPayment, BusinessObject.ExpenseTransactionPayment>(item);
                        if (myBusinessObj == null) { continue; }
                        myBusinessObjList.Add(myBusinessObj);
                    }
                    return myBusinessObjList.OrderBy(m => m.ExpenseTransaction.TransactionDate).ToList();
                }
            }
            catch (Exception ex)
            {
                ErrorManager.LogApplicationError(ex.StackTrace, ex.Source, ex.Message);
                return new List<xPlug.BusinessObject.ExpenseTransactionPayment>();
            }
        }

        public List<BusinessObject.ExpenseTransactionPayment> FilterExpenseTransactionPaymentsByDateRange(DateTime start, DateTime stop)
        {
            try
            {
                using (var db = new ExpenseManagerDBEntities())
                {
                    if (stop < start || start > stop)
                    {
                        return null;
                    }

                    var objList = db.ExpenseTransactionPayments.ToList();
                    //var objList = db.ExpenseTransactionPayments.Where(m => (String.CompareOrdinal(m.LastPaymentDate, start.ToString("yyyy/MM/dd")) >= 0) &&
                    //    (String.CompareOrdinal(m.LastPaymentDate, stop.ToString("yyyy/MM/dd")) <= 0)).ToList();

                    if (!objList.Any())
                    {
                        return new List<BusinessObject.ExpenseTransactionPayment>();
                    }

                    var newTransactionList = new List<ExpenseTransactionPayment>();

                    newTransactionList.AddRange(from transaction in objList
                                                let transactionDate = DateTime.Parse(transaction.LastPaymentDate)
                                                where
                                                    transactionDate == start ||
                                                    (transactionDate > start && transactionDate < stop) ||
                                                    transactionDate == stop
                                                select transaction);

                    var myBusinessObjList = new List<BusinessObject.ExpenseTransactionPayment>();

                    if (!newTransactionList.Any())
                    {
                        return myBusinessObjList;
                    }
                    //Re-Map each Entity Object to Business Object
                    myBusinessObjList.AddRange(newTransactionList.Select(item => ExpenseTransactionPaymentMapper.Map<ExpenseTransactionPayment, BusinessObject.ExpenseTransactionPayment>(item)).Where(myBusinessObj => myBusinessObj != null));
                    return myBusinessObjList.OrderByDescending(m =>m.LastPaymentDate).ToList();
                }
            }
            catch (Exception ex)
            {
                ErrorManager.LogApplicationError(ex.StackTrace, ex.Source, ex.Message);
                return new List<BusinessObject.ExpenseTransactionPayment>();
            }
        }

        public List<BusinessObject.ExpenseTransactionPayment> GetApprovedTransactionPaymentsByDateRange(DateTime start, DateTime stop)
        {
            try
            {
                using (var db = new ExpenseManagerDBEntities())
                {
                    if (stop < start || start > stop)
                    {
                        return null;
                    }

                    //var objList = db.ExpenseTransactionPayments.Where(m => m.ExpenseTransaction.Status == 1).ToList();
                    var objList = db.ExpenseTransactionPayments.Where(m => (String.CompareOrdinal(m.LastPaymentDate, start.ToString("yyyy/MM/dd")) >= 0) &&
                        (System.String.CompareOrdinal(m.LastPaymentDate, stop.ToString("yyyy/MM/dd")) <= 0) && m.ExpenseTransaction.Status == 1).ToList();

                    if (!objList.Any())
                    {
                        return new List<BusinessObject.ExpenseTransactionPayment>();
                    }

                    var newTransactionList = new List<ExpenseTransactionPayment>();

                    //newTransactionList.AddRange(from transaction in objList
                    //                            let transactionDate = DateTime.Parse(transaction.LastPaymentDate)
                    //                            where
                    //                                transactionDate == start ||
                    //                                (transactionDate > start && transactionDate < stop) ||
                    //                                transactionDate == stop
                    //                            select transaction);

                    var myBusinessObjList = new List<BusinessObject.ExpenseTransactionPayment>();

                    if (!newTransactionList.Any())
                    {
                        return myBusinessObjList;
                    }
                    //Re-Map each Entity Object to Business Object
                    myBusinessObjList.AddRange(objList.Select(item => ExpenseTransactionPaymentMapper.Map<ExpenseTransactionPayment, BusinessObject.ExpenseTransactionPayment>(item)).Where(myBusinessObj => myBusinessObj != null));
                    objList.Clear();
                    return myBusinessObjList.OrderBy(m => m.ExpenseTransaction.ExpenseTitle).ToList();
                }
            }
            catch (Exception ex)
            {
                ErrorManager.LogApplicationError(ex.StackTrace, ex.Source, ex.Message);
                return new List<BusinessObject.ExpenseTransactionPayment>();
            }
        }

        public List<BusinessObject.ExpenseTransactionPayment> GetVoidedTransactionPaymentsByDateRange(DateTime start, DateTime stop)
        {
            try
            {
                using (var db = new ExpenseManagerDBEntities())
                {
                    if (stop < start || start > stop)
                    {
                        return null;
                    }

                    //var objList = db.ExpenseTransactionPayments.Where(m => m.ExpenseTransaction.Status == 3).ToList();
                    var objList = db.ExpenseTransactionPayments.ToList().FindAll(m => (String.CompareOrdinal(m.LastPaymentDate, start.ToString("yyyy/MM/dd")) >= 0) &&
                        (String.CompareOrdinal(m.LastPaymentDate, stop.ToString("yyyy/MM/dd")) <= 0) && m.ExpenseTransaction.Status == 3).ToList();

                    if (!objList.Any())
                    {
                        return new List<BusinessObject.ExpenseTransactionPayment>();
                    }

                    //var newTransactionList = new List<ExpenseTransactionPayment>();

                    //newTransactionList.AddRange(from transaction in objList
                    //                            let transactionDate = DateTime.Parse(transaction.LastPaymentDate)
                    //                            where
                    //                                transactionDate == start ||
                    //                                (transactionDate > start && transactionDate < stop) ||
                    //                                transactionDate == stop
                    //                            select transaction);

                    var myBusinessObjList = new List<BusinessObject.ExpenseTransactionPayment>();

                    //if (!newTransactionList.Any())
                    //{
                    //    return myBusinessObjList;
                    //}

                    //Re-Map each Entity Object to Business Object
                    myBusinessObjList.AddRange(objList.Select(item => ExpenseTransactionPaymentMapper.Map<ExpenseTransactionPayment, BusinessObject.ExpenseTransactionPayment>(item)).Where(myBusinessObj => myBusinessObj != null));
                    objList.Clear();
                    return myBusinessObjList.OrderBy(m => m.ExpenseTransaction.ExpenseTitle).ToList();
                }
            }
            catch (Exception ex)
            {
                ErrorManager.LogApplicationError(ex.StackTrace, ex.Source, ex.Message);
                return new List<BusinessObject.ExpenseTransactionPayment>();
            }
        }

        public List<BusinessObject.ExpenseTransactionPayment> GetUnCompletedExpenseTransactionPaymentsByDateRange(DateTime start, DateTime stop)
        {
            try
            {
                using (var db = new ExpenseManagerDBEntities())
                {
                    if (stop < start || start > stop)
                    {
                        return null;
                    }

                    var objList = db.ExpenseTransactionPayments.Where(m => m.Status == 0).ToList();
                    //var objList = db.ExpenseTransactionPayments.ToList().FindAll(m => (String.CompareOrdinal(m.LastPaymentDate, start.ToString("yyyy/MM/dd")) >= 0) &&
                    //    (String.CompareOrdinal(m.LastPaymentDate, stop.ToString("yyyy/MM/dd")) <= 0) && (m.AmountPaid < m.TotalAmountPayable && m.AmountPaid > 0)).ToList();

                    if (!objList.Any())
                    {
                        return new List<BusinessObject.ExpenseTransactionPayment>();
                    }

                    var newTransactionList = new List<ExpenseTransactionPayment>();

                    newTransactionList.AddRange(from transaction in objList
                                                let transactionDate = DateTime.Parse(transaction.LastPaymentDate)
                                                where
                                                    transactionDate == start ||
                                                    (transactionDate > start && transactionDate < stop) ||
                                                    transactionDate == stop
                                                select transaction);

                    var myBusinessObjList = new List<BusinessObject.ExpenseTransactionPayment>();

                    if (!newTransactionList.Any())
                    {
                        return myBusinessObjList;
                    }

                    //Re-Map each Entity Object to Business Object
                    myBusinessObjList.AddRange(newTransactionList.Select(item => ExpenseTransactionPaymentMapper.Map<ExpenseTransactionPayment, BusinessObject.ExpenseTransactionPayment>(item)).Where(myBusinessObj => myBusinessObj != null));
                   
                    return myBusinessObjList.OrderByDescending(m => m.LastPaymentDate).ToList();
                }
            }
            catch (Exception ex)
            {
                ErrorManager.LogApplicationError(ex.StackTrace, ex.Source, ex.Message);
                return new List<BusinessObject.ExpenseTransactionPayment>();
            }
        }

        public List<BusinessObject.ExpenseTransactionPayment> GetCompletedExpenseTransactionPaymentsByDateRange(DateTime start, DateTime stop)
        {
            try
            {
                using (var db = new ExpenseManagerDBEntities())
                {
                    if (stop < start || start > stop)
                    {
                        return null;
                    }

                    var objList = db.ExpenseTransactionPayments.Where(m => m.Status == 1).ToList();
                    if (!objList.Any())
                    {
                        return new List<BusinessObject.ExpenseTransactionPayment>();
                    }

                    var newTransactionList = new List<ExpenseTransactionPayment>();

                    newTransactionList.AddRange(from transaction in objList
                                                let transactionDate = DateTime.Parse(transaction.LastPaymentDate)
                                                where
                                                    transactionDate == start ||
                                                    (transactionDate > start && transactionDate < stop) ||
                                                    transactionDate == stop
                                                select transaction);

                    var myBusinessObjList = new List<BusinessObject.ExpenseTransactionPayment>();

                    if (!newTransactionList.Any())
                    {
                        return myBusinessObjList;
                    }

                    //Re-Map each Entity Object to Business Object
                    myBusinessObjList.AddRange(newTransactionList.Select(item => ExpenseTransactionPaymentMapper.Map<ExpenseTransactionPayment, BusinessObject.ExpenseTransactionPayment>(item)).Where(myBusinessObj => myBusinessObj != null));

                    return myBusinessObjList.OrderByDescending(m => m.LastPaymentDate).ToList();
                }
            }
            catch (Exception ex)
            {
                ErrorManager.LogApplicationError(ex.StackTrace, ex.Source, ex.Message);
                return new List<BusinessObject.ExpenseTransactionPayment>();
            }
        }

        public List<BusinessObject.ExpenseTransactionPayment> GetCurrentTransactionPayments(string date)
        {
            try
            {
                using (var db = new ExpenseManagerDBEntities())
                {

                    var objList = db.ExpenseTransactionPayments.Where(m => m.LastPaymentDate == date).ToList();

                    var myBusinessObjList = new List<BusinessObject.ExpenseTransactionPayment>();

                    if (!objList.Any())
                    {
                        return myBusinessObjList;
                    }
                    //Re-Map each Entity Object to Business Object
                    myBusinessObjList.AddRange(objList.Select(item => ExpenseTransactionPaymentMapper.Map<ExpenseTransactionPayment, BusinessObject.ExpenseTransactionPayment>(item)).Where(myBusinessObj => myBusinessObj != null));

                    foreach (var item in myBusinessObjList)
                    {
                        if (item.Status == 0)
                        {
                            var name = Enum.GetName(typeof(ExpensePaymentStatus), ExpensePaymentStatus.Partly_Paid);
                            if (name != null)
                            {
                                item.PaymentStatus = name.Replace("_", " ");
                            }
                        }

                        if (item.Status == 1)
                        {
                            var name = Enum.GetName(typeof(ExpensePaymentStatus), ExpensePaymentStatus.Fully_Paid);
                            if (name != null)
                            {
                                item.PaymentStatus = name.Replace("_", " ");
                            }
                        }
                    }
                    return myBusinessObjList.OrderBy(m => m.ExpenseTransaction.ExpenseTitle).ToList();
                }
            }
            catch (Exception ex)
            {
                ErrorManager.LogApplicationError(ex.StackTrace, ex.Source, ex.Message);
                return new List<BusinessObject.ExpenseTransactionPayment>();
            }
        }

        public List<BusinessObject.ExpenseTransactionPayment> GetUnCompletedExpenseTransactionPayments()
        {
            try
            {
                using (var db = new ExpenseManagerDBEntities())
                {

                    var objList = db.ExpenseTransactionPayments.Where(m => m.Status == 0);

                    var myBusinessObjList = new List<BusinessObject.ExpenseTransactionPayment>();

                    if (!objList.Any())
                    {
                        return myBusinessObjList;
                    }
                    //Re-Map each Entity Object to Business Object
                    myBusinessObjList.AddRange(objList.Select(item => ExpenseTransactionPaymentMapper.Map<ExpenseTransactionPayment, BusinessObject.ExpenseTransactionPayment>(item)).Where(myBusinessObj => myBusinessObj != null));
                    return myBusinessObjList.OrderBy(m => m.ExpenseTransaction.ExpenseTitle).ToList();
                }
            }
            catch (Exception ex)
            {
                ErrorManager.LogApplicationError(ex.StackTrace, ex.Source, ex.Message);
                return new List<BusinessObject.ExpenseTransactionPayment>();
            }
        }

        public List<BusinessObject.ExpenseTransactionPayment> GetCompletedExpenseTransactionPayments()
        {
            try
            {
                using (var db = new ExpenseManagerDBEntities())
                {

                    var objList = db.ExpenseTransactionPayments.Where(m => m.Status == 1);

                    var myBusinessObjList = new List<BusinessObject.ExpenseTransactionPayment>();

                    if (!objList.Any())
                    {
                        return myBusinessObjList;
                    }
                    //Re-Map each Entity Object to Business Object
                    myBusinessObjList.AddRange(objList.Select(item => ExpenseTransactionPaymentMapper.Map<ExpenseTransactionPayment, BusinessObject.ExpenseTransactionPayment>(item)).Where(myBusinessObj => myBusinessObj != null));
                    return myBusinessObjList.OrderBy(m => m.ExpenseTransaction.ExpenseTitle).ToList();
                }
            }
            catch (Exception ex)
            {
                ErrorManager.LogApplicationError(ex.StackTrace, ex.Source, ex.Message);
                return new List<BusinessObject.ExpenseTransactionPayment>();
            }
        }

        public List<BusinessObject.ExpenseTransactionPayment> GetOrderedExpenseTransactionPayments()
        {
            try
            {
                using (var db = new ExpenseManagerDBEntities())
                {

                    var objList = db.ExpenseTransactionPayments.ToList();

                    var myBusinessObjList = new List<BusinessObject.ExpenseTransactionPayment>();

                    if (!objList.Any())
                    {
                        return myBusinessObjList;
                    }
                    //Re-Map each Entity Object to Business Object
                    myBusinessObjList.AddRange(objList.Select(item => ExpenseTransactionPaymentMapper.Map<ExpenseTransactionPayment, BusinessObject.ExpenseTransactionPayment>(item)).Where(myBusinessObj => myBusinessObj != null));
                    return myBusinessObjList.OrderBy(m => m.ExpenseTransaction.ExpenseTitle).ToList();
                }
            }
            catch (Exception ex)
            {
                ErrorManager.LogApplicationError(ex.StackTrace, ex.Source, ex.Message);
                return new List<BusinessObject.ExpenseTransactionPayment>();
            }
        }

        public long UpdateTransactionPayment(BusinessObject.ExpenseTransactionPayment expenseTransactionPayment)
        {
            try
            {
                var myEntityObj = ExpenseTransactionPaymentMapper.Map<BusinessObject.ExpenseTransactionPayment, ExpenseTransactionPayment>(expenseTransactionPayment);
                
                if (myEntityObj == null)
                {
                    return 0;
                }
                using (var db = new ExpenseManagerDBEntities())
                {
                    db.ExpenseTransactionPayments.Attach(myEntityObj);
                    db.ObjectStateManager.ChangeObjectState(myEntityObj, EntityState.Modified);
                    db.SaveChanges();
                    return myEntityObj.ExpenseTransactionPaymentId;
                }
            }
            catch (Exception ex)
            {
                ErrorManager.LogApplicationError(ex.StackTrace, ex.Source, ex.Message);
                return 0;
            }
        }

        public BusinessObject.ExpenseTransactionPayment GetUncompltedTransactionPayment(long expenseTransactionPaymentId)
        {
            try
            {
                

                if (expenseTransactionPaymentId < 1)
                {
                    return new BusinessObject.ExpenseTransactionPayment();
                }
                using (var db = new ExpenseManagerDBEntities())
                {
                    var entityObj = db.ExpenseTransactionPayments.Where(m => m.ExpenseTransactionPaymentId == expenseTransactionPaymentId && m.Status == 0).ToList();
                    
                    if (!entityObj.Any())
                    {
                        return new BusinessObject.ExpenseTransactionPayment();
                    }

                    var businessObj = ExpenseTransactionPaymentMapper.Map<ExpenseTransactionPayment, BusinessObject.ExpenseTransactionPayment>(entityObj.ElementAt(0));
                    
                    if (businessObj == null || businessObj.ExpenseTransactionPaymentId < 1)
                    {
                        return new BusinessObject.ExpenseTransactionPayment();
                    }

                    return businessObj;
                }
            }
            catch (Exception ex)
            {
                ErrorManager.LogApplicationError(ex.StackTrace, ex.Source, ex.Message);
                return new BusinessObject.ExpenseTransactionPayment();
            }
        }

        public BusinessObject.ExpenseTransactionPayment AddExpenseTransactionPaymentReturnObject(BusinessObject.ExpenseTransactionPayment expenseTransactionPayment)
        {
            try
            {
                //Re-Map Object to Entity Object
                var myEntityObj = ExpenseTransactionPaymentMapper.Map<BusinessObject.ExpenseTransactionPayment, ExpenseTransactionPayment>(expenseTransactionPayment);
                if (myEntityObj == null)
                {
                    return null;
                }
                using (var db = new ExpenseManagerDBEntities())
                {
                    db.AddToExpenseTransactionPayments(myEntityObj);
                    db.SaveChanges();
                    expenseTransactionPayment.ExpenseTransactionPaymentId = myEntityObj.ExpenseTransactionPaymentId;
                    return expenseTransactionPayment;
                }
            }
            catch (Exception ex)
            {
                ErrorManager.LogApplicationError(ex.StackTrace, ex.Source, ex.Message);
                return null;
            }
        }


        public List<BusinessObject.ExpenseTransactionPayment> GetWeeklyTransactionPayments(int status, int dept, string yrVal, string monthVal, int weeklyVal)
        {
            try
            {
                using (var db = new ExpenseManagerDBEntities())
                {
                    var weeklyTransactionList = new List<BusinessObject.ExpenseTransactionPayment>();
                    var entityList = new List<ExpenseTransactionPayment>();
                    if (status == 0)
                    {
                        entityList = db.ExpenseTransactionPayments.Where(m => m.Beneficiary.DepartmentId == dept && (m.ExpenseTransaction.Status == 1 || m.ExpenseTransaction.Status == 3) && m.LastPaymentDate.Contains(yrVal) && m.LastPaymentDate.Contains(monthVal)).ToList();
                    }

                    if (status == 1)
                    {
                        entityList = db.ExpenseTransactionPayments.Where(m =>m.ExpenseTransaction.Status == 1 && m.Beneficiary.DepartmentId == dept && m.LastPaymentDate.Contains(yrVal) && m.LastPaymentDate.Contains(monthVal)).ToList();
                    }
                    
                    if (status == 2)
                    {
                        entityList = db.ExpenseTransactionPayments.Where(m =>m.ExpenseTransaction.Status == 3 && m.Beneficiary.DepartmentId == dept && m.LastPaymentDate.Contains(yrVal) && m.LastPaymentDate.Contains(monthVal)).ToList();
                    }

                    if (!entityList.Any())
                    {
                        return weeklyTransactionList;
                    }

                    foreach (var item in entityList)
                    {
                         var xVal = new WeekOfMonthDeterminant().GetWeekOfMonth(DateTime.Parse(item.LastPaymentDate));

                        if (xVal == weeklyVal)
                        {
                            var myBusinessObj =
                                ExpenseTransactionPaymentMapper
                                    .Map<ExpenseTransactionPayment, BusinessObject.ExpenseTransactionPayment>(item);

                            if (myBusinessObj.Status == 0)
                            {
                                var name = Enum.GetName(typeof (ExpensePaymentStatus), ExpensePaymentStatus.Partly_Paid);
                                if (
                                    name != null)
                                    myBusinessObj.PaymentStatus = name.Replace("_", " ");
                            }

                            if (myBusinessObj.Status == 1)
                            {
                                var name = Enum.GetName(typeof (ExpensePaymentStatus), ExpensePaymentStatus.Fully_Paid);
                                if (
                                    name != null)
                                    myBusinessObj.PaymentStatus = name.Replace("_", " ");
                            }

                            weeklyTransactionList.Add(myBusinessObj);
                        }
                    }

                    //Parallel.ForEach(entityList, item =>
                    //                            {
                    //                                var xVal = new WeekOfMonthDeterminant().GetWeekOfMonth(DateTime.Parse(item.LastPaymentDate));

                    //                                if (xVal == weeklyVal)
                    //                                {
                    //                                    var myBusinessObj = ExpenseTransactionPaymentMapper.Map<ExpenseTransactionPayment, BusinessObject.ExpenseTransactionPayment>(item);
                           
                    //                                    if (myBusinessObj.Status == 0)
                    //                                    {
                    //                                        var name = Enum.GetName(typeof(ExpensePaymentStatus), ExpensePaymentStatus.Partly_Paid);
                    //                                        if (
                    //                                            name != null)
                    //                                            myBusinessObj.PaymentStatus = name.Replace("_", " ");
                    //                                    }

                    //                                    if (myBusinessObj.Status == 1)
                    //                                    {
                    //                                        var name = Enum.GetName(typeof(ExpensePaymentStatus), ExpensePaymentStatus.Fully_Paid);
                    //                                        if (
                    //                                            name != null)
                    //                                            myBusinessObj.PaymentStatus = name.Replace("_", " ");
                    //                                    }

                    //                                    weeklyTransactionList.Add(myBusinessObj);
                    //                                }

                    //                            });


                    return !weeklyTransactionList.Any() ? new List<BusinessObject.ExpenseTransactionPayment>() : weeklyTransactionList.OrderByDescending(m => Convert.ToDateTime(m.LastPaymentDate)).ToList();
                }

            }
            catch (Exception ex)
            {
                ErrorManager.LogApplicationError(ex.StackTrace, ex.Source, ex.Message);
                return new List<BusinessObject.ExpenseTransactionPayment>();
            }
        }

        public List<BusinessObject.ExpenseTransactionPayment> GetMonthlyTransactionPayments(int status, int dept, string yrVal, string monthVal)
        {
            try
            {
                using (var db = new ExpenseManagerDBEntities())
                {
                    var monthlyTransactionList = new List<BusinessObject.ExpenseTransactionPayment>();
                    var entityList = new List<ExpenseTransactionPayment>();
                    if (status == 0)
                    {
                        entityList = db.ExpenseTransactionPayments.Where(m => m.Beneficiary.DepartmentId == dept && m.LastPaymentDate.Contains(yrVal) && m.LastPaymentDate.Contains(monthVal)).ToList();
                    }

                    if (status == 1)
                    {
                        entityList = db.ExpenseTransactionPayments.Where(m =>m.ExpenseTransaction.Status == 0 && m.Beneficiary.DepartmentId == dept && m.LastPaymentDate.Contains(yrVal) && m.LastPaymentDate.Contains(monthVal)).ToList();
                    }

                    if (status == 2)
                    {
                        entityList = db.ExpenseTransactionPayments.Where(m =>m.ExpenseTransaction.Status == 1 && m.Beneficiary.DepartmentId == dept && m.LastPaymentDate.Contains(yrVal) && m.LastPaymentDate.Contains(monthVal)).ToList();
                    }

                    if (status == 3)
                    {
                        entityList = db.ExpenseTransactionPayments.Where(m =>m.ExpenseTransaction.Status == 2 && m.Beneficiary.DepartmentId == dept && m.LastPaymentDate.Contains(yrVal) && m.LastPaymentDate.Contains(monthVal)).ToList();
                    }

                    if (status == 4)
                    {
                        entityList = db.ExpenseTransactionPayments.Where(m =>m.ExpenseTransaction.Status == 3 && m.Beneficiary.DepartmentId == dept && m.LastPaymentDate.Contains(yrVal) && m.LastPaymentDate.Contains(monthVal)).ToList();
                    }

                    if (!entityList.Any())
                    {
                        return monthlyTransactionList;
                    }
                    foreach (var item in entityList)
                    {
                        var myBusinessObj = ExpenseTransactionPaymentMapper.Map<ExpenseTransactionPayment, BusinessObject.ExpenseTransactionPayment>(item);

                        if (myBusinessObj.Status == 0)
                        {
                            var name = Enum.GetName(typeof(ExpensePaymentStatus), ExpensePaymentStatus.Partly_Paid);
                            if (
                                name != null)
                                myBusinessObj.PaymentStatus = name.Replace("_", " ");
                        }

                        if (myBusinessObj.Status == 1)
                        {
                            var name = Enum.GetName(typeof(ExpensePaymentStatus), ExpensePaymentStatus.Fully_Paid);
                            if (
                                name != null)
                                myBusinessObj.PaymentStatus = name.Replace("_", " ");
                        }
                        monthlyTransactionList.Add(myBusinessObj);
                    }

                    //Parallel.ForEach(entityList, item =>
                    //{
                    //    var myBusinessObj = ExpenseTransactionPaymentMapper.Map<ExpenseTransactionPayment, BusinessObject.ExpenseTransactionPayment>(item);
                        
                    //    if (myBusinessObj.Status == 0)
                    //    {
                    //        var name = Enum.GetName(typeof(ExpensePaymentStatus), ExpensePaymentStatus.Partly_Paid);
                    //        if (
                    //            name != null)
                    //            myBusinessObj.PaymentStatus = name.Replace("_", " ");
                    //    }

                    //    if (myBusinessObj.Status == 1)
                    //    {
                    //        var name = Enum.GetName(typeof(ExpensePaymentStatus), ExpensePaymentStatus.Fully_Paid);
                    //        if (
                    //            name != null)
                    //            myBusinessObj.PaymentStatus = name.Replace("_", " ");
                    //    }
                    //    monthlyTransactionList.Add(myBusinessObj);

                    //});


                    return !monthlyTransactionList.Any() ? new List<BusinessObject.ExpenseTransactionPayment>() : monthlyTransactionList.OrderByDescending(m => Convert.ToDateTime(m.LastPaymentDate)).ToList();
                }

            }
            catch (Exception ex)
            {
                ErrorManager.LogApplicationError(ex.StackTrace, ex.Source, ex.Message);
                return new List<BusinessObject.ExpenseTransactionPayment>();
            }
        }

        public List<BusinessObject.ExpenseTransactionPayment> GetTransactionPaymentsByDateRange(string startDate, string endDate, int deptId, int status)
        {
            try
            {
                using (var db = new ExpenseManagerDBEntities())
                {
                    var objList = new List<ExpenseTransactionPayment>();

                    var expenseTransactionList = new List<BusinessObject.ExpenseTransactionPayment>();

                    if (status == 0)
                    {
                        objList = db.ExpenseTransactionPayments.ToList().FindAll(m => m.Beneficiary.DepartmentId == deptId && (String.CompareOrdinal(m.LastPaymentDate, startDate) >= 0) &&
                        (String.CompareOrdinal(m.LastPaymentDate, endDate) <= 0)).ToList();
                    }

                    if (status == 1)
                    {
                        objList = db.ExpenseTransactionPayments.ToList().FindAll(m => m.Beneficiary.DepartmentId == deptId && (String.CompareOrdinal(m.LastPaymentDate, startDate) >= 0) &&
                        (String.CompareOrdinal(m.LastPaymentDate, endDate) <= 0) &&m.ExpenseTransaction.Status == 0).ToList();
                    }

                    if (status == 2)
                    {
                        objList = db.ExpenseTransactionPayments.ToList().FindAll(m => m.Beneficiary.DepartmentId == deptId && (String.CompareOrdinal(m.LastPaymentDate, startDate) >= 0) &&
                        (String.CompareOrdinal(m.LastPaymentDate, endDate) <= 0) &&m.ExpenseTransaction.Status == 1).ToList();
                    }

                    if (status == 3)
                    {
                        objList = db.ExpenseTransactionPayments.ToList().FindAll(m => m.Beneficiary.DepartmentId == deptId && (String.CompareOrdinal(m.LastPaymentDate, startDate) >= 0) &&
                        (String.CompareOrdinal(m.LastPaymentDate, endDate) <= 0) &&m.ExpenseTransaction.Status == 2).ToList();
                    }

                    if (status == 4)
                    {
                        objList = db.ExpenseTransactionPayments.ToList().FindAll(m => m.Beneficiary.DepartmentId == deptId && (String.CompareOrdinal(m.LastPaymentDate, startDate) >= 0) &&
                        (String.CompareOrdinal(m.LastPaymentDate, endDate) <= 0) &&m.ExpenseTransaction.Status == 3).ToList();
                    }

                    if (!objList.Any())
                    {
                        return expenseTransactionList;
                    }

                    foreach (var item in objList)
                    {
                        var myBusinessObj = ExpenseTransactionPaymentMapper.Map<ExpenseTransactionPayment, BusinessObject.ExpenseTransactionPayment>(item);

                        if (myBusinessObj.Status == 0)
                        {
                            var name = Enum.GetName(typeof(ExpensePaymentStatus), ExpensePaymentStatus.Partly_Paid);
                            if (
                                name != null)
                                myBusinessObj.PaymentStatus = name.Replace("_", " ");
                        }

                        if (myBusinessObj.Status == 1)
                        {
                            var name = Enum.GetName(typeof(ExpensePaymentStatus), ExpensePaymentStatus.Fully_Paid);
                            if (
                                name != null)
                                myBusinessObj.PaymentStatus = name.Replace("_", " ");
                        }
                        expenseTransactionList.Add(myBusinessObj);
                    }

                    ////Re-Map each Entity Object to Business Object
                    //Parallel.ForEach(objList, item =>
                    //{
                    //    var myBusinessObj = ExpenseTransactionPaymentMapper.Map<ExpenseTransactionPayment, BusinessObject.ExpenseTransactionPayment>(item);
                        
                    //    if (myBusinessObj.Status == 0)
                    //    {
                    //        var name = Enum.GetName(typeof(ExpensePaymentStatus), ExpensePaymentStatus.Partly_Paid);
                    //        if (
                    //            name != null)
                    //            myBusinessObj.PaymentStatus = name.Replace("_", " ");
                    //    }

                    //    if (myBusinessObj.Status == 1)
                    //    {
                    //        var name = Enum.GetName(typeof(ExpensePaymentStatus), ExpensePaymentStatus.Fully_Paid);
                    //        if (
                    //            name != null)
                    //            myBusinessObj.PaymentStatus = name.Replace("_", " ");
                    //    }
                    //    expenseTransactionList.Add(myBusinessObj);

                    //});

                    if (!expenseTransactionList.Any())
                    {
                        return new List<BusinessObject.ExpenseTransactionPayment>();
                    }
                    return expenseTransactionList.OrderByDescending(m => m.LastPaymentDate).ToList();
                }
            }
            catch (Exception ex)
            {
                ErrorManager.LogApplicationError(ex.StackTrace, ex.Source, ex.Message);
                return new List<BusinessObject.ExpenseTransactionPayment>();
            }
        }
	}
    
	//Class File Generated from Code<->Stripper 1.2.0.0 | All Rights Reserved
}
