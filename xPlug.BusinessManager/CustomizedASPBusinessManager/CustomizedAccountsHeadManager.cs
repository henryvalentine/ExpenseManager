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
	///* Date Generated:	12-09-2013 10:06:30
	///*******************************************************************************


	public partial class AccountsHeadManager
	{
        public List<BusinessObject.AccountsHead> GetFilteredAccountsHeads()
        {
            try
            {
                using (var db = new ExpenseManagerDBEntities())
                {
                    var myBusinessObjList = new List<BusinessObject.AccountsHead>();
                    var accountsHeadsObjList = db.AccountsHeads.Where(m => m.ExpenseCategory.Status == 1 && m.Status == 1 && m.ExpenseItems.Any()).ToList();
                    if (!accountsHeadsObjList.Any())
                    {
                        return myBusinessObjList;
                    }
                    //Re-Map each Entity Object to Business Object
                    foreach (var item in accountsHeadsObjList)
                    {
                        bool any = myBusinessObjList.Any(m => m.AccountsHeadId == item.AccountsHeadId);
                        if (!any)
                        {
                            var myBusinessObj = AccountsHeadMapper.Map<ExpenseManager.EF.AccountsHead, xPlug.BusinessObject.AccountsHead>(item);
                            if (myBusinessObj == null) { continue; }
                            myBusinessObjList.Add(myBusinessObj);
                        }
                    }

                    if (!myBusinessObjList.Any()) { return myBusinessObjList; }
                    return myBusinessObjList.OrderBy(m => m.Title).ToList();
                }
            }
            catch (Exception ex)
            {
                ErrorManager.LogApplicationError(ex.StackTrace, ex.Source, ex.Message);
                return new List<BusinessObject.AccountsHead>();
            }
        }

        public List<BusinessObject.AccountsHead> GetActiveOrderedAccountsHeads()
        {
            try
            {
                using (var db = new ExpenseManagerDBEntities())
                {
                    var myBusinessObjList = new List<BusinessObject.AccountsHead>();
                    var accountsHeadsObjList = db.AccountsHeads.Where(m => m.Status == 1 && m.ExpenseCategory.Status == 1).ToList();
                    if (!accountsHeadsObjList.Any())
                    {
                        return myBusinessObjList;
                    }
                    //Re-Map each Entity Object to Business Object
                    foreach (var item in accountsHeadsObjList)
                    {
                        var myBusinessObj = AccountsHeadMapper.Map<AccountsHead, BusinessObject.AccountsHead>(item);
                            if (myBusinessObj == null)
                            {
                                continue;
                            }
                            myBusinessObjList.Add(myBusinessObj);
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
                return new List<BusinessObject.AccountsHead>();
            }
        }

        public List<BusinessObject.AccountsHead> GetOrderedAccountsHeads()
        {
            try
            {
                using (var db = new ExpenseManagerDBEntities())
                {
                    var myBusinessObjList = new List<BusinessObject.AccountsHead>();
                    var accountsHeadsObjList = db.AccountsHeads.Where(m => m.ExpenseCategory.Status == 1).ToList();
                    if (!accountsHeadsObjList.Any())
                    {
                        return myBusinessObjList;
                    }
                    //Re-Map each Entity Object to Business Object
                    foreach (var item in accountsHeadsObjList)
                    {
                        var myBusinessObj = AccountsHeadMapper.Map<AccountsHead, BusinessObject.AccountsHead>(item);
                        if (myBusinessObj == null)
                        {
                            continue;
                        }
                        myBusinessObjList.Add(myBusinessObj);
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
                return new List<BusinessObject.AccountsHead>();
            }
        }

        public int AddAccountsHeadCheckDuplicate(BusinessObject.AccountsHead accountsHead)
        {
            try
            {
                //Re-Map Object to Entity Object
                var myEntityObj = AccountsHeadMapper.Map<BusinessObject.AccountsHead, AccountsHead>(accountsHead);
                if (myEntityObj == null)
                {
                    return -2;
                }
                using (var db = new ExpenseManagerDBEntities())
                {
                    if (db.AccountsHeads.Count(m => m.Title.ToLower().Replace(" ", string.Empty) == accountsHead.Title.ToLower().Replace(" ", string.Empty)) > 0)
                    {
                        return -3;
                    }
                    db.AddToAccountsHeads(myEntityObj);
                    db.SaveChanges();
                    accountsHead.AccountsHeadId = myEntityObj.AccountsHeadId;
                    return accountsHead.AccountsHeadId;
                }
            }
            catch (Exception ex)
            {
                ErrorManager.LogApplicationError(ex.StackTrace, ex.Source, ex.Message);
                return 0;
            }
        }

        public int UpdateAccountsHeadCheckDuplicate(BusinessObject.AccountsHead accountsHead)
        {
            try
            {
                //Re-Map Object to Entity Object
                var myEntityObj = AccountsHeadMapper.Map<BusinessObject.AccountsHead, AccountsHead>(accountsHead);
                if (myEntityObj == null)
                {
                    return -2;
                }
                using (var db = new ExpenseManagerDBEntities())
                {
                    if (db.AccountsHeads.Count(m => m.Title.ToLower().Replace(" ", string.Empty) == accountsHead.Title.ToLower().Replace(" ", string.Empty) && m.AccountsHeadId != accountsHead.AccountsHeadId) > 0)
                    {
                        return -3;
                    }
                    db.AccountsHeads.Attach(myEntityObj);
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

        public int GetAccountsHeadsCountByCategoryId(Int32 expenseCategoryId)
        {
            try
            {
                using (var db = new ExpenseManagerDBEntities())
                {
                    var itemCount = db.AccountsHeads.Count(m => m.ExpenseCategoryId == expenseCategoryId);

                    return (itemCount> 0)?itemCount:0;
                }
            }
            catch (Exception ex)
            {
                ErrorManager.LogApplicationError(ex.StackTrace, ex.Source, ex.Message);
                return -1;
            }
        }

       public BusinessObject.AccountsHead GetLastInsertedAccountsHeadsByExpenseCategoryId(int expenseCategoryId)
		{
			try
			{
				using (var db = new ExpenseManagerDBEntities())
				{
					var myObjList = db.AccountsHeads.ToList().FindAll(m => m.ExpenseCategoryId == expenseCategoryId);
					var myBusinessObjList = new List<BusinessObject.AccountsHead>();
					if(!myObjList.Any())
					{
					    return new BusinessObject.AccountsHead();
					}
					
                    //Re-Map each Entity Object to Business Object
					foreach (var item in myObjList)
					{
					var myBusinessObj = AccountsHeadMapper.Map<AccountsHead, BusinessObject.AccountsHead>(item);
						if(myBusinessObj == null){continue;}
						myBusinessObjList.Add(myBusinessObj);
					}
                    var obj = myBusinessObjList.OrderByDescending(m => long.Parse(m.Code)).ElementAt(0);
                    return obj;
				}
			}
			catch (Exception ex)
			{
				ErrorManager.LogApplicationError(ex.StackTrace, ex.Source, ex.Message);
				return new BusinessObject.AccountsHead();
			}
		}
        public List<BusinessObject.AccountsHead> GetOrderedAccountsHeadsByExpenseCategoryId(Int32 expenseCategoryId)
        {
            try
            {
                using (var db = new ExpenseManagerDBEntities())
                {
                    var myBusinessObjList = new List<BusinessObject.AccountsHead>();
                    var accountsHeadsObjList = db.AccountsHeads.Where(m => m.ExpenseCategoryId == expenseCategoryId && m.ExpenseCategory.Status == 1).ToList();
                    if (!accountsHeadsObjList.Any())
                    {
                        return myBusinessObjList;
                    }
                    //Re-Map each Entity Object to Business Object
                    foreach (var item in accountsHeadsObjList)
                    {
                        var myBusinessObj = AccountsHeadMapper.Map<AccountsHead, BusinessObject.AccountsHead>(item);
                        if (myBusinessObj == null)
                        {
                            continue;
                        }
                        myBusinessObjList.Add(myBusinessObj);
                    }

                    if (!myBusinessObjList.Any())
                    {
                        return myBusinessObjList;
                    }
                    return myBusinessObjList.OrderBy(m => m.ExpenseCategory.Title).ThenBy(m => m.Title).ToList();
                }
            }
            catch (Exception ex)
            {
                ErrorManager.LogApplicationError(ex.StackTrace, ex.Source, ex.Message);
                return new List<BusinessObject.AccountsHead>();
            }
        }
	}
    

	//Class File Generated from Code<->Stripper 1.2.0.0 | All Rights Reserved
}
