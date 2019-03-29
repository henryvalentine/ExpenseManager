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


	public partial class ExpenseTransactionManager
	{
		public ExpenseTransactionManager()
		{
		}

		public long AddExpenseTransaction(xPlug.BusinessObject.ExpenseTransaction expenseTransaction)
		{
			try
			{
				//Re-Map Object to Entity Object
				var myEntityObj = ExpenseTransactionMapper.Map<xPlug.BusinessObject.ExpenseTransaction, ExpenseTransaction>(expenseTransaction);
				if(myEntityObj == null)
				{return -2;}
				using (var db = new ExpenseManagerDBEntities())
				{
					db.AddToExpenseTransactions(myEntityObj);
					db.SaveChanges();
					expenseTransaction.ExpenseTransactionId = myEntityObj.ExpenseTransactionId;
					return expenseTransaction.ExpenseTransactionId;
				}
			}
			catch (Exception ex)
			{
				ErrorManager.LogApplicationError(ex.StackTrace, ex.Source, ex.Message);
				return 0;
			}
		}

		public bool UpdateExpenseTransaction(xPlug.BusinessObject.ExpenseTransaction expenseTransaction)
		{
			try
			{
				//Re-Map Object to Entity Object
				var myEntityObj = ExpenseTransactionMapper.Map<xPlug.BusinessObject.ExpenseTransaction, ExpenseTransaction>(expenseTransaction);
				if(myEntityObj == null)
				{return false;}
				using (var db = new ExpenseManagerDBEntities())
				{
					db.ExpenseTransactions.Attach(myEntityObj);
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

		public bool DeleteExpenseTransaction(long expenseTransactionId)
		{
			try
			{
				using (var db = new ExpenseManagerDBEntities())
				{
					var myObj = db.ExpenseTransactions.Single(s => s.ExpenseTransactionId == expenseTransactionId);
					if (myObj == null) { return false; };
					db.ExpenseTransactions.DeleteObject(myObj);
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

		public xPlug.BusinessObject.ExpenseTransaction GetExpenseTransaction(long expenseTransactionId)
		{
			try
			{
				using (var db = new ExpenseManagerDBEntities())
				{
					var myObj = db.ExpenseTransactions.SingleOrDefault(s => s.ExpenseTransactionId == expenseTransactionId);
					if(myObj == null){return new xPlug.BusinessObject.ExpenseTransaction();}
					//Re-Map Entity Object to Business Object
					var myBusinessObj = ExpenseTransactionMapper.Map<ExpenseTransaction, xPlug.BusinessObject.ExpenseTransaction>(myObj);
					if(myBusinessObj == null){return new xPlug.BusinessObject.ExpenseTransaction();}
					{return myBusinessObj;}
				}
			}
			catch (Exception ex)
			{
				ErrorManager.LogApplicationError(ex.StackTrace, ex.Source, ex.Message);
				return new xPlug.BusinessObject.ExpenseTransaction();
			}
		}

		public List<xPlug.BusinessObject.ExpenseTransaction> GetExpenseTransactions()
		{
			try
			{
				using (var db = new ExpenseManagerDBEntities())
				{
					var myObjList = db.ExpenseTransactions.ToList();
					var myBusinessObjList = new List<xPlug.BusinessObject.ExpenseTransaction>();
					if(myObjList == null){return myBusinessObjList;}
					//Re-Map each Entity Object to Business Object
					foreach (var item in myObjList)
					{
					var myBusinessObj = ExpenseTransactionMapper.Map<ExpenseTransaction, xPlug.BusinessObject.ExpenseTransaction>(item);
						if(myBusinessObj == null){continue;}
						myBusinessObjList.Add(myBusinessObj);
					}
					return myBusinessObjList;
				}
			}
			catch (Exception ex)
			{
				ErrorManager.LogApplicationError(ex.StackTrace, ex.Source, ex.Message);
				return new List<xPlug.BusinessObject.ExpenseTransaction>();
			}
		}

		public List<xPlug.BusinessObject.ExpenseTransaction>  GetExpenseTransactionsByBeneficiaryId(Int32 beneficiaryId)
		{
			try
			{
				using (var db = new ExpenseManagerDBEntities())
				{
					var myObjList = db.ExpenseTransactions.ToList().FindAll(m => m.BeneficiaryId == beneficiaryId);
					var myBusinessObjList = new List<xPlug.BusinessObject.ExpenseTransaction>();
					if(myObjList == null){return myBusinessObjList;}
					//Re-Map each Entity Object to Business Object
					foreach (var item in myObjList)
					{
					var myBusinessObj = ExpenseTransactionMapper.Map<ExpenseTransaction, xPlug.BusinessObject.ExpenseTransaction>(item);
						if(myBusinessObj == null){continue;}
						myBusinessObjList.Add(myBusinessObj);
					}
					return myBusinessObjList;
				}
			}
			catch (Exception ex)
			{
				ErrorManager.LogApplicationError(ex.StackTrace, ex.Source, ex.Message);
				return new List<xPlug.BusinessObject.ExpenseTransaction>();
			}
		}

		public List<xPlug.BusinessObject.ExpenseTransaction>  GetExpenseTransactionsByBeneficiaryTypeId(Int32 beneficiaryTypeId)
		{
			try
			{
				using (var db = new ExpenseManagerDBEntities())
				{
					var myObjList = db.ExpenseTransactions.ToList().FindAll(m => m.BeneficiaryTypeId == beneficiaryTypeId);
					var myBusinessObjList = new List<xPlug.BusinessObject.ExpenseTransaction>();
					if(myObjList == null){return myBusinessObjList;}
					//Re-Map each Entity Object to Business Object
					foreach (var item in myObjList)
					{
					var myBusinessObj = ExpenseTransactionMapper.Map<ExpenseTransaction, xPlug.BusinessObject.ExpenseTransaction>(item);
						if(myBusinessObj == null){continue;}
						myBusinessObjList.Add(myBusinessObj);
					}
					return myBusinessObjList;
				}
			}
			catch (Exception ex)
			{
				ErrorManager.LogApplicationError(ex.StackTrace, ex.Source, ex.Message);
				return new List<xPlug.BusinessObject.ExpenseTransaction>();
			}
		}

	}


	//Class File Generated from Code<->Stripper 1.2.0.0 | All Rights Reserved
}
