using System;
using System.Linq;
using System.Collections.Generic;
using System.Data;
using xPlug.BusinessObject;
using xPlug.BusinessManager;
using kPortal.CoreUtilities;



namespace xPlug.BusinessService
{
	/// Author:	 Oluwaseyi Adegboyega - sadegboyega@xplugng.com
	/// Copyright © 2013. xPlug Technologies Limited. All Rights Reserved
	///*******************************************************************************
	///* Date Created: 	04-06-2013
	///* Date Generated:	25-11-2013 09:26:18
	///*******************************************************************************


	public partial class PaymentModeService : MarshalByRefObject
	{
		private readonly PaymentModeManager  _paymentModeManager;
		public PaymentModeService()
		{
			_paymentModeManager = new PaymentModeManager();
		}

		public int AddPaymentMode(PaymentMode paymentMode)
		{
			try
			{
				return _paymentModeManager.AddPaymentMode(paymentMode);
			}
			catch (Exception ex)
			{
				ErrorManager.LogApplicationError(ex.StackTrace, ex.Source, ex.Message);
				return 0;
			}
		}

		public bool UpdatePaymentMode(PaymentMode paymentMode)
		{
			try
			{
				return _paymentModeManager.UpdatePaymentMode(paymentMode);
				}
			catch (Exception ex)
			{
				ErrorManager.LogApplicationError(ex.StackTrace, ex.Source, ex.Message);
				return false;
			}
		}

		public bool DeletePaymentMode(Int32 paymentModeId)
		{
			try
			{
				return _paymentModeManager.DeletePaymentMode(paymentModeId);
				}
			catch (Exception ex)
			{
				ErrorManager.LogApplicationError(ex.StackTrace, ex.Source, ex.Message);
				return false;
			}
		}

		public PaymentMode GetPaymentMode(int paymentModeId)
		{
			try
			{
				return _paymentModeManager.GetPaymentMode(paymentModeId);
			}
			catch (Exception ex)
			{
				ErrorManager.LogApplicationError(ex.StackTrace, ex.Source, ex.Message);
				return new PaymentMode();
			}
		}

		public List<PaymentMode> GetPaymentModes()
		{
			try
			{
				var objList = new List<PaymentMode>();
				objList = _paymentModeManager.GetPaymentModes();
				if(objList == null) {return  new List<PaymentMode>();}
				return objList;
			}
			catch (Exception ex)
			{
				ErrorManager.LogApplicationError(ex.StackTrace, ex.Source, ex.Message);
				return new List<PaymentMode>();
			}
		}

	}


	//Class File Generated from Code<->Stripper 1.2.0.0 | All Rights Reserved
}
