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
	///* Date Generated:	01-12-2013 03:58:06
	///*******************************************************************************


	public partial class ChequeManager
	{
		public ChequeManager()
		{
		}

		public int AddCheque(xPlug.BusinessObject.Cheque cheque)
		{
			try
			{
				//Re-Map Object to Entity Object
				var myEntityObj = ChequeMapper.Map<xPlug.BusinessObject.Cheque, Cheque>(cheque);
				if(myEntityObj == null)
				{return -2;}
				using (var db = new ExpenseManagerDBEntities())
				{
					db.AddToCheques(myEntityObj);
					db.SaveChanges();
					cheque.ChequePaymentId = myEntityObj.ChequePaymentId;
					return cheque.ChequePaymentId;
				}
			}
			catch (Exception ex)
			{
				ErrorManager.LogApplicationError(ex.StackTrace, ex.Source, ex.Message);
				return 0;
			}
		}

		public bool UpdateCheque(xPlug.BusinessObject.Cheque cheque)
		{
			try
			{
				//Re-Map Object to Entity Object
				var myEntityObj = ChequeMapper.Map<xPlug.BusinessObject.Cheque, Cheque>(cheque);
				if(myEntityObj == null)
				{return false;}
				using (var db = new ExpenseManagerDBEntities())
				{
					db.Cheques.Attach(myEntityObj);
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

		public bool DeleteCheque(int chequePaymentId)
		{
			try
			{
				using (var db = new ExpenseManagerDBEntities())
				{
					var myObj = db.Cheques.Single(s => s.ChequePaymentId == chequePaymentId);
					if (myObj == null) { return false; };
					db.Cheques.DeleteObject(myObj);
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

		public xPlug.BusinessObject.Cheque GetCheque(int chequePaymentId)
		{
			try
			{
				using (var db = new ExpenseManagerDBEntities())
				{
					var myObj = db.Cheques.SingleOrDefault(s => s.ChequePaymentId == chequePaymentId);
					if(myObj == null){return new xPlug.BusinessObject.Cheque();}
					//Re-Map Entity Object to Business Object
					var myBusinessObj = ChequeMapper.Map<Cheque, xPlug.BusinessObject.Cheque>(myObj);
					if(myBusinessObj == null){return new xPlug.BusinessObject.Cheque();}
					{return myBusinessObj;}
				}
			}
			catch (Exception ex)
			{
				ErrorManager.LogApplicationError(ex.StackTrace, ex.Source, ex.Message);
				return new xPlug.BusinessObject.Cheque();
			}
		}

		public List<xPlug.BusinessObject.Cheque> GetCheques()
		{
			try
			{
				using (var db = new ExpenseManagerDBEntities())
				{
					var myObjList = db.Cheques.ToList();
					var myBusinessObjList = new List<xPlug.BusinessObject.Cheque>();
					if(myObjList == null){return myBusinessObjList;}
					//Re-Map each Entity Object to Business Object
					foreach (var item in myObjList)
					{
					var myBusinessObj = ChequeMapper.Map<Cheque, xPlug.BusinessObject.Cheque>(item);
						if(myBusinessObj == null){continue;}
						myBusinessObjList.Add(myBusinessObj);
					}
					return myBusinessObjList;
				}
			}
			catch (Exception ex)
			{
				ErrorManager.LogApplicationError(ex.StackTrace, ex.Source, ex.Message);
				return new List<xPlug.BusinessObject.Cheque>();
			}
		}

		public List<xPlug.BusinessObject.Cheque>  GetChequesByExpenseTransactionPaymentHistoryId(Int64 expenseTransactionPaymentHistoryId)
		{
			try
			{
				using (var db = new ExpenseManagerDBEntities())
				{
					var myObjList = db.Cheques.ToList().FindAll(m => m.ExpenseTransactionPaymentHistoryId == expenseTransactionPaymentHistoryId);
					var myBusinessObjList = new List<xPlug.BusinessObject.Cheque>();
					if(myObjList == null){return myBusinessObjList;}
					//Re-Map each Entity Object to Business Object
					foreach (var item in myObjList)
					{
					var myBusinessObj = ChequeMapper.Map<Cheque, xPlug.BusinessObject.Cheque>(item);
						if(myBusinessObj == null){continue;}
						myBusinessObjList.Add(myBusinessObj);
					}
					return myBusinessObjList;
				}
			}
			catch (Exception ex)
			{
				ErrorManager.LogApplicationError(ex.StackTrace, ex.Source, ex.Message);
				return new List<xPlug.BusinessObject.Cheque>();
			}
		}

		public List<xPlug.BusinessObject.Cheque>  GetChequesByBankId(Int32 bankId)
		{
			try
			{
				using (var db = new ExpenseManagerDBEntities())
				{
					var myObjList = db.Cheques.ToList().FindAll(m => m.BankId == bankId);
					var myBusinessObjList = new List<xPlug.BusinessObject.Cheque>();
					if(myObjList == null){return myBusinessObjList;}
					//Re-Map each Entity Object to Business Object
					foreach (var item in myObjList)
					{
					var myBusinessObj = ChequeMapper.Map<Cheque, xPlug.BusinessObject.Cheque>(item);
						if(myBusinessObj == null){continue;}
						myBusinessObjList.Add(myBusinessObj);
					}
					return myBusinessObjList;
				}
			}
			catch (Exception ex)
			{
				ErrorManager.LogApplicationError(ex.StackTrace, ex.Source, ex.Message);
				return new List<xPlug.BusinessObject.Cheque>();
			}
		}

	}


	//Class File Generated from Code<->Stripper 1.2.0.0 | All Rights Reserved
}
