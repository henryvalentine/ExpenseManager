using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using ExpenseManager.EF;
using XPLUG.WEBTOOLS;
using xPlug.BusinessObjectMapper;

namespace xPlug.BusinessManager
{
	/// Author:	 Oluwaseyi Adegboyega - sadegboyega@xplugng.com
	/// Copyright © 2013. xPlug Technologies Limited. All Rights Reserved
	///*******************************************************************************
	///* Date Created: 	04-06-2013
	///* Date Generated:	12-09-2013 10:06:32
	///*******************************************************************************


	public partial class ExpenseItemManager
	{
        public int AddExpenseItemCheckDuplicate(BusinessObject.ExpenseItem expenseItem)
		{
			try
			{
				//Re-Map Object to Entity Object
				var myEntityObj = ExpenseItemMapper.Map<xPlug.BusinessObject.ExpenseItem, ExpenseItem>(expenseItem);
				if(myEntityObj == null)
				{return -2;}
				using (var db = new ExpenseManagerDBEntities())
				{
                    if (db.ExpenseItems.Count(m => m.Title.ToLower().Replace(" ", string.Empty) == expenseItem.Title.ToLower().Replace(" ", string.Empty)) > 0)
                    {
                        return -3;
                    }
					db.AddToExpenseItems(myEntityObj);
					db.SaveChanges();
					expenseItem.ExpenseItemId = myEntityObj.ExpenseItemId;
					return expenseItem.ExpenseItemId;
				}
			}
			catch (Exception ex)
			{
				ErrorManager.LogApplicationError(ex.StackTrace, ex.Source, ex.Message);
				return 0;
			}
		}

        public int UpdateExpenseItemCheckDuplicate(BusinessObject.ExpenseItem expenseItem)
		{
			try
			{
				//Re-Map Object to Entity Object
				var myEntityObj = ExpenseItemMapper.Map<BusinessObject.ExpenseItem, ExpenseItem>(expenseItem);
				if(myEntityObj == null)
				{return -2;}
				using (var db = new ExpenseManagerDBEntities())
				{
                    if (db.ExpenseItems.Count(m => m.Title.ToLower().Replace(" ", string.Empty) == expenseItem.Title.ToLower().Replace(" ", string.Empty) && m.ExpenseItemId != expenseItem.ExpenseItemId) > 0)
                    {
                        return -3;
                    }
					db.ExpenseItems.Attach(myEntityObj);
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

		public bool DeleteExpenseItemCheckReference(int expenseItemId)
		{
			try
			{
				using (var db = new ExpenseManagerDBEntities())
				{
                    if(db.TransactionItems.Any(m => m.ExpensenseItemId == expenseItemId))
                    {
                        return false;
                    }
					var myObj = db.ExpenseItems.Single(s => s.ExpenseItemId == expenseItemId);
					if (myObj == null) { return false; };
					db.ExpenseItems.DeleteObject(myObj);
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

		public List<BusinessObject.ExpenseItem> GetAllOrderedExpenseItems()
		{
			try
			{
				using (var db = new ExpenseManagerDBEntities())
				{
					var myObjList = db.ExpenseItems.ToList();
					var myBusinessObjList = new List<BusinessObject.ExpenseItem>();
					if(!myObjList.Any())
					{
					    return myBusinessObjList;
					}
					//Re-Map each Entity Object to Business Object
					foreach (var item in myObjList)
					{
					var myBusinessObj = ExpenseItemMapper.Map<ExpenseItem, BusinessObject.ExpenseItem>(item);
						if(myBusinessObj == null){continue;}
						myBusinessObjList.Add(myBusinessObj);
					}
                    return myBusinessObjList.OrderBy(m => m.Title).ToList();
				}
			}
			catch (Exception ex)
			{
				ErrorManager.LogApplicationError(ex.StackTrace, ex.Source, ex.Message);
				return new List<BusinessObject.ExpenseItem>();
			}
		}

        public List<BusinessObject.ExpenseItem> GetActiveOrderedExpenseItems()
        {
            try
            {
                using (var db = new ExpenseManagerDBEntities())
                {
                    var myObjList = db.ExpenseItems.Where(m => m.Status == 1 && m.AccountsHead.Status == 1).ToList();
                    var myBusinessObjList = new List<BusinessObject.ExpenseItem>();
                    if (!myObjList.Any())
                    {
                        return myBusinessObjList;
                    }
                    //Re-Map each Entity Object to Business Object
                    foreach (var item in myObjList)
                    {
                        var myBusinessObj = ExpenseItemMapper.Map<ExpenseItem, BusinessObject.ExpenseItem>(item);
                        if (myBusinessObj == null) { continue; }
                        myBusinessObjList.Add(myBusinessObj);
                    }
                    return myBusinessObjList.OrderBy(m => m.AccountsHead.Title).ThenBy(m => m.Title).ToList();
                }
            }
            catch (Exception ex)
            {
                ErrorManager.LogApplicationError(ex.StackTrace, ex.Source, ex.Message);
                return new List<xPlug.BusinessObject.ExpenseItem>();
            }
        }

        public List<BusinessObject.ExpenseItem> GetLastInsertedExpenseItem(int accountHead)
        {  
            try
            {
                using (var db = new ExpenseManagerDBEntities())
                {
                    var itemList = db.ExpenseItems.Where(m => m.AccountsHeadId == accountHead).ToList();
                    
                    if (!itemList.Any())
                    {
                        return new List<BusinessObject.ExpenseItem>();
                    }
                    
                    var myObjList = itemList.OrderByDescending(m => long.Parse(m.Code)).Take(1).ToList();
                    
                    var myBusinessObjList = new List<BusinessObject.ExpenseItem>();
                    if (!myObjList.Any())
                    {
                        return myBusinessObjList;
                    }
                    //Re-Map each Entity Object to Business Object
                    foreach (var item in myObjList)
                    {
                        var myBusinessObj = ExpenseItemMapper.Map<ExpenseItem, BusinessObject.ExpenseItem>(item);
                        if (myBusinessObj == null) { continue; }
                        myBusinessObjList.Add(myBusinessObj);
                    }
                    return myBusinessObjList.OrderBy(m => m.AccountsHead.Title).ThenBy(m => m.Title).ToList();
                }
            }
            catch (Exception ex)
            {
                ErrorManager.LogApplicationError(ex.StackTrace, ex.Source, ex.Message);
                return new List<xPlug.BusinessObject.ExpenseItem>();
            }
        }

		public List<BusinessObject.ExpenseItem>  GetOrderedExpenseItemsByExpenseCategoryId(Int32 expenseCategoryId)
		{
			try
			{
				using (var db = new ExpenseManagerDBEntities())
				{
					var myObjList = db.ExpenseItems.Where(m => m.ExpenseCategoryId == expenseCategoryId).ToList();
					var myBusinessObjList = new List<BusinessObject.ExpenseItem>();
					if(!myObjList.Any())
					{
					    return myBusinessObjList;
					}
					//Re-Map each Entity Object to Business Object
					foreach (var item in myObjList)
					{
					var myBusinessObj = ExpenseItemMapper.Map<ExpenseItem, BusinessObject.ExpenseItem>(item);
						if(myBusinessObj == null){continue;}
						myBusinessObjList.Add(myBusinessObj);
					}
                    return myBusinessObjList.OrderBy(m => m.AccountsHead.Title).ThenBy(m => m.Title).ToList();
				}
			}
			catch (Exception ex)
			{
				ErrorManager.LogApplicationError(ex.StackTrace, ex.Source, ex.Message);
				return new List<xPlug.BusinessObject.ExpenseItem>();
			}
		}

		public List<BusinessObject.ExpenseItem>  GetOrderedExpenseItemsByAccountsHeadId(Int32 accountsHeadId)
		{
			try
			{
				using (var db = new ExpenseManagerDBEntities())
				{
					var myObjList = db.ExpenseItems.Where(m => m.AccountsHeadId == accountsHeadId).ToList();
					var myBusinessObjList = new List<BusinessObject.ExpenseItem>();
					if(!myObjList.Any())
					{
					    return myBusinessObjList;
					}
					//Re-Map each Entity Object to Business Object
					foreach (var item in myObjList)
					{
					var myBusinessObj = ExpenseItemMapper.Map<ExpenseItem, BusinessObject.ExpenseItem>(item);
						if(myBusinessObj == null){continue;}
						myBusinessObjList.Add(myBusinessObj);
					}
                    return myBusinessObjList.OrderBy(m => m.AccountsHead.Title).ThenBy(m => m.Title).ToList();
				}
			}
			catch (Exception ex)
			{
				ErrorManager.LogApplicationError(ex.StackTrace, ex.Source, ex.Message);
				return new List<BusinessObject.ExpenseItem>();
			}
		}
	}


	//Class File Generated from Code<->Stripper 1.2.0.0 | All Rights Reserved
}
