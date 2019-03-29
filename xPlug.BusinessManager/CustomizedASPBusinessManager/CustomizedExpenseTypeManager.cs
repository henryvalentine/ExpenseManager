using System;
using System.Data;
using System.Linq;
using ExpenseManager.EF;
using XPLUG.WEBTOOLS;
using xPlug.BusinessObjectMapper;

namespace xPlug.BusinessManager
{
	/// Author:	 Oluwaseyi Adegboyega - sadegboyega@xplugng.com
	/// Copyright © 2013. xPlug Technologies Limited. All Rights Reserved
	///*******************************************************************************
	///* Date Created: 	04-06-2013
	///* Date Generated:	12-09-2013 10:06:26
	///*******************************************************************************


	public partial class ExpenseTypeManager
	{
        public int AddExpenseTypeCheckDuplicate(BusinessObject.ExpenseType expenseType)
        {
            try
            {
                //Re-Map Object to Entity Object
                var myEntityObj = ExpenseTypeMapper.Map<BusinessObject.ExpenseType, ExpenseType>(expenseType);
                if (myEntityObj == null)
                {
                    return -2;
                }
                using (var db = new ExpenseManagerDBEntities())
                {
                    if (db.ExpenseTypes.Count(m => m.Name.ToLower().Replace(" ", string.Empty) == expenseType.Name.ToLower().Replace(" ", string.Empty)) > 0)
                    {
                        return -3;
                    }
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
        public int UpdateExpenseTypeCheckDuplicate(BusinessObject.ExpenseType expenseType)
        {
            try
            {
                //Re-Map Object to Entity Object
                var myEntityObj = ExpenseTypeMapper.Map<BusinessObject.ExpenseType, ExpenseType>(expenseType);
                if (myEntityObj == null)
                {
                    return -2;
                }
                using (var db = new ExpenseManagerDBEntities())
                {
                    if (db.ExpenseTypes.Count(m => m.Name.ToLower().Replace(" ", string.Empty) == expenseType.Name.ToLower().Replace(" ", string.Empty) && m.ExpenseTypeId != expenseType.ExpenseTypeId) > 0)
                    {
                        return -3;
                    }
                    db.ExpenseTypes.Attach(myEntityObj);
                    db.ObjectStateManager.ChangeObjectState(myEntityObj, EntityState.Modified);
                    db.SaveChanges();
                    return 1;
                }
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
