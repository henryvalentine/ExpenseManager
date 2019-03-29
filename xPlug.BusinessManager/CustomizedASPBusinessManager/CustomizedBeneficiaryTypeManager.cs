using System;
using System.Collections.Generic;
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
	///* Date Generated:	25-11-2013 09:26:28
	///*******************************************************************************


	public partial class BeneficiaryTypeManager
	{
        public int AddBeneficiaryTypeCheckDuplicate(BusinessObject.BeneficiaryType beneficiaryType)
        {
            try
            {
                //Re-Map Object to Entity Object
                var myEntityObj = BeneficiaryTypeMapper.Map<BusinessObject.BeneficiaryType, BeneficiaryType>(beneficiaryType);
                if (myEntityObj == null)
                { return -2; }
                using (var db = new ExpenseManagerDBEntities())
                {
                    if (db.BeneficiaryTypes.Any())
                    {
                        if (db.BeneficiaryTypes.Count(m => m.Name.ToLower().Replace(" ", string.Empty) == beneficiaryType.Name.ToLower().Replace(" ", string.Empty)) > 0)
                        {
                            return -3;
                        }
                       
                    }
                    db.AddToBeneficiaryTypes(myEntityObj);
                    db.SaveChanges();
                    beneficiaryType.BeneficiaryTypeId = myEntityObj.BeneficiaryTypeId;
                    return beneficiaryType.BeneficiaryTypeId;
                }
            }
            catch (Exception ex)
            {
                ErrorManager.LogApplicationError(ex.StackTrace, ex.Source, ex.Message);
                return 0;
            }
        }
        public int UpdateBeneficiaryTypeCheckDuplicate(BusinessObject.BeneficiaryType beneficiaryType)
        {
            try
            {
                //Re-Map Object to Entity Object
                var myEntityObj = BeneficiaryTypeMapper.Map<BusinessObject.BeneficiaryType, BeneficiaryType>(beneficiaryType);
                if (myEntityObj == null)
                { return -2; }
                using (var db = new ExpenseManagerDBEntities())
                {
                    if (db.BeneficiaryTypes.Any())
                    {
                        if (db.BeneficiaryTypes.Count(m => m.Name.ToLower().Replace(" ", string.Empty) == beneficiaryType.Name.ToLower().Replace(" ", string.Empty) && m.BeneficiaryTypeId != beneficiaryType.BeneficiaryTypeId) > 0)
                        {
                            return -3;
                        }
                        
                    }
                    db.BeneficiaryTypes.Attach(myEntityObj);
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
        public List<BusinessObject.BeneficiaryType> GetActiveBeneficiaryTypes()
        {
            try
            {
                using (var db = new ExpenseManagerDBEntities())
                {
                    var myObjList = db.BeneficiaryTypes.Where(m => m.Status == 1).ToList();
                    var myBusinessObjList = new List<BusinessObject.BeneficiaryType>();
                    if (!myObjList.Any())
                    {
                        return myBusinessObjList;
                    }
                    //Re-Map each Entity Object to Business Object
                    foreach (var item in myObjList)
                    {
                        var myBusinessObj = BeneficiaryTypeMapper.Map<BeneficiaryType, BusinessObject.BeneficiaryType>(item);

                        if (myBusinessObj == null)
                        {
                            continue;
                        }
                        myBusinessObjList.Add(myBusinessObj);
                    }
                    return myBusinessObjList.OrderBy(m => m.Name).ToList();
                }
            }
            catch (Exception ex)
            {
                ErrorManager.LogApplicationError(ex.StackTrace, ex.Source, ex.Message);
                return new List<BusinessObject.BeneficiaryType>();
            }
        }
        public List<BusinessObject.BeneficiaryType> GetAllBeneficiaryTypes()
        {
            try
            {
                using (var db = new ExpenseManagerDBEntities())
                {
                    var myObjList = db.BeneficiaryTypes.ToList();
                    var myBusinessObjList = new List<BusinessObject.BeneficiaryType>();
                    if (!myObjList.Any())
                    {
                        return myBusinessObjList;
                    }
                    //Re-Map each Entity Object to Business Object
                    foreach (var item in myObjList)
                    {
                        var myBusinessObj = BeneficiaryTypeMapper.Map<BeneficiaryType, BusinessObject.BeneficiaryType>(item);

                        if (myBusinessObj == null)
                        {
                            continue;
                        }
                        myBusinessObjList.Add(myBusinessObj);
                    }
                    return myBusinessObjList.OrderBy(m => m.Name).ToList();
                }
            }
            catch (Exception ex)
            {
                ErrorManager.LogApplicationError(ex.StackTrace, ex.Source, ex.Message);
                return new List<BusinessObject.BeneficiaryType>();
            }
        }
       
	}
	//Class File Generated from Code<->Stripper 1.2.0.0 | All Rights Reserved
}
