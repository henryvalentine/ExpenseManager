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


	public partial class BankManager
	{
		public BankManager()
		{
		}

		public int AddBank(xPlug.BusinessObject.Bank bank)
		{
			try
			{
				//Re-Map Object to Entity Object
				var myEntityObj = BankMapper.Map<xPlug.BusinessObject.Bank, Bank>(bank);
				if(myEntityObj == null)
				{return -2;}
				using (var db = new ExpenseManagerDBEntities())
				{
					db.AddToBanks(myEntityObj);
					db.SaveChanges();
					bank.BankId = myEntityObj.BankId;
					return bank.BankId;
				}
			}
			catch (Exception ex)
			{
				ErrorManager.LogApplicationError(ex.StackTrace, ex.Source, ex.Message);
				return 0;
			}
		}

		public bool UpdateBank(xPlug.BusinessObject.Bank bank)
		{
			try
			{
				//Re-Map Object to Entity Object
				var myEntityObj = BankMapper.Map<xPlug.BusinessObject.Bank, Bank>(bank);
				if(myEntityObj == null)
				{return false;}
				using (var db = new ExpenseManagerDBEntities())
				{
					db.Banks.Attach(myEntityObj);
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

		public bool DeleteBank(int bankId)
		{
			try
			{
				using (var db = new ExpenseManagerDBEntities())
				{
					var myObj = db.Banks.Single(s => s.BankId == bankId);
					if (myObj == null) { return false; };
					db.Banks.DeleteObject(myObj);
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

		public xPlug.BusinessObject.Bank GetBank(int bankId)
		{
			try
			{
				using (var db = new ExpenseManagerDBEntities())
				{
					var myObj = db.Banks.SingleOrDefault(s => s.BankId == bankId);
					if(myObj == null){return new xPlug.BusinessObject.Bank();}
					//Re-Map Entity Object to Business Object
					var myBusinessObj = BankMapper.Map<Bank, xPlug.BusinessObject.Bank>(myObj);
					if(myBusinessObj == null){return new xPlug.BusinessObject.Bank();}
					{return myBusinessObj;}
				}
			}
			catch (Exception ex)
			{
				ErrorManager.LogApplicationError(ex.StackTrace, ex.Source, ex.Message);
				return new xPlug.BusinessObject.Bank();
			}
		}

		public List<xPlug.BusinessObject.Bank> GetBanks()
		{
			try
			{
				using (var db = new ExpenseManagerDBEntities())
				{
					var myObjList = db.Banks.ToList();
					var myBusinessObjList = new List<xPlug.BusinessObject.Bank>();
					if(myObjList == null){return myBusinessObjList;}
					//Re-Map each Entity Object to Business Object
					foreach (var item in myObjList)
					{
					var myBusinessObj = BankMapper.Map<Bank, xPlug.BusinessObject.Bank>(item);
						if(myBusinessObj == null){continue;}
						myBusinessObjList.Add(myBusinessObj);
					}
					return myBusinessObjList;
				}
			}
			catch (Exception ex)
			{
				ErrorManager.LogApplicationError(ex.StackTrace, ex.Source, ex.Message);
				return new List<xPlug.BusinessObject.Bank>();
			}
		}

	}


	//Class File Generated from Code<->Stripper 1.2.0.0 | All Rights Reserved
}
