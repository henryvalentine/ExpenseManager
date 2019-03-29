using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using ExpenseManager.EF;
using xPlug.BusinessObject.CustomizedASPBusinessObject.Enum;
using xPlug.BusinessObjectMapper;
using XPLUG.WEBTOOLS;

namespace xPlug.BusinessManager
{
	/// Author:	 Oluwaseyi Adegboyega - sadegboyega@xplugng.com
	/// Copyright © 2013. xPlug Technologies Limited. All Rights Reserved
	///*******************************************************************************
	///* Date Created: 	04-06-2013
	///* Date Generated:	12-09-2013 10:06:29
	///*******************************************************************************


	public partial class BeneficiaryManager
	{
        public int AddBeneficiaryCheckDuplicate(BusinessObject.Beneficiary beneficiary)
        {
            try
            {
                //Re-Map Object to Entity Object
                var myEntityObj = BeneficiaryMapper.Map<BusinessObject.Beneficiary, Beneficiary>(beneficiary);
                if (myEntityObj == null)
                { return -2; }
                using (var db = new ExpenseManagerDBEntities())
                {
                    if (db.Beneficiaries.Any())
                    {
                        if (db.Beneficiaries.Count(m => m.FullName.ToLower().Replace(" ", string.Empty) == beneficiary.FullName.ToLower().Replace(" ", string.Empty) && m.CompanyName.ToLower().Replace(" ", string.Empty) == beneficiary.CompanyName.ToLower().Replace(" ", string.Empty)) > 0)
                        {
                            return -3;
                        }

                        if (db.Beneficiaries.Count(m => m.GSMNO1 == beneficiary.GSMNO1) > 0)
                        {
                            return -4;
                        }

                        if (db.Beneficiaries.Count(m => m.Email == beneficiary.Email) > 0)
                        {
                            return -5;
                        }
                    }
                    db.AddToBeneficiaries(myEntityObj);
                    db.SaveChanges();
                    beneficiary.BeneficiaryId = myEntityObj.BeneficiaryId;
                    return beneficiary.BeneficiaryId;
                }
            }
            catch (Exception ex)
            {
                ErrorManager.LogApplicationError(ex.StackTrace, ex.Source, ex.Message);
                return 0;
            }
        }
        public int UpdateBeneficiaryCheckDuplicate(BusinessObject.Beneficiary beneficiary)
        {
            try
            {
                //Re-Map Object to Entity Object
                var myEntityObj = BeneficiaryMapper.Map<BusinessObject.Beneficiary, Beneficiary>(beneficiary);
                if (myEntityObj == null)
                { return -2; }
                using (var db = new ExpenseManagerDBEntities())
                {
                    if (db.Beneficiaries.Any())
                    {
                        if (db.Beneficiaries.Count(m => m.FullName.ToLower().Replace(" ", string.Empty) == beneficiary.FullName.ToLower().Replace(" ", string.Empty) && m.CompanyName.ToLower().Replace(" ", string.Empty) == beneficiary.CompanyName.ToLower().Replace(" ", string.Empty) && m.BeneficiaryId != beneficiary.BeneficiaryId) > 0)
                        {
                            return -3;
                        }

                        if (db.Beneficiaries.Count(m => m.GSMNO1 == beneficiary.GSMNO1 && m.BeneficiaryId != beneficiary.BeneficiaryId) > 0)
                        {
                            return -4;
                        }

                        if (db.Beneficiaries.Count(m => m.Email == beneficiary.Email && m.BeneficiaryId != beneficiary.BeneficiaryId) > 0)
                        {
                            return -5;
                        }
                    }
                    db.Beneficiaries.Attach(myEntityObj);
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
        public Dictionary<List<BusinessObject.Beneficiary>, List<BusinessObject.Beneficiary>> GetFilteredBeneficiaries()
        {
            try
            {
                var beneficiaryList = new Dictionary<List<BusinessObject.Beneficiary>, List<BusinessObject.Beneficiary>>();
                var beneficiariesWithUnApprovedTransactions = new List<BusinessObject.Beneficiary>();
                var beneficiariesWithApprovedTransactions = new List<BusinessObject.Beneficiary>();
                var myBusinessObjList = new List<BusinessObject.ExpenseTransaction>();

                using (var db = new ExpenseManagerDBEntities())
                {
                    var myObjList = db.ExpenseTransactions.ToList();

                    if (!myObjList.Any())
                    {
                        return new Dictionary<List<BusinessObject.Beneficiary>, List<BusinessObject.Beneficiary>>();
                    }

                    foreach (var item in myObjList)
                    {
                        var myBusinessObj =
                            ExpenseTransactionMapper.Map<ExpenseTransaction, BusinessObject.ExpenseTransaction>(item);
                        if (myBusinessObj == null)
                        {
                            continue;
                        }

                        myBusinessObjList.Add(myBusinessObj);
                    }

                }

                foreach (var item in myBusinessObjList)
                {
                    if (beneficiariesWithUnApprovedTransactions.All(m => m.BeneficiaryId != item.BeneficiaryId))
                    {
                        beneficiariesWithUnApprovedTransactions.Add(item.Beneficiary);
                    }

                    if (beneficiariesWithApprovedTransactions.All(m => m.BeneficiaryId != item.BeneficiaryId && item.Status == 1))
                    {
                        beneficiariesWithApprovedTransactions.Add(item.Beneficiary);
                    }
                }

                if (!beneficiariesWithUnApprovedTransactions.Any() && !beneficiariesWithApprovedTransactions.Any())
                {
                    return beneficiaryList;
                }
                beneficiaryList.Add(beneficiariesWithUnApprovedTransactions.OrderBy(m => m.FullName).ToList(), beneficiariesWithApprovedTransactions.OrderBy(m => m.FullName).ToList());

                return beneficiaryList;
            }
            catch (Exception ex)
            {
                ErrorManager.LogApplicationError(ex.StackTrace, ex.Source, ex.Message);
                return new Dictionary<List<BusinessObject.Beneficiary>, List<BusinessObject.Beneficiary>>();
            }
        }
        public List<BusinessObject.Beneficiary> GeteBeneficiaryBySearchText(string searchText)
        {
            try
            {
                using (var db = new ExpenseManagerDBEntities())
                {
                    var myObjList = db.Beneficiaries.ToList();
                    var beneficiaries = new List<BusinessObject.Beneficiary>();
                    if (!myObjList.Any())
                    {
                        return beneficiaries;
                    }
                    //Re-Map each Entity Object to Business Object
                    foreach (var item in myObjList)
                    {
                        var myBusinessObj = BeneficiaryMapper.Map<Beneficiary, BusinessObject.Beneficiary>(item);
                        if (myBusinessObj == null)
                        {
                            continue;
                        }

                        if (myBusinessObj.FullName.ToLower().Contains(searchText.ToLower()))
                        {
                            if (myBusinessObj.BeneficiaryTypeId == 2)
                            {
                                var name = Enum.GetName(typeof(DeptAndUnitStatus), DeptAndUnitStatus.Not_Applicable);
                                if (name != null)
                                    myBusinessObj.Department.Name = name.Replace("_", " ");
                                var s = Enum.GetName(typeof(DeptAndUnitStatus), DeptAndUnitStatus.Not_Applicable);
                                if (s != null)
                                    myBusinessObj.Unit.Name = s.Replace("_", " ");
                            }
                            beneficiaries.Add(myBusinessObj);
                        }

                    }

                    if (!beneficiaries.Any())
                    {
                        return new List<BusinessObject.Beneficiary>();
                    }

                    return beneficiaries.OrderBy(m => m.FullName).ToList();
                }
            }
            catch (Exception ex)
            {
                ErrorManager.LogApplicationError(ex.StackTrace, ex.Source, ex.Message);
                return new List<BusinessObject.Beneficiary>();
            }
        }
        public List<BusinessObject.Beneficiary> GetBeneficiariesWithUnCompletedTransactionPayments()
        {
            try
            {
                using (var db = new ExpenseManagerDBEntities())
                {
                    
                    try
                    {
                        var myObjList = db.ExpenseTransactionPayments.Where(m => m.Status == 0 && m.ExpenseTransaction.Status == 1).ToList();
                       
                        if (!myObjList.Any())
                        {
                            return new List<BusinessObject.Beneficiary>();
                        }

                        var beneficiaryList = db.Beneficiaries.Where(m => m.Status == 1).ToList();

                        var newBeneficiaryList = new List<Beneficiary>();

                        foreach (var beneficiary in myObjList.Select(transactionPayment => beneficiaryList.Find(m => m.BeneficiaryId == transactionPayment.BeneficiaryId)).Where(beneficiary => newBeneficiaryList.All(m => m.BeneficiaryId != beneficiary.BeneficiaryId)))
                        {
                            newBeneficiaryList.Add(beneficiary);
                        }

                        if (!newBeneficiaryList.Any())
                        {
                            return new List<BusinessObject.Beneficiary>();
                        }

                        var myBusinessObjList = new List<BusinessObject.Beneficiary>();
                        
                        //Re-Map each Entity Object to Business Object
                        foreach (var item in newBeneficiaryList)
                        {
                            if(myBusinessObjList.All(m => m.BeneficiaryId != item.BeneficiaryId))
                            {
                                var myBusinessObj = BeneficiaryMapper.Map<Beneficiary, BusinessObject.Beneficiary>(item);
                                if (myBusinessObj == null)
                                {
                                    continue;
                                }
                                if (myBusinessObj.BeneficiaryTypeId == 2)
                                {
                                    var name = Enum.GetName(typeof(DeptAndUnitStatus), DeptAndUnitStatus.Not_Applicable);
                                    if (name !=
                                        null)
                                        myBusinessObj.Department.Name = name.Replace("_", " ");
                                    var s = Enum.GetName(typeof(DeptAndUnitStatus), DeptAndUnitStatus.Not_Applicable);
                                    if (s !=
                                        null)
                                        myBusinessObj.Unit.Name = s.Replace("_", " ");
                                }
                                myBusinessObjList.Add(myBusinessObj);
                            }
                            
                        }
                        return myBusinessObjList.OrderBy(m => m.FullName).ToList();

                    }
                    catch (Exception ex)
                    {
                        ErrorManager.LogApplicationError(ex.StackTrace, ex.Source, ex.Message);
                        return null;
                    }
                }
            }
            catch (Exception ex)
            {
                ErrorManager.LogApplicationError(ex.StackTrace, ex.Source, ex.Message);
                return new List<BusinessObject.Beneficiary>();
            }
        }
        public List<BusinessObject.Beneficiary> GetActiveBeneficiaries()
        {
            try
            {
                using (var db = new ExpenseManagerDBEntities())
                {
                    var myObjList = db.Beneficiaries.Where(m => m.Status == 1).ToList();
                    var myBusinessObjList = new List<BusinessObject.Beneficiary>();
                    if (!myObjList.Any())
                    {
                        return myBusinessObjList;
                    }
                    //Re-Map each Entity Object to Business Object
                    foreach (var item in myObjList)
                    {
                        var myBusinessObj = BeneficiaryMapper.Map<Beneficiary, BusinessObject.Beneficiary>(item);

                        if (myBusinessObj == null)
                        {
                            continue;
                        }

                        if (myBusinessObj.BeneficiaryTypeId == 2)
                        {
                            var name = Enum.GetName(typeof(DeptAndUnitStatus), DeptAndUnitStatus.Not_Applicable);
                            if (name != null)
                                myBusinessObj.Department.Name = name.Replace("_", " ");
                            var s = Enum.GetName(typeof(DeptAndUnitStatus), DeptAndUnitStatus.Not_Applicable);
                            if (s != null)
                                myBusinessObj.Unit.Name = s.Replace("_", " ");
                        }
                        myBusinessObjList.Add(myBusinessObj);
                    }
                    return myBusinessObjList.OrderBy(m => m.FullName).ToList();
                }
            }
            catch (Exception ex)
            {
                ErrorManager.LogApplicationError(ex.StackTrace, ex.Source, ex.Message);
                return new List<BusinessObject.Beneficiary>();
            }
        }
        public List<BusinessObject.Beneficiary> GetAllBeneficiaries()
        {
            try
            {
                using (var db = new ExpenseManagerDBEntities())
                {
                    var myObjList = db.Beneficiaries.ToList();
                    var myBusinessObjList = new List<BusinessObject.Beneficiary>();
                    if (!myObjList.Any())
                    {
                        return myBusinessObjList;
                    }
                    //Re-Map each Entity Object to Business Object
                    foreach (var item in myObjList)
                    {
                        var myBusinessObj = BeneficiaryMapper.Map<Beneficiary, BusinessObject.Beneficiary>(item);

                        if (myBusinessObj == null)
                        {
                            continue;
                        }

                        if(myBusinessObj.BeneficiaryTypeId == 2)
                        {
                            var name = Enum.GetName(typeof(DeptAndUnitStatus), DeptAndUnitStatus.Not_Applicable);
                            if (name != null)
                                myBusinessObj.Department.Name = name.Replace("_", " ");
                            var s = Enum.GetName(typeof(DeptAndUnitStatus), DeptAndUnitStatus.Not_Applicable);
                            if (s != null)
                                myBusinessObj.Unit.Name = s.Replace("_", " ");
                        }
                        myBusinessObjList.Add(myBusinessObj);
                    }
                    return myBusinessObjList.OrderBy(m => m.FullName).ToList();
                }
            }
            catch (Exception ex)
            {
                ErrorManager.LogApplicationError(ex.StackTrace, ex.Source, ex.Message);
                return new List<BusinessObject.Beneficiary>();
            }
        }
        public List<BusinessObject.Beneficiary> GetActiveBeneficiariesByBeneficiaryType(int beneficiaryTypeId)
        {
            try
            {
                using (var db = new ExpenseManagerDBEntities())
                {
                    var myObjList = db.Beneficiaries.Where(m => m.BeneficiaryTypeId == beneficiaryTypeId && m.Status == 1).ToList();
                    var beneficiaries = new List<BusinessObject.Beneficiary>();
                    if (!myObjList.Any())
                    {
                        return beneficiaries;
                    }
                    //Re-Map each Entity Object to Business Object
                    foreach (var item in myObjList)
                    {
                        var myBusinessObj = BeneficiaryMapper.Map<Beneficiary, BusinessObject.Beneficiary>(item);
                        if (myBusinessObj == null)
                        {
                            continue;
                        }
                        if (myBusinessObj.BeneficiaryTypeId == 2)
                        {
                            var name = Enum.GetName(typeof(DeptAndUnitStatus), DeptAndUnitStatus.Not_Applicable);
                            if (name != null)
                                myBusinessObj.Department.Name = name.Replace("_", " ");
                            var s = Enum.GetName(typeof(DeptAndUnitStatus), DeptAndUnitStatus.Not_Applicable);
                            if (s != null)
                                myBusinessObj.Unit.Name = s.Replace("_", " ");
                        }
                        beneficiaries.Add(myBusinessObj);

                    }

                    if (!beneficiaries.Any())
                    {
                        return new List<BusinessObject.Beneficiary>();
                    }

                    return beneficiaries.OrderBy(m => m.FullName).ToList();
                }
            }
            catch (Exception ex)
            {
                ErrorManager.LogApplicationError(ex.StackTrace, ex.Source, ex.Message);
                return new List<BusinessObject.Beneficiary>();
            }
        }
        public List<BusinessObject.Beneficiary> GetBeneficiariesWithApprovedUnpaidTransactions()
        {
            try
            {
                using (var db = new ExpenseManagerDBEntities())
                {
                    var myObjList =
                        db.Beneficiaries.Where(m => m.Status == 1).SelectMany(
                            x => x.ExpenseTransactions.Where(m => m.BeneficiaryId == x.BeneficiaryId && m.Status == 1),
                            (x, y) => new { x, y }).SelectMany(
                                @t =>
                                @t.y.ExpenseTransactionPaymentHistories.Where(
                                    m => m.ExpenseTransactionId != @t.y.ExpenseTransactionId), (@t, z) => @t.x);

                    var myBusinessObjList = new List<BusinessObject.Beneficiary>();
                    if (!myObjList.Any())
                    {
                        return myBusinessObjList;
                    }
                    //Re-Map each Entity Object to Business Object
                    myBusinessObjList.AddRange(myObjList.Select(item => BeneficiaryMapper.Map<Beneficiary, BusinessObject.Beneficiary>(item)).Where(myBusinessObj => myBusinessObj != null));
                    return myBusinessObjList.OrderBy(m => m.FullName).ToList();
                }
            }
            catch (Exception ex)
            {
                ErrorManager.LogApplicationError(ex.StackTrace, ex.Source, ex.Message);
                return new List<BusinessObject.Beneficiary>();
            }
        }
	}


	//Class File Generated from Code<->Stripper 1.2.0.0 | All Rights Reserved
}
