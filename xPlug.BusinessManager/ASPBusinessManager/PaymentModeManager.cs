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
	///* Date Generated:	25-11-2013 09:26:18
	///*******************************************************************************


	public partial class PaymentModeManager
	{
		public PaymentModeManager()
		{
		}

		public int AddPaymentMode(xPlug.BusinessObject.PaymentMode paymentMode)
		{
			try
			{
				//Re-Map Object to Entity Object
				var myEntityObj = PaymentModeMapper.Map<xPlug.BusinessObject.PaymentMode, PaymentMode>(paymentMode);
				if(myEntityObj == null)
				{return -2;}
				using (var db = new ExpenseManagerDBEntities())
				{
					db.AddToPaymentModes(myEntityObj);
					db.SaveChanges();
					paymentMode.PaymentModeId = myEntityObj.PaymentModeId;
					return paymentMode.PaymentModeId;
				}
			}
			catch (Exception ex)
			{
				ErrorManager.LogApplicationError(ex.StackTrace, ex.Source, ex.Message);
				return 0;
			}
		}

		public bool UpdatePaymentMode(xPlug.BusinessObject.PaymentMode paymentMode)
		{
			try
			{
				//Re-Map Object to Entity Object
				var myEntityObj = PaymentModeMapper.Map<xPlug.BusinessObject.PaymentMode, PaymentMode>(paymentMode);
				if(myEntityObj == null)
				{return false;}
				using (var db = new ExpenseManagerDBEntities())
				{
					db.PaymentModes.Attach(myEntityObj);
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

		public bool DeletePaymentMode(int paymentModeId)
		{
			try
			{
				using (var db = new ExpenseManagerDBEntities())
				{
					var myObj = db.PaymentModes.Single(s => s.PaymentModeId == paymentModeId);
					if (myObj == null) { return false; };
					db.PaymentModes.DeleteObject(myObj);
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

		public xPlug.BusinessObject.PaymentMode GetPaymentMode(int paymentModeId)
		{
			try
			{
				using (var db = new ExpenseManagerDBEntities())
				{
					var myObj = db.PaymentModes.SingleOrDefault(s => s.PaymentModeId == paymentModeId);
					if(myObj == null){return new xPlug.BusinessObject.PaymentMode();}
					//Re-Map Entity Object to Business Object
					var myBusinessObj = PaymentModeMapper.Map<PaymentMode, xPlug.BusinessObject.PaymentMode>(myObj);
					if(myBusinessObj == null){return new xPlug.BusinessObject.PaymentMode();}
					{return myBusinessObj;}
				}
			}
			catch (Exception ex)
			{
				ErrorManager.LogApplicationError(ex.StackTrace, ex.Source, ex.Message);
				return new xPlug.BusinessObject.PaymentMode();
			}
		}

		public List<xPlug.BusinessObject.PaymentMode> GetPaymentModes()
		{
			try
			{
				using (var db = new ExpenseManagerDBEntities())
				{
					var myObjList = db.PaymentModes.ToList();
					var myBusinessObjList = new List<xPlug.BusinessObject.PaymentMode>();
					if(myObjList == null){return myBusinessObjList;}
					//Re-Map each Entity Object to Business Object
					foreach (var item in myObjList)
					{
					var myBusinessObj = PaymentModeMapper.Map<PaymentMode, xPlug.BusinessObject.PaymentMode>(item);
						if(myBusinessObj == null){continue;}
						myBusinessObjList.Add(myBusinessObj);
					}
					return myBusinessObjList;
				}
			}
			catch (Exception ex)
			{
				ErrorManager.LogApplicationError(ex.StackTrace, ex.Source, ex.Message);
				return new List<xPlug.BusinessObject.PaymentMode>();
			}
		}

	}


	//Class File Generated from Code<->Stripper 1.2.0.0 | All Rights Reserved
}
