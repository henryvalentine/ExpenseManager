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
	///* Date Generated:	01-12-2013 03:58:07
	///*******************************************************************************


	public partial class ChequeService : MarshalByRefObject
	{
		private readonly ChequeManager  _chequeManager;
		public ChequeService()
		{
			_chequeManager = new ChequeManager();
		}

		public int AddCheque(Cheque cheque)
		{
			try
			{
				return _chequeManager.AddCheque(cheque);
			}
			catch (Exception ex)
			{
				ErrorManager.LogApplicationError(ex.StackTrace, ex.Source, ex.Message);
				return 0;
			}
		}

		public bool UpdateCheque(Cheque cheque)
		{
			try
			{
				return _chequeManager.UpdateCheque(cheque);
				}
			catch (Exception ex)
			{
				ErrorManager.LogApplicationError(ex.StackTrace, ex.Source, ex.Message);
				return false;
			}
		}

		public bool DeleteCheque(Int32 chequePaymentId)
		{
			try
			{
				return _chequeManager.DeleteCheque(chequePaymentId);
				}
			catch (Exception ex)
			{
				ErrorManager.LogApplicationError(ex.StackTrace, ex.Source, ex.Message);
				return false;
			}
		}

		public Cheque GetCheque(int chequePaymentId)
		{
			try
			{
				return _chequeManager.GetCheque(chequePaymentId);
			}
			catch (Exception ex)
			{
				ErrorManager.LogApplicationError(ex.StackTrace, ex.Source, ex.Message);
				return new Cheque();
			}
		}

		public List<Cheque> GetCheques()
		{
			try
			{
				var objList = new List<Cheque>();
				objList = _chequeManager.GetCheques();
				if(objList == null) {return  new List<Cheque>();}
				return objList;
			}
			catch (Exception ex)
			{
				ErrorManager.LogApplicationError(ex.StackTrace, ex.Source, ex.Message);
				return new List<Cheque>();
			}
		}

		public List<Cheque>  GetChequesByExpenseTransactionPaymentHistoryId(Int64 expenseTransactionPaymentHistoryId)
		{
			try
			{
				return _chequeManager.GetChequesByExpenseTransactionPaymentHistoryId(expenseTransactionPaymentHistoryId);
			}
			catch (Exception ex)
			{
				ErrorManager.LogApplicationError(ex.StackTrace, ex.Source, ex.Message);
				return new List<Cheque>();
			}
		}

		public List<Cheque>  GetChequesByBankId(Int32 bankId)
		{
			try
			{
				return _chequeManager.GetChequesByBankId(bankId);
			}
			catch (Exception ex)
			{
				ErrorManager.LogApplicationError(ex.StackTrace, ex.Source, ex.Message);
				return new List<Cheque>();
			}
		}

	}


	//Class File Generated from Code<->Stripper 1.2.0.0 | All Rights Reserved
}
