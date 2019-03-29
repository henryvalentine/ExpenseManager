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
	///* Date Generated:	12-09-2013 10:06:26
	///*******************************************************************************


	public partial class ExpenseTypeService
	{
      public List<ExpenseType> GetActiveExpenseTypes()
		{
			try
			{
			    List<ExpenseType> objList = _expenseTypeManager.GetExpenseTypes();
				if(objList == null) {return  new List<ExpenseType>();}
                return objList.Where(m => m.Status == 1).OrderBy(m => m.Name).ToList();
			}
			catch (Exception ex)
			{
				ErrorManager.LogApplicationError(ex.StackTrace, ex.Source, ex.Message);
				return new List<ExpenseType>();
			}
		}

      public int AddExpenseTypeCheckDuplicate(ExpenseType expenseType)
      {
          try
          {
              return _expenseTypeManager.AddExpenseTypeCheckDuplicate(expenseType);
          }
          catch (Exception ex)
          {
              ErrorManager.LogApplicationError(ex.StackTrace, ex.Source, ex.Message);
              return 0;
          }
      }

      public int UpdateExpenseTypeCheckDuplicate(ExpenseType expenseType)
      {
          try
          {
              return _expenseTypeManager.UpdateExpenseTypeCheckDuplicate(expenseType);
          }
          catch (Exception ex)
          {
              ErrorManager.LogApplicationError(ex.StackTrace, ex.Source, ex.Message);
              return 0;
          }
      }
	}


	//Class File Generated from Code<->Stripper 1.2.0.0 | All Rights Reserved
}
