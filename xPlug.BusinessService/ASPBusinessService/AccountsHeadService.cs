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
	///* Date Generated:	25-11-2013 09:26:21
	///*******************************************************************************


	public partial class AccountsHeadService : MarshalByRefObject
	{
		private readonly AccountsHeadManager  _accountsHeadManager;
		public AccountsHeadService()
		{
			_accountsHeadManager = new AccountsHeadManager();
		}

		public int AddAccountsHead(AccountsHead accountsHead)
		{
			try
			{
				return _accountsHeadManager.AddAccountsHead(accountsHead);
			}
			catch (Exception ex)
			{
				ErrorManager.LogApplicationError(ex.StackTrace, ex.Source, ex.Message);
				return 0;
			}
		}

		public int UpdateAccountsHead(AccountsHead accountsHead)
		{
			try
			{
				return _accountsHeadManager.UpdateAccountsHead(accountsHead);
				}
			catch (Exception ex)
			{
				ErrorManager.LogApplicationError(ex.StackTrace, ex.Source, ex.Message);
				return 0;
			}
		}

		public bool DeleteAccountsHead(Int32 accountsHeadId)
		{
			try
			{
				return _accountsHeadManager.DeleteAccountsHead(accountsHeadId);
				}
			catch (Exception ex)
			{
				ErrorManager.LogApplicationError(ex.StackTrace, ex.Source, ex.Message);
				return false;
			}
		}

		public AccountsHead GetAccountsHead(int accountsHeadId)
		{
			try
			{
				return _accountsHeadManager.GetAccountsHead(accountsHeadId);
			}
			catch (Exception ex)
			{
				ErrorManager.LogApplicationError(ex.StackTrace, ex.Source, ex.Message);
				return new AccountsHead();
			}
		}

		public List<AccountsHead> GetAccountsHeads()
		{
			try
			{
				var objList = new List<AccountsHead>();
				objList = _accountsHeadManager.GetAccountsHeads();
				if(objList == null) {return  new List<AccountsHead>();}
				return objList;
			}
			catch (Exception ex)
			{
				ErrorManager.LogApplicationError(ex.StackTrace, ex.Source, ex.Message);
				return new List<AccountsHead>();
			}
		}

		public List<AccountsHead>  GetAccountsHeadsByExpenseCategoryId(Int32 expenseCategoryId)
		{
			try
			{
				return _accountsHeadManager.GetAccountsHeadsByExpenseCategoryId(expenseCategoryId);
			}
			catch (Exception ex)
			{
				ErrorManager.LogApplicationError(ex.StackTrace, ex.Source, ex.Message);
				return new List<AccountsHead>();
			}
		}
        
	}


	//Class File Generated from Code<->Stripper 1.2.0.0 | All Rights Reserved
}
