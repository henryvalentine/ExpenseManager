using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using ExpenseManager.EF;
using XPLUG.WEBTOOLS;
using xPlug.BusinessObjectMapper;
using AccountsHead = xPlug.BusinessObject.AccountsHead;

namespace xPlug.BusinessManager
{
	/// Author:	 Oluwaseyi Adegboyega - sadegboyega@xplugng.com
	/// Copyright © 2013. xPlug Technologies Limited. All Rights Reserved
	///*******************************************************************************
	///* Date Created: 	04-06-2013
	///* Date Generated:	12-09-2013 10:06:27
	///*******************************************************************************


	public partial class ExpenseCategoryManager
	{
        public List<BusinessObject.ExpenseCategory> GetFilteredExpenseCategories()
        {
            try
            {
                using (var db = new ExpenseManagerDBEntities())
                {
                    var myBusinessObjList = new List<xPlug.BusinessObject.ExpenseCategory>();
                    var expenseCategoryList = db.ExpenseCategories.Where(m => m.Status == 1 && m.AccountsHeads.Any()).ToList();
                    if (!expenseCategoryList.Any())
                    {
                        return myBusinessObjList;
                    }
                    //Re-Map each Entity Object to Business Object
                    //myBusinessObjList.AddRange(accountsHeadsObjList.Select(item => ExpenseCategoryMapper.Map<ExpenseCategory, xPlug.BusinessObject.ExpenseCategory>(item.ExpenseCategory)).Where(myBusinessObj => myBusinessObj != null));

                    foreach (var item in expenseCategoryList)
                    {
                        bool any = myBusinessObjList.Any(m => m.ExpenseCategoryId == item.ExpenseCategoryId);
                        if (!any)
                        {
                            var myBusinessObj = ExpenseCategoryMapper.Map<ExpenseCategory, xPlug.BusinessObject.ExpenseCategory>(item);
                            if (myBusinessObj == null)
                            {
                                continue;
                            }
                            myBusinessObjList.Add(myBusinessObj);
                        }
                    }

                    if (!myBusinessObjList.Any())
                    {
                        return myBusinessObjList;
                    }
                    return myBusinessObjList.OrderBy(m => m.Title).ToList();
                }
            }
            catch (Exception ex)
            {
                ErrorManager.LogApplicationError(ex.StackTrace, ex.Source, ex.Message);
                return new List<xPlug.BusinessObject.ExpenseCategory>();
            }
        }
        public List<BusinessObject.ExpenseCategory> GetAllActiveExpenseCategories()
        {
            try
            {
                using (var db = new ExpenseManagerDBEntities())
                {
                    var myBusinessObjList = new List<xPlug.BusinessObject.ExpenseCategory>();
                    var expenseCategoryList = db.ExpenseCategories.Where(m => m.Status == 1).ToList();
                    if (!expenseCategoryList.Any())
                    {
                        return myBusinessObjList;
                    }
                    //Re-Map each Entity Object to Business Object
                    //myBusinessObjList.AddRange(accountsHeadsObjList.Select(item => ExpenseCategoryMapper.Map<ExpenseCategory, xPlug.BusinessObject.ExpenseCategory>(item.ExpenseCategory)).Where(myBusinessObj => myBusinessObj != null));

                    foreach (var item in expenseCategoryList)
                    {
                        bool any = myBusinessObjList.Any(m => m.ExpenseCategoryId == item.ExpenseCategoryId);
                        if (!any)
                        {
                            var myBusinessObj = ExpenseCategoryMapper.Map<ExpenseCategory, xPlug.BusinessObject.ExpenseCategory>(item);
                            if (myBusinessObj == null)
                            {
                                continue;
                            }
                            myBusinessObjList.Add(myBusinessObj);
                        }
                    }

                    if (!myBusinessObjList.Any())
                    {
                        return myBusinessObjList;
                    }
                    return myBusinessObjList.OrderBy(m => m.Title).ToList();
                }
            }
            catch (Exception ex)
            {
                ErrorManager.LogApplicationError(ex.StackTrace, ex.Source, ex.Message);
                return new List<xPlug.BusinessObject.ExpenseCategory>();
            }
        }
        public int AddExpenseCategoryCheckDuplicate(BusinessObject.ExpenseCategory expenseCategory)
        {
            try
            {
                //Re-Map Object to Entity Object
                var myEntityObj = ExpenseCategoryMapper.Map<BusinessObject.ExpenseCategory, ExpenseCategory>(expenseCategory);
                if (myEntityObj == null)
                {
                    return -2;
                }
                using (var db = new ExpenseManagerDBEntities())
                {
                    if (db.ExpenseCategories.Count(m => m.Title.ToLower().Replace(" ", string.Empty) == expenseCategory.Title.ToLower().Replace(" ", string.Empty)) > 0)
                    {
                        return -3;
                    }
                    db.AddToExpenseCategories(myEntityObj);
                    db.SaveChanges();
                    expenseCategory.ExpenseCategoryId = myEntityObj.ExpenseCategoryId;
                    return expenseCategory.ExpenseCategoryId;
                }
            }
            catch (Exception ex)
            {
                ErrorManager.LogApplicationError(ex.StackTrace, ex.Source, ex.Message);
                return 0;
            }
        }
        public int UpdateExpenseCategoryCheckDuplicate(BusinessObject.ExpenseCategory expenseCategory)
        {
            try
            {
                //Re-Map Object to Entity Object
                var myEntityObj = ExpenseCategoryMapper.Map<BusinessObject.ExpenseCategory, ExpenseCategory>(expenseCategory);
                if (myEntityObj == null)
                {
                    return -2;
                }
                using (var db = new ExpenseManagerDBEntities())
                {
                    if (db.ExpenseCategories.Count(m => m.Title.ToLower().Replace(" ", string.Empty) == expenseCategory.Title.ToLower().Replace(" ", string.Empty) && m.ExpenseCategoryId != expenseCategory.ExpenseCategoryId) > 0)
                    {
                        return -3;
                    }
                    db.ExpenseCategories.Attach(myEntityObj);
                    db.ObjectStateManager.ChangeObjectState(myEntityObj, EntityState.Modified);
                    db.SaveChanges();
                    return 1;
                }
            }
            catch (Exception ex)
            {
                ErrorManager.LogApplicationError(ex.StackTrace, ex.Source, ex.Message);
                return 0;
            }
        }
        public List<BusinessObject.ExpenseCategory> GetOrderedExpenseCategories()
        {
            try
            {
                using (var db = new ExpenseManagerDBEntities())
                {
                    var myBusinessObjList = new List<BusinessObject.ExpenseCategory>();
                    var expenseCategoryList = db.ExpenseCategories.ToList();
                    if (!expenseCategoryList.Any())
                    {
                        return myBusinessObjList;
                    }
                    //Re-Map each Entity Object to Business Object
                    //myBusinessObjList.AddRange(accountsHeadsObjList.Select(item => ExpenseCategoryMapper.Map<ExpenseCategory, xPlug.BusinessObject.ExpenseCategory>(item.ExpenseCategory)).Where(myBusinessObj => myBusinessObj != null));

                    foreach (var item in expenseCategoryList)
                    {
                        bool any = myBusinessObjList.Any(m => m.ExpenseCategoryId == item.ExpenseCategoryId);
                        if (!any)
                        {
                            var myBusinessObj = ExpenseCategoryMapper.Map<ExpenseCategory, BusinessObject.ExpenseCategory>(item);
                            if (myBusinessObj == null)
                            {
                                continue;
                            }
                            myBusinessObjList.Add(myBusinessObj);
                        }
                    }

                    if (!myBusinessObjList.Any())
                    {
                        return myBusinessObjList;
                    }
                    return myBusinessObjList.OrderBy(m => long.Parse(m.Code)).ToList();
                }
            }
            catch (Exception ex)
            {
                ErrorManager.LogApplicationError(ex.StackTrace, ex.Source, ex.Message);
                return new List<BusinessObject.ExpenseCategory>();
            }
        }
	
    }
    
	//Class File Generated from Code<->Stripper 1.2.0.0 | All Rights Reserved
}
