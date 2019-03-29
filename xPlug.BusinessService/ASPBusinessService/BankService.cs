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
	///* Date Generated:	01-12-2013 03:58:06
	///*******************************************************************************


	public partial class BankService : MarshalByRefObject
	{
		private readonly BankManager  _bankManager;
		public BankService()
		{
			_bankManager = new BankManager();
		}

		public int AddBank(Bank bank)
		{
			try
			{
				return _bankManager.AddBank(bank);
			}
			catch (Exception ex)
			{
				ErrorManager.LogApplicationError(ex.StackTrace, ex.Source, ex.Message);
				return 0;
			}
		}

		public bool UpdateBank(Bank bank)
		{
			try
			{
				return _bankManager.UpdateBank(bank);
				}
			catch (Exception ex)
			{
				ErrorManager.LogApplicationError(ex.StackTrace, ex.Source, ex.Message);
				return false;
			}
		}

		public bool DeleteBank(Int32 bankId)
		{
			try
			{
				return _bankManager.DeleteBank(bankId);
				}
			catch (Exception ex)
			{
				ErrorManager.LogApplicationError(ex.StackTrace, ex.Source, ex.Message);
				return false;
			}
		}

		public Bank GetBank(int bankId)
		{
			try
			{
				return _bankManager.GetBank(bankId);
			}
			catch (Exception ex)
			{
				ErrorManager.LogApplicationError(ex.StackTrace, ex.Source, ex.Message);
				return new Bank();
			}
		}

		public List<Bank> GetBanks()
		{
			try
			{
				var objList = new List<Bank>();
				objList = _bankManager.GetBanks();
				if(objList == null) {return  new List<Bank>();}
				return objList;
			}
			catch (Exception ex)
			{
				ErrorManager.LogApplicationError(ex.StackTrace, ex.Source, ex.Message);
				return new List<Bank>();
			}
		}

	}


	//Class File Generated from Code<->Stripper 1.2.0.0 | All Rights Reserved
}
