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
	///* Date Generated:	25-11-2013 09:26:27
	///*******************************************************************************


	public partial class ExpenseItemManager
	{
		public ExpenseItemManager()
		{
		}

		public int AddExpenseItem(BusinessObject.ExpenseItem expenseItem)
		{
			try
			{
				//Re-Map Object to Entity Object
				var myEntityObj = ExpenseItemMapper.Map<xPlug.BusinessObject.ExpenseItem, ExpenseItem>(expenseItem);
				if(myEntityObj == null)
				{return -2;}
				using (var db = new ExpenseManagerDBEntities())
				{
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

		public bool UpdateExpenseItem(BusinessObject.ExpenseItem expenseItem)
		{
			try
			{
				//Re-Map Object to Entity Object
				var myEntityObj = ExpenseItemMapper.Map<BusinessObject.ExpenseItem, ExpenseItem>(expenseItem);
				if(myEntityObj == null)
				{return false;}
				using (var db = new ExpenseManagerDBEntities())
				{
					db.ExpenseItems.Attach(myEntityObj);
					 db.ObjectStateManager.ChangeObjectState(myEntityObj, EntityState.Modified);
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

		public bool DeleteExpenseItem(int expenseItemId)
		{
			try
			{
				using (var db = new ExpenseManagerDBEntities())
				{
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

		public BusinessObject.ExpenseItem GetExpenseItem(int expenseItemId)
		{
			try
			{
				using (var db = new ExpenseManagerDBEntities())
				{
					var myObj = db.ExpenseItems.SingleOrDefault(s => s.ExpenseItemId == expenseItemId);
					if(myObj == null){return new xPlug.BusinessObject.ExpenseItem();}
					//Re-Map Entity Object to Business Object
					var myBusinessObj = ExpenseItemMapper.Map<ExpenseItem, xPlug.BusinessObject.ExpenseItem>(myObj);
					if(myBusinessObj == null){return new xPlug.BusinessObject.ExpenseItem();}
					{return myBusinessObj;}
				}
			}
			catch (Exception ex)
			{
				ErrorManager.LogApplicationError(ex.StackTrace, ex.Source, ex.Message);
				return new xPlug.BusinessObject.ExpenseItem();
			}
		}

		public List<BusinessObject.ExpenseItem> GetExpenseItems()
		{
			try
			{
				using (var db = new ExpenseManagerDBEntities())
				{
					var myObjList = db.ExpenseItems.ToList();
					var myBusinessObjList = new List<xPlug.BusinessObject.ExpenseItem>();
					if(myObjList == null){return myBusinessObjList;}
					//Re-Map each Entity Object to Business Object
					foreach (var item in myObjList)
					{
					var myBusinessObj = ExpenseItemMapper.Map<ExpenseItem, xPlug.BusinessObject.ExpenseItem>(item);
						if(myBusinessObj == null){continue;}
						myBusinessObjList.Add(myBusinessObj);
					}
					return myBusinessObjList;
				}
			}
			catch (Exception ex)
			{
				ErrorManager.LogApplicationError(ex.StackTrace, ex.Source, ex.Message);
				return new List<xPlug.BusinessObject.ExpenseItem>();
			}
		}

		public List<BusinessObject.ExpenseItem>  GetExpenseItemsByExpenseCategoryId(Int32 expenseCategoryId)
		{
			try
			{
				using (var db = new ExpenseManagerDBEntities())
				{
					var myObjList = db.ExpenseItems.ToList().FindAll(m => m.ExpenseCategoryId == expenseCategoryId);
					var myBusinessObjList = new List<xPlug.BusinessObject.ExpenseItem>();
					if(myObjList == null){return myBusinessObjList;}
					//Re-Map each Entity Object to Business Object
					foreach (var item in myObjList)
					{
					var myBusinessObj = ExpenseItemMapper.Map<ExpenseItem, xPlug.BusinessObject.ExpenseItem>(item);
						if(myBusinessObj == null){continue;}
						myBusinessObjList.Add(myBusinessObj);
					}
					return myBusinessObjList;
				}
			}
			catch (Exception ex)
			{
				ErrorManager.LogApplicationError(ex.StackTrace, ex.Source, ex.Message);
				return new List<xPlug.BusinessObject.ExpenseItem>();
			}
		}

		public List<BusinessObject.ExpenseItem>  GetExpenseItemsByAccountsHeadId(Int32 accountsHeadId)
		{
			try
			{
				using (var db = new ExpenseManagerDBEntities())
				{
					var myObjList = db.ExpenseItems.ToList().FindAll(m => m.AccountsHeadId == accountsHeadId);
					var myBusinessObjList = new List<xPlug.BusinessObject.ExpenseItem>();
					if(!myObjList.Any())
					{
					    return myBusinessObjList;
					}
					//Re-Map each Entity Object to Business Object
					foreach (var item in myObjList)
					{
					var myBusinessObj = ExpenseItemMapper.Map<ExpenseItem, xPlug.BusinessObject.ExpenseItem>(item);
						if(myBusinessObj == null){continue;}
						myBusinessObjList.Add(myBusinessObj);
					}
					return myBusinessObjList;
				}
			}
			catch (Exception ex)
			{
				ErrorManager.LogApplicationError(ex.StackTrace, ex.Source, ex.Message);
				return new List<xPlug.BusinessObject.ExpenseItem>();
			}
		}

	}


	//Class File Generated from Code<->Stripper 1.2.0.0 | All Rights Reserved
}
