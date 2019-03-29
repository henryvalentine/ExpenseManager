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
	///* Date Generated:	05-12-2013 11:09:29
	///*******************************************************************************


	public partial class PaymentVoucherNumberService : MarshalByRefObject
	{
		private readonly PaymentVoucherNumberManager  _paymentVoucherNumberManager;
		public PaymentVoucherNumberService()
		{
			_paymentVoucherNumberManager = new PaymentVoucherNumberManager();
		}

		public int AddPaymentVoucherNumber(PaymentVoucherNumber paymentVoucherNumber)
		{
			try
			{
				return _paymentVoucherNumberManager.AddPaymentVoucherNumber(paymentVoucherNumber);
			}
			catch (Exception ex)
			{
				ErrorManager.LogApplicationError(ex.StackTrace, ex.Source, ex.Message);
				return 0;
			}
		}

		public bool UpdatePaymentVoucherNumber(PaymentVoucherNumber paymentVoucherNumber)
		{
			try
			{
				return _paymentVoucherNumberManager.UpdatePaymentVoucherNumber(paymentVoucherNumber);
				}
			catch (Exception ex)
			{
				ErrorManager.LogApplicationError(ex.StackTrace, ex.Source, ex.Message);
				return false;
			}
		}

		public bool DeletePaymentVoucherNumber(Int32 paymentVoucherNumber)
		{
			try
			{
				return _paymentVoucherNumberManager.DeletePaymentVoucherNumber(paymentVoucherNumber);
				}
			catch (Exception ex)
			{
				ErrorManager.LogApplicationError(ex.StackTrace, ex.Source, ex.Message);
				return false;
			}
		}

		public PaymentVoucherNumber GetPaymentVoucherNumber(int paymentVoucherNumber)
		{
			try
			{
				return _paymentVoucherNumberManager.GetPaymentVoucherNumber(paymentVoucherNumber);
			}
			catch (Exception ex)
			{
				ErrorManager.LogApplicationError(ex.StackTrace, ex.Source, ex.Message);
				return new PaymentVoucherNumber();
			}
		}

		public List<PaymentVoucherNumber> GetPaymentVoucherNumbers()
		{
			try
			{
				var objList = new List<PaymentVoucherNumber>();
				objList = _paymentVoucherNumberManager.GetPaymentVoucherNumbers();
				if(objList == null) {return  new List<PaymentVoucherNumber>();}
				return objList;
			}
			catch (Exception ex)
			{
				ErrorManager.LogApplicationError(ex.StackTrace, ex.Source, ex.Message);
				return new List<PaymentVoucherNumber>();
			}
		}

	}


	//Class File Generated from Code<->Stripper 1.2.0.0 | All Rights Reserved
}
