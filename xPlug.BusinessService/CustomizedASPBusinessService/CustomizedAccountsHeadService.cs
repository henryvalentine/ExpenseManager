using System;
using System.Collections.Generic;
using System.Linq;
using XPLUG.WEBTOOLS;
using xPlug.BusinessObject;

namespace xPlug.BusinessService
{
	/// Author:	 Oluwaseyi Adegboyega - sadegboyega@xplugng.com
	/// Copyright © 2013. xPlug Technologies Limited. All Rights Reserved
	///*******************************************************************************
	///* Date Created: 	04-06-2013
	///* Date Generated:	12-09-2013 10:06:31
	///*******************************************************************************


	public partial class AccountsHeadService
	{
        public List<AccountsHead> GetFilteredAccountsHeads()
        {
            try
            {
                List<AccountsHead> objList = _accountsHeadManager.GetFilteredAccountsHeads();
                if (objList == null) { return new List<AccountsHead>(); }
                return objList;
            }
            catch (Exception ex)
            {
                ErrorManager.LogApplicationError(ex.StackTrace, ex.Source, ex.Message);
                return new List<AccountsHead>();
            }
        }

        public List<AccountsHead> GetOrderedAccountsHeadsByExpenseCategoryId(Int32 expenseCategoryId)
        {
            try
            {
                return _accountsHeadManager.GetOrderedAccountsHeadsByExpenseCategoryId(expenseCategoryId);
            }
            catch (Exception ex)
            {
                ErrorManager.LogApplicationError(ex.StackTrace, ex.Source, ex.Message);
                return new List<AccountsHead>();
            }
        }

         public List<AccountsHead> GetOrderedAccountsHeads()
        {
            try
            {
                return _accountsHeadManager.GetOrderedAccountsHeads();
               
            }
            catch (Exception ex)
            {
                ErrorManager.LogApplicationError(ex.StackTrace, ex.Source, ex.Message);
                return new List<AccountsHead>();
            }
        }

         public List<AccountsHead> GetActiveOrderedAccountsHeads()
        {
            try
            {
                return _accountsHeadManager.GetActiveOrderedAccountsHeads();

            }
            catch (Exception ex)
            {
                ErrorManager.LogApplicationError(ex.StackTrace, ex.Source, ex.Message);
                return new List<AccountsHead>();
            }
       }

        public int AddAccountsHeadCheckDuplicate(AccountsHead accountsHead)
        {
            try
            {
                return _accountsHeadManager.AddAccountsHeadCheckDuplicate(accountsHead);

            }
            catch (Exception ex)
            {
                ErrorManager.LogApplicationError(ex.StackTrace, ex.Source, ex.Message);
                return 0;
            }
        }

        public int UpdateAccountsHeadCheckDuplicate(AccountsHead accountsHead)
        {
            try
            {
                return _accountsHeadManager.UpdateAccountsHeadCheckDuplicate(accountsHead);

            }
            catch (Exception ex)
            {
                ErrorManager.LogApplicationError(ex.StackTrace, ex.Source, ex.Message);
                return 0;
            }
       }

        public int GetAccountsHeadsCountByCategoryId(Int32 expenseCategoryId)
        {
            try
            {
                return _accountsHeadManager.GetAccountsHeadsCountByCategoryId(expenseCategoryId);
            }
            catch (Exception ex)
            {
                kPortal.CoreUtilities.ErrorManager.LogApplicationError(ex.StackTrace, ex.Source, ex.Message);
                return -1;
            }
        }

        public AccountsHead GetLastInsertedAccountsHeadsByExpenseCategoryId(Int32 expenseCategoryId)
        {
            try
            {
                return _accountsHeadManager.GetLastInsertedAccountsHeadsByExpenseCategoryId(expenseCategoryId);
            }
            catch (Exception ex)
            {
                ErrorManager.LogApplicationError(ex.StackTrace, ex.Source, ex.Message);
                return new AccountsHead();
            }
        }
            
	}

	//Class File Generated from Code<->Stripper 1.2.0.0 | All Rights Reserved
}
