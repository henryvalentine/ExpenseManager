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
	///* Date Generated:	25-11-2013 09:26:28
	///*******************************************************************************


	public partial class ExpenseTypeManager
	{
		public ExpenseTypeManager()
		{
		}

		public int AddExpenseType(xPlug.BusinessObject.ExpenseType expenseType)
		{
			try
			{
				//Re-Map Object to Entity Object
				var myEntityObj = ExpenseTypeMapper.Map<xPlug.BusinessObject.ExpenseType, ExpenseType>(expenseType);
				if(myEntityObj == null)
				{return -2;}
				using (var db = new ExpenseManagerDBEntities())
				{
					db.AddToExpenseTypes(myEntityObj);
					db.SaveChanges();
					expenseType.ExpenseTypeId = myEntityObj.ExpenseTypeId;
					return expenseType.ExpenseTypeId;
				}
			}
			catch (Exception ex)
			{
				ErrorManager.LogApplicationError(ex.StackTrace, ex.Source, ex.Message);
				return 0;
			}
		}

		public bool UpdateExpenseType(xPlug.BusinessObject.ExpenseType expenseType)
		{
			try
			{
				//Re-Map Object to Entity Object
				var myEntityObj = ExpenseTypeMapper.Map<xPlug.BusinessObject.ExpenseType, ExpenseType>(expenseType);
				if(myEntityObj == null)
				{return false;}
				using (var db = new ExpenseManagerDBEntities())
				{
					db.ExpenseTypes.Attach(myEntityObj);
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

		public bool DeleteExpenseType(int expenseTypeId)
		{
			try
			{
				using (var db = new ExpenseManagerDBEntities())
				{
					var myObj = db.ExpenseTypes.Single(s => s.ExpenseTypeId == expenseTypeId);
					if (myObj == null) { return false; };
					db.ExpenseTypes.DeleteObject(myObj);
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

		public xPlug.BusinessObject.ExpenseType GetExpenseType(int expenseTypeId)
		{
			try
			{
				using (var db = new ExpenseManagerDBEntities())
				{
					var myObj = db.ExpenseTypes.SingleOrDefault(s => s.ExpenseTypeId == expenseTypeId);
					if(myObj == null){return new xPlug.BusinessObject.ExpenseType();}
					//Re-Map Entity Object to Business Object
					var myBusinessObj = ExpenseTypeMapper.Map<ExpenseType, xPlug.BusinessObject.ExpenseType>(myObj);
					if(myBusinessObj == null){return new xPlug.BusinessObject.ExpenseType();}
					{return myBusinessObj;}
				}
			}
			catch (Exception ex)
			{
				ErrorManager.LogApplicationError(ex.StackTrace, ex.Source, ex.Message);
				return new xPlug.BusinessObject.ExpenseType();
			}
		}

		public List<xPlug.BusinessObject.ExpenseType> GetExpenseTypes()
		{
			try
			{
				using (var db = new ExpenseManagerDBEntities())
				{
					var myObjList = db.ExpenseTypes.ToList();
					var myBusinessObjList = new List<xPlug.BusinessObject.ExpenseType>();
					if(myObjList == null){return myBusinessObjList;}
					//Re-Map each Entity Object to Business Object
					foreach (var item in myObjList)
					{
					var myBusinessObj = ExpenseTypeMapper.Map<ExpenseType, xPlug.BusinessObject.ExpenseType>(item);
						if(myBusinessObj == null){continue;}
						myBusinessObjList.Add(myBusinessObj);
					}
					return myBusinessObjList;
				}
			}
			catch (Exception ex)
			{
				ErrorManager.LogApplicationError(ex.StackTrace, ex.Source, ex.Message);
				return new List<xPlug.BusinessObject.ExpenseType>();
			}
		}

	}


	//Class File Generated from Code<->Stripper 1.2.0.0 | All Rights Reserved
}
