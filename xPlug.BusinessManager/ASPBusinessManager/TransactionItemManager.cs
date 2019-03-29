using System;
using System.Linq;
using System.Collections.Generic;
using System.Data;
using xPlug.BusinessObjectMapper;

using ExpenseManager.EF;
using XPLUG.WEBTOOLS;


namespace xPlug.BusinessManager
{
	/// Author:	 Oluwaseyi Adegboyega - sadegboyega@xplugng.com
	/// Copyright © 2014. xPlug Technologies Limited. All Rights Reserved
	///*******************************************************************************
	///* Date Created: 	04-06-2013
	///* Date Generated:	17-01-2014 02:29:15
	///*******************************************************************************


	public partial class TransactionItemManager
	{
	    public int AddTransactionItem(BusinessObject.TransactionItem transactionItem)
		{
			try
			{
				//Re-Map Object to Entity Object
				var myEntityObj = TransactionItemMapper.Map<xPlug.BusinessObject.TransactionItem, TransactionItem>(transactionItem);
				if(myEntityObj == null)
				{return -2;}
				using (var db = new ExpenseManagerDBEntities())
				{
					db.AddToTransactionItems(myEntityObj);
					db.SaveChanges();
					transactionItem.TransactionItemId = myEntityObj.TransactionItemId;
					return transactionItem.TransactionItemId;
				}
			}
			catch (Exception ex)
			{
				ErrorManager.LogApplicationError(ex.StackTrace, ex.Source, ex.Message);
				return 0;
			}
		}

		public bool UpdateTransactionItem(xPlug.BusinessObject.TransactionItem transactionItem)
		{
			try
			{
				//Re-Map Object to Entity Object
				var myEntityObj = TransactionItemMapper.Map<xPlug.BusinessObject.TransactionItem, TransactionItem>(transactionItem);
				if(myEntityObj == null)
				{return false;}
				using (var db = new ExpenseManagerDBEntities())
				{
					db.TransactionItems.Attach(myEntityObj);
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

		public bool DeleteTransactionItem(int transactionItemId)
		{
			try
			{
				using (var db = new ExpenseManagerDBEntities())
				{
					var myObj = db.TransactionItems.Single(s => s.TransactionItemId == transactionItemId);
					if (myObj == null) { return false; };
					db.TransactionItems.DeleteObject(myObj);
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

		public xPlug.BusinessObject.TransactionItem GetTransactionItem(int transactionItemId)
		{
			try
			{
				using (var db = new ExpenseManagerDBEntities())
				{
					var myObj = db.TransactionItems.SingleOrDefault(s => s.TransactionItemId == transactionItemId);
					if(myObj == null){return new xPlug.BusinessObject.TransactionItem();}
					//Re-Map Entity Object to Business Object
					var myBusinessObj = TransactionItemMapper.Map<TransactionItem, xPlug.BusinessObject.TransactionItem>(myObj);
					if(myBusinessObj == null){return new xPlug.BusinessObject.TransactionItem();}
					{return myBusinessObj;}
				}
			}
			catch (Exception ex)
			{
				ErrorManager.LogApplicationError(ex.StackTrace, ex.Source, ex.Message);
				return new xPlug.BusinessObject.TransactionItem();
			}
		}

		public List<xPlug.BusinessObject.TransactionItem> GetTransactionItems()
		{
			try
			{
				using (var db = new ExpenseManagerDBEntities())
				{
					var myObjList = db.TransactionItems.ToList();
					var myBusinessObjList = new List<xPlug.BusinessObject.TransactionItem>();
					if(myObjList == null){return myBusinessObjList;}
					//Re-Map each Entity Object to Business Object
					foreach (var item in myObjList)
					{
					var myBusinessObj = TransactionItemMapper.Map<TransactionItem, xPlug.BusinessObject.TransactionItem>(item);
						if(myBusinessObj == null){continue;}
						myBusinessObjList.Add(myBusinessObj);
					}
					return myBusinessObjList;
				}
			}
			catch (Exception ex)
			{
				ErrorManager.LogApplicationError(ex.StackTrace, ex.Source, ex.Message);
				return new List<xPlug.BusinessObject.TransactionItem>();
			}
		}

		public List<xPlug.BusinessObject.TransactionItem>  GetTransactionItemsByExpensenseItemId(Int32 expensenseItemId)
		{
			try
			{
				using (var db = new ExpenseManagerDBEntities())
				{
					var myObjList = db.TransactionItems.ToList().FindAll(m => m.ExpensenseItemId == expensenseItemId);
					var myBusinessObjList = new List<xPlug.BusinessObject.TransactionItem>();
					if(myObjList == null){return myBusinessObjList;}
					//Re-Map each Entity Object to Business Object
					foreach (var item in myObjList)
					{
					var myBusinessObj = TransactionItemMapper.Map<TransactionItem, xPlug.BusinessObject.TransactionItem>(item);
						if(myBusinessObj == null){continue;}
						myBusinessObjList.Add(myBusinessObj);
					}
					return myBusinessObjList;
				}
			}
			catch (Exception ex)
			{
				ErrorManager.LogApplicationError(ex.StackTrace, ex.Source, ex.Message);
				return new List<xPlug.BusinessObject.TransactionItem>();
			}
		}

		public List<xPlug.BusinessObject.TransactionItem>  GetTransactionItemsByExpenseTransactionId(Int64 expenseTransactionId)
		{
			try
			{
				using (var db = new ExpenseManagerDBEntities())
				{
					var myObjList = db.TransactionItems.ToList().FindAll(m => m.ExpenseTransactionId == expenseTransactionId);
					var myBusinessObjList = new List<xPlug.BusinessObject.TransactionItem>();
					if(myObjList == null){return myBusinessObjList;}
					//Re-Map each Entity Object to Business Object
					foreach (var item in myObjList)
					{
					var myBusinessObj = TransactionItemMapper.Map<TransactionItem, xPlug.BusinessObject.TransactionItem>(item);
						if(myBusinessObj == null){continue;}
						myBusinessObjList.Add(myBusinessObj);
					}
					return myBusinessObjList;
				}
			}
			catch (Exception ex)
			{
				ErrorManager.LogApplicationError(ex.StackTrace, ex.Source, ex.Message);
				return new List<xPlug.BusinessObject.TransactionItem>();
			}
		}

		public List<xPlug.BusinessObject.TransactionItem>  GetTransactionItemsByExpenseCategoryId(Int32 expenseCategoryId)
		{
			try
			{
				using (var db = new ExpenseManagerDBEntities())
				{
					var myObjList = db.TransactionItems.ToList().FindAll(m => m.ExpenseCategoryId == expenseCategoryId);
					var myBusinessObjList = new List<xPlug.BusinessObject.TransactionItem>();
					if(myObjList == null){return myBusinessObjList;}
					//Re-Map each Entity Object to Business Object
					foreach (var item in myObjList)
					{
					var myBusinessObj = TransactionItemMapper.Map<TransactionItem, xPlug.BusinessObject.TransactionItem>(item);
						if(myBusinessObj == null){continue;}
						myBusinessObjList.Add(myBusinessObj);
					}
					return myBusinessObjList;
				}
			}
			catch (Exception ex)
			{
				ErrorManager.LogApplicationError(ex.StackTrace, ex.Source, ex.Message);
				return new List<xPlug.BusinessObject.TransactionItem>();
			}
		}

		public List<xPlug.BusinessObject.TransactionItem>  GetTransactionItemsByExpenseTypeId(Int32 expenseTypeId)
		{
			try
			{
				using (var db = new ExpenseManagerDBEntities())
				{
					var myObjList = db.TransactionItems.ToList().FindAll(m => m.ExpenseTypeId == expenseTypeId);
					var myBusinessObjList = new List<xPlug.BusinessObject.TransactionItem>();
					if(myObjList == null){return myBusinessObjList;}
					//Re-Map each Entity Object to Business Object
					foreach (var item in myObjList)
					{
					var myBusinessObj = TransactionItemMapper.Map<TransactionItem, xPlug.BusinessObject.TransactionItem>(item);
						if(myBusinessObj == null){continue;}
						myBusinessObjList.Add(myBusinessObj);
					}
					return myBusinessObjList;
				}
			}
			catch (Exception ex)
			{
				ErrorManager.LogApplicationError(ex.StackTrace, ex.Source, ex.Message);
				return new List<xPlug.BusinessObject.TransactionItem>();
			}
		}

	}


	//Class File Generated from Code<->Stripper 1.2.0.0 | All Rights Reserved
}
