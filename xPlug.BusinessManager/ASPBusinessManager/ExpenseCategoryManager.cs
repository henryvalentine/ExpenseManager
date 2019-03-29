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
	///* Date Generated:	25-11-2013 09:26:29
	///*******************************************************************************


	public partial class ExpenseCategoryManager
	{
		public ExpenseCategoryManager()
		{
		}

		public int AddExpenseCategory(xPlug.BusinessObject.ExpenseCategory expenseCategory)
		{
			try
			{
				//Re-Map Object to Entity Object
				var myEntityObj = ExpenseCategoryMapper.Map<xPlug.BusinessObject.ExpenseCategory, ExpenseCategory>(expenseCategory);
				if(myEntityObj == null)
				{return -2;}
				using (var db = new ExpenseManagerDBEntities())
				{
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

		public bool UpdateExpenseCategory(xPlug.BusinessObject.ExpenseCategory expenseCategory)
		{
			try
			{
				//Re-Map Object to Entity Object
				var myEntityObj = ExpenseCategoryMapper.Map<xPlug.BusinessObject.ExpenseCategory, ExpenseCategory>(expenseCategory);
				if(myEntityObj == null)
				{return false;}
				using (var db = new ExpenseManagerDBEntities())
				{
					db.ExpenseCategories.Attach(myEntityObj);
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

		public bool DeleteExpenseCategory(int expenseCategoryId)
		{
			try
			{
				using (var db = new ExpenseManagerDBEntities())
				{
					var myObj = db.ExpenseCategories.Single(s => s.ExpenseCategoryId == expenseCategoryId);
					if (myObj == null) { return false; };
					db.ExpenseCategories.DeleteObject(myObj);
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

		public xPlug.BusinessObject.ExpenseCategory GetExpenseCategory(int expenseCategoryId)
		{
			try
			{
				using (var db = new ExpenseManagerDBEntities())
				{
					var myObj = db.ExpenseCategories.SingleOrDefault(s => s.ExpenseCategoryId == expenseCategoryId);
					if(myObj == null){return new xPlug.BusinessObject.ExpenseCategory();}
					//Re-Map Entity Object to Business Object
					var myBusinessObj = ExpenseCategoryMapper.Map<ExpenseCategory, xPlug.BusinessObject.ExpenseCategory>(myObj);
					if(myBusinessObj == null){return new xPlug.BusinessObject.ExpenseCategory();}
					{return myBusinessObj;}
				}
			}
			catch (Exception ex)
			{
				ErrorManager.LogApplicationError(ex.StackTrace, ex.Source, ex.Message);
				return new xPlug.BusinessObject.ExpenseCategory();
			}
		}

		public List<xPlug.BusinessObject.ExpenseCategory> GetExpenseCategories()
		{
			try
			{
				using (var db = new ExpenseManagerDBEntities())
				{
					var myObjList = db.ExpenseCategories.ToList();
					var myBusinessObjList = new List<xPlug.BusinessObject.ExpenseCategory>();
					if(myObjList == null){return myBusinessObjList;}
					//Re-Map each Entity Object to Business Object
					foreach (var item in myObjList)
					{
					var myBusinessObj = ExpenseCategoryMapper.Map<ExpenseCategory, xPlug.BusinessObject.ExpenseCategory>(item);
						if(myBusinessObj == null){continue;}
						myBusinessObjList.Add(myBusinessObj);
					}
					return myBusinessObjList;
				}
			}
			catch (Exception ex)
			{
				ErrorManager.LogApplicationError(ex.StackTrace, ex.Source, ex.Message);
				return new List<xPlug.BusinessObject.ExpenseCategory>();
			}
		}

	}


	//Class File Generated from Code<->Stripper 1.2.0.0 | All Rights Reserved
}
