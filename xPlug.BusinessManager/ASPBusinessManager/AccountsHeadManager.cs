using System;
using System.Linq;
using System.Collections.Generic;
using System.Data;
using XPLUG.WEBTOOLS;
using xPlug.BusinessObjectMapper;
using ExpenseManager.EF;


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
		public AccountsHeadManager()
		{
		}

		public int AddAccountsHead(xPlug.BusinessObject.AccountsHead accountsHead)
		{
			try
			{
				//Re-Map Object to Entity Object
				var myEntityObj = AccountsHeadMapper.Map<xPlug.BusinessObject.AccountsHead, AccountsHead>(accountsHead);
				if(myEntityObj == null)
				{return -2;}
                using (var db = new ExpenseManagerDBEntities())
				{
                    if (db.AccountsHeads.Any())
                    {
                        var objTitle = myEntityObj.Title.ToLower().Replace(" ", string.Empty.Trim());
                        if (db.AccountsHeads.Count(m => m.Title.ToLower().Replace(" ", string.Empty.Trim()) == objTitle) > 0)
                        {
                            return -3;
                        }

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

		public int UpdateAccountsHead(BusinessObject.AccountsHead accountsHead)
		{
			try
			{
				//Re-Map Object to Entity Object
				var myEntityObj = AccountsHeadMapper.Map<BusinessObject.AccountsHead, AccountsHead>(accountsHead);
				if(myEntityObj == null)
				{return -2;}
				using (var db = new ExpenseManagerDBEntities())
				{
                    var objTitle = myEntityObj.Title.ToLower().Replace(" ", string.Empty.Trim());
                    if (db.AccountsHeads.Count(m => m.Title.ToLower().Replace(" ", string.Empty.Trim()) == objTitle && m.AccountsHeadId != myEntityObj.AccountsHeadId) > 0)
                    {
                        return -3;
                    }
                   
					db.AccountsHeads.Attach(myEntityObj);
					 db.ObjectStateManager.ChangeObjectState(myEntityObj, EntityState.Modified);
					db.SaveChanges();
                    return accountsHead.AccountsHeadId;
				}
			}
			catch (Exception ex)
			{
				ErrorManager.LogApplicationError(ex.StackTrace, ex.Source, ex.Message);
				return 0;
			}
		}

		public bool DeleteAccountsHead(int accountsHeadId)
		{
			try
			{
				using (var db = new ExpenseManagerDBEntities())
				{
					var myObj = db.AccountsHeads.Single(s => s.AccountsHeadId == accountsHeadId);
					if (myObj == null) { return false; };
					db.AccountsHeads.DeleteObject(myObj);
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

		public BusinessObject.AccountsHead GetAccountsHead(int accountsHeadId)
		{
			try
			{
				using (var db = new ExpenseManagerDBEntities())
				{
					var myObj = db.AccountsHeads.SingleOrDefault(s => s.AccountsHeadId == accountsHeadId);
					if(myObj == null){return new xPlug.BusinessObject.AccountsHead();}
					//Re-Map Entity Object to Business Object
					var myBusinessObj = AccountsHeadMapper.Map<AccountsHead, xPlug.BusinessObject.AccountsHead>(myObj);
					if(myBusinessObj == null){return new xPlug.BusinessObject.AccountsHead();}
					{return myBusinessObj;}
				}
			}
			catch (Exception ex)
			{
				ErrorManager.LogApplicationError(ex.StackTrace, ex.Source, ex.Message);
				return new xPlug.BusinessObject.AccountsHead();
			}
		}

		public List<xPlug.BusinessObject.AccountsHead> GetAccountsHeads()
		{
			try
			{
				using (var db = new ExpenseManagerDBEntities())
				{
					var myObjList = db.AccountsHeads.ToList();
					var myBusinessObjList = new List<xPlug.BusinessObject.AccountsHead>();
					if(myObjList == null){return myBusinessObjList;}
					//Re-Map each Entity Object to Business Object
					foreach (var item in myObjList)
					{
					    var myBusinessObj = AccountsHeadMapper.Map<AccountsHead, xPlug.BusinessObject.AccountsHead>(item);
						if(myBusinessObj == null){continue;}
						myBusinessObjList.Add(myBusinessObj);
					}
					return myBusinessObjList;
				}
			}
			catch (Exception ex)
			{
				ErrorManager.LogApplicationError(ex.StackTrace, ex.Source, ex.Message);
				return new List<xPlug.BusinessObject.AccountsHead>();
			}
		}

		public List<xPlug.BusinessObject.AccountsHead>  GetAccountsHeadsByExpenseCategoryId(Int32 expenseCategoryId)
		{
			try
			{
				using (var db = new ExpenseManagerDBEntities())
				{
					var myObjList = db.AccountsHeads.ToList().FindAll(m => m.ExpenseCategoryId == expenseCategoryId);
					var myBusinessObjList = new List<xPlug.BusinessObject.AccountsHead>();
					if(myObjList == null){return myBusinessObjList;}
					//Re-Map each Entity Object to Business Object
					foreach (var item in myObjList)
					{
					var myBusinessObj = AccountsHeadMapper.Map<AccountsHead, xPlug.BusinessObject.AccountsHead>(item);
						if(myBusinessObj == null){continue;}
						myBusinessObjList.Add(myBusinessObj);
					}
					return myBusinessObjList;
				}
			}
			catch (Exception ex)
			{
				ErrorManager.LogApplicationError(ex.StackTrace, ex.Source, ex.Message);
				return new List<xPlug.BusinessObject.AccountsHead>();
			}
		}
        
	}


	//Class File Generated from Code<->Stripper 1.2.0.0 | All Rights Reserved
}
