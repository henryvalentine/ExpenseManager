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
	///* Date Generated:	25-11-2013 09:26:28
	///*******************************************************************************


	public partial class ExpenseTransactionPaymentManager
	{
		public ExpenseTransactionPaymentManager()
		{
		}

		public long AddExpenseTransactionPayment(BusinessObject.ExpenseTransactionPayment expenseTransactionPayment)
		{
			try
			{
				//Re-Map Object to Entity Object
				var myEntityObj = ExpenseTransactionPaymentMapper.Map<BusinessObject.ExpenseTransactionPayment, ExpenseTransactionPayment>(expenseTransactionPayment);
				if(myEntityObj == null)
				{return -2;}
				using (var db = new ExpenseManagerDBEntities())
				{
					db.AddToExpenseTransactionPayments(myEntityObj);
					db.SaveChanges();
					expenseTransactionPayment.ExpenseTransactionPaymentId = myEntityObj.ExpenseTransactionPaymentId;
					return expenseTransactionPayment.ExpenseTransactionPaymentId;
				}
			}
			catch (Exception ex)
			{
				ErrorManager.LogApplicationError(ex.StackTrace, ex.Source, ex.Message);
				return 0;
			}
		}

		public bool UpdateExpenseTransactionPayment(xPlug.BusinessObject.ExpenseTransactionPayment expenseTransactionPayment)
		{
			try
			{
				//Re-Map Object to Entity Object
				var myEntityObj = ExpenseTransactionPaymentMapper.Map<xPlug.BusinessObject.ExpenseTransactionPayment, ExpenseTransactionPayment>(expenseTransactionPayment);
				if(myEntityObj == null)
				{return false;}
				using (var db = new ExpenseManagerDBEntities())
				{
					db.ExpenseTransactionPayments.Attach(myEntityObj);
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

		public bool DeleteExpenseTransactionPayment(long expenseTransactionPaymentId)
		{
			try
			{
				using (var db = new ExpenseManagerDBEntities())
				{
					var myObj = db.ExpenseTransactionPayments.Single(s => s.ExpenseTransactionPaymentId == expenseTransactionPaymentId);
					if (myObj == null) { return false; };
					db.ExpenseTransactionPayments.DeleteObject(myObj);
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

		public xPlug.BusinessObject.ExpenseTransactionPayment GetExpenseTransactionPayment(long expenseTransactionPaymentId)
		{
			try
			{
				using (var db = new ExpenseManagerDBEntities())
				{
					var myObj = db.ExpenseTransactionPayments.SingleOrDefault(s => s.ExpenseTransactionPaymentId == expenseTransactionPaymentId);
					if(myObj == null){return new xPlug.BusinessObject.ExpenseTransactionPayment();}
					//Re-Map Entity Object to Business Object
					var myBusinessObj = ExpenseTransactionPaymentMapper.Map<ExpenseTransactionPayment, xPlug.BusinessObject.ExpenseTransactionPayment>(myObj);
					if(myBusinessObj == null){return new xPlug.BusinessObject.ExpenseTransactionPayment();}
					{return myBusinessObj;}
				}
			}
			catch (Exception ex)
			{
				ErrorManager.LogApplicationError(ex.StackTrace, ex.Source, ex.Message);
				return new xPlug.BusinessObject.ExpenseTransactionPayment();
			}
		}

		public List<xPlug.BusinessObject.ExpenseTransactionPayment> GetExpenseTransactionPayments()
		{
			try
			{
				using (var db = new ExpenseManagerDBEntities())
				{
					var myObjList = db.ExpenseTransactionPayments.ToList();
					var myBusinessObjList = new List<xPlug.BusinessObject.ExpenseTransactionPayment>();
					if(myObjList == null){return myBusinessObjList;}
					//Re-Map each Entity Object to Business Object
					foreach (var item in myObjList)
					{
					var myBusinessObj = ExpenseTransactionPaymentMapper.Map<ExpenseTransactionPayment, xPlug.BusinessObject.ExpenseTransactionPayment>(item);
						if(myBusinessObj == null){continue;}
						myBusinessObjList.Add(myBusinessObj);
					}
					return myBusinessObjList;
				}
			}
			catch (Exception ex)
			{
				ErrorManager.LogApplicationError(ex.StackTrace, ex.Source, ex.Message);
				return new List<xPlug.BusinessObject.ExpenseTransactionPayment>();
			}
		}

		public List<xPlug.BusinessObject.ExpenseTransactionPayment>  GetExpenseTransactionPaymentsByExpenseTransactionId(Int64 expenseTransactionId)
		{
			try
			{
				using (var db = new ExpenseManagerDBEntities())
				{
					var myObjList = db.ExpenseTransactionPayments.ToList().FindAll(m => m.ExpenseTransactionId == expenseTransactionId);
					var myBusinessObjList = new List<xPlug.BusinessObject.ExpenseTransactionPayment>();
					if(myObjList == null){return myBusinessObjList;}
					//Re-Map each Entity Object to Business Object
					foreach (var item in myObjList)
					{
					var myBusinessObj = ExpenseTransactionPaymentMapper.Map<ExpenseTransactionPayment, xPlug.BusinessObject.ExpenseTransactionPayment>(item);
						if(myBusinessObj == null){continue;}
						myBusinessObjList.Add(myBusinessObj);
					}
					return myBusinessObjList;
				}
			}
			catch (Exception ex)
			{
				ErrorManager.LogApplicationError(ex.StackTrace, ex.Source, ex.Message);
				return new List<xPlug.BusinessObject.ExpenseTransactionPayment>();
			}
		}

		public List<xPlug.BusinessObject.ExpenseTransactionPayment>  GetExpenseTransactionPaymentsByBeneficiaryId(Int32 beneficiaryId)
		{
			try
			{
				using (var db = new ExpenseManagerDBEntities())
				{
					var myObjList = db.ExpenseTransactionPayments.ToList().FindAll(m => m.BeneficiaryId == beneficiaryId);
					var myBusinessObjList = new List<xPlug.BusinessObject.ExpenseTransactionPayment>();
					if(myObjList == null){return myBusinessObjList;}
					//Re-Map each Entity Object to Business Object
					foreach (var item in myObjList)
					{
					var myBusinessObj = ExpenseTransactionPaymentMapper.Map<ExpenseTransactionPayment, xPlug.BusinessObject.ExpenseTransactionPayment>(item);
						if(myBusinessObj == null){continue;}
						myBusinessObjList.Add(myBusinessObj);
					}
					return myBusinessObjList;
				}
			}
			catch (Exception ex)
			{
				ErrorManager.LogApplicationError(ex.StackTrace, ex.Source, ex.Message);
				return new List<xPlug.BusinessObject.ExpenseTransactionPayment>();
			}
		}

	}


	//Class File Generated from Code<->Stripper 1.2.0.0 | All Rights Reserved
}
