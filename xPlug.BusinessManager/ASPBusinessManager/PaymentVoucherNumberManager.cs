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
	///* Date Generated:	05-12-2013 11:55:57
	///*******************************************************************************


	public partial class PaymentVoucherNumberManager
	{
		public PaymentVoucherNumberManager()
		{
		}

		public int AddPaymentVoucherNumber(xPlug.BusinessObject.PaymentVoucherNumber paymentVoucherNumber)
		{
			try
			{
				//Re-Map Object to Entity Object
				var myEntityObj = PaymentVoucherNumberMapper.Map<xPlug.BusinessObject.PaymentVoucherNumber, PaymentVoucherNumber>(paymentVoucherNumber);
				if(myEntityObj == null)
				{return -2;}
				using (var db = new ExpenseManagerDBEntities())
				{
					db.AddToPaymentVoucherNumbers(myEntityObj);
					db.SaveChanges();
					paymentVoucherNumber.PaymentVoucherNumberId = myEntityObj.PaymentVoucherNumberId;
					return paymentVoucherNumber.PaymentVoucherNumberId;
				}
			}
			catch (Exception ex)
			{
				ErrorManager.LogApplicationError(ex.StackTrace, ex.Source, ex.Message);
				return 0;
			}
		}

		public bool UpdatePaymentVoucherNumber(xPlug.BusinessObject.PaymentVoucherNumber paymentVoucherNumber)
		{
			try
			{
				//Re-Map Object to Entity Object
				var myEntityObj = PaymentVoucherNumberMapper.Map<xPlug.BusinessObject.PaymentVoucherNumber, PaymentVoucherNumber>(paymentVoucherNumber);
				if(myEntityObj == null)
				{return false;}
				using (var db = new ExpenseManagerDBEntities())
				{
					db.PaymentVoucherNumbers.Attach(myEntityObj);
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

		public bool DeletePaymentVoucherNumber(int paymentVoucherNumberId)
		{
			try
			{
				using (var db = new ExpenseManagerDBEntities())
				{
					var myObj = db.PaymentVoucherNumbers.Single(s => s.PaymentVoucherNumberId == paymentVoucherNumberId);
					if (myObj == null) { return false; };
					db.PaymentVoucherNumbers.DeleteObject(myObj);
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

		public xPlug.BusinessObject.PaymentVoucherNumber GetPaymentVoucherNumber(int paymentVoucherNumberId)
		{
			try
			{
				using (var db = new ExpenseManagerDBEntities())
				{
					var myObj = db.PaymentVoucherNumbers.SingleOrDefault(s => s.PaymentVoucherNumberId == paymentVoucherNumberId);
					if(myObj == null){return new xPlug.BusinessObject.PaymentVoucherNumber();}
					//Re-Map Entity Object to Business Object
					var myBusinessObj = PaymentVoucherNumberMapper.Map<PaymentVoucherNumber, xPlug.BusinessObject.PaymentVoucherNumber>(myObj);
					if(myBusinessObj == null){return new xPlug.BusinessObject.PaymentVoucherNumber();}
					{return myBusinessObj;}
				}
			}
			catch (Exception ex)
			{
				ErrorManager.LogApplicationError(ex.StackTrace, ex.Source, ex.Message);
				return new xPlug.BusinessObject.PaymentVoucherNumber();
			}
		}

		public List<xPlug.BusinessObject.PaymentVoucherNumber> GetPaymentVoucherNumbers()
		{
			try
			{
				using (var db = new ExpenseManagerDBEntities())
				{
					var myObjList = db.PaymentVoucherNumbers.ToList();
					var myBusinessObjList = new List<xPlug.BusinessObject.PaymentVoucherNumber>();
					if(myObjList == null){return myBusinessObjList;}
					//Re-Map each Entity Object to Business Object
					foreach (var item in myObjList)
					{
					var myBusinessObj = PaymentVoucherNumberMapper.Map<PaymentVoucherNumber, xPlug.BusinessObject.PaymentVoucherNumber>(item);
						if(myBusinessObj == null){continue;}
						myBusinessObjList.Add(myBusinessObj);
					}
					return myBusinessObjList;
				}
			}
			catch (Exception ex)
			{
				ErrorManager.LogApplicationError(ex.StackTrace, ex.Source, ex.Message);
				return new List<xPlug.BusinessObject.PaymentVoucherNumber>();
			}
		}

	}


	//Class File Generated from Code<->Stripper 1.2.0.0 | All Rights Reserved
}
