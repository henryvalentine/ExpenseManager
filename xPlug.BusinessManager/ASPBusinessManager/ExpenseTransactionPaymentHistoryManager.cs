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


	public partial class ExpenseTransactionPaymentHistoryManager
	{
		public ExpenseTransactionPaymentHistoryManager()
		{
		}

		public long AddExpenseTransactionPaymentHistory(xPlug.BusinessObject.ExpenseTransactionPaymentHistory expenseTransactionPaymentHistory)
		{
			try
			{
				//Re-Map Object to Entity Object
				var myEntityObj = ExpenseTransactionPaymentHistoryMapper.Map<xPlug.BusinessObject.ExpenseTransactionPaymentHistory, ExpenseTransactionPaymentHistory>(expenseTransactionPaymentHistory);
				if(myEntityObj == null)
				{return -2;}
				using (var db = new ExpenseManagerDBEntities())
				{
					db.AddToExpenseTransactionPaymentHistories(myEntityObj);
					db.SaveChanges();
					expenseTransactionPaymentHistory.ExpenseTransactionPaymentHistoryId = myEntityObj.ExpenseTransactionPaymentHistoryId;
                    var paymentVoucher = new BusinessObject.PaymentVoucherNumber
				                             {
                                                 TransactionId = expenseTransactionPaymentHistory.ExpenseTransactionPaymentHistoryId,
                                                 TransactionTotalAmount = expenseTransactionPaymentHistory.ExpenseTransaction.TotalApprovedAmount,
                                                 PaymentDate = expenseTransactionPaymentHistory.PaymentDate,
                                                 DateSubmitted = DateMap.GetLocalDate()
				                             };
                    var pcvEntityObj = PaymentVoucherNumberMapper.Map<BusinessObject.PaymentVoucherNumber, PaymentVoucherNumber>(paymentVoucher);
                    db.AddToPaymentVoucherNumbers(pcvEntityObj);
                    db.SaveChanges();

					return expenseTransactionPaymentHistory.ExpenseTransactionPaymentHistoryId;
				}
			}
			catch (Exception ex)
			{
				ErrorManager.LogApplicationError(ex.StackTrace, ex.Source, ex.Message);
				return 0;
			}
		}

		public bool UpdateExpenseTransactionPaymentHistory(xPlug.BusinessObject.ExpenseTransactionPaymentHistory expenseTransactionPaymentHistory)
		{
			try
			{
				//Re-Map Object to Entity Object
				var myEntityObj = ExpenseTransactionPaymentHistoryMapper.Map<xPlug.BusinessObject.ExpenseTransactionPaymentHistory, ExpenseTransactionPaymentHistory>(expenseTransactionPaymentHistory);
				if(myEntityObj == null)
				{return false;}
				using (var db = new ExpenseManagerDBEntities())
				{
					db.ExpenseTransactionPaymentHistories.Attach(myEntityObj);
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

		public bool DeleteExpenseTransactionPaymentHistory(long expenseTransactionPaymentHistoryId)
		{
			try
			{
				using (var db = new ExpenseManagerDBEntities())
				{
					var myObj = db.ExpenseTransactionPaymentHistories.Single(s => s.ExpenseTransactionPaymentHistoryId == expenseTransactionPaymentHistoryId);
					if (myObj == null) { return false; };
					db.ExpenseTransactionPaymentHistories.DeleteObject(myObj);
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

		public xPlug.BusinessObject.ExpenseTransactionPaymentHistory GetExpenseTransactionPaymentHistory(long expenseTransactionPaymentHistoryId)
		{
			try
			{
				using (var db = new ExpenseManagerDBEntities())
				{
					var myObj = db.ExpenseTransactionPaymentHistories.SingleOrDefault(s => s.ExpenseTransactionPaymentHistoryId == expenseTransactionPaymentHistoryId);
					if(myObj == null){return new xPlug.BusinessObject.ExpenseTransactionPaymentHistory();}
					//Re-Map Entity Object to Business Object
					var myBusinessObj = ExpenseTransactionPaymentHistoryMapper.Map<ExpenseTransactionPaymentHistory, xPlug.BusinessObject.ExpenseTransactionPaymentHistory>(myObj);
					if(myBusinessObj == null)
					{
					    return new xPlug.BusinessObject.ExpenseTransactionPaymentHistory();
					}
					{return myBusinessObj;}
				}
			}
			catch (Exception ex)
			{
				ErrorManager.LogApplicationError(ex.StackTrace, ex.Source, ex.Message);
				return new xPlug.BusinessObject.ExpenseTransactionPaymentHistory();
			}
		}

		public List<xPlug.BusinessObject.ExpenseTransactionPaymentHistory> GetExpenseTransactionPaymentHistories()
		{
			try
			{
				using (var db = new ExpenseManagerDBEntities())
				{
					var myObjList = db.ExpenseTransactionPaymentHistories.ToList();
					var myBusinessObjList = new List<xPlug.BusinessObject.ExpenseTransactionPaymentHistory>();
					if(myObjList == null){return myBusinessObjList;}
					//Re-Map each Entity Object to Business Object
					foreach (var item in myObjList)
					{
					var myBusinessObj = ExpenseTransactionPaymentHistoryMapper.Map<ExpenseTransactionPaymentHistory, xPlug.BusinessObject.ExpenseTransactionPaymentHistory>(item);
						if(myBusinessObj == null){continue;}
						myBusinessObjList.Add(myBusinessObj);
					}
					return myBusinessObjList;
				}
			}
			catch (Exception ex)
			{
				ErrorManager.LogApplicationError(ex.StackTrace, ex.Source, ex.Message);
				return new List<xPlug.BusinessObject.ExpenseTransactionPaymentHistory>();
			}
		}

		public List<xPlug.BusinessObject.ExpenseTransactionPaymentHistory>  GetExpenseTransactionPaymentHistoriesByExpenseTransactionId(Int64 expenseTransactionId)
		{
			try
			{
				using (var db = new ExpenseManagerDBEntities())
				{
					var myObjList = db.ExpenseTransactionPaymentHistories.ToList().FindAll(m => m.ExpenseTransactionId == expenseTransactionId);
					var myBusinessObjList = new List<xPlug.BusinessObject.ExpenseTransactionPaymentHistory>();
					if(myObjList == null){return myBusinessObjList;}
					//Re-Map each Entity Object to Business Object
					foreach (var item in myObjList)
					{
					var myBusinessObj = ExpenseTransactionPaymentHistoryMapper.Map<ExpenseTransactionPaymentHistory, xPlug.BusinessObject.ExpenseTransactionPaymentHistory>(item);
						if(myBusinessObj == null){continue;}
						myBusinessObjList.Add(myBusinessObj);
					}
					return myBusinessObjList;
				}
			}
			catch (Exception ex)
			{
				ErrorManager.LogApplicationError(ex.StackTrace, ex.Source, ex.Message);
				return new List<xPlug.BusinessObject.ExpenseTransactionPaymentHistory>();
			}
		}

		public List<xPlug.BusinessObject.ExpenseTransactionPaymentHistory>  GetExpenseTransactionPaymentHistoriesByExpenseTransactionPaymentId(Int64 expenseTransactionPaymentId)
		{
			try
			{
				using (var db = new ExpenseManagerDBEntities())
				{
					var myObjList = db.ExpenseTransactionPaymentHistories.ToList().FindAll(m => m.ExpenseTransactionPaymentId == expenseTransactionPaymentId);
					var myBusinessObjList = new List<xPlug.BusinessObject.ExpenseTransactionPaymentHistory>();
					if(myObjList == null){return myBusinessObjList;}
					//Re-Map each Entity Object to Business Object
					foreach (var item in myObjList)
					{
					var myBusinessObj = ExpenseTransactionPaymentHistoryMapper.Map<ExpenseTransactionPaymentHistory, xPlug.BusinessObject.ExpenseTransactionPaymentHistory>(item);
						if(myBusinessObj == null){continue;}
						myBusinessObjList.Add(myBusinessObj);
					}
					return myBusinessObjList;
				}
			}
			catch (Exception ex)
			{
				ErrorManager.LogApplicationError(ex.StackTrace, ex.Source, ex.Message);
				return new List<xPlug.BusinessObject.ExpenseTransactionPaymentHistory>();
			}
		}

		public List<xPlug.BusinessObject.ExpenseTransactionPaymentHistory>  GetExpenseTransactionPaymentHistoriesByPaymentModeId(Int32 paymentModeId)
		{
			try
			{
				using (var db = new ExpenseManagerDBEntities())
				{
					var myObjList = db.ExpenseTransactionPaymentHistories.ToList().FindAll(m => m.PaymentModeId == paymentModeId);
					var myBusinessObjList = new List<xPlug.BusinessObject.ExpenseTransactionPaymentHistory>();
					if(myObjList == null){return myBusinessObjList;}
					//Re-Map each Entity Object to Business Object
					foreach (var item in myObjList)
					{
					var myBusinessObj = ExpenseTransactionPaymentHistoryMapper.Map<ExpenseTransactionPaymentHistory, xPlug.BusinessObject.ExpenseTransactionPaymentHistory>(item);
						if(myBusinessObj == null){continue;}
						myBusinessObjList.Add(myBusinessObj);
					}
					return myBusinessObjList;
				}
			}
			catch (Exception ex)
			{
				ErrorManager.LogApplicationError(ex.StackTrace, ex.Source, ex.Message);
				return new List<xPlug.BusinessObject.ExpenseTransactionPaymentHistory>();
			}
		}

		public List<xPlug.BusinessObject.ExpenseTransactionPaymentHistory>  GetExpenseTransactionPaymentHistoriesByBeneficiaryId(Int32 beneficiaryId)
		{
			try
			{
				using (var db = new ExpenseManagerDBEntities())
				{
					var myObjList = db.ExpenseTransactionPaymentHistories.ToList().FindAll(m => m.BeneficiaryId == beneficiaryId);
					var myBusinessObjList = new List<xPlug.BusinessObject.ExpenseTransactionPaymentHistory>();
					if(myObjList == null){return myBusinessObjList;}
					//Re-Map each Entity Object to Business Object
					foreach (var item in myObjList)
					{
					var myBusinessObj = ExpenseTransactionPaymentHistoryMapper.Map<ExpenseTransactionPaymentHistory, xPlug.BusinessObject.ExpenseTransactionPaymentHistory>(item);
						if(myBusinessObj == null){continue;}
						myBusinessObjList.Add(myBusinessObj);
					}
					return myBusinessObjList;
				}
			}
			catch (Exception ex)
			{
				ErrorManager.LogApplicationError(ex.StackTrace, ex.Source, ex.Message);
				return new List<xPlug.BusinessObject.ExpenseTransactionPaymentHistory>();
			}
		}

	}


	//Class File Generated from Code<->Stripper 1.2.0.0 | All Rights Reserved
}
