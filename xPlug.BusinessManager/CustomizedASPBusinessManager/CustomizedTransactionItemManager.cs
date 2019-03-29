using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using ExpenseManager.EF;
using XPLUG.WEBTOOLS;
using xPlug.BusinessObjectMapper;

namespace xPlug.BusinessManager
{
	/// Author:	 Oluwaseyi Adegboyega - sadegboyega@xplugng.com
	/// Copyright © 2013. xPlug Technologies Limited. All Rights Reserved
	///*******************************************************************************
	///* Date Created: 	04-06-2013
	///* Date Generated:	25-11-2013 09:26:28
	///*******************************************************************************


	public partial class TransactionItemManager
	{
        public int AddTransactionItems(List<BusinessObject.TransactionItem> transactionItems, BusinessObject.ExpenseTransaction expenseTransaction)
        {
            try
            {
                //Re-Map Object to Entity Object
                double totalPrice = 0;
                if(!transactionItems.Any())
                {
                    return -1;
                }
                using (var db = new ExpenseManagerDBEntities())
                {
                    if (transactionItems.Any(transactionItem => db.ExpenseTransactions.Count(m =>
                        m.TransactionDate == expenseTransaction.TransactionDate && m.Status == 0 && m.BeneficiaryId == expenseTransaction.BeneficiaryId && m.TransactionItems.Count(x => x.ExpensenseItemId == transactionItem.ExpensenseItemId) > 0) > 0))
                    {
                        return -3;
                    }
                    var transactionEntityObj = ExpenseTransactionMapper.Map<BusinessObject.ExpenseTransaction, ExpenseTransaction>(expenseTransaction);
                    if (transactionEntityObj == null)
                    {
                        return -2;
                    }
                    db.AddToExpenseTransactions(transactionEntityObj);
                    db.SaveChanges();
                    expenseTransaction.ExpenseTransactionId = transactionEntityObj.ExpenseTransactionId;
                    if (expenseTransaction.ExpenseTransactionId < 1)
                    {
                        return -4;
                    }
                    foreach (var transactionItem in transactionItems)
                    {
                        transactionItem.ApprovedQuantity = 0;
                        transactionItem.ApprovedTotalPrice = 0;
                        transactionItem.ApprovedUnitPrice = 0;
                        totalPrice += (transactionItem.RequestedQuantity*transactionItem.RequestedUnitPrice);
                        transactionItem.ExpenseTransactionId = expenseTransaction.ExpenseTransactionId;
                        var myEntityObj = TransactionItemMapper.Map<BusinessObject.TransactionItem, TransactionItem>(transactionItem);
                        if (myEntityObj == null)
                        {
                            return -5;
                        }
                        db.AddToTransactionItems(myEntityObj);
                        db.SaveChanges();
                        transactionItem.TransactionItemId = myEntityObj.TransactionItemId;
                   }

                    var transactionEntityToUpdate = db.ExpenseTransactions.SingleOrDefault(m => m.ExpenseTransactionId == expenseTransaction.ExpenseTransactionId);
                    
                    if (transactionEntityToUpdate == null)
                    {
                        return -6;
                    }
                    transactionEntityToUpdate.TotalTransactionAmount = totalPrice;
                    db.ObjectStateManager.ChangeObjectState(transactionEntityToUpdate, EntityState.Modified);
                    db.SaveChanges();

               }

                return 1;
            }
            catch (Exception ex)
            {
                ErrorManager.LogApplicationError(ex.StackTrace, ex.Source, ex.Message);
                return 0;
            }
        }
        public Dictionary<List<BusinessObject.TransactionItem>, List<BusinessObject.ExpenseItem>> GetTransactionItemsByExpenseTransaction(long expenseTransactionId)
        {
            try
            {
                using (var db = new ExpenseManagerDBEntities())
                {
                    var myObjList = db.TransactionItems.Where(m => m.ExpenseTransactionId == expenseTransactionId).ToList();
                    var myBusinessObjList = new List<BusinessObject.TransactionItem>();
                    var dictCollection = new Dictionary<List<BusinessObject.TransactionItem>, List<BusinessObject.ExpenseItem>>();
                    var expenseItems = new List<BusinessObject.ExpenseItem>();
                    if (!myObjList.Any())
                    {
                        return dictCollection;
                    }

                    //Re-Map each Entity Object to Business Object
                    foreach (var item in myObjList)
                    {
                        var myBusinessObj = TransactionItemMapper.Map<TransactionItem, BusinessObject.TransactionItem>(item);
                        if (myBusinessObj == null)
                        {
                            continue;
                        }

                        var transItem = item;
                        var expenseItem = (from x in db.ExpenseItems
                                          where x.ExpenseItemId == transItem.ExpensenseItemId
                                          select x).ToList();

                        if (!expenseItem.Any())
                        {
                            return dictCollection;
                        }
                        var itemObj = ExpenseItemMapper.Map<ExpenseItem, BusinessObject.ExpenseItem>(expenseItem.ElementAt(0));
                        myBusinessObj.TotalPrice = myBusinessObj.RequestedQuantity * myBusinessObj.RequestedUnitPrice;
                        myBusinessObj.ApprovedTotalPrice = 0;
                        expenseItems.Add(itemObj);
                        myBusinessObjList.Add(myBusinessObj);
                    }

                    if (!expenseItems.Any() || !myBusinessObjList.Any())
                    {
                        return dictCollection;
                    }

                   var orderTransactionItems = myBusinessObjList.OrderBy(m => m.ExpenseItem.Title).ToList();
                   var orderedExpenseItems = expenseItems.OrderBy(m => m.Title).ToList();
                   dictCollection.Add(orderTransactionItems, orderedExpenseItems);
                   return dictCollection;
                }
            }
            catch (Exception ex)
            {
                ErrorManager.LogApplicationError(ex.StackTrace, ex.Source, ex.Message);
                return new Dictionary<List<BusinessObject.TransactionItem>, List<BusinessObject.ExpenseItem>>(); 
            }
        }
        public BusinessObject.ExpenseTransaction UpdateTransactionItemAndTotalAmount(BusinessObject.TransactionItem transactionItem)
        {
            try
            {
                //Re-Map Object to Entity Object
                var myEntityObj = TransactionItemMapper.Map<BusinessObject.TransactionItem, TransactionItem>(transactionItem);
                if (myEntityObj == null)
                {
                    return new BusinessObject.ExpenseTransaction();
                }
                using (var db = new ExpenseManagerDBEntities())
                {

                    db.TransactionItems.Attach(myEntityObj);
                    db.ObjectStateManager.ChangeObjectState(myEntityObj, EntityState.Modified);
                    db.SaveChanges();
                    var transactionAmount = db.TransactionItems.Where(m => m.ExpenseTransactionId == myEntityObj.ExpenseTransactionId).Sum(m => m.RequestedQuantity * m.RequestedUnitPrice);
                    var transaction = db.ExpenseTransactions.Single(m => m.ExpenseTransactionId == myEntityObj.ExpenseTransactionId);
                    transaction.TotalTransactionAmount = transactionAmount;
                    if (transaction.Status == 2)
                    {
                        transaction.Status = 0;
                    }
                    db.ObjectStateManager.ChangeObjectState(myEntityObj, EntityState.Modified);
                    db.SaveChanges();
                    var myBusinessObj = ExpenseTransactionMapper.Map<ExpenseTransaction, BusinessObject.ExpenseTransaction>(transaction);
                    return myBusinessObj;
                }
            }
            catch (Exception ex)
            {
                ErrorManager.LogApplicationError(ex.StackTrace, ex.Source, ex.Message);
                return new BusinessObject.ExpenseTransaction();
            }
        }
        public bool UpdateTransactionAndItems(BusinessObject.ExpenseTransaction transaction, List<BusinessObject.TransactionItem> updatedTransactionItemList)
        {
            try
            {
                //Re-Map Object to Entity Object

                double approvedTotalPrice = 0;
                using (var db = new ExpenseManagerDBEntities())
                {
                   
                    foreach (var item in updatedTransactionItemList)
                    {
                        var myEntityObj = TransactionItemMapper.Map<BusinessObject.TransactionItem, TransactionItem>(item);
                        if (myEntityObj == null)
                        {
                            continue;
                        }

                        myEntityObj.Status = 1;

                        if(myEntityObj.ApprovedQuantity < 1)
                        {
                            myEntityObj.ApprovedQuantity = myEntityObj.RequestedQuantity;
                        }
                        
                        if (myEntityObj.ApprovedUnitPrice < 1)
                        {
                            myEntityObj.ApprovedUnitPrice = myEntityObj.RequestedUnitPrice;
                        }
                        
                        myEntityObj.ApprovedTotalPrice = myEntityObj.ApprovedQuantity * myEntityObj.ApprovedUnitPrice;
                        approvedTotalPrice += myEntityObj.ApprovedTotalPrice;
                        db.TransactionItems.Attach(myEntityObj);
                        db.ObjectStateManager.ChangeObjectState(myEntityObj, EntityState.Modified);
                        db.SaveChanges();
                    }

                    var transactionEntityToUpdate = ExpenseTransactionMapper.Map<BusinessObject.ExpenseTransaction, ExpenseTransaction>(transaction);
                    transactionEntityToUpdate.TotalApprovedAmount = approvedTotalPrice;
                    db.ExpenseTransactions.Attach(transactionEntityToUpdate);
                    db.ObjectStateManager.ChangeObjectState(transactionEntityToUpdate, EntityState.Modified);
                    db.SaveChanges();
                }
                return true;
            }
            catch (Exception ex)
            {
                ErrorManager.LogApplicationError(ex.StackTrace, ex.Source, ex.Message);
                return false;
            }
        }

        public bool UpdateVoidedTransactionItemsAndTransaction(BusinessObject.ExpenseTransaction transaction, List<BusinessObject.TransactionItem> voidedTransactionItemList)
        {
            try
            {
                //Re-Map Object to Entity Object

                double approvedTotalPrice = 0;
                using (var db = new ExpenseManagerDBEntities())
                {
                    foreach (var item in voidedTransactionItemList)
                    {
                        var myEntityObj = TransactionItemMapper.Map<BusinessObject.TransactionItem, TransactionItem>(item);
                        if (myEntityObj == null)
                        {
                            continue;
                        }

                        myEntityObj.Status = 0;

                        if (myEntityObj.ApprovedQuantity > 1 && myEntityObj.ApprovedUnitPrice > 1)
                        {
                            myEntityObj.ApprovedTotalPrice = myEntityObj.ApprovedQuantity * myEntityObj.ApprovedUnitPrice;
                        }

                        approvedTotalPrice += myEntityObj.ApprovedTotalPrice;
                        db.TransactionItems.Attach(myEntityObj);
                        db.ObjectStateManager.ChangeObjectState(myEntityObj, EntityState.Modified);
                        db.SaveChanges();
                    }

                    var transactionEntityToUpdate = ExpenseTransactionMapper.Map<BusinessObject.ExpenseTransaction, ExpenseTransaction>(transaction);
                    transactionEntityToUpdate.TotalApprovedAmount = transactionEntityToUpdate.TotalApprovedAmount - approvedTotalPrice;
                    db.ExpenseTransactions.Attach(transactionEntityToUpdate);
                    db.ObjectStateManager.ChangeObjectState(transactionEntityToUpdate, EntityState.Modified);
                    db.SaveChanges();
                }
                return true;
            }
            catch (Exception ex)
            {
                ErrorManager.LogApplicationError(ex.StackTrace, ex.Source, ex.Message);
                return false;
            }
        }

        public bool RevertChanges(List<BusinessObject.TransactionItem> transactionItems)
        {
            try
            {
                //Re-Map each Object to Entity Object
                
                if (transactionItems == null)
                {
                    return false;
                }

                using (var db = new ExpenseManagerDBEntities())
                {
                    foreach (var item in transactionItems)
                    {
                        var myEntityObj = TransactionItemMapper.Map<BusinessObject.TransactionItem, TransactionItem>(item);

                        if (myEntityObj == null)
                        {
                            continue;
                        }
                        myEntityObj.ApprovedQuantity = 0;
                        myEntityObj.ApprovedUnitPrice = 0;
                        myEntityObj.ApprovedTotalPrice = 0;
                        db.TransactionItems.Attach(myEntityObj);
                        db.ObjectStateManager.ChangeObjectState(myEntityObj, EntityState.Modified);
                        db.SaveChanges();
                    }
                    return true;
                }
            }
            catch (Exception ex)
            {
                ErrorManager.LogApplicationError(ex.StackTrace, ex.Source, ex.Message);
                return false;
            }
        }
        public BusinessObject.ExpenseTransaction DeleteTransactionItemUpdateTotalAmount(int transactionItemId)
        {
            try
            {
                using (var db = new ExpenseManagerDBEntities())
                {

                    var myObj = db.TransactionItems.Single(s => s.TransactionItemId == transactionItemId);
                    
                    if (myObj == null)
                    {
                        return new BusinessObject.ExpenseTransaction();
                    }
                    db.TransactionItems.DeleteObject(myObj);
                    db.SaveChanges();
                    var transactionAmount = db.TransactionItems.Where(m => m.ExpenseTransactionId == myObj.ExpenseTransactionId).Sum(m => m.RequestedQuantity * m.RequestedUnitPrice);
                    var transaction = db.ExpenseTransactions.Single(m => m.ExpenseTransactionId == myObj.ExpenseTransactionId);
                    transaction.TotalTransactionAmount = transactionAmount;
                    if(transaction.Status == 2)
                    {
                        transaction.Status = 0;
                    }
                    db.ObjectStateManager.ChangeObjectState(transaction, EntityState.Modified);
                    db.SaveChanges();
                    var myBusinessObj = ExpenseTransactionMapper.Map<ExpenseTransaction, BusinessObject.ExpenseTransaction>(transaction);
                    return myBusinessObj;
                }
            }
            catch (Exception ex)
            {
                ErrorManager.LogApplicationError(ex.StackTrace, ex.Source, ex.Message);
                return new BusinessObject.ExpenseTransaction();
            }
        }
        public bool DeleteTransactionAndItem(int transactionItemId)
        {
            try
            {
                using (var db = new ExpenseManagerDBEntities())
                {
                    var myObj = db.TransactionItems.Single(s => s.TransactionItemId == transactionItemId);

                    if (myObj == null)
                    {
                        return false;
                    }
                    db.TransactionItems.DeleteObject(myObj);
                    db.SaveChanges();
                    var transaction = db.ExpenseTransactions.Single(m => m.ExpenseTransactionId == myObj.ExpenseTransactionId);
                    db.ExpenseTransactions.DeleteObject(transaction);
                    db.SaveChanges();
                    return true;
                }
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
                using (var db = new ExpenseManagerDBEntities())
                {
                    var myObj = db.TransactionItems.Where(s => s.TransactionItemId == expenseTransactionId).ToList();

                    if (!myObj.Any())
                    {
                        return false;
                    }
                    foreach (var transactionItem in myObj)
                    {
                        db.TransactionItems.DeleteObject(transactionItem);
                        db.SaveChanges();
                    }
                    
                    var transaction = db.ExpenseTransactions.Single(m => m.ExpenseTransactionId == expenseTransactionId);
                    db.ExpenseTransactions.DeleteObject(transaction);
                    db.SaveChanges();
                    return true;
                }
            }
            catch (Exception ex)
            {
                ErrorManager.LogApplicationError(ex.StackTrace, ex.Source, ex.Message);
                return false;
            }
        }
        public BusinessObject.ExpenseTransaction UpdateTransactionItemAndApprovedTotalAmount(BusinessObject.TransactionItem transactionItem)
        {
            try
            {
                //Re-Map Object to Entity Object
                var myEntityObj = TransactionItemMapper.Map<BusinessObject.TransactionItem, TransactionItem>(transactionItem);
                if (myEntityObj == null)
                {
                    return new BusinessObject.ExpenseTransaction();
                }
                using (var db = new ExpenseManagerDBEntities())
                {

                    db.TransactionItems.Attach(myEntityObj);
                    db.ObjectStateManager.ChangeObjectState(myEntityObj, EntityState.Modified);
                    db.SaveChanges();
                    var transactionAmount = db.TransactionItems.Where(m => m.ExpenseTransactionId == myEntityObj.ExpenseTransactionId).Sum(m => m.RequestedQuantity * m.RequestedUnitPrice);
                    var transaction = db.ExpenseTransactions.Single(m => m.ExpenseTransactionId == myEntityObj.ExpenseTransactionId);
                    transaction.TotalTransactionAmount = transactionAmount;
                    db.ExpenseTransactions.Attach(transaction);
                    db.ObjectStateManager.ChangeObjectState(myEntityObj, EntityState.Modified);
                    db.SaveChanges();
                    var myBusinessObj = ExpenseTransactionMapper.Map<ExpenseTransaction, BusinessObject.ExpenseTransaction>(transaction);
                    return myBusinessObj;
                }
            }
            catch (Exception ex)
            {
                ErrorManager.LogApplicationError(ex.StackTrace, ex.Source, ex.Message);
                return new BusinessObject.ExpenseTransaction();
            }
        }
        public List<BusinessObject.TransactionItem> GetApprovedTransactionItemsByExpenseTransaction(long expenseTransactionId)
        {
            try
            {
                using (var db = new ExpenseManagerDBEntities())
                {
                    var myObjList = db.TransactionItems.Where(m => m.ExpenseTransactionId == expenseTransactionId && m.ApprovedQuantity > 0 && m.ApprovedUnitPrice > 0 && m.ApprovedTotalPrice > 0).ToList();
                    var myBusinessObjList = new List<BusinessObject.TransactionItem>();
                    if (!myObjList.Any())
                    {
                        return myBusinessObjList;
                    }
                    //Re-Map each Entity Object to Business Object
                    foreach (var item in myObjList)
                    {
                        var myBusinessObj = TransactionItemMapper.Map<TransactionItem, BusinessObject.TransactionItem>(item);
                        if (myBusinessObj == null)
                        {
                            continue;
                        }
                        myBusinessObj.TotalPrice = myBusinessObj.RequestedQuantity * myBusinessObj.RequestedUnitPrice;
                        myBusinessObjList.Add(myBusinessObj);
                    }
                    return myBusinessObjList.OrderBy(m => m.ExpenseItem.Title).ToList();
                }
            }
            catch (Exception ex)
            {
                ErrorManager.LogApplicationError(ex.StackTrace, ex.Source, ex.Message);
                return new List<BusinessObject.TransactionItem>();
            }
        }
        public bool UpdatePendingTransactionAndItem(BusinessObject.ExpenseTransaction transaction, BusinessObject.TransactionItem updatedTransactionItem)
        {
            try
            {
                //Re-Map Object to Entity Object

                using (var db = new ExpenseManagerDBEntities())
                {
                    var myEntityObj = TransactionItemMapper.Map<BusinessObject.TransactionItem, TransactionItem>(updatedTransactionItem);
                    if (myEntityObj == null)
                    {
                       return false;
                    }

                    db.TransactionItems.Attach(myEntityObj);
                    db.ObjectStateManager.ChangeObjectState(myEntityObj, EntityState.Modified);
                    db.SaveChanges();

                    var totalPrice = db.TransactionItems.Where(x => x.ExpenseTransactionId == transaction.ExpenseTransactionId).ToList().Sum(m => m.RequestedQuantity * m.RequestedUnitPrice);
                    transaction.TotalTransactionAmount = totalPrice;
                    var transactionEntityToUpdate = ExpenseTransactionMapper.Map<BusinessObject.ExpenseTransaction, ExpenseTransaction>(transaction);
                    db.ExpenseTransactions.Attach(transactionEntityToUpdate);
                    db.ObjectStateManager.ChangeObjectState(transactionEntityToUpdate, EntityState.Modified);
                    db.SaveChanges();
                }
                return true;
            }
            catch (Exception ex)
            {
                ErrorManager.LogApplicationError(ex.StackTrace, ex.Source, ex.Message);
                return false;
            }
        }
        public bool ModifyPendingTransactionAndItem(BusinessObject.ExpenseTransaction transaction, int transactionItemId)
        {
            try
            {
                //Re-Map Object to Entity Object

                using (var db = new ExpenseManagerDBEntities())
                {
                    var myObj = db.TransactionItems.Single(s => s.TransactionItemId == transactionItemId);

                    if (myObj == null)
                    {
                        return false;
                    }
                    db.TransactionItems.DeleteObject(myObj);
                    db.SaveChanges();

                    if (db.TransactionItems.Any())
                    {
                        var totalPrice = db.TransactionItems.Where(x => x.ExpenseTransactionId == transaction.ExpenseTransactionId).ToList().Sum(m => m.RequestedQuantity * m.RequestedUnitPrice);
                        transaction.TotalTransactionAmount = totalPrice;
                    }
                    
                    var transactionEntityToUpdate = ExpenseTransactionMapper.Map<BusinessObject.ExpenseTransaction, ExpenseTransaction>(transaction);
                    db.ExpenseTransactions.Attach(transactionEntityToUpdate);
                    db.ObjectStateManager.ChangeObjectState(transactionEntityToUpdate, EntityState.Modified);
                    db.SaveChanges();
                }
                return true;
            }
            catch (Exception ex)
            {
                ErrorManager.LogApplicationError(ex.StackTrace, ex.Source, ex.Message);
                return false;
            }
        }

        public List<BusinessObject.TransactionItem> GetExpenseItemCostsByDateRange(int expenseItemId, string startDate, string endDate)
        {
            try
            {
                using (var db = new ExpenseManagerDBEntities())
                {
                    var myObjList = db.TransactionItems.Where(m => m.ExpensenseItemId == expenseItemId).ToList().FindAll(m => (String.CompareOrdinal(m.ExpenseTransaction.DateApproved, startDate) >= 0) &&
                       (String.CompareOrdinal(m.ExpenseTransaction.DateApproved, endDate) <= 0)).ToList();
                    var transactionItems = new List<BusinessObject.TransactionItem>();
                    if (!myObjList.Any())
                    {
                        return transactionItems;
                    }

                    double grandTotal = 0;
                    //Re-Map each Entity Object to Business Object
                    foreach (var item in myObjList)
                    {
                        if (!transactionItems.Exists(m => m.ExpensenseItemId == item.ExpensenseItemId && m.ExpenseTransaction.DateApproved == item.ExpenseTransaction.DateApproved))
                        {
                            var myBusinessObj = TransactionItemMapper.Map<TransactionItem, BusinessObject.TransactionItem>(item);
                            if (myBusinessObj == null)
                            {
                                continue;
                            }

                            var tempList =
                                myObjList.FindAll(
                                    x =>
                                        x.ExpensenseItemId == myBusinessObj.ExpensenseItemId &&
                                        x.ExpenseTransaction.DateApproved == myBusinessObj.ExpenseTransaction.DateApproved);
                            
                            myBusinessObj.TotalApprovedPrice = tempList.Sum(m => m.ApprovedTotalPrice);
                            myBusinessObj.TotalApprovedQuantity = tempList.Sum(m => m.ApprovedQuantity);
                            grandTotal += myBusinessObj.TotalApprovedPrice;
                            transactionItems.Add(myBusinessObj);
                        }
                    }
                    if (!transactionItems.Any())
                    {
                        return new List<BusinessObject.TransactionItem>();
                    }
                    Parallel.ForEach(transactionItems, m => { m.GrandTotalApprovedPrice = grandTotal; });
                    return transactionItems.OrderByDescending(m => DateTime.Parse(m.ExpenseTransaction.TransactionDate)).ToList();
                }
            }
            catch (Exception ex)
            {
                ErrorManager.LogApplicationError(ex.StackTrace, ex.Source, ex.Message);
                return new List<BusinessObject.TransactionItem>();
            }
        }

        public List<BusinessObject.TransactionItem> GetAccountsHeadsCostsByDateRange(int accountHeadId, string startDate, string endDate)
        {
            try
            {
                using (var db = new ExpenseManagerDBEntities())
                {
                    var myObjList = db.TransactionItems.Where(m => m.ExpenseItem.AccountsHeadId == accountHeadId).ToList().FindAll(m => (String.CompareOrdinal(m.ExpenseTransaction.DateApproved, startDate) >= 0) &&
                       (String.CompareOrdinal(m.ExpenseTransaction.DateApproved, endDate) <= 0)).ToList();
                    var accountsHeadsCosts = new List<BusinessObject.TransactionItem>();
                    if (!myObjList.Any())
                    {
                        return accountsHeadsCosts;
                    }

                    double grandTotal = 0;
                    //Re-Map each Entity Object to Business Object
                    foreach (var item in myObjList)
                    {

                        if (!accountsHeadsCosts.Exists(m => m.ExpenseItem.AccountsHeadId == item.ExpenseItem.AccountsHeadId && m.ExpenseTransaction.DateApproved == item.ExpenseTransaction.DateApproved))
                        {
                            var myBusinessObj = TransactionItemMapper.Map<TransactionItem, BusinessObject.TransactionItem>(item);
                            if (myBusinessObj == null)
                            {
                                continue;
                            }

                            var tempList =
                                myObjList.FindAll(
                                    x =>
                                        x.ExpenseItem.AccountsHeadId == myBusinessObj.ExpenseItem.AccountsHeadId &&
                                        x.ExpenseTransaction.DateApproved == myBusinessObj.ExpenseTransaction.DateApproved);

                            myBusinessObj.TotalApprovedPrice = tempList.Sum(m => m.ApprovedTotalPrice);
                            grandTotal += myBusinessObj.TotalApprovedPrice;
                            accountsHeadsCosts.Add(myBusinessObj);
                        }
                    }
                    if (!accountsHeadsCosts.Any())
                    {
                        return new List<BusinessObject.TransactionItem>();
                    }
                    Parallel.ForEach(accountsHeadsCosts, m => { m.GrandTotalApprovedPrice = grandTotal; });
                    return accountsHeadsCosts.OrderByDescending(m => DateTime.Parse(m.ExpenseTransaction.TransactionDate)).ToList();
                }
            }
            catch (Exception ex)
            {
                ErrorManager.LogApplicationError(ex.StackTrace, ex.Source, ex.Message);
                return new List<BusinessObject.TransactionItem>();
            }
        }

        public List<List<BusinessObject.TransactionItem>> GetItemsByDateRange(long expenseTransactionId)
        {
            try
            {
                using (var db = new ExpenseManagerDBEntities())
                {
                    var myObjList = db.TransactionItems.Where(m => m.ExpenseTransaction.ExpenseTransactionId == expenseTransactionId).ToList();
                    var itemsCollection = new List<List<BusinessObject.TransactionItem>>();
                    if (!myObjList.Any())
                    {
                        return itemsCollection;
                    }

                    
                    //Re-Map each Entity Object to Business Object
                    foreach (var item in myObjList)
                    {

                        if (!itemsCollection.Exists(m => m.All(r => r.ExpensenseItemId == item.ExpensenseItemId && r.ExpenseTransaction.DateApproved == item.ExpenseTransaction.DateApproved)))
                        {

                            var tempList = myObjList.FindAll(x => x.ExpensenseItemId == item.ExpensenseItemId);
                            if (!tempList.Any())
                            continue;
                            
                            var tempList1 = new List<BusinessObject.TransactionItem>();
                           
                            foreach (var transactionItem in tempList)
                            {
                                var myBusinessObj = TransactionItemMapper.Map<TransactionItem, BusinessObject.TransactionItem>(transactionItem);
                               
                                if (myBusinessObj == null)
                                {
                                    continue;
                                }

                                tempList1.Add(myBusinessObj);
                                
                            }
                            if (tempList.Any())
                            {
                                itemsCollection.Add(tempList1.OrderBy(m => m.ExpenseItem.Title).ToList());
                            }
                        }
                    }
                    if (!itemsCollection.Any())
                    {
                        return new List<List<BusinessObject.TransactionItem>>();
                    }
                    return itemsCollection;
                }
            }
            catch (Exception ex)
            {
                ErrorManager.LogApplicationError(ex.StackTrace, ex.Source, ex.Message);
                return new List<List<BusinessObject.TransactionItem>>();
            }
        }

        public List<BusinessObject.TransactionItem> GetSingleTransactionItems(long expenseTransactionId)
        {
            try
            {
                using (var db = new ExpenseManagerDBEntities())
                {
                    var myObjList = db.TransactionItems.Where(m => m.ExpenseTransactionId == expenseTransactionId).ToList();
                    var myBusinessObjList = new List<BusinessObject.TransactionItem>();
                    if (!myObjList.Any())
                    {
                        return myBusinessObjList;
                    }

                    foreach (var item in myObjList)
                    {
                        if (myBusinessObjList.Exists(m => m.ExpensenseItemId == item.ExpensenseItemId))
                        {
                            var jxc = myBusinessObjList.Find(m => m.ExpensenseItemId == item.ExpensenseItemId);
                            jxc.Ismultiple = true;
                            jxc.RequestedQuantity += item.RequestedQuantity;
                            jxc.RequestedUnitPrice += item.RequestedUnitPrice;
                            jxc.ApprovedQuantity += item.ApprovedQuantity;
                            jxc.ApprovedUnitPrice += item.ApprovedUnitPrice;
                            continue;
                        }
                        
                        var myBusinessObj = TransactionItemMapper.Map<TransactionItem, BusinessObject.TransactionItem>(item);
                        
                        if (myBusinessObj == null)
                        {
                            continue;
                        }

                        myBusinessObj.TotalPrice = myBusinessObj.RequestedQuantity * myBusinessObj.RequestedUnitPrice;
                        myBusinessObjList.Add(myBusinessObj);
                    }

                    if (!myBusinessObjList.Any())
                    {
                        return new List<BusinessObject.TransactionItem>();
                    }

                   return myBusinessObjList.OrderBy(m => m.ExpenseItem.Title).ToList();
                }
            }
            catch (Exception ex)
            {
                ErrorManager.LogApplicationError(ex.StackTrace, ex.Source, ex.Message);
                return new List<BusinessObject.TransactionItem>(); 
            }
        }

	    public List<BusinessObject.TransactionItem> GetDetailedTransactionItems(int transactionItemId)
	    {
            try
            {
                using (var db = new ExpenseManagerDBEntities())
                {
                    var myObj = db.TransactionItems.Where(m => m.TransactionItemId == transactionItemId).ToList();
                    var myBusinessObjList = new List<BusinessObject.TransactionItem>();
                    if (!myObj.Any())
                    {
                        return myBusinessObjList;
                    }

                    var gtx = myObj[0];

                    var myObjList = db.TransactionItems.Where(m => m.ExpenseTransactionId == gtx.ExpenseTransactionId && (m.ExpensenseItemId == gtx.ExpensenseItemId)).ToList();

                    if (!myObjList.Any())
                    {
                        return myBusinessObjList;
                    }

                    foreach (var item in myObjList)
                    {
                        var myBusinessObj = TransactionItemMapper.Map<TransactionItem, BusinessObject.TransactionItem>(item);

                        if (myBusinessObj == null)
                        {
                            continue;
                        }

                        //myBusinessObj.TotalPrice = myBusinessObj.RequestedQuantity * myBusinessObj.RequestedUnitPrice;
                        myBusinessObjList.Add(myBusinessObj);
                    }

                    if (!myBusinessObjList.Any())
                    {
                        return new List<BusinessObject.TransactionItem>();
                    }

                    return myBusinessObjList.OrderBy(m => m.ExpenseItem.Title).ToList();
                }
            }
            catch (Exception ex)
            {
                ErrorManager.LogApplicationError(ex.StackTrace, ex.Source, ex.Message);
                return new List<BusinessObject.TransactionItem>();
            }
	    }
	}


	//Class File Generated from Code<->Stripper 1.2.0.0 | All Rights Reserved
}
